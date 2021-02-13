using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFUi.Models
{
    public class DoctorModel
    {
        private readonly Doctor _doctor;

        public int Id => _doctor.Id;
        public string LastName => _doctor.LastName;
        public string FirstName => _doctor.FirstName;
        public string FullName => $"{ LastName } { FirstName }";
        public string NPWZ => _doctor.NPWZ;
        public IEnumerable<Schedule> Schedules => _doctor.Schedules;
        public DateTime DateFrom => Schedules.Min(sc => sc.Date);
        public DateTime DateTo => Schedules.Max(sc => sc.Date);

        public DoctorModel(Doctor doctor)
        {
            _doctor = doctor;
        }
    }
}
