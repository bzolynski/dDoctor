using Application.Services;
using Application.Services.PatientServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.States.Navigation;

namespace WPFUi.ViewModels
{
    public class AddPatientViewModel : ViewModelBase
    {

        // Private fields
        #region Private fields

        private string _lastName;
        private string _firstName;
        private string _phoneNumber;
        private string _email;
        private DateTime _birthDate;
        private string _postCode;
        private string _city;
        private string _street;

        private readonly IPatientService _patientService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRenavigator _patientsRenavigator;

        #endregion

        // Bindings
        #region Bindings

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }       
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }        
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }        
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }        
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }        
        public string PostCode
        {
            get { return _postCode; }
            set
            {
                _postCode = value;
                OnPropertyChanged(nameof(PostCode));
            }
        }       
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }
        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                OnPropertyChanged(nameof(Street));
            }
        }


        #endregion

        // Commands
        #region Commands

        public ICommand SubmitFormCommand { get; set; }
        public ICommand CloseFormCommand { get; set; }

        #endregion

        // Constructors
        #region Constructors

        public AddPatientViewModel(IPatientService patientService, IDateTimeService dateTimeService, IRenavigator patientsRenavigator)
        {
            _patientService = patientService;
            _dateTimeService = dateTimeService;
            _patientsRenavigator = patientsRenavigator;

            BirthDate = _dateTimeService.Now;

            SubmitFormCommand = new AsyncRelayCommand(CreateNewPatient, (ex) => throw ex);

            CloseFormCommand = new RelayCommand(Close);
        }

        

        #endregion

        // Mehtods
        #region Methods

        private async Task CreateNewPatient(object obj)
        {
            var newPatient = await _patientService.CreatePatient(new Domain.Entities.Patient
            {
                LastName = LastName,
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

            ClearForm();

            _patientsRenavigator.Renavigate();

            
        }
        private void Close(object obj)
        {
            _patientsRenavigator.Renavigate();
        }
        private void ClearForm()
        {
            LastName = "";
            FirstName = "";
            PhoneNumber = "";
            Email = "";
            BirthDate = _dateTimeService.Now;
            PostCode = "";
            City = "";
            Street = "";
        }
        #endregion
    }
}
