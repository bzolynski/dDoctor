using Domain.Entities;
using Persistance.Services.SpecializationDataServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.SpecializationServices
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationDataService _specializationDataService;

        public SpecializationService(ISpecializationDataService specializationDataService)
        {
            _specializationDataService = specializationDataService;
        }
        public async Task<IEnumerable<Specialization>> GetAll()
        {
            return await _specializationDataService.GetAll();
        }

        public async Task<Specialization> Create(string specializationCode, string specializationName)
        {
            var specialization = new Specialization { Code = specializationCode, Name = specializationName };

            var result = await _specializationDataService.Create(specialization);

            return result;
        }
    }
}
