using FluentValidation;
using System;
using System.Linq;
using WPFUi.ViewModels.UserVMs;

namespace WPFUi.Validators
{
    public class UserFormValidator : AbstractValidator<UserFormViewModel>
    {
        public UserFormValidator()
        {
            RuleFor(x => x.SelectedAccountType)
                .NotNull();

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name can not be empty")
                .Length(2, 50).WithMessage("Invalid lenght of last name")
                .Must(BeValidName).WithMessage("Last name contains invalid characters");

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name can not be empty")
                .Length(2, 50).WithMessage("Invalid lenght of first name")
                .Must(BeValidName).WithMessage("First name contains invalid characters");

            // TODO: Custom REGEX for email
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email can not be empty")
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email");

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
                .Must(BeNumeric).WithMessage("Invalid NPWZ number.").When(x => x.SelectedAccountType == Domain.Enums.AccountType.Doctor);
        }


        private bool BeNumeric(string npwz)
        {
            return npwz.All(Char.IsNumber);
        }

        private bool BeValidName(string name)
        {
            return name.All(Char.IsLetter);
        }
    }
}
