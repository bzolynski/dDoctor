using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public partial class Registrant : Person
    {
        public Account Account { get; set; }
        public int UserId { get; set; }


    }
}
