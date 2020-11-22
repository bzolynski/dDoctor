using Application.Services.SpecializationServices;
using FluentValidation;
using System;
using System.Linq;
using WPFUi.ViewModels;

namespace WPFUi.Validators
{
    public class SpecializationFormValidator : AbstractValidator<SpecializationFormViewModel>
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationFormValidator(ISpecializationService specializationService)
        {
            _specializationService = specializationService;

            RuleFor(x => x.SpecializationCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(4)
                .Must(BeNumeric).WithMessage("It is not a valid number")
                .MustAsync(async (code, token) =>
                    await _specializationService.GetByCode(code) == null ? true : false)
                    .WithMessage("Code already exists.");


            RuleFor(x => x.SpecializationName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeLettersOnly)
                .MinimumLength(3)
                .MaximumLength(25)
                .MustAsync(async (name, token) => 
                    await _specializationService.GetByName(name) == null ? true : false)
                    .WithMessage("Name already exists.");
        }

        private bool BeLettersOnly(string name)
        {            
            return name.Any(Char.IsLetter);
        }

        private bool BeNumeric(string code)
        {
            return code.Any(Char.IsNumber);

        }
    }
}
