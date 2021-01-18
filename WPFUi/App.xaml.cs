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
                    services.AddSingleton<HomeView>();
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<PatientsView>();
                    services.AddSingleton<PatientsViewModel>();
                    services.AddSingleton<PatientPickerViewModel>();

                    services.AddSingleton<RenavigatorViewModelFactory<GenerateScheduleViewModel>>();
                    services.AddSingleton<RenavigatorViewModelFactory<ManageSchedulesViewModel>>();
                    services.AddSingleton<RenavigatorViewModelFactory<PatientsViewModel>>();
                    services.AddSingleton<RenavigatorViewModelFactory<HomeViewModel>>();
                    services.AddSingleton<RenavigatorViewModelFactory<AppointmentsViewModel>>();

                    services.AddScoped<IRootViewModelFactory, RootViewModelFactory>();
                    services.AddScoped<IViewModelFactory<HomeViewModel>, HomeViewModelFactory>();
                    services.AddScoped<IViewModelFactory<PatientsViewModel>, PatientsViewModelFactory>();
                    services.AddScoped<SpecializationFormViewModel>();

                    services.AddScoped<IViewModelFactory<AppointmentsViewModel>>(services => new AppointmentsViewModelFactory(
                            services.GetRequiredService<IReservationService>(),
                            services.GetRequiredService<IDateTimeService>(),
                            services.GetRequiredService<RenavigatorViewModelFactory<HomeViewModel>>(),
                            services.GetRequiredService<IScheduleService>(),
                            services.GetRequiredService<IMapper>(),
                            services.GetRequiredService<IPatientService>(),
                            services.GetRequiredService<ISpecializationService>(),
                            services.GetRequiredService<IDoctorService>()));

                    services.AddScoped<IViewModelFactory<ManageSchedulesViewModel>>(services => new ManageSchedulesViewModelFactory(
                        services.GetRequiredService<IDoctorService>(),
                        services.GetRequiredService<IScheduleService>(),
                        services.GetRequiredService<IDateTimeService>(),
                        services.GetRequiredService<IMapper>(),
                        services.GetRequiredService<RenavigatorViewModelFactory<GenerateScheduleViewModel>>()));

                    services.AddScoped<IViewModelFactory<GenerateScheduleViewModel>>(services => new GenerateScheduleViewModelFactory(
                        services.GetRequiredService<IScheduleService>(),
                        services.GetRequiredService<IDoctorService>(),
                        services.GetRequiredService<ISpecializationService>(),
                        services.GetRequiredService<IMapper>(),
                        services.GetRequiredService<RenavigatorViewModelFactory<HomeViewModel>>(),
                        services.GetRequiredService<SpecializationFormValidator>(),
                        services.GetRequiredService<GenerateScheduleValidator>()));

                    services.AddScoped<IViewModelFactory<ManageUsersViewModel>, ManageUsersViewModelFactory>();

                    services.AddAutoMapper(Assembly.GetExecutingAssembly());

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
