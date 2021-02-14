using Domain.Entities.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Specialization : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public HashSet<Schedule> Schedules { get; set; }
    }
}
