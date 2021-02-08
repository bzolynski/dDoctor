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
            //TODO: Better messages
            RuleFor(x => x.StartDate)
                .Cascade(CascadeMode.Stop)
                .Must(x => x >= DateTime.Today).WithMessage("Start date can't be earlier than today.");

            RuleFor(x => x.EndDate)
                .Cascade(CascadeMode.Stop)
                .Must((vm, x) => x >=vm.StartDate).WithMessage("End date must be later than start date.");

            RuleFor(x => x.EndTime)
                .Cascade(CascadeMode.Stop)
                .Must((vm, x) => x > vm.StartTime).WithMessage("End time must be later than start time")
                .NotNull();

            // TODO: Maximum value validation for time per patient
            RuleFor(x => x.MaxTimePerPatient)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Can not be empty")
                .Must(time => time.All(Char.IsNumber)).WithMessage("Must be valid number.");

            RuleFor(x => x.SelectedDaysOfWeek)
                .Cascade(CascadeMode.Stop)
                .Must(days => days?.Count > 0).WithMessage("At least one day must be selected");

            RuleFor(x => x.SelectedDoctor)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Select doctor");

            RuleFor(x => x.SelectedSpecialization)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Please select specialization");
        }
    }
}
