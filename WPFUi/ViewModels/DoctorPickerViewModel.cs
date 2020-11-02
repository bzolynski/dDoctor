using Application.Services.DoctorServices;
using Application.Services.PatientServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WPFUi.Models;

namespace WPFUi.ViewModels
{
    public class DoctorPickerViewModel : ViewModelBase
    {
        public event Action SelectedDoctorChanged;

        // Private fields
        #region Private fields

        private DoctorPickerModel _selectedDoctor;
        private string _searchText;
        
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;

        #endregion

        // Bindings
        #region Bindings

        public ObservableCollection<DoctorPickerModel> DoctorList { get; set; }
        public ObservableCollection<DoctorPickerModel> DoctorDisplayList { get; set; }

        public DoctorPickerModel SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
                SelectedDoctorChanged?.Invoke();
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                DoctorSearch();
            }
        }

        #endregion

        // Constructors
        #region Constructors

        private DoctorPickerViewModel(IDoctorService doctorService, IMapper mapper)
        {
            _doctorService = doctorService;
            _mapper = mapper;
        }

        public static DoctorPickerViewModel LoadDoctorPickerViewModel(IDoctorService doctorService, IMapper mapper)
        {
            var doctorPickerViewModel = new DoctorPickerViewModel(doctorService, mapper);

            doctorPickerViewModel.LoadDoctors();

            return doctorPickerViewModel;
        }

        #endregion

        // Methods
        #region Methods
        private void LoadDoctors()
        {
            _doctorService.GetAll().ContinueWith(task =>
            {
                if (task.Exception == null)
                {
                    DoctorList = new ObservableCollection<DoctorPickerModel>(_mapper.Map<ObservableCollection<DoctorPickerModel>>(task.Result));
                    DoctorDisplayList = new ObservableCollection<DoctorPickerModel>(DoctorList);
                    OnPropertyChanged(nameof(DoctorList));
                    OnPropertyChanged(nameof(DoctorDisplayList));
                }
            });
        }

        private void DoctorSearch()
        {
            DoctorDisplayList = new ObservableCollection<DoctorPickerModel>(DoctorList
               .Where(x => x.FullName.ToUpper().Contains(SearchText.ToUpper()) || x.NPWZ.ToUpper().Contains(SearchText.ToUpper())));
            OnPropertyChanged(nameof(DoctorDisplayList));
        }
        #endregion
    }
}
