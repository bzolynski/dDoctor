using Domain.Entities;
using Domain.Enums;
using System;

namespace WPFUi.Models
{
    public class ReservationModel
    {
        public readonly Reservation reservation;

        public int Id => reservation.Id;
        public TimeSpan Hour => reservation.Hour;
        public ReservationStatus Status => reservation.Status;
        public Patient Patient => reservation.Patient;
        public int? PatientId => reservation.PatientId;
        public Schedule Schedule => reservation.Schedule;
        public int? ScheduleId => reservation.ScheduleId;
        public string Details => reservation.Details;

        public string Time => $"{ Hour.ToString(@"hh\:mm") } - { (Hour + Schedule.MaxTimePerPatient).ToString(@"hh\:mm") }";
        public string PatientFullName => Patient != null ? $"{ Patient.LastName } { Patient.FirstName }" : "";

        public ReservationModel(Reservation reservation)
        {
            this.reservation = reservation;
        }
    }
}
