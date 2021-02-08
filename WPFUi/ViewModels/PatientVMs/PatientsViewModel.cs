using Application.Services;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using WPFUi.Commands.Common;
using WPFUi.Models;
using WPFUi.Validators;

namespace WPFUi.ViewModels.PatientVMs
{
    public class PatientsViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields

        private PatientFormViewModel _patientFormViewModel;

        private Patient _selectedPatient;
        private List<Patient> _patients;
        private string _searchText = string.Empty;
        private bool _canDeletePatientBinding;
        private readonly IPatientService _patientService;
        private readonly IReservationService _reservationService;
        private readonly PatientFormValidator _validationRules;

        #endregion

        // Bindings
        #region Bindings

        public ICollectionView PatientsCollectionView { get; set; }
        public ObservableCollection<Reservation> Reservations { get; set; }

        public PatientFormViewModel PatientFormViewModel
        {
            get { return _patientFormViewModel; }
            set
            {
                _patientFormViewModel = value;
                OnPropertyChanged(nameof(PatientFormViewModel));
            }
        }

        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;

                OnPropertyChanged(nameof(SelectedPatient));

                if (value != null)
                    LoadReservations();
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                PatientsCollectionView.Refresh();
            }
        }

        // TODO: Find better name
        public bool CanDeletePatientBinding
        {
            get => _canDeletePatientBinding;
            set
            {
                _canDeletePatientBinding = value;
            }
        }


        #endregion

        // Commands
        #region Commands

        public ICommand OpenEditPatientFormCommand { get; set; }
        public ICommand OpenAddPatientFormCommand { get; set; }
        public ICommand DeletePatientCommand { get; set; }
        public ICommand ReloadPatientListCommand { get; set; }

        #endregion

        // Constructors
        #region Constructors
        public PatientsViewModel(IPatientService patientService, 
            IReservationService reservationService, 
            PatientFormValidator validationRules)
        {

            _patientService = patientService;
            _reservationService = reservationService;
            _validationRules = validationRules;

            _patients = new List<Patient>();
            PatientsCollectionView = CollectionViewSource.GetDefaultView(_patients);
            PatientsCollectionView.Filter = FilterPatients;
            PatientsCollectionView.SortDescriptions.Add(new SortDescription(nameof(Patient.LastName), ListSortDirection.Ascending));

            OpenEditPatientFormCommand = new RelayCommand(OpenEditPatientForm, CanOpenEditPatientForm);
            OpenAddPatientFormCommand = new RelayCommand(OpenAddPatientForm);
            DeletePatientCommand = new AsyncRelayCommand(DeletePatient, CanDeletePatient, (ex) => throw ex);
            ReloadPatientListCommand = new RelayCommand(ReloadPatientList);

            LoadPatients();
        }

        


        #endregion

        // Methods
        #region Methods
        private void LoadPatients()
        {
            _patientService.GetAllPatients().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    foreach (var patient in task.Result)
                    {
                        _patients.Add(patient);
                    }

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => PatientsCollectionView.Refresh()));
                }
            });
        }

        private void LoadReservations()
        {
            _reservationService.GetManyByPatient(_selectedPatient.Id).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Reservations = new ObservableCollection<Reservation>(task.Result);
                    OnPropertyChanged(nameof(Reservations));
                }
            });
        }

        private bool CanDeletePatient(object obj = null)
        {
            // TODO: Check if patient has reservations or past visits
            if (SelectedPatient == null || Reservations?.Count > 0)
                CanDeletePatientBinding = false;
            else
                CanDeletePatientBinding = true;


            OnPropertyChanged(nameof(CanDeletePatientBinding));
            return _canDeletePatientBinding;
        }

        private async Task DeletePatient(object obj)
        {
            await _patientService.DeletePatient(_selectedPatient.Id);
            _patients.Remove(SelectedPatient);
            PatientsCollectionView.Refresh();
        }

        private void OpenAddPatientForm(object obj)
        {
            PatientFormViewModel = new PatientFormViewModel(_patientService, _validationRules);

            PatientFormViewModel.FormSubmited += PatientFormViewModel_FormSubmited;
        }



        private bool CanOpenEditPatientForm(object obj) => SelectedPatient == null ? false : true;
      

        private void OpenEditPatientForm(object obj)
        {

            PatientFormViewModel = new PatientFormViewModel(_selectedPatient, _patientService, _validationRules);

            PatientFormViewModel.FormSubmited += PatientFormViewModel_FormSubmited;

        }

        private bool FilterPatients(object obj)
        {
            if (obj is Patient patient)
                return 
                    patient.FirstName.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
                    patient.LastName.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
                    patient.Address.Street.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
                    patient.Address.City.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase);
            return false;
        }

        private void ReloadPatientList(object obj = null)
        {
            _patients?.Clear();
            Reservations?.Clear();
            LoadPatients();
        }

        private void PatientFormViewModel_FormSubmited() => PatientFormViewModel = null;
        #endregion
    }
}
