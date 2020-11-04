using Domain.Entities;
using System;

namespace WPFUi.Models
{
    public class PatientDisplayModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Comments { get; set; }

        public string BirthDateString 
        {
            get { return BirthDate.ToString("dd'-'MM'-'yyyy"); }
        }


        public string FullName => $"{ LastName } { FirstName }";
        public string FullAddress => $"{ Address.City }, { Address.Street } { Address.BuildingNumber } { Address.FlatNumber } ";
        public string FullAddressWithPostCode => $"{ Address.PostCode } { Address.City }, { Address.Street } { Address.BuildingNumber } { Address.FlatNumber }";


        public DateTime BirthDate { get; set; }
        public Address Address { get; set; }


    }
}
