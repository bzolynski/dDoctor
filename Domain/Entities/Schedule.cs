using Domain.Entities.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public DateTime Date { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public TimeSpan MaxTimePerPatient { get; set; }
        public ScheduleStatus Status { get; set; }

        public Specialization Specialization { get; set; }
        public int SpecializationId { get; set; }
        public Doctor Doctor { get; set; }
        public int DoctorId { get; set; }

    }
}
