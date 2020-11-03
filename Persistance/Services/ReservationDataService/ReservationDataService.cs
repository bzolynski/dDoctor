using Domain.Entities;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.ReservationDataService
{
    public class ReservationDataService : IReservationDataService
    {
        private readonly NonQueryDataService<Reservation> _nonQueryDataService;

        public ReservationDataService(NonQueryDataService<Reservation> nonQueryDataService)
        {
            _nonQueryDataService = nonQueryDataService;
        }
        public async Task<Reservation> Create(Reservation entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public async Task<Reservation> Update(int id, Reservation entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }

        public async Task<Reservation> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Reservation>> GetAll()
        {
            throw new NotImplementedException();
        }

        
    }
}
