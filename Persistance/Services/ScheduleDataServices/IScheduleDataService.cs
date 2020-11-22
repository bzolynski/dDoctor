using Domain.Entities;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistance.Services.ScheduleDataServices
{
    public interface IScheduleDataService : IDataService<Schedule>
    {
        public Task<IEnumerable<Schedule>> GetManyByDoctorId(int doctorId);
        public Task<IEnumerable<Schedule>> GetManyBySpecializationAndDoctor(int specializationId, int doctorId);

        public Task<IEnumerable<Reservation>> GetSpecifiedReservations(int doctorId, int specializationId, DateTime date);

        public Task CreateReservations(IEnumerable<Reservation> reservations);

        // **************************************
        #region Used by AppointmentViewModel
        Task<IEnumerable<Schedule>> GetManyByDate(DateTime date);
        Task<IEnumerable<Schedule>> GetManyByDateAndDoctor(DateTime date, int doctorId);
        Task<IEnumerable<Schedule>> GetManyByDateAndSpecialization(DateTime date, int specializationId);
        Task<IEnumerable<Schedule>> GetManyByDateAndSpecializationAndDoctor(DateTime date, int specializationId, int doctorId);
        Task<IEnumerable<Schedule>> GetSchedulesForDates();
        #endregion
    }
}
