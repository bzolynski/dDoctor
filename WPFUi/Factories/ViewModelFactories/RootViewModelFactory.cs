using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.States.Navigation;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public class RootViewModelFactory : IRootViewModelFactory
    {
        private readonly IViewModelFactory<HomeViewModel> _homeViewModelFactory;
        private readonly IViewModelFactory<PatientsViewModel> _patientsViewModelFactory;
        private readonly IViewModelFactory<ScheduleViewModel> _scheduleViewModelFactory;

        public RootViewModelFactory(IViewModelFactory<HomeViewModel> homeViewModelFactory, IViewModelFactory<PatientsViewModel> patientsViewModelFactory, IViewModelFactory<ScheduleViewModel> scheduleViewModelFactory)
        {
            _homeViewModelFactory = homeViewModelFactory;
            _patientsViewModelFactory = patientsViewModelFactory;
            _scheduleViewModelFactory = scheduleViewModelFactory;
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
                default:
                    throw new Exception(); // TODO: Custom exception
            }
        }
    }
}
