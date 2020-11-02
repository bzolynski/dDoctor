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
    }
}
