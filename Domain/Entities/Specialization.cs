using Domain.Entities.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Specialization : BaseEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
