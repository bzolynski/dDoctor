using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFUi.Models
{
    public class ScheduleModel
    {
        public readonly Schedule schedule;

        public DateTime Date => schedule.Date;
        public TimeSpan StartHour => schedule.StartHour;
        public TimeSpan EndHour => schedule.EndHour;
        public ICollection<ReservationModel> Reservations => schedule.Reservations.Select(x => new ReservationModel(x)).ToList();
        public TimeSpan MaxTimePerPatient => schedule.MaxTimePerPatient;
        public ScheduleStatus Status => schedule.Status;
        public Specialization Specialization => schedule.Specialization;
        public Doctor Doctor => schedule.Doctor;
        public string DoctorFullName => $"{ Doctor.LastName } { Doctor.FirstName }";
        public string Availability =>
            $"({ Reservations.Count() - Reservations.Where(x => x.Patient == null).Count() } / { Reservations.Count() })";

        public ScheduleModel(Schedule schedule)
        {
            this.schedule = schedule;
        }
    }
}
