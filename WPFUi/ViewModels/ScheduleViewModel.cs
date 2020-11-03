using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WPFUi.Commands.Common;

namespace WPFUi.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields
        private DateTime _selectedDate;
        private Specialization _selectedSpecialization;
        private Reservation _selectedReservation;
        private Doctor _selectedDoctor;

        private readonly IDoctorService _doctorService;
        private readonly IScheduleService _scheduleService;
        private readonly ISpecializationService _specializationService;
        private readonly IPatientService _patientService;

        #endregion

        // Bindings
        #region Bindings
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                UpdateSchedule();
            }
        }
        public Specialization SelectedSpecialization
        {
            get { return _selectedSpecialization; }
            set
            {
                _selectedSpecialization = value;
                OnPropertyChanged(nameof(SelectedSpecialization));
                UpdateSchedule();
            }
        }
        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
                UpdateSchedule();
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

        private RegisterViewModel _registerViewModel;

        public RegisterViewModel RegisterViewModel
        {
            get { return _registerViewModel; }
            set
            {
                _registerViewModel = value;
                OnPropertyChanged(nameof(RegisterViewModel));
            }
        }



        public ObservableCollection<Schedule> Schedules { get; set; }
        public ObservableCollection<Reservation> Reservations { get; set; }

        public ObservableCollection<Specialization> Specializations { get; set; }
        public ObservableCollection<Doctor> Doctors { get; set; }





        #endregion

        // Commands
        #region Commands

        public ICommand Register { get; set; }
        public ICommand UnRegister { get; set; }

        #endregion



        // Constructors
        #region Constructors
        public ScheduleViewModel(IDateTimeService dateTimeService, IDoctorService doctorService, IScheduleService scheduleService, ISpecializationService specializationService, IPatientService patientService)
        {
            _scheduleService = scheduleService;
            _doctorService = doctorService;
            _specializationService = specializationService;
            _patientService = patientService;
            SelectedReservation = new Reservation();
            Reservations = new ObservableCollection<Reservation>();
            SelectedDate = dateTimeService.Now;


            Register = new RelayCommand(TestRegister, CanTestRegister);

            LoadDoctors();
            LoadSpecializations();
        }        

        #endregion


        // Methods
        #region Methods


        private bool CanTestRegister(object obj)
        {

            if (SelectedReservation.Patient == null && Schedules != null)
                return true;
            return false;


        }

        private void TestRegister(object obj)
        {
            RegisterViewModel = new RegisterViewModel(this, _patientService);
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

        private void UpdateSchedule()
        {

            //_scheduleService.GetSchedule(SelectedDoctor?.Id, SelectedSpecialization?.Id, SelectedDate).ContinueWith(task =>
            //{
            //    if (task.Exception == null)
            //    {
            //        Schedules = new ObservableCollection<Schedule>(task.Result);
            //        OnPropertyChanged(nameof(Schedules));
            //        var cos = task.Result.FirstOrDefault();
            //        if (Schedules.Count > 0)
            //            Reservations = new ObservableCollection<Reservation>(cos.Reservations);
            //        else
            //            Reservations.Clear();

            //        OnPropertyChanged(nameof(Reservations));
            //    }
            //});





        }
        #endregion
    }
}
