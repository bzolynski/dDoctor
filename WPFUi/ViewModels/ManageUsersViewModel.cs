using Application.Services.UserService;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Validators;

namespace WPFUi.ViewModels
{
    public class ManageUsersViewModel : ViewModelBase, IDataErrorInfo
    {
        // Validation
        #region Validation
        private readonly UserFormValidator _userFormValidator;
        public string Error => null;

        public Dictionary<string, string> ErrorCollection { get; set; } = new Dictionary<string, string>();

        private bool _canSubmit;

        public string this[string propertyName]
        {
            get
            {

                var errorList = _userFormValidator.Validate(this).Errors;

                _canSubmit = errorList.Count > 0 ? false : true;

                var error = errorList.FirstOrDefault(e => e.PropertyName == propertyName);

                if (ErrorCollection.ContainsKey(propertyName) && error != null)
                    ErrorCollection[propertyName] = error.ErrorMessage;
                else if (error != null)
                    ErrorCollection.Add(propertyName, error.ErrorMessage);
                else
                    ErrorCollection.Remove(propertyName);

                OnPropertyChanged(nameof(ErrorCollection));

                return error != null ? error.ErrorMessage : null;
            }
        }


        #endregion

        // Private fields
        #region Private fields

        private string _registrationResult;
        private string _userName;
        private string _firstName;
        private string _lastName;
        private AccountType _accountType;
        private bool _isNPWZEnabled;
        private string _nPWZ;
        private string _password;
        private List<Account> _users;


        private readonly IAccountService _accountService;


        #endregion

        // Properties
        #region Properties

        public ICollectionView UsersCollectionView { get; set; }
        private Account _selectedUser;

        public Account SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }


        public string RegistrationResult
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
                GenerateUsername();
            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
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

        public string Email { get; set; }
        public string Password { 
            get => _password; 
            set 
            {
                _password = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            } 
        }
        public string ConfirmPassword { get; set; }

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
        public ICommand CreateUserCommand { get; set; }


        #endregion

        // Constructors
        #region Constructors

        public ManageUsersViewModel(IAccountService userService, UserFormValidator userFormValidator)
        {
            CreateUserCommand = new AsyncRelayCommand(CreateUser, CanCreateUser, (ex) => throw ex);
            _accountService = userService;
            _userFormValidator = userFormValidator;

            _users = new List<Account>();
            UsersCollectionView = CollectionViewSource.GetDefaultView(_users);

            LoadUsers();
        }

        private bool CanCreateUser(object obj)
        {
            return _canSubmit;
        }

        #endregion

        // Methods
        #region Methods

        private void LoadUsers()
        {
            _accountService.GetAllUsers().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    foreach (var user in task.Result)
                    {
                        _users.Add(user);
                    }

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => UsersCollectionView.Refresh()));
                }
            });
        }

        private async Task CreateUser(object obj)
        {
            RegistrationResult = (await _accountService.CreateUser(UserName, Email, Password, ConfirmPassword, SelectedAccountType, FirstName, LastName, NPWZ)).ToString();

        }

        private void GenerateUsername()
        {
            if (!String.IsNullOrWhiteSpace(FirstName) && !String.IsNullOrWhiteSpace(LastName))
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



        #endregion

    }
}
