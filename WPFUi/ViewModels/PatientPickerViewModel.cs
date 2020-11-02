using Application.Services.PatientServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WPFUi.Models;

namespace WPFUi.ViewModels
{
    public class PatientPickerViewModel : ViewModelBase
    {
        public event Action SelectedPatientChanged;

        // Private fields
        #region Private fields

        private PatientDisplayModel _selectedPatient;
        private string _searchPatientText;

        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        #endregion

        // Bindings
        #region Bindings

        public ObservableCollection<PatientDisplayModel> PatientsList { get; set; }
        public ObservableCollection<PatientDisplayModel> PatientsDisplayList { get; set; }

        public PatientDisplayModel SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
                SelectedPatientChanged?.Invoke();
            }
        }

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

        // Constructors
        #region Constructors

        private PatientPickerViewModel(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        public static PatientPickerViewModel LoadPatientPickerViewModel(IPatientService patientService, IMapper mapper)
        {
            var patientPickerViewModel = new PatientPickerViewModel(patientService, mapper);

            patientPickerViewModel.LoadPatients();

            return patientPickerViewModel;
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
