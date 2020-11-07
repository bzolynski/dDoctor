using Application.Services.UserService;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public class ManageUsersViewModelFactory : IViewModelFactory<ManageUsersViewModel>
    {
        private readonly IAccountService _userService;

        public ManageUsersViewModelFactory(
            IAccountService userService)
        {
            _userService = userService;
        }

        public ManageUsersViewModel CreateViewModel()
        {
            return new ManageUsersViewModel(
                _userService);
        }
    }
}
