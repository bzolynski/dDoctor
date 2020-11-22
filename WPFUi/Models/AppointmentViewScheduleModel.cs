using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFUi.Models
{
    public class AppointmentViewScheduleModel
    {
        public string DoctorFullName { get; set; }
        //public Doctor Doctor { get; set; }
        public Specialization Specialization { get; set; }

        public string Availability => 
            $"({ Reservations.Count() - Reservations.Where(x => x.Patient == null).Count() } / { Reservations.Count() })";

        public DateTime Date { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public ICollection<AppointmentViewReservationModel> Reservations { get; set; }
        public TimeSpan MaxTimePerPatient { get; set; }

    }

    public class AppointmentViewReservationModel
    {
        public Patient Patient { get; set; }
        public TimeSpan Hour { get; set; }

        public int Id { get; set; }

        public string Time => $"{ Hour.ToString(@"hh\:mm") } - { (Hour + Schedule.MaxTimePerPatient).ToString(@"hh\:mm") }";

        public string PatientFullName => Patient != null ? $"{ Patient.LastName } { Patient.FirstName }" : "";

        public int? PatientId { get; set; }

        public Schedule Schedule { get; set; }
        public int ScheduleId { get; set; }
    }
}
