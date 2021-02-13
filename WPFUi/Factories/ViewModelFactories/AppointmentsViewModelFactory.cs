using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using AutoMapper;
using WPFUi.States.Navigation;
using WPFUi.ViewModels.AppointmentVMs;
using WPFUi.ViewModels.PatientVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class AppointmentsViewModelFactory : IViewModelFactory<AppointmentsViewModel>
    {
        private readonly IReservationService _reservationService;
        private readonly IRenavigator _homeRenavigator;
        private readonly IScheduleService _scheduleService;
        private readonly IPatientService _patientService;
        private readonly ISpecializationService _specializationService;
        private readonly IDoctorService _doctorService;

        public AppointmentsViewModelFactory(
            IReservationService reservationService,
            IRenavigator homeRenavigator,
            IScheduleService scheduleService,
            IPatientService patientService,
            ISpecializationService specializationService,
            IDoctorService doctorService)
        {
            _reservationService = reservationService;
            _homeRenavigator = homeRenavigator;
            _scheduleService = scheduleService;
            _patientService = patientService;
            _specializationService = specializationService;
            _doctorService = doctorService;
        }
        public AppointmentsViewModel CreateViewModel()
        {
            return new AppointmentsViewModel(
                _reservationService,
                _homeRenavigator,
                _scheduleService,
                _patientService,
                _specializationService,
                _doctorService
                );
        }
    }
}
