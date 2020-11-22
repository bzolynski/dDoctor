using Domain.Entities;
using Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.UserService
{
    public interface IAccountService
    {
        Task<RegistrationResult> CreateUser(string userName, string email, string password, string confirmPassword, AccountType accountType, string firstName, string lastName, string NPWZ = null);
        Task<Account> Login(string username, string password);
        Task<IEnumerable<Account>> GetAllUsers(string username);
        Task<string> GenerateValidUserName(string firstName, string lastName);

    }
}
