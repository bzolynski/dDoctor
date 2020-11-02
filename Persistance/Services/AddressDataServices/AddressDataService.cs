using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistance.Services.AddressDataServices
{
    public class AddressDataService : IAddressDataService
    {
        private readonly ApplicationDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Address> _nonQueryDataService;

        public AddressDataService(ApplicationDbContextFactory contextFactory, NonQueryDataService<Address> nonQueryDataService)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = nonQueryDataService;
        }
        public async Task<Address> Create(Address entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);

        }

        public async Task<Address> Update(int id, Address entity)
        {
            return await _nonQueryDataService.Update(id, entity);

        }

        public async Task<Address> Get(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var entity = await context.Addresses.FindAsync(id);

                return entity;
            }
        }

        public async Task<IEnumerable<Address>> GetAll()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var entities = await context.Addresses.ToListAsync();

                return entities;
            }
        }


    }
}
