using Domain.Entities;
using Persistance.Services.ReservationDataService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationDataService _reservationDataService;

        public ReservationService(IReservationDataService reservationDataService)
        {
            _reservationDataService = reservationDataService;
        }

        public async Task<IEnumerable<Reservation>> GetManyByPatient(int patientId)
        {
            return await _reservationDataService.GetManyByPatient(patientId);
        }

        public async Task<Reservation> Create(Reservation reservation)
        {
            return await _reservationDataService.Create(reservation);
        }

        public async Task<Reservation> RegisterPatient(Reservation reservation, int patientId)
        {
            reservation.PatientId = patientId;
            return await _reservationDataService.Update(reservation.Id, reservation);
        }

        public async Task<IEnumerable<Reservation>> GetAppointmentsByDate (DateTime date)
        {
            return await _reservationDataService.GetManyByDateWithAllDetails(date);
        }

        

        public async Task<Reservation> CancelAppointment(Reservation reservation)
        {
            return await _reservationDataService.Update(reservation.Id, new Reservation { Hour = reservation.Hour, ScheduleId = reservation.ScheduleId });
        }

    }
}
