using Application.Services;
using Application.Services.PatientServices;
using AutoMapper;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public class PatientsViewModelFactory : IViewModelFactory<PatientsViewModel>
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateTimeService;
        private readonly IAgeService _ageService;

        public PatientsViewModelFactory(IPatientService patientService, IMapper mapper, IDateTimeService dateTimeService, IAgeService ageService)
        {
            _patientService = patientService;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _ageService = ageService;
        }
        public PatientsViewModel CreateViewModel()
        {
            return PatientsViewModel.LoadPatientsViewModel(_patientService, _mapper, _dateTimeService, _ageService);
        }
    }
}
