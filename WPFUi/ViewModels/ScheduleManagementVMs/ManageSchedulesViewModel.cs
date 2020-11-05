using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;
using WPFUi.States.Navigation;

namespace WPFUi.ViewModels.ScheduleManagementVMs
{
    public class ManageSchedulesViewModel : ViewModelBase
    {


        // Private fields
        #region Private fields

        private string _searchText;
        private DateTime _dateFrom;
        private DateTime _dateTo;

        private readonly IDoctorService _doctorService;
        private readonly IScheduleService _scheduleService;
        private readonly IMapper _mapper;
        private readonly IRenavigator _generateScheduleRenavigator;

        #endregion

        // Bindings
        #region Bindings
        public List<Doctor> Doctors { get; set; }
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
                    LoadSchedule();
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                DoctorSearch();
            }
        }

        

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set 
            { 
                _dateFrom = value; 
                if(_selectedDoctor != null)
                    LoadSchedule();
            }
        }


        public DateTime DateTo
        {
            get { return _dateTo; }
            set 
            { 
                _dateTo = value;
                if (_selectedDoctor != null)
                    LoadSchedule();
            }
        }


        #endregion

        // Commands
        #region Commands

        public ICommand DeleteSelectedScheduleDayCommand { get; set; }
        public ICommand GenerateNewScheduleCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        public ManageSchedulesViewModel(IDoctorService doctorService, IScheduleService scheduleService, IDateTimeService dateTimeService, IMapper mapper, IRenavigator generateScheduleRenavigator)
        {
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _mapper = mapper;
            _generateScheduleRenavigator = generateScheduleRenavigator;

            DeleteSelectedScheduleDayCommand = new AsyncRelayCommand(DeleteSelectedScheduleDay, (ex) => throw ex);
            GenerateNewScheduleCommand = new RelayCommand(GenerateNewSchedule);

            LoadDoctors();

            DateFrom = new DateTime(dateTimeService.Now.Year, dateTimeService.Now.Month, 1);
            DateTo = new DateTime(dateTimeService.Now.Year, dateTimeService.Now.Month, DateTime.DaysInMonth(dateTimeService.Now.Year, dateTimeService.Now.Month));
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
                    Doctors = new List<Doctor>(task.Result);
                    DoctorDisplayModels = new ObservableCollection<ManageScheduleDoctorModel>(_mapper.Map<ObservableCollection<ManageScheduleDoctorModel>>(task.Result));
                    OnPropertyChanged(nameof(DoctorDisplayModels));
                }
            });
        }

        private void LoadSchedule()
        {
            _scheduleService.GetSchedulesInSpecifiedDateRangeByDoctorId(_selectedDoctor.Id, _dateFrom, _dateTo).ContinueWith(task =>
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

        private void DoctorSearch()
        {
            DoctorDisplayModels = new ObservableCollection<ManageScheduleDoctorModel>(
                _mapper.Map<List<ManageScheduleDoctorModel>>(Doctors.Where(
                    x => x.FirstName.ToUpper().Contains(SearchText.ToUpper()) || 
                    x.LastName.ToUpper().Contains(SearchText.ToUpper()) || 
                    x.NPWZ.ToString().Contains(SearchText.ToUpper()))));

            OnPropertyChanged(nameof(DoctorDisplayModels));
        }

        private void GenerateNewSchedule(object obj)
        {
            _generateScheduleRenavigator.Renavigate();
        }
        #endregion

    }
}
