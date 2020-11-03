using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<IEnumerable<Schedule>> GetManyByDoctorId(int doctorId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Schedules
                    .Include(s => s.Reservations)
                    .ThenInclude(sh => sh.Patient)
                    .Include(s => s.Specialization)
                    .Include(s => s.Doctor)
                    .Where(sc => sc.DoctorId == doctorId)
                    .ToListAsync();
            }
        }




        public async Task<IEnumerable<Schedule>> GetManyBySpecializationAndDoctor(int specializationId, int doctorId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var schedule =  await context.Schedules
                    .Include(s => s.Specialization)
                    .Include(s => s.Doctor)
                    .Where(x => x.DoctorId == doctorId && x.SpecializationId == specializationId)
                    .ToListAsync();

                return schedule;
            }
        }

        public async Task<IEnumerable<Reservation>> GetSpecifiedReservations(int doctorId, int specializationId, DateTime date)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Reservations
                    .Where(r => r.Schedule.DoctorId == doctorId && r.Schedule.SpecializationId == specializationId && r.Schedule.Date == date.Date)
                    .OrderBy(r => r.Hour)
                    .ToListAsync();
            }
        }

        public async Task CreateReservations(IEnumerable<Reservation> reservations)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Reservations.AddRange(reservations);

                await context.SaveChangesAsync();
            }
        }
    }
}
