using Application.Services;
using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using Application.Services.ScheduleServices;
using Application.Services.SpecializationServices;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IDoctorService, DoctorService>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<ISpecializationService, SpecializationService>();
            services.AddTransient<IAgeService, AgeServices>();
        }
    }
}
