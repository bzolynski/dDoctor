using Domain.Entities;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.ScheduleDataServices
{
    public interface IScheduleDataService : IDataService<Schedule>
    {
        public Task<IEnumerable<Schedule>> GetManyByDoctor(int doctorId);
        public Task<IEnumerable<Schedule>> GetManyBySpecialization(int specializationId);
        public Task<IEnumerable<Schedule>> GetManyByDoctorAndSpecialization(int doctorId, int specializationId);

        public Task<Reservation> CreateReservation(Reservation reservation);
        public Task<bool> DeleteReservation(int id);
    }
}
