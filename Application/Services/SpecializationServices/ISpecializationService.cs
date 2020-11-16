using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.SpecializationServices
{
    public interface ISpecializationService
    {
        Task<Specialization> Create(string specializationCode, string specializationName);
        public Task<IEnumerable<Specialization>> GetAll();
        
    }
}