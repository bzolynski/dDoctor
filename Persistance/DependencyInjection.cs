﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Services.AddressDataServices;
using Persistance.Services.Common;
using Persistance.Services.DoctorDataServices;
using Persistance.Services.PatientDataServices;
using Persistance.Services.ReservationDataService;
using Persistance.Services.ScheduleDataServices;
using Persistance.Services.SpecializationDataServices;
using Persistance.Services.UserDataServices;

namespace Persistance
{
    public static class DependencyInjection
    {
        public static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton<ApplicationDbContextFactory>(new ApplicationDbContextFactory(connectionString));

            services.AddScoped<NonQueryDataService<Patient>>();
            services.AddScoped<NonQueryDataService<Address>>();
            services.AddScoped<NonQueryDataService<Schedule>>();
            services.AddScoped<NonQueryDataService<Reservation>>();
            services.AddScoped<NonQueryDataService<Specialization>>();
            services.AddScoped<NonQueryDataService<Doctor>>();
            services.AddScoped<NonQueryDataService<Account>>();

            services.AddScoped<IAddressDataService, AddressDataService>();
            services.AddScoped<IPatientDataService, PatientDataService>();
            services.AddScoped<IScheduleDataService, ScheduleDataService>();
            services.AddScoped<IDoctorDataService, DoctorDataService>();
            services.AddScoped<ISpecializationDataService, SpecializationDataService>();
            services.AddScoped<IReservationDataService, ReservationDataService>();
            services.AddScoped<IAccountDataService, AccountDataService>();
        }
    }
}
