using Domain.Entities;
using Persistance.Services.Common;
using System.Threading.Tasks;

namespace Persistance.Services.SpecializationDataServices
{
    public interface ISpecializationDataService : IDataService<Specialization>
    {
        Task<Specialization> GetByCode(string code);
        Task<Specialization> GetByName(string name);
    }
}
