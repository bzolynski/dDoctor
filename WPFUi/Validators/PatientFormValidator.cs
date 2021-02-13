using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using WPFUi.ViewModels.PatientVMs;

namespace WPFUi.Validators
{
    public class PatientFormValidator : AbstractValidator<PatientFormViewModel>
    {
        // TODO: Make some kinde of Regex store
        private readonly Regex _validNameRegex = new Regex("^[a-zA-Z][a-zA-Z -]*[a-zA-Z]$");
        public PatientFormValidator()
        {
            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name can not be empty")
                .Matches(_validNameRegex).WithMessage("Last name contains invalid characters")
                .Length(2, 50).WithMessage("Invalid lenght of last name");

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name can not be empty")
                .Matches(_validNameRegex).WithMessage("First name contains invalid characters")
                .Length(2, 50).WithMessage("Invalid lenght of first name");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .Matches(new Regex("[0-9]*")).When(vm => vm.PhoneNumber.Length > 0).WithMessage("Invalid number");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .Matches(new Regex("^[a-zA-Z0-9.]+[@][a-z0-9.]{2,}[.][a-z]{2,4}$")).When(vm => vm.Email?.Length > 0).WithMessage("Invalid email");

            RuleFor(x => x.BirthDate)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Birth date can not be null")
                .Must(x => x.Date <= DateTime.Today.Date).WithMessage("Invalid birth date");

            RuleFor(x => x.Comments)
                .MaximumLength(250).WithMessage("Comment must be 250 characters at max");

            RuleFor(x => x.PostCode)
                .NotEmpty().WithMessage("Postcode can not be empty");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City can not be empty");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street can not be empty");

            RuleFor(x => x.BuildingNumber)
                .NotEmpty().WithMessage("Building number can not be empty")
                .MaximumLength(5).WithMessage("Too long");

            RuleFor(x => x.FlatNumber)
                .MaximumLength(5).WithMessage("Too long");

        }
    }
}
