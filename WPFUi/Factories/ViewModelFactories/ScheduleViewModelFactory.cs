using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public class ScheduleViewModelFactory : IViewModelFactory<ScheduleViewModel>
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IDoctorService _doctorService;
        private readonly IScheduleService _scheduleService;
        private readonly ISpecializationService _specializationService;
        private readonly IPatientService _patientService;

        public ScheduleViewModelFactory(IDateTimeService dateTimeService, IDoctorService doctorService, IScheduleService scheduleService, ISpecializationService specializationService, IPatientService patientService)
        {
            _dateTimeService = dateTimeService;
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _specializationService = specializationService;
            _patientService = patientService;
        }
        public ScheduleViewModel CreateViewModel()
        {
            return ScheduleViewModel.LoadScheduleViewModel(_dateTimeService, _doctorService, _scheduleService, _specializationService, _patientService);
        }
    }
}
