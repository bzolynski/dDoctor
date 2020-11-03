using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ReservationServices
{
    public interface IReservationService
    {
        public Task<Reservation> Create(Reservation reservation);
        public Task<Reservation> RegisterPatient(Reservation reservation, int patientId);
    }
}
