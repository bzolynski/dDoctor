using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.SpecializationServices
{
    public interface ISpecializationService
    {
        public Task<IEnumerable<Specialization>> GetAll();
    }
}