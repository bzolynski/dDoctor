using Application.Services.ReservationServices;
using Domain.Entities;
using Domain.Enums;
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
        private readonly IReservationService _reservationService;

        public ScheduleService(IScheduleDataService scheduleDataService, IReservationService reservationService)
        {
            _scheduleDataService = scheduleDataService;
            _reservationService = reservationService;
        }

        public async Task GenerateSchedules(int doctorId, int specializationId, TimeSpan startHour, TimeSpan endHour, TimeSpan maxTimePerPatient, DateTime startDay, DateTime endDay, List<DayOfWeek> daysOfWeek)
        {

            for (var dt = startDay; dt <= endDay; dt = dt.AddDays(1))
            {
                if (daysOfWeek.Contains(dt.DayOfWeek))
                {
                    var schedule = await _scheduleDataService.Create(new Schedule
                    {
                        DoctorId = doctorId,
                        SpecializationId = specializationId,
                        MaxTimePerPatient = maxTimePerPatient,
                        StartHour = startHour,
                        EndHour = endHour,
                        Date = new DateTime(dt.Year, dt.Month, dt.Day),
                        Status = ScheduleStatus.Ok
                    });

                    for (var ts = startHour; ts < endHour; ts += maxTimePerPatient)
                    {

                        await _reservationService.Create(new Reservation
                        {
                            Hour = ts,
                            ScheduleId = schedule.Id,
                            Status = ReservationStatus.Free
                        });
                    }
                }
            }
        }


        // **************************************
        #region Used by AppointmentViewModel

        /// <summary>
        /// Gets schedules for selected date and if given, for selected specialization and doctor.
        /// </summary>
        /// <param name="date">Selected date</param>
        /// <param name="specialization">Selected specialization</param>
        /// <param name="doctor">Selected doctor</param>
        /// <returns>Schedules including: reservations, doctor, patient</returns>
        public async Task<IEnumerable<Schedule>> GetSchedules(DateTime date, Specialization specialization = null, Doctor doctor = null)
        {
            if (doctor != null & specialization != null)
                return await _scheduleDataService.GetManyByDateAndSpecializationAndDoctor(date, specialization.Id, doctor.Id);
            else if (doctor != null)
                return await _scheduleDataService.GetManyByDateAndDoctor(date, doctor.Id);
            else if (specialization != null)
                return await _scheduleDataService.GetManyByDateAndSpecialization(date, specialization.Id);

            return await _scheduleDataService.GetManyByDate(date);
        }

        /// <summary>
        /// Returns list of schedules used to get avalieble dates for reservations
        /// </summary>
        /// <returns>List of schedules</returns>
        public async Task<IEnumerable<Schedule>> GetSchedulesForDates()
        {
            return await _scheduleDataService.GetSchedulesForDates();
        }
        #endregion


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

            return schedules.Where(sc => sc.Date >= dateFrom.Date && sc.Date <= dateTo && sc.Status != ScheduleStatus.Canceled);
        }

        /// <summary>
        /// Changes status of schedule and each reservation in that schedule to canceled
        /// </summary>
        /// <param name="scheduleId">Id of schedule</param>
        /// <returns></returns>
        public async Task ChangeSchedulesStatusToCanceled(int scheduleId)
        {
            var schedule = await _scheduleDataService.Get(scheduleId);
            schedule.Status = ScheduleStatus.Canceled;

            foreach (var reservation in schedule.Reservations)
            {
                reservation.Status = ReservationStatus.Canceled;
            }

            await _scheduleDataService.Update(schedule.Id, schedule);

        }
    }
}
