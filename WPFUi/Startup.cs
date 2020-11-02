using Application;
using Persistance;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using WPFUi.Factories.ViewModelFactories;
using WPFUi.States.Navigation;
using WPFUi.ViewModels;
using WPFUi.Views;
using Application.Services.PatientServices;
using Application.Services;
using WPFUi.ViewModels.ScheduleManagementVMs;
using Application.Services.DoctorServices;
using Application.Services.ScheduleServices;

namespace WPFUi
{
    public class Startup
    {
        private IConfiguration Configuration => AddConfiguration();

        private IConfiguration AddConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) //current directory is bin/Debug/netcoreapp3.1
                .AddJsonFile("appsettings.json");

            return configurationBuilder.Build();
        }
        public IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddApplication();
            services.AddPersistance(Configuration);

            services.AddSingleton(Configuration);

            services.AddSingleton<ShellView>();
            services.AddSingleton<ShellViewModel>();
            services.AddSingleton<HomeView>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<PatientsView>();
            services.AddSingleton<PatientsViewModel>();
            services.AddSingleton<ScheduleView>();
            services.AddSingleton<ScheduleViewModel>();

            services.AddSingleton<RenavigatorViewModelFactory<GenerateScheduleViewModel>>();
            services.AddSingleton<RenavigatorViewModelFactory<ManageSchedulesViewModel>>();
            services.AddSingleton<RenavigatorViewModelFactory<PatientsViewModel>>();

            services.AddScoped<IRootViewModelFactory, RootViewModelFactory>();
            services.AddScoped<IViewModelFactory<HomeViewModel>, HomeViewModelFactory>();
            services.AddScoped<IViewModelFactory<PatientsViewModel>, PatientsViewModelFactory>();
            services.AddScoped<IViewModelFactory<ScheduleViewModel>, ScheduleViewModelFactory>();
            services.AddScoped<IViewModelFactory<AddAppointmentViewModel>, AddAppointmentViewModelFactory>();
            services.AddScoped<IViewModelFactory<AddPatientViewModel>>(services => new AddPatientViewModelFactory(
                    services.GetRequiredService<IPatientService>(),
                    services.GetRequiredService<IDateTimeService>(),
                    services.GetRequiredService<RenavigatorViewModelFactory<PatientsViewModel>>()));
            services.AddScoped<IViewModelFactory<ManageSchedulesViewModel>>(services => new ManageSchedulesViewModelFactory(
                services.GetRequiredService<IDoctorService>(),
                services.GetRequiredService<IScheduleService>(),
                services.GetRequiredService<IDateTimeService>(),
                services.GetRequiredService<IMapper>(),
                services.GetRequiredService<RenavigatorViewModelFactory<GenerateScheduleViewModel>>()));

            services.AddScoped<IViewModelFactory<GenerateScheduleViewModel>, GenerateScheduleViewModelFactory>();


            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // States
            services.AddSingleton<INavigator, Navigator>();

            return services.BuildServiceProvider();
        }
    }
}
