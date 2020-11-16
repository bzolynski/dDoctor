using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WPFUi.ViewModels;

namespace WPFUi.Validators
{
    public class SpecializationFormValidator : AbstractValidator<SpecializationFormViewModel>
    {
        public SpecializationFormValidator()
        {
            RuleFor(x => x.SpecializationCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(4)
                .Must(BeNumeric);

            RuleFor(x => x.SpecializationName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(BeLettersOnly)
                .MinimumLength(3)
                .MaximumLength(25);

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
