using Domain.Entities.Common;

namespace Domain.Entities
{
    public partial class Registrant : Person
    {
        public Account Account { get; set; }
        public int UserId { get; set; }


    }
}
