using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistance.Services.PatientDataServices
{
    public class PatientDataService : IPatientDataService
    {
        private readonly ApplicationDbContextFactory _dbContextFactory;
        private readonly NonQueryDataService<Patient> _nonQueryDataService;

        public PatientDataService(ApplicationDbContextFactory dbContextFactory, NonQueryDataService<Patient> nonQueryDataService)
        {
            _dbContextFactory = dbContextFactory;
            _nonQueryDataService = nonQueryDataService;
        }
        public async Task<Patient> Create(Patient entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);

        }
        public async Task<Patient> Update(int id, Patient entity)
        {
            return await _nonQueryDataService.Update(id, entity);

        }

        public async Task<Patient> Get(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var entity = await context.Patients
                    .Include(p => p.Address)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return entity;
            }
        }

        public async Task<IEnumerable<Patient>> GetAll()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var entities = await context.Patients
                    .Include(p => p.Address)
                    .OrderBy(p => p.LastName)
                    .ToListAsync();

                return entities;
            }
        }



    }
}
