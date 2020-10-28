using Application.Services;
using Application.Services.PatientServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WPFUi.Commands;
using WPFUi.Commands.Common;
using WPFUi.Models;

namespace WPFUi.ViewModels
{
    public class PatientsViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields

        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly IAgeService _ageService;

        private PatientFormViewModel _patientFormViewModel;
        private PatientDisplayModel _selectedPatient;
        private PatientSearchParametersModel _patientSearchParameters;
        #endregion

        // Bindings
        #region Bindings

        // TODO: PatientDisplayList sort after edit or add
        public ObservableCollection<PatientDisplayModel> PatientsList { get; set; }

        public ObservableCollection<PatientDisplayModel> PatientsDisplayList { get; set; }

        public PatientFormViewModel PatientFormViewModel
        {
            get { return _patientFormViewModel; }
            set 
            { 
                _patientFormViewModel = value;             
                OnPropertyChanged(nameof(PatientFormViewModel));
            }
        }

        public PatientDisplayModel SelectedPatient
        {
            get { return _selectedPatient; }
            set 
            { 
                _selectedPatient = value;

                OnPropertyChanged(nameof(SelectedPatient));
                
            }
        }
 
        public PatientSearchParametersModel PatientSearchParameters
        {
            get { return _patientSearchParameters; }
            set
            {
                _patientSearchParameters = value;
                OnPropertyChanged(nameof(PatientSearchParameters));
            }
        }




        #endregion

        // Commands
        #region Commands

        public ICommand OpenNewPatientFormCommand { get; set; }
        public ICommand OpenEditPatientFormCommand { get; set; }
        public ICommand DeletePatientCommand { get; set; }
        public ICommand PatientSearchCommand { get; set; }
        public ICommand ClearSearchParametersCommand { get; set; }
        public ICommand ReloadPatientListCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        private PatientsViewModel(IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService, IAgeService ageService)
        {
            PatientSearchParameters = new PatientSearchParametersModel();

            _patientService = patientService;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _ageService = ageService;

            OpenNewPatientFormCommand = new RelayCommand(OpenNewPatientForm);
            OpenEditPatientFormCommand = new RelayCommand(OpenEditPatientForm, CanOpenEditPatientForm);
            DeletePatientCommand = new AsyncRelayCommand(DeletePatient, CanDeletePatient,(ex) => throw ex);
            ClearSearchParametersCommand = new RelayCommand(ClearSearchParameters);
            PatientSearchCommand = new RelayCommand(PatientSearch);
            ReloadPatientListCommand = new RelayCommand(ReloadPatientList);

        }

        public static PatientsViewModel LoadPatientsViewModel(IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService, IAgeService ageService)
        {
            var patientsViewModel = new PatientsViewModel(patientService, mapper, dateTimeService, ageService);

            patientsViewModel.LoadPatients();


            return patientsViewModel;
        }
        #endregion

        // Methods
        #region Methods
        private void LoadPatients()
        {
            _patientService.GetAllPatients().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    PatientsList = new ObservableCollection<PatientDisplayModel>(_mapper.Map<ObservableCollection<PatientDisplayModel>>(task.Result.OrderBy(x => x.Lastname)));
                    PatientsDisplayList = new ObservableCollection<PatientDisplayModel>(PatientsList);
                    OnPropertyChanged(nameof(PatientsList));                    
                    OnPropertyChanged(nameof(PatientsDisplayList));
                }
            });
        }

        private void OpenNewPatientForm(object obj)
        {
            PatientFormViewModel = new PatientFormViewModel(this, _patientService, _mapper, _dateTimeService);
        }

        private async Task DeletePatient()
        {
            await _patientService.DeletePatient(SelectedPatient.Id);

            PatientsList.Remove(SelectedPatient);
            PatientsDisplayList.Remove(SelectedPatient);
        }

        private bool CanDeletePatient(object obj)
        {
            if (SelectedPatient == null)
                return false;

            return true;
        }

        private bool CanOpenEditPatientForm(object obj)
        {
            if (SelectedPatient == null)
                return false;

            return true;
        }

        private void OpenEditPatientForm(object obj)
        {
            PatientFormViewModel = new PatientFormViewModel(this, SelectedPatient, _patientService, _mapper, _dateTimeService);
        }

        private void ClearSearchParameters(object obj)
        {
            PatientSearchParameters = new PatientSearchParametersModel();
            PatientsDisplayList = new ObservableCollection<PatientDisplayModel>(PatientsList);
            OnPropertyChanged(nameof(PatientsDisplayList));
        }

        private void PatientSearch(object obj)
        {
            PatientsDisplayList = new ObservableCollection<PatientDisplayModel>(PatientsList
               .Where(x => x.FirstName.ToUpper().Contains(PatientSearchParameters.FirstName.ToUpper()))
               .Where(x => x.Address.City.ToUpper().Contains(PatientSearchParameters.City.ToUpper()))
               .Where(x => x.Address.Street.ToUpper().Contains(PatientSearchParameters.StreetName.ToUpper()))
               .Where(x => x.LastName.ToUpper().Contains(PatientSearchParameters.LastName.ToUpper()))
               .Where(x => 
               {
                   if(PatientSearchParameters.Age != null)
                       return _ageService.Calculate(x.BirthDate) == PatientSearchParameters.Age;
                   return true;
               }));
            OnPropertyChanged(nameof(PatientsDisplayList));
        }

        private void ReloadPatientList(object obj)
        {
            PatientsDisplayList.Clear();
            PatientsList.Clear();
            LoadPatients();
        }
        #endregion
    }
}
