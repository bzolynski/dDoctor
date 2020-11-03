using Application.Services;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;

namespace WPFUi.ViewModels
{
    public class PatientsViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields

        private PatientFormViewModel _patientFormViewModel;
        private PatientDisplayModel _selectedPatient;
        private string _searchText;

        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly IReservationService _reservationService;

        #endregion

        // Bindings
        #region Bindings

        // TODO: PatientDisplayList sort after edit or add
        public ObservableCollection<PatientDisplayModel> PatientsList { get; set; }
        public ObservableCollection<PatientDisplayModel> PatientsDisplayList { get; set; }
        public ObservableCollection<Reservation> Reservations { get; set; }

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

                if (value != null)
                    LoadReservations();
            }
        }
 
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                PatientSearch();
            }
        }




        #endregion

        // Commands
        #region Commands

        public ICommand OpenEditPatientFormCommand { get; set; }
        public ICommand DeletePatientCommand { get; set; }
        public ICommand ReloadPatientListCommand { get; set; }

        #endregion

        // Constructors
        #region Constructors
        public PatientsViewModel(IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService, IReservationService reservationService)
        {

            _patientService = patientService;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _reservationService = reservationService;
            OpenEditPatientFormCommand = new RelayCommand(OpenEditPatientForm);
            DeletePatientCommand = new AsyncRelayCommand(DeletePatient,(ex) => throw ex);
            ReloadPatientListCommand = new RelayCommand(ReloadPatientList);

            LoadPatients();
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

        private void LoadReservations()
        {
            _reservationService.GetManyByPatient(_selectedPatient.Id).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Reservations = new ObservableCollection<Reservation>(task.Result);
                    OnPropertyChanged(nameof(Reservations));
                }
            });
        }

        private async Task DeletePatient(object obj)
        {
            if (obj is PatientDisplayModel)
            {
                var patient = (PatientDisplayModel)obj;
                await _patientService.DeletePatient(patient.Id);

                PatientsList.Remove(SelectedPatient);
                PatientsDisplayList.Remove(SelectedPatient);

            }
        }

        private void OpenEditPatientForm(object obj)
        {
            if (obj is PatientDisplayModel)
            {
                var patient = (PatientDisplayModel)obj;
                PatientFormViewModel = new PatientFormViewModel(this, patient, _patientService, _mapper, _dateTimeService);
            }
        }

        private void PatientSearch()
        {
            PatientsDisplayList = new ObservableCollection<PatientDisplayModel>(PatientsList
               .Where(x => x.FullName.ToUpper().Contains(SearchText.ToUpper()) || 
               x.FullAddressWithPostCode.ToUpper().Contains(SearchText.ToUpper())));
            OnPropertyChanged(nameof(PatientsDisplayList));
        }

        private void ReloadPatientList(object obj)
        {
            PatientsDisplayList.Clear();
            PatientsList.Clear();
            Reservations.Clear();
            LoadPatients();
        }
        #endregion
    }
}
