using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public AccountType AccountType { get; set; }


        public Registrant Registrant { get; set; }
        public Doctor Doctor { get; set; }
    }
}
