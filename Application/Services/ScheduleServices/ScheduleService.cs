﻿using Domain.Entities;
using Persistance.Services.ScheduleDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.ScheduleServices
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleDataService _scheduleDataService;

        public ScheduleService(IScheduleDataService scheduleDataService)
        {
            _scheduleDataService = scheduleDataService;
        }

        public async Task Create(int doctorId, int specializationId, TimeSpan startHour, TimeSpan endHour, TimeSpan maxTimePerPatient, DateTime startDay, DateTime endDay, List<DayOfWeek> daysOfWeek)
        {
            

            List<Reservation> reservationsToGenerate = new List<Reservation>();

            

            List<DateTime> datesToGenerate = new List<DateTime>();

            int scheduleId = -1;

            for (var dt = startDay; dt < endDay; dt = dt.AddDays(1))
            {
                if (daysOfWeek.Contains(dt.DayOfWeek))
                {
                    var cos = await _scheduleDataService.Create(new Schedule
                    {
                        DoctorId = doctorId,
                        SpecializationId = specializationId,
                        MaxTimePerPatient = maxTimePerPatient,
                        StartHour = startHour,
                        EndHour = endHour,
                        Date = new DateTime(dt.Year, dt.Month, dt.Day)
                    });

                    for (var ts = startHour; ts < endHour; ts += maxTimePerPatient)
                    {
                        reservationsToGenerate.Add(new Reservation
                        {
                            Hour = ts,
                            ScheduleId = cos.Id
                        });
                    }

                    await _scheduleDataService.CreateReservations(reservationsToGenerate);

                    reservationsToGenerate.Clear();

                }
            }


            //var schedule = new Schedule
            //{
            //    DoctorId = doctorId,
            //    SpecializationId = specializationId,
            //    MaxTimePerPatient = maxTimePerPatient,
            //    Reservations = reservationsToGenerate,
            //    StartHour = startHour,
            //    EndHour = endHour,
            //};

            
        }

        //public async Task<IEnumerable<Schedule>> GetSchedule(int? doctorId, int? specializationId, DateTime date)
        //{
        //    if (doctorId == null)
        //        return await _scheduleDataService.GetManyBySpecialization((int)specializationId, date);
        //    if (specializationId == null)
        //        return await _scheduleDataService.GetManyByDoctor((int)doctorId, date);

        //    return await _scheduleDataService.GetManyBySpecializationAndDoctor((int)doctorId, (int)specializationId);
        //}



        public async Task<IEnumerable<Schedule>> GetSchedulesBySpecializationAndDoctor(int specializationId, int doctorId)
        {
            return await _scheduleDataService.GetManyBySpecializationAndDoctor(specializationId, doctorId);
        }

        

        public async Task<IEnumerable<Reservation>> GetSpecifiedReservations(int doctorId, int specializationId, DateTime date)
        {
            return await _scheduleDataService.GetSpecifiedReservations(doctorId, specializationId, date);
        }

        /// <summary>
        /// Returns list of schedules needed for ManageSchedulesViewModel to display doctor's schedules.
        /// </summary>
        /// <param name="doctorId">Id of selected doctor</param>
        /// <param name="dateFrom">Start date picked from side panel. By dfault its first day of current month.</param>
        /// <param name="dateTo">End date picked from side panel. By default its last day of current month.</param>
        /// <returns>List of schedules</returns>
        public async Task<IEnumerable<Schedule>> GetSchedulesInSpecifiedDateRangeByDoctorId(int doctorId, DateTime dateFrom, DateTime dateTo)
        {
            var schedules = await _scheduleDataService.GetManyByDoctorId(doctorId);

            return schedules.Where(sc => sc.Date > dateFrom.Date && sc.Date < dateTo);
        }
    }
}
