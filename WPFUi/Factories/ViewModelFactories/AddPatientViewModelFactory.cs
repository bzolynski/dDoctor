using Application.Services;
using Application.Services.PatientServices;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.States.Navigation;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public class AddPatientViewModelFactory : IViewModelFactory<AddPatientViewModel>
    {
        private readonly IPatientService _patientService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRenavigator _renavigator;

        public AddPatientViewModelFactory(IPatientService patientService, IDateTimeService dateTimeService, IRenavigator renavigator)
        {
            _patientService = patientService;
            _dateTimeService = dateTimeService;
            _renavigator = renavigator;
        }
        public AddPatientViewModel CreateViewModel()
        {
            return new AddPatientViewModel(_patientService, _dateTimeService, _renavigator);
        }
    }
}
