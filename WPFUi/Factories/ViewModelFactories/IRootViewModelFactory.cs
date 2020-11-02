using WPFUi.States.Navigation;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public interface IRootViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
