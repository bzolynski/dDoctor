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

        // States
        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        // Commands
        public ICommand UpdateCurrentViewModelCommand { get; set; }

        //Constructor
        public ShellViewModel(INavigator navigator, IRootViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _navigator.StateChanged += Navigator_StateChanged;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navigator, viewModelFactory);
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
