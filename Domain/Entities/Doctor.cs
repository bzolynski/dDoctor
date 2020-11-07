﻿using Domain.Entities.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Doctor : Person
    {
        public int NPWZ { get; set; }
        public ICollection<Schedule> Schedules { get; set; }

        public Account Account { get; set; }
        public int UserId { get; set; }

    }
}
