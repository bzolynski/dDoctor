using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using WPFUi.ViewModels.UserVMs;

namespace WPFUi.Validators
{
    public class UserFormValidator : AbstractValidator<UserFormViewModel>
    {
        private readonly Regex _validNameRegex = new Regex("^[a-zA-Z][a-zA-Z -]*[a-zA-Z]$");

        public UserFormValidator()
        {
            RuleFor(x => x.SelectedAccountType)
                .NotNull();

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name can not be empty")
                .Length(2, 50).WithMessage("Invalid lenght of last name")
                .Matches(_validNameRegex).WithMessage("Last name contains invalid characters");

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name can not be empty")
                .Length(2, 50).WithMessage("Invalid lenght of first name")
                .Matches(_validNameRegex).WithMessage("First name contains invalid characters");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .Matches(new Regex("^[a-zA-Z0-9.]+[@][a-z0-9.]{2,}[.][a-z]{2,4}$")).When(vm => vm.Email?.Length > 0).WithMessage("Invalid email"); ;

            // TODO: Password validation
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");

            RuleFor(x => x.ConfirmPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.NPWZ)
                .NotEmpty().When(x => x.SelectedAccountType == Domain.Enums.AccountType.Doctor)
                .Length(7, 7).WithMessage("Must be 7 numbers long.").When(x => x.SelectedAccountType == Domain.Enums.AccountType.Doctor)
                .Matches(new Regex("[0-9]*")).WithMessage("Invalid NPWZ number.").When(x => x.SelectedAccountType == Domain.Enums.AccountType.Doctor);
        }
    }
}
