using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.DoctorServices
{
    public interface IDoctorService
    {
        public Task<IEnumerable<Doctor>> GetAll();
        public Task<IEnumerable<Doctor>> GetManyBySpecialization(int specializationId);
        public Task<IEnumerable<Doctor>> GetDoctorsWhoHaveSchedule();

    }
}
