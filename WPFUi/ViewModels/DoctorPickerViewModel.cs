using Application.Services.DoctorServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using WPFUi.Models;

namespace WPFUi.ViewModels
{
    public class DoctorPickerViewModel : ViewModelBase
    {
        public event Action SelectedDoctorChanged;

        // Private fields
        #region Private fields

        private List<DoctorPickerModel> _doctorList;
        private DoctorPickerModel _selectedDoctor;
        private string _searchText = string.Empty;
        
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;

        #endregion

        // Bindings
        #region Bindings

        public ICollectionView DoctorCollectionView{ get; }

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
                DoctorCollectionView.Refresh();
            }
        }

        #endregion

        // Constructors
        #region Constructors

        public DoctorPickerViewModel(IDoctorService doctorService, IMapper mapper)
        {
            _doctorService = doctorService;
            _mapper = mapper;

            _doctorList = new List<DoctorPickerModel>();
            DoctorCollectionView = CollectionViewSource.GetDefaultView(_doctorList);
            DoctorCollectionView.Filter = FilterDoctors;
            DoctorCollectionView.SortDescriptions.Add(new SortDescription(nameof(DoctorPickerModel.LastName), ListSortDirection.Ascending));

            LoadDoctors();
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
                    foreach (var doctor in task.Result)
                    {
                        _doctorList.Add(_mapper.Map<DoctorPickerModel>(doctor));
                    }

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => DoctorCollectionView.Refresh()));
                }
            });

        }

        private bool FilterDoctors(object obj)
        {
            if (obj is DoctorPickerModel doctor)
            {
                return doctor.FullName.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
        #endregion
    }
}
