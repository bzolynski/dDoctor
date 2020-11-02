using Domain.Entities.Common;
using System;

namespace Domain.Entities
{
    public class Patient : Person
    {
        // TODO: Add PESEL
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }

        public Address Address { get; set; }
        public int AddressId { get; set; }

        public Reservation ScheduleHour { get; set; }

    }
}
