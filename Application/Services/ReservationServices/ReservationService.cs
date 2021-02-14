using Application.Services.ScheduleServices;
using Domain.Entities;
using Domain.Enums;
using Persistance.Services.ReservationDataService;
using Persistance.Services.ScheduleDataServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationDataService _reservationDataService;
        private readonly IScheduleDataService _scheduleDataService;

        public ReservationService(IReservationDataService reservationDataService, IScheduleDataService scheduleDataService)
        {
            _reservationDataService = reservationDataService;
            _scheduleDataService = scheduleDataService;
        }

        public async Task<bool> Delete(Reservation reservation)
        {
            return await _reservationDataService.Delete(reservation.Id);
        }

        public async Task<Reservation> Update(int reservationId, Reservation reservation)
        {
            return await _reservationDataService.Update(reservationId, reservation);
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
            if (reservation.Status != ReservationStatus.Canceled)
                reservation.Status = ReservationStatus.Pending;

            return await _reservationDataService.Update(reservation.Id, reservation);
        }

        public async Task<IEnumerable<Reservation>> GetAppointmentsByDate(DateTime date)
        {
            return await _reservationDataService.GetManyByDateWithAllDetails(date);
        }

        public async Task<Reservation> GetById(int id)
        {
            return await _reservationDataService.Get(id);
        }

        public async Task CancelAppointment(Reservation reservation)
        {
            if (reservation.Status == ReservationStatus.Canceled)
            {
                await _reservationDataService.Delete(reservation.Id);
                reservation.Schedule.Reservations.Remove(reservation);
            }
            else
                await _reservationDataService.Update(reservation.Id, new Reservation
                {
                    Hour = reservation.Hour,
                    ScheduleId = reservation.ScheduleId,
                    Status = ReservationStatus.Free
                });

            if (reservation.Schedule.Reservations.Count == 0)
                await _scheduleDataService.Delete(reservation.Schedule.Id);
        }

    }
}
