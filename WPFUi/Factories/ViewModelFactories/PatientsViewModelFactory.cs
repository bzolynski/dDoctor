using Application.Services;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using AutoMapper;
using WPFUi.Validators;
using WPFUi.ViewModels.PatientVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class PatientsViewModelFactory : IViewModelFactory<PatientsViewModel>
    {
        private readonly IPatientService _patientService;
        private readonly IReservationService _reservationService;
        private readonly PatientFormValidator _validationRules;

        public PatientsViewModelFactory(
            IPatientService patientService, 
            IReservationService reservationService,
            PatientFormValidator validationRules)
        {
            _patientService = patientService;
            _reservationService = reservationService;
            _validationRules = validationRules;
        }
        public PatientsViewModel CreateViewModel()
        {
            return new PatientsViewModel(
                _patientService, 
                _reservationService,
                _validationRules);
        }
    }
}
