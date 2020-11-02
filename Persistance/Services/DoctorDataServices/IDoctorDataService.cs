using Domain.Entities;
using Persistance.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistance.Services.DoctorDataServices
{
    public interface IDoctorDataService : IDataService<Doctor>
    {
        public Task<IEnumerable<Doctor>> GetManyBySpecialization(int specializationId);
        public Task<IEnumerable<Doctor>> GetDoctorsWhoHaveSchedule();
    }
}
