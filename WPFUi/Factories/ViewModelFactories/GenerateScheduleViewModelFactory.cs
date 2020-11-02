using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.ViewModels.ScheduleManagementVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class GenerateScheduleViewModelFactory : IViewModelFactory<GenerateScheduleViewModel>
    {
        private readonly IScheduleService _scheduleService;
        private readonly IDoctorService _doctorService;
        private readonly ISpecializationService _specializationService;
        private readonly IMapper _mapper;

        public GenerateScheduleViewModelFactory(IScheduleService scheduleService, IDoctorService doctorService, ISpecializationService specializationService, IMapper mapper)
        {
            _scheduleService = scheduleService;
            _doctorService = doctorService;
            _specializationService = specializationService;
            _mapper = mapper;
        }
        public GenerateScheduleViewModel CreateViewModel()
        {
            return new GenerateScheduleViewModel(_scheduleService, _doctorService, _specializationService, _mapper);
        }
    }
}
