using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Specialization : BaseEntity
    {
        public int Code { get; set; }
        public string Name { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
