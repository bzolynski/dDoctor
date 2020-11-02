using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistance.Services.DoctorDataServices
{
    public class DoctorDataService : IDoctorDataService
    {
        private readonly ApplicationDbContextFactory _dbContextFactory;
        private readonly NonQueryDataService<Doctor> _nonQueryDataService;

        public DoctorDataService(ApplicationDbContextFactory dbContextFactory, NonQueryDataService<Doctor> nonQueryDataService)
        {
            _dbContextFactory = dbContextFactory;
            _nonQueryDataService = nonQueryDataService;
        }

        public async Task<Doctor> Create(Doctor entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<Doctor> Update(int id, Doctor entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Doctor> Get(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                // TODO: Think about this
                return await context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            }
        }

        public async Task<IEnumerable<Doctor>> GetAll()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Doctors
                    .OrderBy(d => d.LastName)
                    .ToListAsync();
            }
        }



        public async Task<IEnumerable<Doctor>> GetDoctorsWhoHaveSchedule()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Doctors
                    .Include(d => d.Schedules)
                    .OrderBy(d => d.LastName)
                    .Where(d => d.Schedules.Count > 0)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Doctor>> GetManyBySpecialization(int specializationId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Doctors
                    .Where(d => d.Schedules.Any(sc => sc.SpecializationId == specializationId))
                    .ToListAsync();
                    
            }
        }
    }
}
