using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WPFUi.Commands;
using WPFUi.Factories.ViewModelFactories;
using WPFUi.States.Navigation;

namespace WPFUi.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        // Private fields
        private readonly INavigator _navigator;
        private bool _isExpandedNavigationVisible;
        private bool _isShortNavigationVisible;

        // States
        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        // Bindings
        
        public bool IsShortNavigationVisible
        {
            get { return _isShortNavigationVisible; }
            set
            {
                _isShortNavigationVisible = value;
                OnPropertyChanged(nameof(IsShortNavigationVisible));
            }
        }

        public bool IsExpandedNavigationVisible
        {
            get { return _isExpandedNavigationVisible; }
            set
            {
                _isExpandedNavigationVisible = value;
                OnPropertyChanged(nameof(IsExpandedNavigationVisible));
                IsShortNavigationVisible = !value;
                OnPropertyChanged(nameof(IsShortNavigationVisible));
            }
        }

        // Commands
        public ICommand UpdateCurrentViewModelCommand { get; set; }

        //Constructor
        public ShellViewModel(INavigator navigator, IRootViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _navigator.StateChanged += Navigator_StateChanged;
            _navigator.CurrentViewModel = viewModelFactory.CreateViewModel(ViewType.Home);

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navigator, viewModelFactory);
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            if (_navigator.CurrentViewModel is HomeViewModel)
                IsExpandedNavigationVisible = true;
            else
                IsExpandedNavigationVisible = false;
        }
    }
}
