using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;
using WPFUi.States.Navigation;
using WPFUi.Validators;

namespace WPFUi.ViewModels.ScheduleManagementVMs
{
    public class GenerateScheduleViewModel : ViewModelBase, IDataErrorInfo
    {
        // Validators
        #region Validators

        // TODO: Fix validation
        public Dictionary<string, string> ErrorCollection { get; set; } = new Dictionary<string, string>();

        private bool _canSubmit;

        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                var errorList = _generateScheduleValidator.Validate(this).Errors;

                _canSubmit = errorList.Count > 0 ? false : true;

                var error = errorList.FirstOrDefault(e => e.PropertyName == propertyName);

                if (ErrorCollection.ContainsKey(propertyName) && error != null)
                    ErrorCollection[propertyName] = error.ErrorMessage;
                else if (error != null)
                    ErrorCollection.Add(propertyName, error.ErrorMessage);
                else
                    ErrorCollection.Remove(propertyName);

                OnPropertyChanged(nameof(ErrorCollection));

                return error != null ? error.ErrorMessage : null;
            }
        }

        #endregion

        // Private fields
        #region Private fields

        private readonly IScheduleService _scheduleService;
        private readonly ISpecializationService _specializationService;
        private readonly IRenavigator _manageSchedulesRenavigator;
        private readonly GenerateScheduleValidator _generateScheduleValidator;
        private bool _isSpecializationFormVisible = false;

        #endregion

        // Bindings
        #region Bindings

        public DoctorPickerModel SelectedDoctor => DoctorPicker.SelectedDoctor;
        public DoctorPickerViewModel DoctorPicker { get; set; }
        public ObservableCollection<Specialization> Specializations { get; set; }

        public Specialization SelectedSpecialization { get; set; }
        public SpecializationFormViewModel SpecializationFormViewModel{ get; set; }

        public List<DayOfWeek> SelectedDaysOfWeek { get; set; }

        public TimeSpan MaxTimePerPatient { get; set; }

        public List<TimeSpan> TimeIntervalsList { get; set; }

        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }


        // TODO: Made custom datepicker and change those ugly things
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }


        public bool IsSpecializationFormVisible
        {
            get { return _isSpecializationFormVisible; }
            set
            {
                _isSpecializationFormVisible = value;
                OnPropertyChanged(nameof(IsSpecializationFormVisible));
            }
        }


        #endregion

        // Commands
        #region Commands

        public ICommand SelectDaysOfWeekCommand { get; set; }
        public ICommand GenerateScheduleCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ShowSpecializationFormCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        public GenerateScheduleViewModel(
            IScheduleService scheduleService, 
            IDoctorService doctorService, 
            ISpecializationService specializationService, 
            IMapper mapper,
            IRenavigator manageSchedulesRenavigator,
            SpecializationFormValidator specializationFormValidator, 
            GenerateScheduleValidator generateScheduleValidator)
        {

            _scheduleService = scheduleService;
            _specializationService = specializationService;
            _manageSchedulesRenavigator = manageSchedulesRenavigator;
            _generateScheduleValidator = generateScheduleValidator;
            SpecializationFormViewModel = new SpecializationFormViewModel(specializationService, specializationFormValidator);

            if (SelectedDaysOfWeek == null)
                SelectedDaysOfWeek = new List<DayOfWeek>();

            DoctorPicker = new DoctorPickerViewModel(doctorService, mapper);

            DoctorPicker.SelectedDoctorChanged += DoctorPicker_SelectedDoctorChanged;

            SelectDaysOfWeekCommand = new RelayCommand(SelectDaysOfWeek);
            CancelCommand = new RelayCommand(Cancel);
            GenerateScheduleCommand = new AsyncRelayCommand(GenerateSchedule, CanGenerateSchedule, (ex) => throw ex);
            ShowSpecializationFormCommand = new RelayCommand(ShowSpecializationForm, CanShowSpecializationFormCommand);

            SpecializationFormViewModel.SpecializationAdded += SpecializationFormViewModel_SpecializationAdded;
            SpecializationFormViewModel.SpecializationFormClosed += SpecializationFormViewModel_SpecializationFormClosed;

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

        private bool CanGenerateSchedule(object obj)
        {
            return _canSubmit;
        }

        private async Task GenerateSchedule(object arg)
        {
            await _scheduleService.GenerateSchedules(SelectedDoctor.Id, SelectedSpecialization.Id, StartHour.TimeOfDay, EndHour.TimeOfDay, MaxTimePerPatient, StartDate, EndDate, SelectedDaysOfWeek.ToList());
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

                OnPropertyChanged(nameof(SelectedDaysOfWeek));
            }
        }        

        private void Cancel(object obj)
        {
            _manageSchedulesRenavigator.Renavigate();
        }

        private void DoctorPicker_SelectedDoctorChanged()
        {
            OnPropertyChanged(nameof(SelectedDoctor));
        }

        private void SpecializationFormViewModel_SpecializationFormClosed()
        {
            IsSpecializationFormVisible = false;
        }

        private void SpecializationFormViewModel_SpecializationAdded()
        {
            LoadSpecializations();
            IsSpecializationFormVisible = false;
        }

        private bool CanShowSpecializationFormCommand(object obj)
        {
            return !IsSpecializationFormVisible;
        }

        private void ShowSpecializationForm(object obj)
        {
            IsSpecializationFormVisible = true;
        }

        #endregion

    }
}
