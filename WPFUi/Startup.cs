﻿using Application;
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
using WPFUi.ViewModels.AppointmentVMs;
using Application.Services.ReservationServices;
using Application.Services.SpecializationServices;
using WPFUi.ViewModels.PatientVMs;
using FluentValidation;
using WPFUi.Validators;

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

            services.AddSingleton(Configuration);

            services.AddApplication();
            services.AddPersistance(Configuration);

            
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
            return services.BuildServiceProvider();
        }
    }
}
