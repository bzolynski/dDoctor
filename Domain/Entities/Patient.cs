using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Patient : BaseEntity
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Address Address { get; set; }

        public int AddressId { get; set; }

    }
}
