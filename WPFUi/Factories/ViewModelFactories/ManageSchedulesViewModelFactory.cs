using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using AutoMapper;
using WPFUi.States.Navigation;
using WPFUi.ViewModels.ScheduleManagementVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class ManageSchedulesViewModelFactory : IViewModelFactory<ManageSchedulesViewModel>
    {
        private readonly IDoctorService _doctorService;
        private readonly IScheduleService _scheduleService;
        private readonly IMapper _mapper;
        private readonly IRenavigator _generateScheduleRenavigator;

        public ManageSchedulesViewModelFactory(
            IDoctorService doctorService, 
            IScheduleService scheduleService, 
            IMapper mapper,
            IRenavigator generateScheduleRenavigator)
        {
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _mapper = mapper;
            _generateScheduleRenavigator = generateScheduleRenavigator;
        }
        public ManageSchedulesViewModel CreateViewModel()
        {
            return new ManageSchedulesViewModel(
                _doctorService, 
                _scheduleService, 
                _mapper,
                _generateScheduleRenavigator);
        }
    }
}
