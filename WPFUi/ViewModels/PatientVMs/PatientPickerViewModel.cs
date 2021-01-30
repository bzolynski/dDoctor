using Application.Services.PatientServices;
using AutoMapper;
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

namespace WPFUi.ViewModels.PatientVMs
{
    public class PatientPickerViewModel : ViewModelBase
    {
        public event Action SelectedPatientChanged;

        // Private fields
        #region Private fields
        
        private List<Patient> _patientsList;
        private string _searchText = string.Empty;
        private Patient _selectedPatient;

        private readonly IPatientService _patientService;

        #endregion

        // Bindings
        #region Bindings
        public ICollectionView PatientsCollectionView { get; }

        public Patient SelectedDisplayPatient { get; set; }

        public Patient SelectedPatient 
        { 
            get => _selectedPatient;
            set 
            {
                if(value != _selectedPatient)
                {
                    _selectedPatient = value;
                    SelectedPatientChanged?.Invoke();
                }
                
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

        #endregion

        // Commands
        #region Commands

        public ICommand LoadPatientCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors

        public PatientPickerViewModel(IPatientService patientService)
        {
            _patientService = patientService;

            _patientsList = new List<Patient>();

            PatientsCollectionView = CollectionViewSource.GetDefaultView(_patientsList);
            PatientsCollectionView.Filter = FilterPatients;
            PatientsCollectionView.SortDescriptions.Add(new SortDescription(nameof(SelectedPatient.LastName), ListSortDirection.Ascending));

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
                        _patientsList.Add(patient);

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => PatientsCollectionView.Refresh()));
                }
            });
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
        #endregion

    }
}
