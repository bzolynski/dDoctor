using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Services.ScheduleDataServices
{
    public class ScheduleDataService : IScheduleDataService
    {
        private readonly ApplicationDbContextFactory _dbContextFactory;
        private readonly NonQueryDataService<Schedule> _scheduleNonQueryDataService;
        private readonly NonQueryDataService<Reservation> _reservationNonQueryDataService;

        public ScheduleDataService(ApplicationDbContextFactory dbContextFactory, NonQueryDataService<Schedule> scheduleNonQueryDataService, NonQueryDataService<Reservation> reservationNonQueryDataService)
        {
            _dbContextFactory = dbContextFactory;
            _scheduleNonQueryDataService = scheduleNonQueryDataService;
            _reservationNonQueryDataService = reservationNonQueryDataService;
        }
        public async Task<Schedule> Create(Schedule entity)
        {
            return await _scheduleNonQueryDataService.Create(entity);
        }
        public async Task<Schedule> Update(int id, Schedule entity)
        {
            return await _scheduleNonQueryDataService.Update(id, entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _scheduleNonQueryDataService.Delete(id);
        }

        public async Task<Schedule> Get(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Schedules
                    .Include(s => s.Reservations)
                    .ThenInclude(sh => sh.Patient)
                    .Include(s => s.Specialization)
                    .Include(s => s.Doctor)
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
        }

        public async Task<IEnumerable<Schedule>> GetAll()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Schedules
                    .Include(s => s.Reservations)
                    .ThenInclude(sh => sh.Patient)
                    .Include(s => s.Specialization)
                    .Include(s => s.Doctor)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Schedule>> GetManyByDoctor(int doctorId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Schedules
                    .Include(s => s.Reservations)
                    .ThenInclude(sh => sh.Patient)
                    .Include(s => s.Specialization)
                    .Include(s => s.Doctor).Where(d => d.Id == doctorId)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Schedule>> GetManyBySpecialization(int specializationId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Schedules
                    .Include(s => s.Reservations)
                    .ThenInclude(sh => sh.Patient)
                    .Include(s => s.Specialization).Where(sp => sp.Id == specializationId)
                    .Include(s => s.Doctor)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Schedule>> GetManyByDoctorAndSpecialization(int doctorId, int specializationId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Schedules
                    .Include(s => s.Reservations)
                    .ThenInclude(sh => sh.Patient)
                    .Include(s => s.Specialization).Where(sp => sp.Id == specializationId)
                    .Include(s => s.Doctor).Where(d => d.Id == doctorId)
                    .ToListAsync();
            }
        }

        public async Task<Reservation> CreateReservation(Reservation reservation)
        {
            return await _reservationNonQueryDataService.Create(reservation);
        }

        public async Task<bool> DeleteReservation(int id)
        {
            return await _reservationNonQueryDataService.Delete(id);

        }
    }
}
