using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using AutoMapper;
using WPFUi.States.Navigation;
using WPFUi.ViewModels.AppointmentVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class AddAppointmentViewModelFactory : IViewModelFactory<AddAppointmentViewModel>
    {
        private readonly ISpecializationService _specializationService;
        private readonly IDoctorService _doctorService;
        private readonly IScheduleService _scheduleService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IPatientService _patientService;
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;
        private readonly IRenavigator _renavigator;

        public AddAppointmentViewModelFactory(
            ISpecializationService specializationService, 
            IDoctorService doctorService, 
            IScheduleService scheduleService, 
            IDateTimeService dateTimeService, 
            IPatientService patientService,
            IReservationService reservationService,
            IMapper mapper,
            IRenavigator renavigator)
        {
            _specializationService = specializationService;
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _dateTimeService = dateTimeService;
            _patientService = patientService;
            _reservationService = reservationService;
            _mapper = mapper;
            _renavigator = renavigator;
        }
        public AddAppointmentViewModel CreateViewModel()
        {
            return new AddAppointmentViewModel(
                _specializationService, 
                _doctorService, 
                _scheduleService, 
                _dateTimeService, 
                _patientService,
                _reservationService,
                _mapper,
                _renavigator);
        }
    }
}
