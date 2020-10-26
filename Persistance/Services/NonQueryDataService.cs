using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services
{
    public class NonQueryDataService<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContextFactory _contextFactory;

        public NonQueryDataService(ApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();
                
                return result.Entity;
            }
        }

        public async Task<TEntity> Update(int id, TEntity entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                entity.Id = id;

                var result = context.Set<TEntity>().Update(entity);
                await context.SaveChangesAsync();

                return result.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var entity = await context.Set<TEntity>().FindAsync(id);
                context.Set<TEntity>().Remove(entity);

                await context.SaveChangesAsync();

                return true;
            }
        }
    }
}
