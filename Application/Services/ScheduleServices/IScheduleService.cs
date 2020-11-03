using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.ScheduleServices
{
    public interface IScheduleService
    {
        //Task<IEnumerable<Schedule>> GetSchedule(int? doctorId, int? specializationId, DateTime date);
        Task<IEnumerable<Schedule>> GetSchedulesBySpecializationAndDoctor(int specializationId, int doctorId);
        Task<IEnumerable<Reservation>> GetSpecifiedReservations(int doctorId, int specializationId, DateTime date);

        /// <summary>
        /// Returns list of schedules needed for ManageSchedulesViewModel to display doctor's schedules.
        /// </summary>
        /// <param name="doctorId">Id of selected doctor</param>
        /// <param name="dateFrom">Start date picked from side panel. By dfault its first day of current month.</param>
        /// <param name="dateTo">End date picked from side panel. By default its last day of current month.</param>
        /// <returns>List of schedules</returns>
        Task<IEnumerable<Schedule>> GetSchedulesInSpecifiedDateRangeByDoctorId(int doctorId, DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// Generates schedules for given doctor and specialization between given dates
        /// </summary>
        /// <param name="doctorId">Id of a doctor</param>
        /// <param name="specializationId">Id of specialization</param>
        /// <param name="startHour">Hour of first appointment</param>
        /// <param name="endHour">Hour by which the doctor should be done with last patient</param>
        /// <param name="maxTimePerPatient">Time given for patient</param>
        /// <param name="startDay">Date from when the schedule should be generated</param>
        /// <param name="endDay">Date by which the timesheet should be generated</param>
        /// <param name="daysOfWeek">Days of the week in which doctor takes on patients</param>
        /// <returns></returns>
        public Task GenerateSchedules (int doctorId, int specializationId, TimeSpan startHour, TimeSpan endHour, TimeSpan maxTimePerPatient, DateTime startDay, DateTime endDay, List<DayOfWeek> daysOfWeek);


    }
}