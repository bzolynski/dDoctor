using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFUi.ViewModels;
using WPFUi.Views;

namespace WPFUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceProvider = new Startup().CreateServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var window = scope.ServiceProvider.GetRequiredService<ShellView>();

                window.DataContext = scope.ServiceProvider.GetRequiredService<ShellViewModel>();

                window.Show();
            }
        }
    }
}
