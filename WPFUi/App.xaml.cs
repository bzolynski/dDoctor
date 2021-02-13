using Application;
using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistance;
using System.IO;
using System.Reflection;
using System.Windows;
using WPFUi.Factories.ViewModelFactories;
using WPFUi.States.Navigation;
using WPFUi.Validators;
using WPFUi.ViewModels;
using WPFUi.ViewModels.AppointmentVMs;
using WPFUi.ViewModels.PatientVMs;
using WPFUi.ViewModels.ScheduleManagementVMs;
using WPFUi.ViewModels.UserVMs;
using WPFUi.Views;

namespace WPFUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((hostContext, services) =>
                {

                    services.AddApplication();
                    services.AddPersistance(hostContext.Configuration);


                    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

                    services.AddSingleton<ShellView>();
                    services.AddSingleton<ShellViewModel>();
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<PatientsViewModel>();
                    services.AddSingleton<PatientPickerViewModel>();
                    services.AddSingleton<GenerateScheduleViewModel>();
                    services.AddSingleton<SpecializationFormViewModel>();

                    services.AddScoped<IRootViewModelFactory, RootViewModelFactory>();
                    services.AddScoped<IViewModelFactory<GenerateScheduleViewModel>, GenerateScheduleViewModelFactory>();
                    services.AddScoped<IViewModelFactory<ManageSchedulesViewModel>, ManageSchedulesViewModelFactory>();
                    services.AddScoped<IViewModelFactory<AppointmentsViewModel>, AppointmentsViewModelFactory>();
                    services.AddScoped<IViewModelFactory<HomeViewModel>, HomeViewModelFactory>();
                    services.AddScoped<IViewModelFactory<ManageUsersViewModel>, ManageUsersViewModelFactory>();
                    services.AddScoped<IViewModelFactory<PatientsViewModel>, PatientsViewModelFactory>();

                    // States
                    services.AddSingleton<INavigator, Navigator>();
                });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var window = _host.Services.GetRequiredService<ShellView>();

            window.DataContext = _host.Services.GetRequiredService<ShellViewModel>();

            window.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }


    }
}
