using Domain.Entities;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Services.PatientDataServices
{
    public interface IPatientDataService : IDataService<Patient>
    {
    }
}
