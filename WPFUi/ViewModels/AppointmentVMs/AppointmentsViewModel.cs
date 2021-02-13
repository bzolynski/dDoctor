 using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands;
using WPFUi.Commands.Common;
using WPFUi.Models;
using WPFUi.States.Navigation;
using WPFUi.ViewModels.PatientVMs;

namespace WPFUi.ViewModels.AppointmentVMs
{
    public class AppointmentsViewModel : ViewModelBase
    {

        // Private fields
        #region Private fields

        private DateTime _selectedDate;
        private ReservationModel _selectedReservation;
        private ReservationDetailsViewModel _reservationDetails;
        private Doctor _selectedDoctor;

        private readonly IReservationService _reservationService;
        private readonly IRenavigator _homeRenavigator;
        private readonly IScheduleService _scheduleService;
        private readonly IPatientService _patientService;
        private readonly ISpecializationService _specializationService;
        private readonly IDoctorService _doctorService;


        #endregion

        // Bindings
        #region Bindings

        public ObservableCollection<ScheduleModel> ScheduleViewModels { get; set; }
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Specialization> Specializations { get; set; }
        public IEnumerable<DateTime> CanceledDates { get; set; }
        public IEnumerable<DateTime> FullDates { get; set; }
        public IEnumerable<DateTime> AvaliebleDates { get; set; }

