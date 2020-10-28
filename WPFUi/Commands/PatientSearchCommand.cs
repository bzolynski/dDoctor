using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPFUi.Models;
using WPFUi.ViewModels;

namespace WPFUi.Commands
{
    public class PatientSearchCommand : ICommand
    {
        private readonly PatientsViewModel _patientsViewModel;

        public PatientSearchCommand(PatientsViewModel patientsViewModel)
        {
            _patientsViewModel = patientsViewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {


            _patientsViewModel.PatientsDisplayList = new ObservableCollection<PatientDisplayModel>(_patientsViewModel.PatientsList
                .Where(x => x.FirstName.ToUpper().Contains(_patientsViewModel.PatientSearchParameters.FirstName.ToUpper()))
                .Where(x => x.Address.City.ToUpper().Contains(_patientsViewModel.PatientSearchParameters.City.ToUpper()))
                .Where(x => x.Address.Street.ToUpper().Contains(_patientsViewModel.PatientSearchParameters.StreetName.ToUpper()))
                .Where(x => x.LastName.ToUpper().Contains(_patientsViewModel.PatientSearchParameters.LastName.ToUpper())));

            //.Where(x => x.BirthDate (_patientsViewModel.PatientSearchParameters.Age)) TODO: AGE CALC
            _patientsViewModel.OnPropertyChanged(nameof(_patientsViewModel.PatientsDisplayList));

        }
    }
}
