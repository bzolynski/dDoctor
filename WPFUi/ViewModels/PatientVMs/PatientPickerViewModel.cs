using Application.Services.PatientServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;

namespace WPFUi.ViewModels.PatientVMs
{
    public class PatientPickerViewModel : ViewModelBase
    {
        public event Action SelectedPatientChanged;

        // Private fields
        #region Private fields

        private string _searchPatientText;
        private Patient _patient;
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        #endregion

        // Bindings
        #region Bindings

        public Patient Patient 
        { 
            get => _patient;
            set 
            {
                _patient = value;
                SelectedPatientChanged?.Invoke();
            }
        }



        public ObservableCollection<PatientDisplayModel> PatientsList { get; set; }
        public ObservableCollection<PatientDisplayModel> PatientsDisplayList { get; set; }



        public PatientDisplayModel SelectedDisplayPatient { get; set; }

        public string SearchText
        {
            get { return _searchPatientText; }
            set
            {
                _searchPatientText = value;
                OnPropertyChanged(nameof(SearchText));
                PatientSearch();
            }
        }

        #endregion

        // Commands
        #region Commands

        public ICommand LoadPatientCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors

        public PatientPickerViewModel(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;

            LoadPatientCommand = new AsyncRelayCommand(LoadPatient, (ex) => throw ex);

            LoadPatients();
        }



        #endregion

        // Methods
        #region Methods

        private async Task LoadPatient(object arg)
        {
            Patient = await _patientService.GetById(SelectedDisplayPatient.Id);
        }

        private void LoadPatients()
        {
            _patientService.GetAllPatients().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    PatientsList = new ObservableCollection<PatientDisplayModel>(_mapper.Map<ObservableCollection<PatientDisplayModel>>(task.Result));
                    PatientsDisplayList = new ObservableCollection<PatientDisplayModel>(PatientsList);
                    OnPropertyChanged(nameof(PatientsList));
                    OnPropertyChanged(nameof(PatientsDisplayList));
                }
            });
        }              

        private void PatientSearch()
        {
            PatientsDisplayList = new ObservableCollection<PatientDisplayModel>(PatientsList
               .Where(x => x.FullName.ToUpper().Contains(SearchText.ToUpper()) ||
               x.FullAddressWithPostCode.ToUpper().Contains(SearchText.ToUpper())));
            OnPropertyChanged(nameof(PatientsDisplayList));
        }
        #endregion

    }
}
