using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Application.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
