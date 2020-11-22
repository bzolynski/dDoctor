using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistance.Services.SpecializationDataServices
{
    public class SpecializationDataService : ISpecializationDataService
    {
        private readonly ApplicationDbContextFactory _dbContextFactory;
        private readonly NonQueryDataService<Specialization> _nonQueryDataService;

        public SpecializationDataService(ApplicationDbContextFactory dbContextFactory, NonQueryDataService<Specialization> nonQueryDataService)
        {
            _dbContextFactory = dbContextFactory;
            _nonQueryDataService = nonQueryDataService;
        }
        public async Task<Specialization> Create(Specialization entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Specialization> Update(int id, Specialization entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }

        public async Task<Specialization> Get(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Specializations.FindAsync(id);
            }
        }

        public async Task<IEnumerable<Specialization>> GetAll()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Specializations.ToListAsync();
            }
        }

        public async Task<Specialization> GetByCode(string code)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Specializations.FirstOrDefaultAsync(x => x.Code == code);
            }
        }

        public async Task<Specialization> GetByName(string name)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Specializations.FirstOrDefaultAsync(x => x.Name == name);
            }
        }
    }
}
