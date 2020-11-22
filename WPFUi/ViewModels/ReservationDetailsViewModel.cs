using Application.Services.PatientServices;
using Application.Services.ReservationServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Models;
using WPFUi.ViewModels.PatientVMs;

namespace WPFUi.ViewModels
{
    public class ReservationDetailsViewModel : ViewModelBase
    {


        // Private fields
        #region Private fields

        private readonly int _reservationId;
        private readonly IReservationService _reservationService;

        #endregion

        // Properties
        #region Properties

        private Reservation _reservation;

        public Reservation Reservation
        {
            get { return _reservation; }
            set
            {
                _reservation = value;
                OnPropertyChanged(nameof(Reservation));
            }
        }

        public Patient SelectedPatient => PatientPicker.Patient;
        public PatientPickerViewModel PatientPicker { get; set; }

        #endregion

        // Commands
        #region Commands

        public event Action DetailsClosed;
        public event Action ReservationSubmitted;

        public ICommand CloseCommand { get; set; }
        public ICommand SubmitReservationCommand { get; set; }
        #endregion

        // Constructors
        #region Constructors

        public ReservationDetailsViewModel(
            int reservationId, 
            IReservationService reservationService, 
            IPatientService patientService,
            IMapper mapper)
        {
            _reservationId = reservationId;
            _reservationService = reservationService;
            PatientPicker = new PatientPickerViewModel(patientService, mapper);

            PatientPicker.SelectedPatientChanged += PatientPicker_SelectedPatientChanged;

            CloseCommand = new RelayCommand(Close);
            SubmitReservationCommand = new AsyncRelayCommand(SubmitReservation, (ex) => throw ex);

            LoadReservation();
        }

        


        #endregion

        // Methods
        #region Methods

        private void Close(object obj)
        {
            DetailsClosed?.Invoke();
        }

        private async Task SubmitReservation(object arg)
        {
            await _reservationService.RegisterPatient(Reservation, Reservation.Patient.Id);
            ReservationSubmitted?.Invoke();

        }

        private void LoadReservation()
        {
            _reservationService.GetById(_reservationId).ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    Reservation = task.Result;
                }
            });
        }

        

        private void PatientPicker_SelectedPatientChanged()
        {
            Reservation.Patient = SelectedPatient;
            OnPropertyChanged(nameof(Reservation));
        }

        #endregion

    }
}
