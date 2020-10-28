using System;

namespace Application.Services
{
    public interface IAgeService
    {
        int Calculate(DateTime birthDay);
    }
}