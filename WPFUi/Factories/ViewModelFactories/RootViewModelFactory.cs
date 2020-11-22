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
        private readonly IViewModelFactory<ManageSchedulesViewModel> _manageScheduleViewModelFactory;
        private readonly IViewModelFactory<GenerateScheduleViewModel> _generateScheduleViewModelFactory;
        private readonly IViewModelFactory<AppointmentsViewModel> _viewAppointmentsViewModelFactory;
        private readonly IViewModelFactory<ManageUsersViewModel> _manageUsersViewModelFactory;

        public RootViewModelFactory(
            IViewModelFactory<HomeViewModel> homeViewModelFactory,
            IViewModelFactory<PatientsViewModel> patientsViewModelFactory, 
            IViewModelFactory<ManageSchedulesViewModel> manageScheduleViewModelFactory,
            IViewModelFactory<GenerateScheduleViewModel> generateScheduleViewModelFactory,
            IViewModelFactory<AppointmentsViewModel> viewAppointmentsViewModelFactory,
            IViewModelFactory<ManageUsersViewModel> manageUsersViewModelFactory)
        {
            _homeViewModelFactory = homeViewModelFactory;
            _patientsViewModelFactory = patientsViewModelFactory;
            _manageScheduleViewModelFactory = manageScheduleViewModelFactory;
            _generateScheduleViewModelFactory = generateScheduleViewModelFactory;
            _viewAppointmentsViewModelFactory = viewAppointmentsViewModelFactory;
            _manageUsersViewModelFactory = manageUsersViewModelFactory;
        }
        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Home:
                    return _homeViewModelFactory.CreateViewModel();
                case ViewType.Patients:
                    return _patientsViewModelFactory.CreateViewModel();
                case ViewType.ManageSchedules:
                    return _manageScheduleViewModelFactory.CreateViewModel();
                case ViewType.GenerateSchedule:
                    return _generateScheduleViewModelFactory.CreateViewModel();
                case ViewType.Appointments:
                    return _viewAppointmentsViewModelFactory.CreateViewModel();
                case ViewType.ManageUsers:
                    return _manageUsersViewModelFactory.CreateViewModel();


                default:
                    throw new Exception(); // TODO: Custom exception
            }
        }
    }
}
