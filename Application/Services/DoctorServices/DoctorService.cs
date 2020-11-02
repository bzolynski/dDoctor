using Domain.Entities;
using Persistance.Services.DoctorDataServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.DoctorServices
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorDataService _doctorDataService;

        public DoctorService(IDoctorDataService doctorDataService)
        {
            _doctorDataService = doctorDataService;
        }
        public async Task<IEnumerable<Doctor>> GetAll()
        {
            return await _doctorDataService.GetAll();
        }

        public async Task<IEnumerable<Doctor>> GetManyBySpecialization(int specializationId)
        {
            return await _doctorDataService.GetManyBySpecialization(specializationId);
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsWhoHaveSchedule()
        {
            return await _doctorDataService.GetDoctorsWhoHaveSchedule();
        }
    }
}
