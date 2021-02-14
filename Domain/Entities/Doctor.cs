using Domain.Entities.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Doctor : Person
    {
        public string NPWZ { get; set; }
        public HashSet<Schedule> Schedules { get; set; }

        public Account Account { get; set; }
        public int UserId { get; set; }

    }
}
