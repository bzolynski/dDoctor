using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
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

            RuleFor(x => x.MaxTimePerPatient)
                .Cascade(CascadeMode.Stop)
                .NotNull();

            RuleFor(x => x.SelectedDaysOfWeek)
                .Cascade(CascadeMode.Stop)
                .NotNull();

            RuleFor(x => x.SelectedDoctor)
                .Cascade(CascadeMode.Stop)
                .NotNull();

            RuleFor(x => x.SelectedSpecialization)
                .Cascade(CascadeMode.Stop)
                .NotNull();
        }
    }
}
