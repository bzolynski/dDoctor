using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public interface IViewModelFactory<TViewModel> where TViewModel : ViewModelBase
    {
        TViewModel CreateViewModel();
    }
}
