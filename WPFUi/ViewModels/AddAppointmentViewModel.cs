using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;

namespace WPFUi.ViewModels
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

        public Specialization SelectedSpecialization
        {
            get { return _selectedSpecialization; }
            set
            {
                _selectedSpecialization = value;
                OnPropertyChanged(nameof(SelectedSpecialization));
                if(value != null)
                {
                    CanSelectDate = false;
                    CanSelectHour = false;
                    DatePickerText = "Select date";
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
                    LoadSchedule(SelectedSpecialization.Id, value.Id);                   
                }

            }
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                    
                if (SelectedSpecialization != null && SelectedDoctor != null && value != _selectedDate)
                {
                    OnPropertyChanged(nameof(SelectedDate));
                    _selectedDate = value;
                    DatePickerText = value.Date.ToString("dd-MM-yyyy");
                    CanSelectHour = true;
                    LoadReservations(SelectedSpecialization.Id, SelectedDoctor.Id, value);    
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
        public string DatePickerText
        {
            get { return _datePickerText; }
            set
            {
                _datePickerText = value;
                OnPropertyChanged(nameof(DatePickerText));

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


        #endregion

        // Constructors
        #region Constructors

        private AddAppointmentViewModel(ISpecializationService specializationService, IDoctorService doctorService, IScheduleService scheduleService, IDateTimeService dateTimeService, IPatientService patientService, IMapper mapper)
        {
            _specializationService = specializationService;
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _dateTimeService = dateTimeService;

            _selectedDate = Today;
            PatientPicker = PatientPickerViewModel.LoadPatientPickerViewModel(patientService, mapper);


            PatientPicker.SelectedPatientChanged += PatientPicker_SelectedPatientChanged;

        }

        

        public static AddAppointmentViewModel LoadAddAppointmentViewModel(ISpecializationService specializationService, IDoctorService doctorService, IScheduleService scheduleService, IDateTimeService dateTimeService, IPatientService patientService, IMapper mapper)
        {
            var addAppointmentViewModel = new AddAppointmentViewModel(specializationService, doctorService, scheduleService, dateTimeService, patientService, mapper);

            addAppointmentViewModel.LoadSpecializations();

            return addAppointmentViewModel;
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
        #endregion
    }
}
