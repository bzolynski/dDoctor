using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;

namespace WPFUi.ViewModels.AppointmentVMs
{
    public class AddAppointmentViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields

        private Specialization _selectedSpecialization;
        private Doctor _selectedDoctor;
        private DateTime _selectedDate;
        private Reservation _selectedReservation;
        private bool _canSelectDoctor;
        private bool _canSelectDate;
        private bool _canSelectHour;
        private string _datePickerText = "Select date";

        private readonly IDoctorService _doctorService;
        private readonly IScheduleService _scheduleService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IReservationService _reservationService;
        private readonly ISpecializationService _specializationService;

        #endregion

        // Bindings
        #region Bindings


        public DateTime Today => _dateTimeService.Now.Date;

        public PatientDisplayModel SelectedPatient => PatientPicker.SelectedPatient;
        public PatientPickerViewModel PatientPicker { get; set; }


        public IEnumerable<DateTime> AvaliebleDates { get; set; }
        public ObservableCollection<Schedule> Schedules { get; set; }
        public ObservableCollection<Specialization> Specializations { get; set; }
        public ObservableCollection<Reservation> Reservations { get; set; }
        public ObservableCollection<Doctor> Doctors { get; set; }

        public string DatePickerText
        {
            get { return _datePickerText; }
            set
            {
                _datePickerText = value;
                OnPropertyChanged(nameof(DatePickerText));

            }
        }


        public Specialization SelectedSpecialization
        {
            get { return _selectedSpecialization; }
            set
            {
                _selectedSpecialization = value;
                OnPropertyChanged(nameof(SelectedSpecialization));
                if (value != null)
                {
                    CanSelectDate = false;
                    CanSelectHour = false;

                    DatePickerText = "Select date";
                    SelectedReservation = null;
                    _selectedDate = Today;

                    CanSelectDoctor = true;

                    LoadDoctors(value.Id);
                }

            }
        }
        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
                if (value != null)
                {
                    CanSelectDate = true;
                    CanSelectHour = false;

                    DatePickerText = "Select date";
                    _selectedDate = Today;
                    SelectedReservation = null;

                    LoadSchedule(_selectedSpecialization.Id, value.Id);
                }

            }
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {

                if (SelectedSpecialization != null && SelectedDoctor != null)
                {
                    OnPropertyChanged(nameof(SelectedDate));
                    _selectedDate = value;
                    DatePickerText = value.Date.ToString("dd-MM-yyyy");
                    SelectedReservation = null;

                    CanSelectHour = true;
                    LoadReservations(_selectedSpecialization.Id, _selectedDoctor.Id, value);
                }
            }
        }

        public Reservation SelectedReservation
        {
            get { return _selectedReservation; }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }



        public bool CanSelectDoctor
        {
            get { return _canSelectDoctor; }
            set
            {
                _canSelectDoctor = value;

                OnPropertyChanged(nameof(CanSelectDoctor));
            }
        }
        public bool CanSelectDate
        {
            get { return _canSelectDate; }
            set
            {
                _canSelectDate = value;
                OnPropertyChanged(nameof(CanSelectDate));
            }
        }
        public bool CanSelectHour
        {
            get { return _canSelectHour; }
            set
            {
                _canSelectHour = value;
                OnPropertyChanged(nameof(CanSelectHour));
            }
        }


        #endregion


        // Commands
        #region Commands

        public ICommand AddAppointmentCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors

        public AddAppointmentViewModel(
            ISpecializationService specializationService,
            IDoctorService doctorService,
            IScheduleService scheduleService,
            IDateTimeService dateTimeService,
            IPatientService patientService,
            IReservationService reservationService,
            IMapper mapper)
        {
            _specializationService = specializationService;
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _dateTimeService = dateTimeService;
            _reservationService = reservationService;

            // TODO: Do smth with this - problem with updating datepicker when select "today date"
            _selectedDate = Today;
            PatientPicker = new PatientPickerViewModel(patientService, mapper);

            AddAppointmentCommand = new AsyncRelayCommand(AddAppointment, CanAddAppointment, (ex) => throw ex);

            PatientPicker.SelectedPatientChanged += PatientPicker_SelectedPatientChanged;

            LoadSpecializations();


        }



        #endregion

        // Mehtods
        #region Methods

        private void PatientPicker_SelectedPatientChanged()
        {
            OnPropertyChanged(nameof(SelectedPatient));
        }

        private void LoadSpecializations()
        {
            _specializationService.GetAll().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Specializations = new ObservableCollection<Specialization>(task.Result);
                    OnPropertyChanged(nameof(Specializations));
                }
            });
        }

        private bool CanAddAppointment(object obj)
        {
            if (SelectedSpecialization != null && SelectedDoctor != null && SelectedPatient != null && SelectedReservation != null)
                return true;
            return false;
        }

        private async Task AddAppointment(object obj)
        {
            await _reservationService.RegisterPatient(SelectedReservation, SelectedPatient.Id);
            ClearForm();
        }

        private void LoadDoctors(int specializationId)
        {
            _doctorService.GetManyBySpecialization(specializationId).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Doctors = new ObservableCollection<Doctor>(task.Result);
                    OnPropertyChanged(nameof(Doctors));
                }
            });
        }

        private void LoadSchedule(int specializationId, int doctorId)
        {
            _scheduleService.GetSchedulesBySpecializationAndDoctor(specializationId, doctorId).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Schedules = new ObservableCollection<Schedule>(task.Result);
                    OnPropertyChanged(nameof(Schedules));

                    AvaliebleDates = new HashSet<DateTime>(Schedules.Select(sc => sc.Date));
                    OnPropertyChanged(nameof(AvaliebleDates));
                }
            });
        }

        private void LoadReservations(int specializationId, int doctorId, DateTime selectedDate)
        {
            _scheduleService.GetSpecifiedReservations(doctorId, specializationId, selectedDate).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Reservations = new ObservableCollection<Reservation>(task.Result);
                    OnPropertyChanged(nameof(Reservations));
                }
            });
        }

        private void ClearForm()
        {
            CanSelectDate = false;
            CanSelectHour = false;
            CanSelectDoctor = false;

            SelectedSpecialization = null;
            SelectedDoctor = null;

            _selectedDate = Today;
            DatePickerText = "Select date";
            SelectedReservation = null;

        }
        #endregion
    }
}
