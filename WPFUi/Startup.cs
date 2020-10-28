using Application.Services;
using Application.Services.PatientServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance;
using Persistance.Services;
using Persistance.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using WPFUi.Factories.ViewModelFactories;
using WPFUi.States.Navigation;
using WPFUi.ViewModels;
using WPFUi.Views;

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

            services.AddSingleton<ShellView>();
            services.AddSingleton<ShellViewModel>();
            services.AddSingleton<HomeView>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<PatientsView>();
            services.AddSingleton<PatientsViewModel>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var connectionString = Configuration.GetConnectionString("Default");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ApplicationDbContextFactory>();

            services.AddScoped<NonQueryDataService<Patient>>();
            services.AddScoped<NonQueryDataService<Address>>();

            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IAgeService, AgeServices>();

            services.AddScoped<IAddressDataService, AddressDataService>();
            services.AddScoped<IPatientDataService, PatientDataService>();

            services.AddScoped<IRootViewModelFactory, RootViewModelFactory>();
            services.AddScoped<IViewModelFactory<HomeViewModel>, HomeViewModelFactory>();
            services.AddScoped<IViewModelFactory<PatientsViewModel>, PatientsViewModelFactory>();


            // States
            services.AddSingleton<INavigator, Navigator>();

            return services.BuildServiceProvider();
        }
    }
}
