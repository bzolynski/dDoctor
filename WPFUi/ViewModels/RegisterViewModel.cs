using Application.Services.PatientServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFUi.Commands.Common;

namespace WPFUi.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        //Private fields
        #region Private fields
        private ScheduleViewModel _scheduleViewModel;
        private readonly IPatientService _patientService;
        private Patient _selectedPatient;

        #endregion

        // Bindings
        #region Bindings
        public DateTime Date { get; set; }
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }
        public ObservableCollection<Patient> Patients { get; set; }


        #endregion

        // Commands
        #region Commands

        public ICommand CancelCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors
        public RegisterViewModel(ScheduleViewModel scheduleViewModel, IPatientService patientService)
        {
            _scheduleViewModel = scheduleViewModel;
            _patientService = patientService;
            Date = _scheduleViewModel.SelectedDate;
            
            
            CancelCommand = new RelayCommand(Cancel);
        }


        #endregion

        // Methods
        #region Methods
        private void Cancel(object obj)
        {
            CloseForm();
        }

        private void CloseForm()
        {
            _scheduleViewModel.RegisterViewModel = null;
        }

        private void LoadPatients()
        {
            _patientService.GetAllPatients().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Patients = new ObservableCollection<Patient>(task.Result);
                    OnPropertyChanged(nameof(Patients));
                }
            });
        }

        #endregion
    }
}
