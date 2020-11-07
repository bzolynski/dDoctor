using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserService
{
    public interface IAccountService
    {
        public Task<RegistrationResult> CreateUser(string userName, string email, string password, string confirmPassword, AccountType accountType, Doctor doctor = null, Registrant registrant = null);

        public Task<Account> Login(string username, string password);
    }
}
