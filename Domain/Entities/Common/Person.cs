using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Common
{
    public class Person : BaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
