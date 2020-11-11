using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.UserDataServices
{
    public class AccountDataService : IAccountDataService
    {
        private readonly ApplicationDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Account> _nonQueryDataService;

        public AccountDataService(ApplicationDbContextFactory contextFactory, NonQueryDataService<Account> nonQueryDataService)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = nonQueryDataService;
        }

        public async Task<Account> Create(Account entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Account> Update(int id, Account entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }

        public async Task<Account> Get(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Accounts
                    .Include(u => u.Doctor)
                    .Include(u => u.Registrant)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Accounts
                    .Include(a => a.Doctor)
                    .Include(a => a.Registrant)
                    .ToListAsync();
            }

        }

        public async Task<Account> GetByUsername(string username)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
            }
        }

        public async Task<Account> GetByEmail(string email)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            }
        }
    }
}
