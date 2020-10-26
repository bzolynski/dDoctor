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

        public RootViewModelFactory(IViewModelFactory<HomeViewModel> homeViewModelFactory, IViewModelFactory<PatientsViewModel> patientsViewModelFactory)
        {
            _homeViewModelFactory = homeViewModelFactory;
            _patientsViewModelFactory = patientsViewModelFactory;
        }
        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Home:
                    return _homeViewModelFactory.CreateViewModel();
                case ViewType.Patients:
                    return _patientsViewModelFactory.CreateViewModel();
                default:
                    throw new Exception(); // TODO: Custom exception
            }
        }
    }
}
