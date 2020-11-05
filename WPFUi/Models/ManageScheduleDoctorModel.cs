using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WPFUi.Models
{
    public class ManageScheduleDoctorModel
    {
        public int Id { get; set; }
        public string FullName => $"{ LastName } { FirstName }";
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string NPWZ { get; set; }
        public DateTime DateFrom => Schedules.Min(sc => sc.Date);
        public DateTime DateTo => Schedules.Max(sc => sc.Date);

        public IEnumerable<Schedule> Schedules { get; set; }
    }
}
