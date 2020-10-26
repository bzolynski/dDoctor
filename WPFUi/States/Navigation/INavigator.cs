using System;
using WPFUi.ViewModels;

namespace WPFUi.States.Navigation
{
    public interface INavigator
    {
        ViewModelBase CurrentViewModel { get; set; }

        event Action StateChanged;
    }
}