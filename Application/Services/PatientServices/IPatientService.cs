using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.PatientServices
{
    public interface IPatientService
    {
        Task CreatePatient(Patient patient, Address address);
        Task DeletePatient(int id);
        Task<IEnumerable<Patient>> GetAllPatients();
    }
}