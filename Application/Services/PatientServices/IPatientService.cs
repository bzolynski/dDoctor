using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.PatientServices
{
    public interface IPatientService
    {
        Task<Patient> CreatePatient(Patient patient);
        Task<Patient> UpdatePatient(int id, Patient patient);

        Task DeletePatient(int id);
        Task<IEnumerable<Patient>> GetAllPatients();
        Task<Patient> GetById(int id);
    }
}