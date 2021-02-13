using Application.Services.SpecializationServices;
using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using WPFUi.ViewModels;

namespace WPFUi.Validators
{
    public class SpecializationFormValidator : AbstractValidator<SpecializationFormViewModel>
    {
        private readonly ISpecializationService _specializationService;
        private readonly Regex _onlyLetters = new Regex("^[A-Za-z ]+$");
        public SpecializationFormValidator(ISpecializationService specializationService)
        {
            _specializationService = specializationService;

            RuleFor(x => x.SpecializationCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Enter specialization code.")
                .Length(4).WithMessage("Code must be 4 characters long.")
                .Must(BeNumeric).WithMessage("It is not a valid number.")
                .MustAsync(async (code, token) =>
                    await _specializationService.GetByCode(code) == null ? true : false)
                    .WithMessage("Code already exists.");


            RuleFor(x => x.SpecializationName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Enter specialization name.")
                .Matches(_onlyLetters).WithMessage("Enter valid specialization name (letters only).")
                .MinimumLength(3).WithMessage("Must be at least 3 characters long")
                .MaximumLength(25).WithMessage("Must be at max 25 characters long")
                .MustAsync(async (name, token) => 
                    await _specializationService.GetByName(name) == null ? true : false)
                    .WithMessage("Name already exists.");
        }


        private bool BeNumeric(string code)
        {
            return code.All(Char.IsNumber);
        }
    }
}
