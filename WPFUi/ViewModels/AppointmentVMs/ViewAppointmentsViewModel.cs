using Application.Services;
using Application.Services.ReservationServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.States.Navigation;

namespace WPFUi.ViewModels.AppointmentVMs
{
    public class ViewAppointmentsViewModel : ViewModelBase
    {

        // Private fields
        #region Private fields

        private string _searchText;
        private DateTime _selectedDate;
        private readonly IReservationService _reservationService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IRenavigator _homeRenavigator;

        #endregion

        // Bindings
        #region Bindings

        public ObservableCollection<Reservation> Reservations { get; set; }
        public ObservableCollection<Reservation> ReservationsDisplay { get; set; }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                LoadAppointments();
            }
        }

        private Reservation _selectedReservation;

        public Reservation SelectedReservation
        {
            get { return _selectedReservation; }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }


        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                SearchMethod();
            }
        }



        #endregion

        // Commands
        #region Commands

        public ICommand ResetFiltersCommand { get; set; }
        public ICommand CancelAppointmentCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        #endregion

        // Constructors
        #region Constructors
        public ViewAppointmentsViewModel(IReservationService reservationService, IDateTimeService dateTimeService, IRenavigator homeRenavigator)
        {
            _reservationService = reservationService;
            _dateTimeService = dateTimeService;
            _homeRenavigator = homeRenavigator;

            SelectedDate = dateTimeService.Now;

            ResetFiltersCommand = new RelayCommand(ResetFilters);
            CancelAppointmentCommand = new AsyncRelayCommand(CancelAppointment, CanCancelAppointment, (ex) => throw ex);
            CloseCommand = new RelayCommand(Close);

            LoadAppointments();

        }

        


        #endregion

        // Methods
        #region Methods

        private void LoadAppointments()
        {
            _reservationService.GetAppointmentsByDate(_selectedDate).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Reservations = new ObservableCollection<Reservation>(task.Result);
                    ReservationsDisplay = new ObservableCollection<Reservation>(Reservations);
                    OnPropertyChanged(nameof(ReservationsDisplay));
                }
            });
        }

        private bool CanCancelAppointment(object obj)
        {
            if (_selectedReservation == null)
                return false;
            return true;
        }

        private async Task CancelAppointment(object arg)
        {
            await _reservationService.CancelAppointment(_selectedReservation);
            LoadAppointments();
        }

        private void SearchMethod()
        {
            ReservationsDisplay = new ObservableCollection<Reservation>(Reservations.Where(x =>
                x.Patient.FirstName.ToUpper().Contains(_searchText.ToUpper()) ||
                x.Patient.LastName.ToUpper().Contains(_searchText.ToUpper()) ||
                x.Schedule.Doctor.FirstName.ToUpper().Contains(_searchText.ToUpper()) ||
                x.Schedule.Doctor.FirstName.ToUpper().Contains(_searchText.ToUpper())));
                OnPropertyChanged(nameof(ReservationsDisplay));
        }

        private void ResetFilters(object obj)
        {
            SelectedDate = _dateTimeService.Now;
            SearchText = "";
        }

        private void Close(object obj)
        {
            _homeRenavigator.Renavigate();
        }
        #endregion

    }
}
