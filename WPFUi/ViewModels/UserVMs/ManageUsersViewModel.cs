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
using WPFUi.Models;
using WPFUi.Validators;

namespace WPFUi.ViewModels.UserVMs
{
    public class ManageUsersViewModel : ViewModelBase
    {

        // Private fields
        #region Private fields
        
        private List<AccountViewModel> _users;
        private AccountViewModel _selectedUser;
        private readonly UserFormValidator _userFormValidator;
        private readonly IAccountService _accountService;
        private UserFormViewModel _userFormViewModel;
        private string _searchText = string.Empty;


        #endregion

        // Properties
        #region Properties

        private bool _isUserFormClosed = true;

        public bool IsUserFormClosed
        {
            get { return _isUserFormClosed; }
            set
            {
                _isUserFormClosed = value;
                OnPropertyChanged(nameof(IsUserFormClosed));
            }
        }


        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                UsersCollectionView.Refresh();
            }
        }


        public UserFormViewModel UserFormViewModel
        {
            get { return _userFormViewModel; }
            set
            {
                _userFormViewModel = value;
                OnPropertyChanged(nameof(UserFormViewModel));
            }
        }


        public ICollectionView UsersCollectionView { get; set; }

        public AccountViewModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }


        #endregion

        // Commands
        #region Commands
        public ICommand OpenEditUserFormCommand { get; set; }
        public ICommand OpenNewUserFormCommand { get; set; }

        #endregion

        // Constructors
        #region Constructors

        public ManageUsersViewModel(IAccountService userService, UserFormValidator userFormValidator)
        {
            _userFormValidator = userFormValidator;
            _accountService = userService;

            OpenNewUserFormCommand = new RelayCommand(OpenNewUserForm, (obj) => _isUserFormClosed);
            OpenEditUserFormCommand = new RelayCommand(OpenEditUserForm, (obj) => _selectedUser != null && _isUserFormClosed);
            _users = new List<AccountViewModel>();
            UsersCollectionView = CollectionViewSource.GetDefaultView(_users);
            UsersCollectionView.SortDescriptions.Add(new SortDescription(nameof(AccountViewModel.LastName), ListSortDirection.Ascending));
            UsersCollectionView.Filter = FilterUsers;
            LoadUsers();
        }

        private bool FilterUsers(object obj)
        {
            if(obj is AccountViewModel model)
                return model.FullName.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase);

            return false;
        }

        private void OpenEditUserForm(object obj)
        {
            IsUserFormClosed = false;
            UserFormViewModel = new UserFormViewModel(_accountService, _userFormValidator, _users, _selectedUser);
            UserFormViewModel.UserFormSubmited += UserFormViewModel_UserFormSubmited;
        }

        private void OpenNewUserForm(object obj)
        {
            IsUserFormClosed = false;
            UserFormViewModel = new UserFormViewModel(_accountService, _userFormValidator, _users);
            UserFormViewModel.UserFormSubmited += UserFormViewModel_UserFormSubmited;
        }

        private void UserFormViewModel_UserFormSubmited()
        {
            UsersCollectionView.Refresh();
            UserFormViewModel = null;
            IsUserFormClosed = true;
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
                        _users.Add(new AccountViewModel(user));
                    }

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => UsersCollectionView.Refresh()));
                }
            });
        }



        



        #endregion

    }
}
