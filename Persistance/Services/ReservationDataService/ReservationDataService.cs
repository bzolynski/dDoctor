using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.ReservationDataService
{
    public class ReservationDataService : IReservationDataService
    {
        private readonly ApplicationDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Reservation> _nonQueryDataService;

        public ReservationDataService(ApplicationDbContextFactory contextFactory, NonQueryDataService<Reservation> nonQueryDataService)
        {
            _contextFactory = contextFactory;
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

        public Task<Reservation> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reservation>> GetAll()
        {
            throw new NotImplementedException();

        }

        public async Task<IEnumerable<Reservation>> GetManyByPatient(int patientId)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Reservations
                    .Include(r => r.Schedule)
                    .ThenInclude(s => s.Doctor)
                    .Include(s => s.Schedule.Specialization)
                    .Where(r => r.PatientId == patientId)
                    .OrderBy(r => r.Schedule.Date)
                    .ThenBy(r => r.Hour)
                    .ToListAsync();

            }
        }

        public async Task<IEnumerable<Reservation>> GetManyByDateWithAllDetails(DateTime date)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Reservations
                    .Include(r => r.Patient)
                    .ThenInclude(p => p.Address)
                    .Include(r => r.Schedule)
                    .ThenInclude(s => s.Doctor)
                    .Include(s => s.Schedule.Specialization)
                    .Where(r => r.Schedule.Date.Date == date.Date && r.PatientId != null)
                    .OrderBy(r => r.Schedule.Doctor)
                    .ThenBy(r => r.Hour)
                    .ToListAsync();

            }
        }
    }
}
