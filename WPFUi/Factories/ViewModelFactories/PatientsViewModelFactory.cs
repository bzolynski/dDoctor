using Application.Services;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using AutoMapper;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public class PatientsViewModelFactory : IViewModelFactory<PatientsViewModel>
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly IReservationService _reservationService;

        public PatientsViewModelFactory(IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService, IReservationService reservationService)
        {
            _patientService = patientService;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _reservationService = reservationService;
        }
        public PatientsViewModel CreateViewModel()
        {
            return new PatientsViewModel(_patientService, _mapper, _dateTimeService, _reservationService);
        }
    }
}
