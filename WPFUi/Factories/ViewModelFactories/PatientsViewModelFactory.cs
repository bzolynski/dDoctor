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
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly IReservationService _reservationService;
        private readonly PatientFormValidator _validationRules;

        public PatientsViewModelFactory(
            IPatientService patientService, 
            IMapper mapper, 
            IDateTimeService dateTimeService, 
            IReservationService reservationService,
            PatientFormValidator validationRules)
        {
            _patientService = patientService;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _reservationService = reservationService;
            _validationRules = validationRules;
        }
        public PatientsViewModel CreateViewModel()
        {
            return new PatientsViewModel(
                _patientService, 
                _mapper, 
                _dateTimeService, 
                _reservationService,
                _validationRules);
        }
    }
}
