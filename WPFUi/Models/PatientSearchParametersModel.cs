using System;
using System.Collections.Generic;
using System.Text;

namespace WPFUi.Models
{
    public class PatientSearchParametersModel : ObservableObject
    {
        // TODO: PESEL
        // public int PESEL { get; set; }

        // Private fields
        private string _lastName = "";
        private string _firstName = "";
        private string _city = "";
        private string _streetName = "";
        private int? _age = null;

        // Properties
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

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public string StreetName
        {
            get { return _streetName; }
            set
            {
                _streetName = value;
                OnPropertyChanged(nameof(StreetName));
            }
        }

        public int? Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

    }
}
