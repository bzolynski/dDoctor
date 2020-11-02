using System.Windows.Input;
using WPFUi.Commands.Common;

namespace WPFUi.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {

        public ICommand OpenGitHubProfileCommand { get; set; }
        public ICommand UpdateCurrentViewModelCommand { get; set; }
        public HomeViewModel()
        {

            OpenGitHubProfileCommand = new RelayCommand(OpenGitHubProfile);
            //UpdateCurrentViewModelCommand = 
        }

        private void OpenGitHubProfile(object obj)
        {
            // TODO: Change this. Seems to be dangerous
            System.Diagnostics.Process.Start("cmd", "/C start" + " " + "http://www.github.com/bzolynski");

        }

    }
}
