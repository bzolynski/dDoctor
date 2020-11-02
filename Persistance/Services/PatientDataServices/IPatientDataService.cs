using Domain.Entities;
using Persistance.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistance.Services.PatientDataServices
{
    public interface IPatientDataService : IDataService<Patient>
    {
    }
}
