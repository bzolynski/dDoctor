using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public class HomeViewModelFactory : IViewModelFactory<HomeViewModel>
    {

        public HomeViewModel CreateViewModel()
        {
            return new HomeViewModel();
        }
    }
}
