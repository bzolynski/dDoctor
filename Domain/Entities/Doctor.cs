using Domain.Entities.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Doctor : Person
    {
        public int NPWZ { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
