using Application.Services.DoctorServices;
using AutoMapper;
using Domain.Entities;
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

        private List<Doctor> _doctorList;
        private Doctor _selectedDoctor;
        private string _searchText = string.Empty;
        
        private readonly IDoctorService _doctorService;

        #endregion

        // Bindings
        #region Bindings

        public ICollectionView DoctorCollectionView{ get; }

        public Doctor SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                if(value != _selectedDoctor)
                {
                    _selectedDoctor = value;
                    SelectedDoctorChanged?.Invoke();
                }
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

        public DoctorPickerViewModel(IDoctorService doctorService)
        {
            _doctorService = doctorService;

            _doctorList = new List<Doctor>();
            DoctorCollectionView = CollectionViewSource.GetDefaultView(_doctorList);
            DoctorCollectionView.Filter = FilterDoctors;
            DoctorCollectionView.SortDescriptions.Add(new SortDescription(nameof(Doctor.LastName), ListSortDirection.Ascending));

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
                        _doctorList.Add(doctor);
                    }

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => DoctorCollectionView.Refresh()));
                }
            });

        }

        private bool FilterDoctors(object obj)
        {
            if (obj is Doctor doctor)
            {
                return doctor.LastName.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) ||
                       doctor.FirstName.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
        #endregion
    }
}
