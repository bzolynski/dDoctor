using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.Interfaces
{
    public interface IDataService<TEntity>
    {
        Task<TEntity> Create(TEntity entity);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(int id);
        Task<TEntity> Update(int id, TEntity entity);
        Task<bool> Delete(int id);

    }
}
