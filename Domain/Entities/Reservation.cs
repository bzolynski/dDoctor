using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Reservation : BaseEntity
    {
        public DateTime Hour { get; set; }
        
        public Patient Patient { get; set; }
        public int PatientId { get; set; }

        public Schedule Schedule { get; set; }
        public int ScheduleId { get; set; }
    }
}
