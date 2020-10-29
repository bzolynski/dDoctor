using Domain.Entities.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public DayOfTheWeek DayOfTheWeek { get; set; }
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public TimeSpan MaxTimePerPatient { get; set; }

        public Specialization Specialization { get; set; }
        public int SpecializationId { get; set; }
        public Doctor Doctor { get; set; }
        public int DoctorId { get; set; }

    }
}
