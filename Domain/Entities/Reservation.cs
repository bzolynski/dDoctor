using Domain.Entities.Common;
using System;

namespace Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public TimeSpan Hour { get; set; }
        
        public Patient Patient { get; set; }
        public int? PatientId { get; set; }

        public Schedule Schedule { get; set; }
        public int ScheduleId { get; set; }
    }
}
