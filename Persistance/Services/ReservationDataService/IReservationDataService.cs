using Domain.Entities;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistance.Services.ReservationDataService
{
    public interface IReservationDataService : IDataService<Reservation>
    {
        public Task<IEnumerable<Reservation>> GetManyByPatient(int patientId);
        public Task<IEnumerable<Reservation>> GetManyByDateWithAllDetails(DateTime date);
    }
}
