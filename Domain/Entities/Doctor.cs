using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Doctor : Person
    {
        public int NPWZ { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
