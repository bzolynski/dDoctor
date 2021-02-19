using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.States.Navigation;
using WPFUi.Validators;

namespace WPFUi.ViewModels.ScheduleManagementVMs
{
    public class GenerateScheduleViewModel : ViewModelBase, IDataErrorInfo
    {

        // Validators
        #region Validators

        private bool _canSubmit;

        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                var errorList = _generateScheduleValidator.Validate(this).Errors;

                _canSubmit = errorList.Count > 0 ? false : true;

                var error = errorList.FirstOrDefault(e => e.PropertyName == propertyName);

                return error != null ? error.ErrorMessage : null;
            }
        }

        #endregion

        // Private fields
        #region Private fields

        private readonly IScheduleService _scheduleService;
        private readonly ISpecializationService _specializationService;
        private readonly GenerateScheduleValidator _generateScheduleValidator;

        private bool _isSpecializationFormVisible = false;
        private Specialization _selectedSpecialization;
        private TimeSpan _maxTimePerPatient = new TimeSpan(0, 15, 0);
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today.AddDays(1);

        #endregion

        // Bindings
        #region Bindings

        public Doctor SelectedDoctor => DoctorPicker.SelectedDoctor;
        public DoctorPickerViewModel DoctorPicker { get; set; }
        public ObservableCollection<Specialization> Specializations { get; set; }

        public Specialization SelectedSpecialization
        {
            get { return _selectedSpecialization; }
            set
            {
                _selectedSpecialization = value;
                OnPropertyChanged(nameof(SelectedSpecialization));
            }
        }

        public SpecializationFormViewModel SpecializationFormViewModel { get; set; }

        public List<DayOfWeek> SelectedDaysOfWeek { get; set; }

        public string MaxTimePerPatient
        {
            get => _maxTimePerPatient.ToString("%m");

            set
            {
                try
                {
                    _maxTimePerPatient = new TimeSpan(0, int.Parse(value), 0);

                }
                catch (Exception)
                { }
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public TimeSpan StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }
        
        public TimeSpan EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged(nameof(EndTime));
            }
        }

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

        public event Action FormSubmited;
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
            SpecializationFormValidator specializationFormValidator,
            GenerateScheduleValidator generateScheduleValidator)
        {
            _scheduleService = scheduleService;
            _specializationService = specializationService;
            _generateScheduleValidator = generateScheduleValidator;

            if (SelectedDaysOfWeek == null)
                SelectedDaysOfWeek = new List<DayOfWeek>();

            SetDefaultPropertyValues();

            DoctorPicker = new DoctorPickerViewModel(doctorService);
            SpecializationFormViewModel = new SpecializationFormViewModel(specializationService, specializationFormValidator);

            SelectDaysOfWeekCommand = new RelayCommand(SelectDaysOfWeek);
            CancelCommand = new RelayCommand(CloseForm);
            GenerateScheduleCommand = new AsyncRelayCommand(GenerateSchedule, (obj) => _canSubmit, (ex) => throw ex);
            ShowSpecializationFormCommand = new RelayCommand((obj) => IsSpecializationFormVisible = true, (obj) => !IsSpecializationFormVisible);


            DoctorPicker.SelectedDoctorChanged += () => OnPropertyChanged(nameof(SelectedDoctor));
            SpecializationFormViewModel.SpecializationAdded += SpecializationFormViewModel_SpecializationAdded;
            SpecializationFormViewModel.SpecializationFormClosed += () => IsSpecializationFormVisible = false;



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

        private void CloseForm(object obj)
        {
            SetDefaultPropertyValues();
            FormSubmited?.Invoke();
        }

        private async Task GenerateSchedule(object arg)
        {
            if (_maxTimePerPatient != null)
                await _scheduleService.GenerateSchedules(SelectedDoctor.Id, _selectedSpecialization.Id, _startTime, _endTime, _maxTimePerPatient, _startDate, _endDate, SelectedDaysOfWeek.ToList());
            SetDefaultPropertyValues();
            FormSubmited?.Invoke();
        }

        /// <summary>
        /// Adds selected days to SelectedDaysOfWeek list.
        /// </summary>
        /// <param name="obj">Array of bool and DayOfWeek. Bool value indicates the "check" value of combobox for corressponging DayOfWeek.</param>
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

        private void SpecializationFormViewModel_SpecializationAdded()
        {
            LoadSpecializations();
            IsSpecializationFormVisible = false;
        }

        // TODO: Do smth with this
        private void SetDefaultPropertyValues()
        {
            _maxTimePerPatient = new TimeSpan(0, 15, 0);
            OnPropertyChanged(nameof(MaxTimePerPatient));
            StartTime = new TimeSpan(8, 0, 0);
            EndTime = new TimeSpan(12, 00, 0);
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(1);
            SelectedSpecialization = null;

            if (DoctorPicker != null)
                DoctorPicker.SelectedDoctor = null;
            
    }
        #endregion

    }
}
