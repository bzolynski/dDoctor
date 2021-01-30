using FluentValidation;
using System;
using System.Linq;
using WPFUi.ViewModels.PatientVMs;

namespace WPFUi.Validators
{
    public class PatientFormValidator : AbstractValidator<PatientFormViewModel>
    {        
        public PatientFormValidator()
        {
            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name can not be empty")
                .Must(BeValidName).WithMessage("Last name contains invalid characters")
                .Length(2, 50).WithMessage("Invalid lenght of last name");

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name can not be empty")
                .Must(BeValidName).WithMessage("First name contains invalid characters")
                .Length(2, 50).WithMessage("Invalid lenght of first name");

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop);
            // .Must(BeValidNumber).WithMessage("Invalid number");

            // TODO: Custom REGEX for email
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email").When(x => x.Email?.Length > 0);

            RuleFor(x => x.BirthDate)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Birth date can not be null")
                .Must(BeValidAge).WithMessage("Invalid birth date");

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

      

        private bool BeValidAge(DateTime date)
        {
            return date.Date <= DateTime.Now.Date;
        }

        private bool BeValidName(string name)
        {
            return name.All(Char.IsLetter);
        }

    }
}
