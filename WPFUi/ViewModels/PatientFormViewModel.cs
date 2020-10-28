using Application.Services;
using Application.Services.PatientServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;

namespace WPFUi.ViewModels
{
    public class PatientFormViewModel : ViewModelBase
    {
        private readonly PatientsViewModel _patientsViewModel;
        private readonly PatientDisplayModel _patient;
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;


        // Bindings
        #region Bindings
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        #endregion

        // Commands
        #region Commands
        public ICommand SubmitFormCommand{ get; set; }
        public ICommand CancelFormCommand { get; set; }

        #endregion

        // Constructors
        #region Constructors
        private PatientFormViewModel(IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService)
        {
            CancelFormCommand = new RelayCommand(CancelForm);
            _patientService = patientService;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }

        public PatientFormViewModel(PatientsViewModel patientsViewModel, IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService) : this(patientService, mapper, dateTimeService)
        {
            _patientsViewModel = patientsViewModel;

            BirthDate = _dateTimeService.Now;

            SubmitFormCommand = new AsyncRelayCommand(SubmitNewPatientForm, (ex) => { throw ex; });
        }

        public PatientFormViewModel(PatientsViewModel patientsViewModel, PatientDisplayModel patient, IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService) : this(patientService, mapper, dateTimeService)
        {
            _patientsViewModel = patientsViewModel;
            _patient = patient;


            LastName = patient.LastName;
            FirstName = patient.FirstName;
            PhoneNumber = patient.PhoneNumber;
            Email = patient.Email;
            BirthDate = patient.BirthDate;
            PostCode = patient.Address.PostCode;
            City = patient.Address.City;
            Street = patient.Address.Street;

            SubmitFormCommand = new AsyncRelayCommand(SubmitEditPatientForm, (ex) => { throw ex; });

        }

        #endregion

        // Methods
        #region Methods
        private async Task SubmitNewPatientForm()
        {
            var newPatient = await _patientService.CreatePatient(new Domain.Entities.Patient
            {
                Lastname = LastName,
                FirstName = FirstName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                BirthDate = BirthDate,
                Address = new Domain.Entities.Address
                {
                    City = City,
                    PostCode = PostCode,
                    Street = Street
                }
            });

            _patientsViewModel.PatientsList.Add(_mapper.Map<PatientDisplayModel>(newPatient));
            _patientsViewModel.PatientsDisplayList.Add(_mapper.Map<PatientDisplayModel>(newPatient));
            CloseForm();
        }

        private async Task SubmitEditPatientForm()
        {
            var editedPatient = await _patientService.UpdatePatient(_patient.Id, new Domain.Entities.Patient
            {
                Lastname = LastName,
                FirstName = FirstName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                BirthDate = BirthDate,
                Address = new Domain.Entities.Address
                {
                    City = City,
                    PostCode = PostCode,
                    Street = Street
                }
            });

            _patientsViewModel.PatientsList.Remove(_patient);
            _patientsViewModel.PatientsList.Add(_mapper.Map<PatientDisplayModel>(editedPatient)); 
            _patientsViewModel.PatientsDisplayList.Remove(_patient);
            _patientsViewModel.PatientsDisplayList.Add(_mapper.Map<PatientDisplayModel>(editedPatient));

            CloseForm();

        }


        private void CancelForm(object param)
        {
            CloseForm();
        }

        private void CloseForm()
        {
            _patientsViewModel.PatientFormViewModel = null;

        }

        #endregion
    }
}
