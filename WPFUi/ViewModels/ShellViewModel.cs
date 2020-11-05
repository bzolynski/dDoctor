using System;
using System.Windows.Input;
using WPFUi.Commands;
using WPFUi.Commands.Common;
using WPFUi.Factories.ViewModelFactories;
using WPFUi.States.Navigation;

namespace WPFUi.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        // Private fields
        #region Private fields

        private readonly INavigator _navigator;

        #endregion

        // States
        #region States

        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        #endregion


        // Bindings
        #region Bindings

        #endregion

        // Commands
        #region Commands

        public Action Close { get; set; }

        public ICommand UpdateCurrentViewModelCommand { get; set; }
        public ICommand CloseApplicationCommand { get; set; }
        #endregion


        // Constructors
        #region Constructors

        public ShellViewModel(INavigator navigator, IRootViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _navigator.StateChanged += Navigator_StateChanged;
            _navigator.CurrentViewModel = viewModelFactory.CreateViewModel(ViewType.Home);

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navigator, viewModelFactory);

            CloseApplicationCommand = new RelayCommand(CloseApplication);
        }

        #endregion

        // Methods
        #region Methods

        private void CloseApplication(object obj)
        {
            Close?.Invoke();
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        #endregion


    }
}
