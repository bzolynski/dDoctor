using Application.Services;
using Application.Services.PatientServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;
using WPFUi.Validators;

namespace WPFUi.ViewModels.PatientVMs
{
    public class PatientFormViewModel : ViewModelBase, IDataErrorInfo
    {
        // Validation
        #region Validation       

        public Dictionary<string, string> ErrorCollection { get; set; } = new Dictionary<string, string>();

        private readonly PatientFormValidator _patientFormValidator;

        private bool _canSubmit;

        public string Error => null;
        
        public string this[string propertyName] 
        {
            get 
            {
                var errorList = _patientFormValidator.Validate(this).Errors;

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

        private readonly PatientDisplayModel _patient;
        private readonly IPatientService _patientService;
        private readonly IDateTimeService _dateTimeService;

        #endregion

        // Properties
        #region Properties

        

        public string LastName { get; set; }
        public string FirstName { get; set; }

        //public int? PhoneNumber { get; set; }

        private int? _phoneNumber;

        public string PhoneNumber
        {
            get { return _phoneNumber.ToString(); }
            set 
            {
                if (value == null)                
                    _phoneNumber = null;

                try
                {
                    _phoneNumber = int.Parse(value);
                }
                catch (Exception)
                {}
            }
        }


        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Comments { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string FlatNumber { get; set; }
        









        #endregion

        // Commands
        #region Commands

        public event Action FormSubmited;

        public ICommand SubmitFormCommand { get; set; }
        public ICommand CloseFormCommand { get; set; }

        

        #endregion

        // Constructors
        #region Constructors


        // For new
        public PatientFormViewModel(IPatientService patientService, IDateTimeService dateTimeService, PatientFormValidator patientFormValidator)
        {

            CloseFormCommand = new RelayCommand(CancelForm);
            _patientService = patientService;
            _dateTimeService = dateTimeService;
            _patientFormValidator = patientFormValidator;

            BirthDate = _dateTimeService.Now;

            SubmitFormCommand = new AsyncRelayCommand(SubmitNewPatientForm, CanSubmitForm, (ex) => { throw ex; });
        }

        //For edit
        public PatientFormViewModel(PatientDisplayModel patient, IPatientService patientService, IDateTimeService dateTimeService, PatientFormValidator validationRules) : this(patientService, dateTimeService, validationRules)
        {
            _patient = patient;


            LastName = patient.LastName;
            FirstName = patient.FirstName;
            PhoneNumber = patient.PhoneNumber.ToString();
            Email = patient.Email;
            Comments = patient.Comments;
            BirthDate = patient.BirthDate;
            PostCode = patient.Address.PostCode;
            City = patient.Address.City;
            Street = patient.Address.Street;
            BuildingNumber = patient.Address.BuildingNumber;
            FlatNumber = patient.Address.FlatNumber;

            SubmitFormCommand = new AsyncRelayCommand(SubmitEditPatientForm, CanSubmitForm, (ex) => { throw ex; });

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
                PhoneNumber = _phoneNumber,
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
                PhoneNumber = _phoneNumber,
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

        private bool CanSubmitForm(object obj)
        {
            return _canSubmit;

        }

        private void CancelForm(object obj)
        {
            FormSubmited?.Invoke();
        }

        #endregion
    }
}
