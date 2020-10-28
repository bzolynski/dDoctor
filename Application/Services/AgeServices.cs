using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class AgeServices : IAgeService
    {
        private readonly IDateTimeService _dateTimeService;

        public AgeServices(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }
        public int Calculate(DateTime birthDay)
        {
            var age = new DateTime(_dateTimeService.Now.Ticks - birthDay.Ticks).Year - 1;

            return age;
        }
    }
}
