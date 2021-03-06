﻿using Application.Services.UserService;
using WPFUi.Validators;
using WPFUi.ViewModels.UserVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class ManageUsersViewModelFactory : IViewModelFactory<ManageUsersViewModel>
    {
        private readonly IAccountService _userService;
        private readonly UserFormValidator _userFormValidator;

        public ManageUsersViewModelFactory(
            IAccountService userService,
            UserFormValidator userFormValidator)
        {
            _userService = userService;
            _userFormValidator = userFormValidator;
        }

        public ManageUsersViewModel CreateViewModel()
        {
            return new ManageUsersViewModel(
                _userService,
                _userFormValidator);
        }
    }
}
