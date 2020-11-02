using Domain.Entities;
using Persistance.Services.AddressDataServices;
using Persistance.Services.PatientDataServices;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Application.Services.PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly IPatientDataService _patientDataService;
        private readonly IAddressDataService _addressDataService;


        public PatientService(IPatientDataService patientDataService, IAddressDataService addressDataService)
        {
            _patientDataService = patientDataService;
            _addressDataService = addressDataService;

        }

        public async Task<Patient> CreatePatient(Patient patient)
        {
            return await _patientDataService.Create(patient);
        }

        public async Task<Patient> UpdatePatient(int id, Patient patient)
        {
            return await _patientDataService.Update(id, patient);
        }

        // TODO: Check if patient has any reservation etc
        public async Task DeletePatient(int id)
        {
            await _patientDataService.Delete(id);
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            return await _patientDataService.GetAll();
        }

 


    }
}
