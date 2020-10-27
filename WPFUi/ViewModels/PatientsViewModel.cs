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

        private PatientFormViewModel _patientFormViewModel;
        private PatientDisplayModel _selectedPatient;
        #endregion

        // Bindings
        #region Bindings
        public ObservableCollection<PatientDisplayModel> Patients { get; set; }
        

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
        #endregion

        // Commands
        #region Commands

        public ICommand OpenNewPatientFormCommand { get; set; }
        public ICommand OpenEditPatientFormCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        private PatientsViewModel(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;

            OpenNewPatientFormCommand = new RelayCommand(OpenNewPatientForm);
            OpenEditPatientFormCommand = new RelayCommand(OpenEditPatientForm, CanOpenEditPatientForm);
        }

        public static PatientsViewModel LoadPatientsViewModel(IPatientService patientService, IMapper mapper)
        {
            var patientsViewModel = new PatientsViewModel(patientService, mapper);

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
                    Patients = new ObservableCollection<PatientDisplayModel>(_mapper.Map<IEnumerable<PatientDisplayModel>>(task.Result.OrderBy(x => x.Lastname)));
                    OnPropertyChanged(nameof(Patients));
                   
                }
            });
        }

        private void OpenNewPatientForm(object param)
        {
            PatientFormViewModel = new PatientFormViewModel(this, _patientService, _mapper);
        }

        private bool CanOpenEditPatientForm(object param)
        {
            if (SelectedPatient == null)
                return false;

            return true;
        }

        private void OpenEditPatientForm(object param)
        {
            PatientFormViewModel = new PatientFormViewModel(this, SelectedPatient, _patientService, _mapper);
        }
        #endregion
    }
}
