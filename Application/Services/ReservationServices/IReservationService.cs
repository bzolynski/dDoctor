﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.ReservationServices
{
    public interface IReservationService
    {
        public Task<Reservation> Create(Reservation reservation);
        public Task<Reservation> RegisterPatient(Reservation reservation, int patientId);
        public Task<IEnumerable<Reservation>> GetManyByPatient(int patientId);
        public Task<IEnumerable<Reservation>> GetAppointmentsByDate(DateTime date);
        public Task CancelAppointment(Reservation reservation);
        Task<Reservation> GetById(int id);
        Task<Reservation> Update(int reservationId, Reservation reservation);
        Task<bool> Delete(Reservation reservation);
    }
}
