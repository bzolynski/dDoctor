using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using Application.Services.UserService;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IDoctorService, DoctorService>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<ISpecializationService, SpecializationService>();
            services.AddTransient<IAgeService, AgeServices>();
            services.AddTransient<IAccountService, AccountService>();

            services.AddSingleton<IPasswordHasher<Account>, PasswordHasher<Account>>();
        }
    }
}
