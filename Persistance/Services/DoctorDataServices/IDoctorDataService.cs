using Domain.Entities;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.DoctorDataServices
{
    public interface IDoctorDataService : IDataService<Doctor>
    {
        public Task<IEnumerable<Doctor>> GetManyBySpecialization(int specializationId);
    }
}
