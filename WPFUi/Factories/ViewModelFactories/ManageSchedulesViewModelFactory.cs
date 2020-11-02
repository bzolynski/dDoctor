using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly IDateTimeService _dateTimeService;

        public ManageSchedulesViewModelFactory(
            IDoctorService doctorService, 
            IScheduleService scheduleService, 
            IDateTimeService dateTimeService,
            IMapper mapper,
            IRenavigator generateScheduleRenavigator)
        {
            _doctorService = doctorService;
            _scheduleService = scheduleService;
            _dateTimeService = dateTimeService;
            _mapper = mapper;
            _generateScheduleRenavigator = generateScheduleRenavigator;
        }
        public ManageSchedulesViewModel CreateViewModel()
        {
            return ManageSchedulesViewModel.LoadManageSchedulesViewModel(
                _doctorService, 
                _scheduleService, 
                _dateTimeService, 
                _mapper,
                _generateScheduleRenavigator);
        }
    }
}