        public ReservationDetailsViewModel ReservationDetails
        {
            get { return _reservationDetails; }
            set
            {
                _reservationDetails = value;
                OnPropertyChanged(nameof(ReservationDetails));
            }
        }


        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
                LoadSchedules();
            }
        }

        private Specialization _selectedSpecialization;

        public Specialization SelectedSpecialization
        {
            get { return _selectedSpecialization; }
            set
            {
                _selectedSpecialization = value;
                OnPropertyChanged(nameof(SelectedSpecialization));
                LoadSchedules();
            }
        }


        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                SelectedReservation = null;
                OnPropertyChanged(nameof(SelectedDate));
                LoadSchedules();
            }
        }
        public DateTime Today => DateTime.Today;

        public ReservationModel SelectedReservation
        {
            get { return _selectedReservation; }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }

        #endregion

        // Commands
        #region Commands

        /// <summary>
        /// Used to change selected reservation in items control in AppointmentsView.xaml
        /// </summary>
        public ICommand SelectReservationCommand { get; set; }
        public ICommand UnregisterPatientCommand { get; set; }
        public ICommand OpenReservationDetailsCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand SetSelectedDateToTodayCommand { get; set; }
        public ICommand SetSelectedDoctorToNullCommand { get; set; }
        public ICommand SetSelectedSpecializationToNullCommand { get; set; }

        #endregion

        // Constructors
        #region Constructors
        public AppointmentsViewModel(
            IReservationService reservationService,
            IRenavigator homeRenavigator,
            IScheduleService scheduleService,
            IPatientService patientService,
            ISpecializationService specializationService,
            IDoctorService doctorService)
        {
            _reservationService = reservationService;
            _homeRenavigator = homeRenavigator;
            _scheduleService = scheduleService;
            _patientService = patientService;
            _specializationService = specializationService;
            _doctorService = doctorService;

            SelectedDate = DateTime.Today;

            UnregisterPatientCommand = new AsyncRelayCommand(UnregisterPatient, (obj) => SelectedReservation != null, (ex) => throw ex);

            SelectReservationCommand = new RelayCommand(SelectReservation);
            OpenReservationDetailsCommand = new RelayCommand(OpenReservationDetails);

            CloseCommand = new RelayCommand((obj) => _homeRenavigator.Renavigate());

            SetSelectedDateToTodayCommand = new RelayCommand((obj) => SelectedDate = DateTime.Now);
            SetSelectedDoctorToNullCommand = new RelayCommand((obj) => SelectedDoctor = null);
            SetSelectedSpecializationToNullCommand = new RelayCommand((obj) => SelectedSpecialization = null);
            
            LoadSchedules();
            LoadDoctors();
            LoadSpecializations();

        }

        private async Task UnregisterPatient(object arg)
        {
            if (SelectedReservation != null)
                await _reservationService.CancelAppointment(_selectedReservation.reservation);

            LoadSchedules();
        }



        #endregion

        // Methods
        #region Methods

        private void OpenReservationDetails(object arg)
        {
            if (SelectedReservation != null)
            {
                ReservationDetails = new ReservationDetailsViewModel(SelectedReservation.Id, _reservationService, _patientService);

                ReservationDetails.DetailsClosed += ReservationDetails_DetailsClosed;
                ReservationDetails.ReservationSubmitted += ReservationDetails_ReservationSubmitted;
            }
        }

        private void ReservationDetails_ReservationSubmitted()
        {
            LoadSchedules();
            ReservationDetails = null;
        }

        private void ReservationDetails_DetailsClosed()
        {
            ReservationDetails = null;
        }

        private void SelectReservation(object obj)
        {
            if (obj is ReservationModel reservation)
            {
                SelectedReservation = reservation;
            }
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

        private void LoadDoctors()
        {
            _doctorService.GetAll().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Doctors = new ObservableCollection<Doctor>(task.Result);
                    OnPropertyChanged(nameof(Doctors));
                }
            });
        }

        private void LoadSchedules()
        {
            _scheduleService.GetSchedules(SelectedDate, SelectedSpecialization, SelectedDoctor).ContinueWith(task =>
            {
                if (task.Exception == null)
                {

                    ScheduleViewModels = new ObservableCollection<ScheduleModel>();

                    foreach (var schedule in task.Result)
                        ScheduleViewModels.Add(new ScheduleModel(schedule));
                    
                    OnPropertyChanged(nameof(ScheduleViewModels));
                    LoadAvaliebleDates();
                }
            });
        }

        private void LoadAvaliebleDates()
        {
            _scheduleService.GetSchedulesForDates().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    // TODO: Find better solution !!!!!!!!!!!!!!

                    if(SelectedDoctor != null && SelectedSpecialization != null)
                    {
                        AvaliebleDates = task.Result.Where(x => x.DoctorId == SelectedDoctor.Id && x.SpecializationId == SelectedSpecialization.Id && x.Status == ScheduleStatus.Ok).Select(x => x.Date).ToList();

                        FullDates = task.Result.Where(x => x.DoctorId == SelectedDoctor.Id && x.SpecializationId == SelectedSpecialization.Id && x.Status == ScheduleStatus.Full).Select(x => x.Date).ToList();

                        CanceledDates = task.Result.Where(x => x.DoctorId == SelectedDoctor.Id && x.SpecializationId == SelectedSpecialization.Id && x.Status == ScheduleStatus.Canceled).Select(x => x.Date).ToList();
                    }
                        
                    else if(SelectedDoctor != null)
                    {
                        AvaliebleDates = task.Result.Where(x => x.DoctorId == SelectedDoctor.Id && x.Status == ScheduleStatus.Ok).Select(x => x.Date).ToList();

                        FullDates = task.Result.Where(x => x.DoctorId == SelectedDoctor.Id && x.Status == ScheduleStatus.Full).Select(x => x.Date).ToList();

                        CanceledDates = task.Result.Where(x => x.DoctorId == SelectedDoctor.Id && x.Status == ScheduleStatus.Canceled).Select(x => x.Date).ToList();

                    }
                    else if(SelectedSpecialization != null)
                    {
                        AvaliebleDates = task.Result.Where(x => x.SpecializationId == SelectedSpecialization.Id && x.Status == ScheduleStatus.Ok).Select(x => x.Date).ToList();

                        FullDates = task.Result.Where(x => x.SpecializationId == SelectedSpecialization.Id && x.Status == ScheduleStatus.Full).Select(x => x.Date).ToList();

                        CanceledDates = task.Result.Where(x => x.SpecializationId == SelectedSpecialization.Id && x.Status == ScheduleStatus.Canceled).Select(x => x.Date).ToList();

                    }
                    else
                    {
                        AvaliebleDates = task.Result.Where(x => x.Status == ScheduleStatus.Ok).Select(x => x.Date).ToList();


                        FullDates = task.Result.Where(x => x.Status == ScheduleStatus.Full).Select(x => x.Date).ToList();

                        CanceledDates = task.Result.Where(x => x.Status == ScheduleStatus.Canceled).Select(x => x.Date).ToList();
                    }

                    
                    OnPropertyChanged(nameof(AvaliebleDates));
                    OnPropertyChanged(nameof(CanceledDates));
                    OnPropertyChanged(nameof(FullDates));

                }
            });
        }

        #endregion

    }
}
