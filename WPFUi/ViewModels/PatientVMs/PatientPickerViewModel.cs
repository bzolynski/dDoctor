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
        
        private List<PatientDisplayModel> _patientsList;
        private string _searchText = string.Empty;
        private Patient _patient;

        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        #endregion

        // Bindings
        #region Bindings
        public ICollectionView PatientsCollectionView { get; }

        public PatientDisplayModel SelectedDisplayPatient { get; set; }

        public Patient Patient 
        { 
            get => _patient;
            set 
            {
                _patient = value;
                SelectedPatientChanged?.Invoke();
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

        public PatientPickerViewModel(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;

            _patientsList = new List<PatientDisplayModel>();

            PatientsCollectionView = CollectionViewSource.GetDefaultView(_patientsList);
            PatientsCollectionView.Filter = FilterPatients;
            PatientsCollectionView.SortDescriptions.Add(new SortDescription(nameof(PatientDisplayModel.LastName), ListSortDirection.Ascending));

            LoadPatientCommand = new AsyncRelayCommand(LoadPatient, (ex) => throw ex);
            LoadPatients();
        }

        #endregion

        // Methods
        #region Methods

        private async Task LoadPatient(object arg)
        {
            Patient = await _patientService.GetById(SelectedDisplayPatient.Id);
        }

        private void LoadPatients()
        {
            _patientService.GetAllPatients().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    foreach (var patient in task.Result)
                        _patientsList.Add(_mapper.Map<PatientDisplayModel>(patient));

                    PatientsCollectionView.Refresh();
                }
            });
        }

        private bool FilterPatients(object obj)
        {
            if (obj is PatientDisplayModel patient)
                return patient.FullName.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase);

            return false;
        }
        #endregion

    }
}
