using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using AutoMapper;
using WPFUi.States.Navigation;
using WPFUi.Validators;
using WPFUi.ViewModels.ScheduleManagementVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class GenerateScheduleViewModelFactory : IViewModelFactory<GenerateScheduleViewModel>
    {
        private readonly IScheduleService _scheduleService;
        private readonly IDoctorService _doctorService;
        private readonly ISpecializationService _specializationService;
        private readonly IMapper _mapper;
        private readonly IRenavigator _manageSchedulesRenavigator;
        private readonly SpecializationFormValidator _specializationFormValidator;
        private readonly GenerateScheduleValidator _generateScheduleValidator;

        public GenerateScheduleViewModelFactory(
            IScheduleService scheduleService, 
            IDoctorService doctorService, 
            ISpecializationService specializationService, 
            IMapper mapper,
            IRenavigator manageSchedulesRenavigator,
            SpecializationFormValidator specializationFormValidator,
            GenerateScheduleValidator generateScheduleValidator)
        {
            _scheduleService = scheduleService;
            _doctorService = doctorService;
            _specializationService = specializationService;
            _mapper = mapper;
            _manageSchedulesRenavigator = manageSchedulesRenavigator;
            _specializationFormValidator = specializationFormValidator;
            _generateScheduleValidator = generateScheduleValidator;
        }
        public GenerateScheduleViewModel CreateViewModel()
        {
            return new GenerateScheduleViewModel(
                _scheduleService, 
                _doctorService,
                _specializationService, 
                _mapper,
                _manageSchedulesRenavigator,
                _specializationFormValidator,
                _generateScheduleValidator);
        }
    }
}
