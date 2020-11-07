using Application.Services.UserService;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;

namespace WPFUi.ViewModels
{
    public class ManageUsersViewModel : ViewModelBase
    {

        // Private fields
        #region Private fields

        private string _registrationResult;

        private readonly IAccountService _accountService;


        #endregion

        // Properties
        #region Properties

        public string RegistrationResult
        {
            get { return _registrationResult; }
            set
            {
                _registrationResult = value;
                OnPropertyChanged(nameof(RegistrationResult));
            }
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public AccountType AccountType { get; set; }
        public Registrant Registrant { get; set; }
        public Doctor Doctor { get; set; }


        #endregion

        // Commands
        #region Commands
        public ICommand CreateUserCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors

        public ManageUsersViewModel(IAccountService userService)
        {
            CreateUserCommand = new AsyncRelayCommand(CreateUser, (ex) => throw ex);
            _accountService = userService;
        }

        #endregion

        // Methods
        #region Methods

        private async Task CreateUser(object arg)
        {
            RegistrationResult = (await _accountService.CreateUser(UserName, Email, Password, ConfirmPassword, AccountType, Doctor, Registrant)).ToString();

        }

  

        #endregion

    }
}
