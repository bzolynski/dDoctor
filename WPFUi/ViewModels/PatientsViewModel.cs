using Application.Services.PatientServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUi.Models;

namespace WPFUi.ViewModels
{
    public class PatientsViewModel : ViewModelBase
    {
        // Private fields
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        private PatientDisplayModel _selectedPatient;

        // Bindings
        public ObservableCollection<PatientDisplayModel> Patients { get; set; }
        public PatientFormViewModel PatientFormViewModel { get; set; } = new PatientFormViewModel();

        public PatientDisplayModel SelectedPatient
        {
            get { return _selectedPatient; }
            set 
            { 
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }


        // Constructor
        private PatientsViewModel(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        public static PatientsViewModel LoadPatientsViewModel(IPatientService patientService, IMapper mapper)
        {
            var patientsViewModel = new PatientsViewModel(patientService, mapper);

            patientsViewModel.LoadPatients();

            return patientsViewModel;
        }

        // Methods
        private void LoadPatients()
        {
            _patientService.GetAllPatients().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Patients = new ObservableCollection<PatientDisplayModel>(_mapper.Map<IEnumerable<PatientDisplayModel>>(task.Result));
                    OnPropertyChanged(nameof(Patients));
                   
                }
            });
        }
    }
}
