using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
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

        private List<Schedule> _schedules;
        private string _searchText = string.Empty;
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private List<DoctorModel> _doctors;
        private DoctorModel _selectedDoctor;
        private Schedule _selectedSchedule;
        private bool _isGenerateScheduleViewVisible = false;

        private readonly IDoctorService _doctorService;
        private readonly IScheduleService _scheduleService;
        #endregion

        // Bindings
        #region Bindings

        public GenerateScheduleViewModel GenerateScheduleViewModel { get; set; }
        public ICollectionView DoctorsCollectionView { get; set; }
        public ICollectionView SchedulesCollectionView { get; set; }


        public bool IsGenerateScheduleViewVisible
        {
            get { return _isGenerateScheduleViewVisible; }
            set
            {
                _isGenerateScheduleViewVisible = value;
                OnPropertyChanged(nameof(IsGenerateScheduleViewVisible));
            }
        }


        public Schedule SelectedSchedule
        {
            get { return _selectedSchedule; }
            set
            {
                _selectedSchedule = value;
                OnPropertyChanged(nameof(SelectedSchedule));
            }
        }

        public DoctorModel SelectedDoctor
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
                DoctorsCollectionView.Refresh();
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

        public ICommand EditSelectedScheduleDayCommand { get; set; }
        public ICommand DeleteSelectedScheduleDayCommand { get; set; }
        public ICommand GenerateNewScheduleCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        public ManageSchedulesViewModel(
            IDoctorService doctorService,
            IScheduleService scheduleService,
            GenerateScheduleViewModel generateScheduleViewModel)
        {
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            GenerateScheduleViewModel = generateScheduleViewModel;

            _doctors = new List<DoctorModel>();
            DoctorsCollectionView = CollectionViewSource.GetDefaultView(_doctors);
            DoctorsCollectionView.Filter = DoctorsFilter;
            DoctorsCollectionView.SortDescriptions.Add(new SortDescription(nameof(DoctorModel.LastName), ListSortDirection.Ascending));

            _schedules = new List<Schedule>();
            SchedulesCollectionView = CollectionViewSource.GetDefaultView(_schedules);
            EditSelectedScheduleDayCommand = new AsyncRelayCommand(EditSelectedScheduleDay, CanEditSelectedScheduleDay, (ex) => throw ex);
            DeleteSelectedScheduleDayCommand = new AsyncRelayCommand(DeleteSelectedScheduleDay, CanDeleteSelectedScheduleDay, (ex) => throw ex);
            GenerateNewScheduleCommand = new RelayCommand(GenerateNewSchedule);
            GenerateScheduleViewModel.FormSubmited += GenerateScheduleViewModel_FormSubmited;

            LoadDoctors();

            DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        private void GenerateScheduleViewModel_FormSubmited()
        {
            IsGenerateScheduleViewVisible = false;
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
                    foreach (var doctor in task.Result)
                        _doctors.Add(new DoctorModel(doctor));

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => DoctorsCollectionView.Refresh()));
                    
                }
            });
        }

        private void LoadSchedule()
        {
            _scheduleService.GetSchedulesInSpecifiedDateRangeByDoctorId(_selectedDoctor.Id, _dateFrom, _dateTo).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    _schedules.Clear();
                    foreach (var schedule in task.Result)
                        _schedules.Add(schedule);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => SchedulesCollectionView.Refresh()));


                }
            });
        }

        private bool CanEditSelectedScheduleDay(object obj)
        {
            return SelectedSchedule != null ? true : false;
        }

        private async Task EditSelectedScheduleDay(object arg)
        {

        }

        private bool CanDeleteSelectedScheduleDay(object obj)
        {
            return SelectedSchedule != null ? true : false;
        }

        private async Task DeleteSelectedScheduleDay(object obj)
        {
            if (SelectedSchedule != null)
            {
                await _scheduleService.ChangeSchedulesStatusToCanceled(SelectedSchedule.Id);
                _schedules.Remove(SelectedSchedule);

                SchedulesCollectionView.Refresh();
            }
        }

        private bool DoctorsFilter(object obj)
        {
            if (obj is DoctorModel doctor)
                return doctor.FullName.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) || doctor.NPWZ.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase);

            return false;
        }
               
        private void GenerateNewSchedule(object obj)
        {
            IsGenerateScheduleViewVisible = true;
        }
        #endregion

    }
}
