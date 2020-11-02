using System;
using WPFUi.States.Navigation;
using WPFUi.ViewModels;
using WPFUi.ViewModels.ScheduleManagementVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class RootViewModelFactory : IRootViewModelFactory
    {
        private readonly IViewModelFactory<HomeViewModel> _homeViewModelFactory;
        private readonly IViewModelFactory<PatientsViewModel> _patientsViewModelFactory;
        private readonly IViewModelFactory<ScheduleViewModel> _scheduleViewModelFactory;
        private readonly IViewModelFactory<AddPatientViewModel> _addPatientViewModelFactory;
        private readonly IViewModelFactory<AddAppointmentViewModel> _appointmenrViewModelFactory;
        private readonly IViewModelFactory<ManageSchedulesViewModel> _manageScheduleViewModelFactory;
        private readonly IViewModelFactory<GenerateScheduleViewModel> _generateScheduleViewModelFactory;

        public RootViewModelFactory(IViewModelFactory<HomeViewModel> homeViewModelFactory,
            IViewModelFactory<PatientsViewModel> patientsViewModelFactory, 
            IViewModelFactory<ScheduleViewModel> scheduleViewModelFactory, 
            IViewModelFactory<AddPatientViewModel> addPatientViewModelFactory, 
            IViewModelFactory<AddAppointmentViewModel> appointmenrViewModelFactory,
            IViewModelFactory<ManageSchedulesViewModel> manageScheduleViewModelFactory,
            IViewModelFactory<GenerateScheduleViewModel> generateScheduleViewModelFactory)
        {
            _homeViewModelFactory = homeViewModelFactory;
            _patientsViewModelFactory = patientsViewModelFactory;
            _scheduleViewModelFactory = scheduleViewModelFactory;
            _addPatientViewModelFactory = addPatientViewModelFactory;
            _appointmenrViewModelFactory = appointmenrViewModelFactory;
            _manageScheduleViewModelFactory = manageScheduleViewModelFactory;
            _generateScheduleViewModelFactory = generateScheduleViewModelFactory;
        }
        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Home:
                    return _homeViewModelFactory.CreateViewModel();
                case ViewType.Patients:
                    return _patientsViewModelFactory.CreateViewModel();
                case ViewType.Schedule:
                    return _scheduleViewModelFactory.CreateViewModel();
                case ViewType.AddPatient:
                    return _addPatientViewModelFactory.CreateViewModel();
                case ViewType.AddAppointment:
                    return _appointmenrViewModelFactory.CreateViewModel();
                case ViewType.ManageSchedules:
                    return _manageScheduleViewModelFactory.CreateViewModel();
                case ViewType.GenerateSchedule:
                    return _generateScheduleViewModelFactory.CreateViewModel();
                default:
                    throw new Exception(); // TODO: Custom exception
            }
        }
    }
}
