using Domain.Entities.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Patient : Person
    {
        // TODO: Add PESEL
        public int? PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Comments { get; set; }

        public Address Address { get; set; }
        public int AddressId { get; set; }

        public HashSet<Reservation> Reservations { get; set; }

    }
}
