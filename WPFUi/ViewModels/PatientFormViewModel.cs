using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WPFUi.Models;

namespace WPFUi.ViewModels
{
    public class PatientFormViewModel : ViewModelBase
    {

        // Bindings
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }




        // Commands
        public ICommand SubmitFormCommand{ get; set; }
        public ICommand CancelFormCommand { get; set; }

        // Constructors
        public PatientFormViewModel()
        {

        }

        public PatientFormViewModel(PatientDisplayModel patient)
        {
            LastName = patient.LastName;
            FirstName = patient.FirstName;
            PhoneNumber = patient.PhoneNumber;
            Email = patient.Email;
            BirthDate = patient.BirthDate;
            PostCode = patient.Address.PostCode;
            City = patient.Address.City;
            Street = patient.Address.Street;
        }
    }
}
