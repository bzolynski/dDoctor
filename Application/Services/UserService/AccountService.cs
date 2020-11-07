using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Persistance.Services.UserDataServices;
using System;
using System.Threading.Tasks;

namespace Application.Services.UserService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDataService _accountDataService;
        private readonly IPasswordHasher<Account> _passwordHasher;

        public AccountService(IAccountDataService accountDataService, IPasswordHasher<Account> passwordHasher)
        {
            _accountDataService = accountDataService;
            _passwordHasher = passwordHasher;
        }
        public async Task<RegistrationResult> CreateUser(string userName, string email, string password, string confirmPassword, AccountType accountType, Doctor doctor = null, Registrant registrant = null)
        {
            if (password != confirmPassword)
                return RegistrationResult.PasswordsDoNotMatch;

            var accountByUsername = await _accountDataService.GetByUsername(userName);

            if (accountByUsername != null)
                return RegistrationResult.UsernameAlreadyExists;

            var accountByEmail = await _accountDataService.GetByEmail(email);

            if (accountByEmail != null)
                return RegistrationResult.EmailAlreadyExists;

            var account = new Account
            {
                Username = userName,
                Email = email,
                AccountType = accountType
            };

            switch (accountType)
            {
                case AccountType.Admin:
                    break;
                case AccountType.Doctor:
                    account.Doctor = doctor;
                    break;
                case AccountType.Registrant:
                    account.Registrant = registrant;
                    break;
                default:
                    throw new Exception();
            }

            var hashedPassword = _passwordHasher.HashPassword(account, password);

            account.PasswordHash = hashedPassword;

            await _accountDataService.Create(account);

            return RegistrationResult.Success;
        }

        public async Task<Account> Login(string username, string password)
        {
            var account = await _accountDataService.GetByUsername(username);

            // TODO: Custom exception
            if (account == null)
                throw new Exception();

            var verificationResult = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, password);

            // TODO: Custom exception
            if (verificationResult != PasswordVerificationResult.Success)
                throw new Exception();

            return account;
        }


    }
}
