using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public int BuildingNumber { get; set; }
        public int? FlatNumber { get; set; }

        public Patient Patient { get; set; }
    }
}
