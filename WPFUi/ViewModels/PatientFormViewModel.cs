using Application.Services;
using Application.Services.PatientServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;

namespace WPFUi.ViewModels
{
    public class PatientFormViewModel : ViewModelBase
    {

        // Private fields
        #region Private fields

        private readonly PatientDisplayModel _patient;
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;

        #endregion

        // Properties
        #region Properties
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Comments { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }
        public int? FlatNumber { get; set; }

        #endregion

        // Commands
        #region Commands

        public event Action FormSubmited;

        public ICommand SubmitFormCommand{ get; set; }
        public ICommand CloseFormCommand { get; set; }

        #endregion

        // Constructors
        #region Constructors
       

        // For new
        public PatientFormViewModel(IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService)
        {

            CloseFormCommand = new RelayCommand(CancelForm);
            _patientService = patientService;
            _mapper = mapper;
            _dateTimeService = dateTimeService;

            BirthDate = _dateTimeService.Now;

            SubmitFormCommand = new AsyncRelayCommand(SubmitNewPatientForm, (ex) => { throw ex; });
        }

        //For edit
        public PatientFormViewModel(PatientDisplayModel patient, IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService) : this(patientService, mapper, dateTimeService)
        {
            _patient = patient;


            LastName = patient.LastName;
            FirstName = patient.FirstName;
            PhoneNumber = patient.PhoneNumber;
            Email = patient.Email;
            Comments = patient.Comments;
            BirthDate = patient.BirthDate;
            PostCode = patient.Address.PostCode;
            City = patient.Address.City;
            Street = patient.Address.Street;
            BuildingNumber = patient.Address.BuildingNumber;
            FlatNumber = patient.Address.FlatNumber;

            SubmitFormCommand = new AsyncRelayCommand(SubmitEditPatientForm, (ex) => { throw ex; });

        }

        #endregion

        // Methods
        #region Methods

        private async Task SubmitNewPatientForm(object obj)
        {
            var newPatient = await _patientService.CreatePatient(new Patient
            {
                LastName = LastName,
                FirstName = FirstName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                BirthDate = BirthDate,
                Comments = Comments,
                Address = new Address
                {
                    City = City,
                    PostCode = PostCode,
                    Street = Street,
                    BuildingNumber = BuildingNumber,
                    FlatNumber = FlatNumber
                }
            });

            FormSubmited?.Invoke();

        }

        private async Task SubmitEditPatientForm(object obj)
        {
            var editedPatient = await _patientService.UpdatePatient(_patient.Id, new Patient
            {
                LastName = LastName,
                FirstName = FirstName,
                Email = Email,
                PhoneNumber = PhoneNumber,
                BirthDate = BirthDate,
                Comments = Comments,
                Address = new Address
                {
                    City = City,
                    PostCode = PostCode,
                    Street = Street,
                    BuildingNumber = BuildingNumber,
                    FlatNumber = FlatNumber
                }
            });

            FormSubmited?.Invoke();
        }


        private void CancelForm(object obj)
        {
            FormSubmited?.Invoke();
        }

        #endregion
    }
}
