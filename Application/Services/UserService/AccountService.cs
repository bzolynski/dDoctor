using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Persistance.Services.UserDataServices;
using System;
using System.Collections.Generic;
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

        public async Task<RegistrationResult> CreateUser(string userName, string email, string password, string confirmPassword, AccountType accountType, string firstName, string lastName, string NPWZ = null)
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
                    var doctor = new Doctor
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        NPWZ = NPWZ
                    };
                    account.Doctor = doctor;
                    break;
                case AccountType.Registrant:
                    var registrant = new Registrant
                    {
                        FirstName = firstName,
                        LastName = lastName
                    };
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
        
        public async Task<string> GenerateValidUserName(string firstName, string lastName)
        {
            string username = "";

            for (int i = 1; i < firstName.Length; i++)
            {
                username = $"{ firstName.Substring(0, i) }{ lastName }";

                if (await CheckIfUsernameValid(username))
                    break;

                if (i == firstName.Length)
                    for (int j = 0; j < 100; j++)
                    {
                        username = $"{ firstName }{ lastName }{j}";
                        if (await CheckIfUsernameValid(username))
                            break;
                    }

            }

            return username;
        }

        private async Task<bool> CheckIfUsernameValid(string username)
        {
            return await _accountDataService.GetByUsername(username.ToUpper()) == null ? true : false;
        }

        public async Task<IEnumerable<Account>> GetAllUsers(string username)
        {
            return await _accountDataService.GetAll();
        }
    }
}
