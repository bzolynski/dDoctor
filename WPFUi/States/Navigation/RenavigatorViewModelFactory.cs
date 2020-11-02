using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.Factories.ViewModelFactories;
using WPFUi.ViewModels;

namespace WPFUi.States.Navigation
{
    public class RenavigatorViewModelFactory<TViewModel> : IRenavigator where TViewModel : ViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly IViewModelFactory<TViewModel> _viewModelFactory;

        public RenavigatorViewModelFactory(INavigator navigator, IViewModelFactory<TViewModel> viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }
        public void Renavigate()
        {
            _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel();
        }
    }
}
