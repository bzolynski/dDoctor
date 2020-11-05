using System;
using WPFUi.States.Navigation;
using WPFUi.ViewModels;
using WPFUi.ViewModels.AppointmentVMs;
using WPFUi.ViewModels.PatientVMs;
using WPFUi.ViewModels.ScheduleManagementVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class RootViewModelFactory : IRootViewModelFactory
    {
        private readonly IViewModelFactory<HomeViewModel> _homeViewModelFactory;
        private readonly IViewModelFactory<PatientsViewModel> _patientsViewModelFactory;
        private readonly IViewModelFactory<AddAppointmentViewModel> _appointmenrViewModelFactory;
        private readonly IViewModelFactory<ManageSchedulesViewModel> _manageScheduleViewModelFactory;
        private readonly IViewModelFactory<GenerateScheduleViewModel> _generateScheduleViewModelFactory;
        private readonly IViewModelFactory<ViewAppointmentsViewModel> _viewAppointmentsViewModelFactory;

        public RootViewModelFactory(IViewModelFactory<HomeViewModel> homeViewModelFactory,
            IViewModelFactory<PatientsViewModel> patientsViewModelFactory, 
            IViewModelFactory<AddAppointmentViewModel> appointmenrViewModelFactory,
            IViewModelFactory<ManageSchedulesViewModel> manageScheduleViewModelFactory,
            IViewModelFactory<GenerateScheduleViewModel> generateScheduleViewModelFactory,
            IViewModelFactory<ViewAppointmentsViewModel> viewAppointmentsViewModelFactory)
        {
            _homeViewModelFactory = homeViewModelFactory;
            _patientsViewModelFactory = patientsViewModelFactory;
            _appointmenrViewModelFactory = appointmenrViewModelFactory;
            _manageScheduleViewModelFactory = manageScheduleViewModelFactory;
            _generateScheduleViewModelFactory = generateScheduleViewModelFactory;
            _viewAppointmentsViewModelFactory = viewAppointmentsViewModelFactory;
        }
        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Home:
                    return _homeViewModelFactory.CreateViewModel();
                case ViewType.Patients:
                    return _patientsViewModelFactory.CreateViewModel();
                case ViewType.AddAppointment:
                    return _appointmenrViewModelFactory.CreateViewModel();
                case ViewType.ManageSchedules:
                    return _manageScheduleViewModelFactory.CreateViewModel();
                case ViewType.GenerateSchedule:
                    return _generateScheduleViewModelFactory.CreateViewModel();
                case ViewType.ViewAppointments:
                    return _viewAppointmentsViewModelFactory.CreateViewModel();
                default:
                    throw new Exception(); // TODO: Custom exception
            }
        }
    }
}
