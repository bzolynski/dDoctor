using Application.Services.UserService;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;
using WPFUi.Validators;

namespace WPFUi.ViewModels.UserVMs
{
    public class UserFormViewModel : ViewModelBase, IDataErrorInfo
    {
        public event Action UserFormSubmited;


        // Validation
        #region Validation
        private readonly UserFormValidator _userFormValidator;
        

        public string Error => null;


        private bool _canSubmit;

        public string this[string propertyName]
        {
            get
            {
                var errorList = _userFormValidator.Validate(this).Errors;

                _canSubmit = errorList.Count > 0 ? false : true;

                var error = errorList.FirstOrDefault(e => e.PropertyName == propertyName);

                return error != null ? error.ErrorMessage : null;
            }
        }


        #endregion

        // Private fields
        #region Private fields
        private string _userName;
        private string _firstName;
        private string _lastName;
        private AccountType _accountType;
        private bool _isNPWZEnabled;
        private string _nPWZ;
        private string _password; 
        private string _email;
        private string _confirmPassword;
        private RegistrationResult _registrationResult;
        private readonly List<AccountModel> _users;
        private readonly IAccountService _accountService;
        private readonly AccountModel _selectedUser;
        #endregion

        // Properties
        #region Properties

        public RegistrationResult RegistrationResult
        {
            get { return _registrationResult; }
            set
            {
                _registrationResult = value;
                OnPropertyChanged(nameof(RegistrationResult));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                if(_lastName != _selectedUser?.LastName)
                    GenerateUsername();
            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                if(_firstName != _selectedUser?.FirstName)
                    GenerateUsername();
            }
        }
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public string NPWZ
        {
            get { return _nPWZ; }
            set
            {
                _nPWZ = value;
                OnPropertyChanged(nameof(NPWZ));
            }
        }


        public AccountType SelectedAccountType
        {
            get { return _accountType; }
            set
            {
                _accountType = value;

                switch (value)
                {
                    case AccountType.Admin:
                        NPWZ = null;
                        IsNPWZEnabled = false;
                        break;
                    case AccountType.Doctor:
                        IsNPWZEnabled = true;
                        NPWZ = "";
                        break;
                    case AccountType.Registrant:
                        NPWZ = null;
                        IsNPWZEnabled = false;
                        break;
                    default:
                        throw new Exception();
                        // TODO: Exception
                }
                OnPropertyChanged(nameof(SelectedAccountType));

            }
        }


        public bool IsNPWZEnabled
        {
            get { return _isNPWZEnabled; }
            set
            {
                _isNPWZEnabled = value;
                OnPropertyChanged(nameof(IsNPWZEnabled));
            }
        }
        #endregion

        // Commands
        #region Commands
        public ICommand SubmitUserFormCommand { get; set; }
        public ICommand CloseUserFormCommand { get; set; }

        #endregion

        // Constructors
        #region Constructors

        // For new
        public UserFormViewModel(IAccountService accountService, UserFormValidator userFormValidator, List<AccountModel> users) 
        {
            _userFormValidator = userFormValidator;
            _users = users;
            _accountService = accountService;

            SubmitUserFormCommand = new AsyncRelayCommand(CreateUser, (obj) => _canSubmit, (ex) => throw ex);
            CloseUserFormCommand = new RelayCommand(CloseUserForm);
        }

        // For edit
        public UserFormViewModel(IAccountService accountService, UserFormValidator userFormValidator, List<AccountModel> users, AccountModel selectedUser) : this(accountService, userFormValidator, users)
        {
            _selectedUser = selectedUser;

            SelectedAccountType = selectedUser.AccountType;
            LastName = selectedUser.LastName;
            FirstName = selectedUser.FirstName;
            UserName = selectedUser.Username;
            Email = selectedUser.Email;

            SubmitUserFormCommand = new AsyncRelayCommand(EditUser, (obj) => _canSubmit, (ex) => throw ex);
        }



        #endregion

        // Methods
        #region Methods

        private async Task EditUser(object arg)
        {
            Account user;
            (RegistrationResult, user) = await _accountService.EditUser(_selectedUser.Id, UserName, Email, Password, ConfirmPassword, SelectedAccountType, FirstName, LastName, NPWZ);

            if (user != null && RegistrationResult == RegistrationResult.Success)
            {
                _users.Remove(_selectedUser);
                _users.Add(new AccountModel(user));
                CloseUserForm();
            }
        }


        private async Task CreateUser(object obj)
        {
            Account user;
            (RegistrationResult, user) = await _accountService.CreateUser(UserName, Email, Password, ConfirmPassword, SelectedAccountType, FirstName, LastName, NPWZ);

            if (user != null && RegistrationResult == RegistrationResult.Success)
            {
                _users.Add(new AccountModel(user));
                CloseUserForm();
            }

        }

        private void GenerateUsername()
        {
            if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
            {
                _accountService.GenerateValidUserName(FirstName, LastName).ContinueWith(task =>
                {
                    if (task.Exception == null)
                    {
                        var result = task.Result;
                        if (result != null)
                            _userName = result.ToUpper();

                        OnPropertyChanged(nameof(UserName));
                    }
                });
            }
        }

        private void CloseUserForm(object obj = null)
        {
            ClearUserForm();
            UserFormSubmited?.Invoke();
        }

        private void ClearUserForm()
        {
            LastName = "";
            FirstName = "";
            UserName = "";
            Email = "";
            Password = "";
            ConfirmPassword = "";
        }
        #endregion

    }
}
