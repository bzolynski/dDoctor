using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;
using WPFUi.States.Navigation;

namespace WPFUi.ViewModels.ScheduleManagementVMs
{
    public class ManageSchedulesViewModel : ViewModelBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IScheduleService _scheduleService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMapper _mapper;
        private readonly IRenavigator _generateScheduleRenavigator;

        // Private fields
        #region Private fields

        #endregion

        // Bindings
        #region Bindings
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<ManageScheduleDoctorModel> DoctorDisplayModels { get; set; }
        public ObservableCollection<Schedule> Schedules { get; set; }

        public GenerateScheduleViewModel GenerateSchedule { get; set; }


        private ManageScheduleDoctorModel _selectedDoctor;

        private Schedule _selectedSchedule;

        public Schedule SelectedSchedule
        {
            get { return _selectedSchedule; }
            set
            {
                _selectedSchedule = value;
                OnPropertyChanged(nameof(SelectedSchedule));
            }
        }


        public ManageScheduleDoctorModel SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
                if (value != null)
                    LoadSchedule(value.Id, DateFrom, DateTo);
            }
        }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        #endregion

        // Commands
        #region Commands

        public ICommand DeleteSelectedScheduleDayCommand { get; set; }
        public ICommand GenerateNewScheduleCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        private ManageSchedulesViewModel(IDoctorService doctorService, IScheduleService scheduleService, IDateTimeService dateTimeService, IMapper mapper, IRenavigator generateScheduleRenavigator)
        {
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _dateTimeService = dateTimeService;
            _mapper = mapper;
            _generateScheduleRenavigator = generateScheduleRenavigator;
            DeleteSelectedScheduleDayCommand = new AsyncRelayCommand(DeleteSelectedScheduleDay, (ex) => throw ex);
            GenerateNewScheduleCommand = new RelayCommand(GenerateNewSchedule);
            DateFrom = new DateTime(dateTimeService.Now.Year, dateTimeService.Now.Month, 1);
            DateTo = new DateTime(dateTimeService.Now.Year, dateTimeService.Now.Month, DateTime.DaysInMonth(dateTimeService.Now.Year, dateTimeService.Now.Month));
        }



        public static ManageSchedulesViewModel LoadManageSchedulesViewModel(IDoctorService doctorService, IScheduleService scheduleService, IDateTimeService dateTimeService, IMapper mapper, IRenavigator generateScheduleRenavigator)
        {
            var manageSchedulesViewModel = new ManageSchedulesViewModel(doctorService, scheduleService, dateTimeService, mapper, generateScheduleRenavigator);

            manageSchedulesViewModel.LoadDoctors();

            return manageSchedulesViewModel;
        }

        #endregion

        // Methods
        #region Methods

        private void LoadDoctors()
        {
            _doctorService.GetDoctorsWhoHaveSchedule().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Doctors = new ObservableCollection<Doctor>(task.Result);
                    DoctorDisplayModels = new ObservableCollection<ManageScheduleDoctorModel>(_mapper.Map<ObservableCollection<ManageScheduleDoctorModel>>(task.Result));
                    OnPropertyChanged(nameof(DoctorDisplayModels));
                }
            });
        }

        private void LoadSchedule(int doctorId, DateTime dateFrom, DateTime dateTo)
        {
            _scheduleService.GetSchedulesInSpecifiedDateRangeByDoctorId(doctorId, dateFrom, dateTo).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Schedules = new ObservableCollection<Schedule>(task.Result);
                    OnPropertyChanged(nameof(Schedules));
                }
            });
        }

        private async Task DeleteSelectedScheduleDay(object obj)
        {
            if (SelectedSchedule != null)
            {

            }
        }

        private void GenerateNewSchedule(object obj)
        {
            _generateScheduleRenavigator.Renavigate();
        }
        #endregion

    }
}
