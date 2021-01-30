using FluentValidation;
using System;
using System.Linq;
using WPFUi.ViewModels.ScheduleManagementVMs;

namespace WPFUi.Validators
{
    public class GenerateScheduleValidator : AbstractValidator<GenerateScheduleViewModel>
    {
        public GenerateScheduleValidator()
        {
            RuleFor(x => x.StartDate)
                .Cascade(CascadeMode.Stop)
                .NotNull();

            RuleFor(x => x.EndDate)
                .Cascade(CascadeMode.Stop)
                .NotNull();

            RuleFor(x => x.StartHour)
                .Cascade(CascadeMode.Stop)
                .NotNull();

            RuleFor(x => x.EndHour)
                .Cascade(CascadeMode.Stop)
                .NotNull();

            // TODO: Maximum value validation for time per patient
            RuleFor(x => x.MaxTimePerPatient)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Can not be empty")
                .Must(time => time.All(Char.IsNumber)).WithMessage("Must be valid number.");

            RuleFor(x => x.SelectedDaysOfWeek)
                .Cascade(CascadeMode.Stop)
                .Must(days => days.Count > 0).WithMessage("At least one day must be selected");

            RuleFor(x => x.SelectedDoctor)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Select doctor");

            RuleFor(x => x.SelectedSpecialization)
                .Cascade(CascadeMode.Stop)
                .NotNull();
        }
    }
}
