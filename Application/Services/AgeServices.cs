using System;

namespace Application.Services
{
    public class AgeServices : IAgeService
    {

        public int Calculate(DateTime birthDay)
        {
            var age = new DateTime(DateTime.Now.Ticks - birthDay.Ticks).Year - 1;

            return age;
        }
    }
}
