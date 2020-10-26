using Application.Services.PatientServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public class PatientsViewModelFactory : IViewModelFactory<PatientsViewModel>
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public PatientsViewModelFactory(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }
        public PatientsViewModel CreateViewModel()
        {
            return PatientsViewModel.LoadPatientsViewModel(_patientService, _mapper);
        }
    }
}
