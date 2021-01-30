using Domain.Entities;
using Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.UserService
{
    public interface IAccountService
    {
        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="userName">User's username</param>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <param name="confirmPassword">User's confirmed password</param>
        /// <param name="accountType">User's account type</param>
        /// <param name="firstName">User's first name</param>
        /// <param name="lastName">User's last name</param>
        /// <param name="NPWZ">(optional) Doctors's NPWZ</param>
        /// <returns>Created account object</returns>
        Task<(RegistrationResult registrationResult, Account user)> CreateUser(string userName, string email, string password, string confirmPassword, AccountType accountType, string firstName, string lastName, string NPWZ = null);
        
        /// <summary>
        /// Edit's user
        /// </summary>
        /// <param name="usersId">User's id</param>
        /// <param name="userName">User's username</param>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <param name="confirmPassword">User's confirmed password</param>
        /// <param name="accountType">User's account type</param>
        /// <param name="firstName">User's first name</param>
        /// <param name="lastName">User's last name</param>
        /// <param name="NPWZ">(optional) Doctors's NPWZ</param>
        /// <returns>Edited account object</returns>
        Task<(RegistrationResult registrationResult, Account user)> EditUser(int userId, string userName, string email, string password, string confirmPassword, AccountType accountType, string firstName, string lastName, string NPWZ = null);
        Task<Account> Login(string username, string password);
        Task<IEnumerable<Account>> GetAllUsers();
        Task<string> GenerateValidUserName(string firstName, string lastName);

    }
}
