using Application.Services;
using Application.Services.ReservationServices;
using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.States.Navigation;
using WPFUi.ViewModels.AppointmentVMs;

namespace WPFUi.Factories.ViewModelFactories
{
    public class ViewAppointmentsViewModelFactory : IViewModelFactory<ViewAppointmentsViewModel>
    {
        private readonly IReservationService _reservationService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRenavigator _homeRenavigator;

        public ViewAppointmentsViewModelFactory(
            IReservationService reservationService, 
            IDateTimeService dateTimeService,
            IRenavigator homeRenavigator)
        {
            _reservationService = reservationService;
            _dateTimeService = dateTimeService;
            _homeRenavigator = homeRenavigator;
        }
        public ViewAppointmentsViewModel CreateViewModel()
        {
            return new ViewAppointmentsViewModel(
                _reservationService,
                _dateTimeService,
                _homeRenavigator
                );
        }
    }
}
