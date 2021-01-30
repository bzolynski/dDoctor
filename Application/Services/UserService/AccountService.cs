using AutoMapper.QueryableExtensions;
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


        public async Task<(RegistrationResult registrationResult, Account user)> EditUser(int userId, string userName, string email, string password, string confirmPassword, AccountType accountType, string firstName, string lastName, string NPWZ = null)
        {
            if (password != confirmPassword)
                return (RegistrationResult.PasswordsDoNotMatch, null);

            var accountByUsername = await _accountDataService.GetByUsername(userName);

            if (accountByUsername != null && accountByUsername.Id != userId)
                return (RegistrationResult.UsernameAlreadyExists, null);

            var accountByEmail = await _accountDataService.GetByEmail(email);

            if (accountByEmail != null && accountByEmail.Id != userId)
                return (RegistrationResult.EmailAlreadyExists, null);

            var account = await _accountDataService.Get(userId);

            account.Username = userName;
            account.Email = email;
            account.AccountType = accountType;

            switch (accountType)
            {
                // TODO: Admin acc
                case AccountType.Admin:
                    break;
                case AccountType.Doctor:
                    account.Doctor.FirstName = firstName;
                    account.Doctor.LastName = lastName;
                    account.Doctor.NPWZ = NPWZ;
                    break;
                case AccountType.Registrant:
                    account.Registrant.FirstName = firstName;
                    account.Registrant.LastName = lastName;
                    break;
                default:
                    throw new Exception();
            }

            var hashedPassword = _passwordHasher.HashPassword(account, password);

            account.PasswordHash = hashedPassword;

            var result = await _accountDataService.Update(userId, account);

            return (RegistrationResult.Success, result);

        }

        public async Task<(RegistrationResult registrationResult, Account user)> CreateUser(string userName, string email, string password, string confirmPassword, AccountType accountType, string firstName, string lastName, string NPWZ = null)
        {

            if (password != confirmPassword)
                return (RegistrationResult.PasswordsDoNotMatch, null);

            var accountByUsername = await _accountDataService.GetByUsername(userName);

            if (accountByUsername != null)
                return (RegistrationResult.UsernameAlreadyExists, null);

            var accountByEmail = await _accountDataService.GetByEmail(email);

            if (accountByEmail != null)
                return (RegistrationResult.EmailAlreadyExists, null);

            var account = new Account
            {
                Username = userName,
                Email = email,
                AccountType = accountType
            };

            switch (accountType)
            {
                // TODO: Admin acc
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

            var result = await _accountDataService.Create(account);

            return (RegistrationResult.Success, result);
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

        private async Task<RegistrationResult> ValidateInputs(string password, string confirmPassword, string userName, string email)
        {
            var result = RegistrationResult.Success;
            if (password != confirmPassword)
                result = RegistrationResult.PasswordsDoNotMatch;

            var accountByUsername = await _accountDataService.GetByUsername(userName);

            if (accountByUsername != null)
                result = RegistrationResult.UsernameAlreadyExists;

            var accountByEmail = await _accountDataService.GetByEmail(email);

            if (accountByEmail != null)
                result = RegistrationResult.EmailAlreadyExists;

            return result;
        }

        public async Task<IEnumerable<Account>> GetAllUsers()
        {
            return await _accountDataService.GetAll();
        }




    }
}
