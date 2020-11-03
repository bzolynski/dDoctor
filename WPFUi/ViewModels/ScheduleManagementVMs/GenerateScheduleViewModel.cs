using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
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

namespace WPFUi.ViewModels.ScheduleManagementVMs
{
    public class GenerateScheduleViewModel : ViewModelBase 
    {
        private readonly IScheduleService _scheduleService;
        private readonly ISpecializationService _specializationService;

        // Private fields
        #region Private fields

        #endregion

        // Bindings
        #region Bindings

        public DoctorPickerModel SelectedDoctor => DoctorPicker.SelectedDoctor;
        public DoctorPickerViewModel DoctorPicker { get; set; }
        public ObservableCollection<Specialization> Specializations { get; set; }

        public Specialization SelectedSpecialization { get; set; }

        public List<DayOfWeek> SelectedDaysOfWeek { get; set; }

        public TimeSpan MaxTimePerPatient { get; set; }

        public List<TimeSpan> TimeIntervalsList { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        // TODO: Made custom datepicker and change those ugly things
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }


        #endregion

        // Commands
        #region Commands

        public ICommand SelectDaysOfWeekCommand { get; set; }
        public ICommand GenerateScheduleCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        public GenerateScheduleViewModel(IScheduleService scheduleService, IDoctorService doctorService, ISpecializationService specializationService, IMapper mapper)
        {

            if (SelectedDaysOfWeek == null)
                SelectedDaysOfWeek = new List<DayOfWeek>();
            DoctorPicker = new DoctorPickerViewModel(doctorService, mapper);

            DoctorPicker.PropertyChanged += DoctorPicker_SelectedDoctorChanged;

            SelectDaysOfWeekCommand = new RelayCommand(SelectDaysOfWeek);
            GenerateScheduleCommand = new AsyncRelayCommand(GenerateSchedule, (ex) => throw ex);
            _scheduleService = scheduleService;
            _specializationService = specializationService;

            TimeIntervalsList = new List<TimeSpan>
            {
                new TimeSpan(0, 10, 0),
                new TimeSpan(0, 15, 0),
                new TimeSpan(0, 20, 0),
                new TimeSpan(0, 30, 0)
            };

            LoadSpecializations();
        }

        #endregion

        // Methods
        #region Methods

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

        private async Task GenerateSchedule(object arg)
        {
            await _scheduleService.GenerateSchedules(SelectedDoctor.Id, SelectedSpecialization.Id, StartHour.TimeOfDay, EndHour.TimeOfDay, MaxTimePerPatient, StartDate, EndDate, SelectedDaysOfWeek);
        }

        private void SelectDaysOfWeek(object obj)
        {
            var parameters = (object[])obj;
            if (parameters[1] is DayOfWeek dayOfWeek)
            {
                if ((bool)parameters[0] == true)
                    SelectedDaysOfWeek.Add(dayOfWeek);
                else
                    SelectedDaysOfWeek.Remove(dayOfWeek);

            }
        }

        private void DoctorPicker_SelectedDoctorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedDoctor));
        }

        #endregion

    }
}
