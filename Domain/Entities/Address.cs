using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }

        public Patient Patient { get; set; }

    }
}
