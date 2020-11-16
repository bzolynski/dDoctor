using Application.Services.SpecializationServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUi.Commands.Common;
using WPFUi.Validators;

namespace WPFUi.ViewModels
{
    public class SpecializationFormViewModel : ViewModelBase, IDataErrorInfo
    {
        // Validators
        #region Validators

        public Dictionary<string, string> ErrorCollection { get; set; } = new Dictionary<string, string>();

        private bool _canSubmit;

        public string Error => null;

        public string this[string propertyName] 
        { 
            get
            {
                var errorList = _specializationFormValidator.Validate(this).Errors;

                _canSubmit = errorList.Count > 0 ? false : true;

                var error = errorList.FirstOrDefault(e => e.PropertyName == propertyName);

                if (ErrorCollection.ContainsKey(propertyName) && error != null)
                    ErrorCollection[propertyName] = error.ErrorMessage;
                else if (error != null)
                    ErrorCollection.Add(propertyName, error.ErrorMessage);
                else
                    ErrorCollection.Remove(propertyName);

                OnPropertyChanged(nameof(ErrorCollection));

                return error != null ? error.ErrorMessage : null;
            } 
        }

        #endregion


        // Private fields
        #region Private fields

        private readonly ISpecializationService _specializationService;
        private readonly SpecializationFormValidator _specializationFormValidator;

        #endregion

        // Properties
        #region Properties

        public string SpecializationCode { get; set; }
        public string SpecializationName { get; set; }
        #endregion

        // Commands
        #region Commands

        public event Action SpecializationAdded;
        public event Action SpecializationFormClosed;
        public ICommand SubmitSpecializationFormCommand { get; set; }
        public ICommand CancelSpecializationFormCommand { get; set; }


        #endregion

        // Constructors
        #region Constructors

        // TODO: Check if new code is unique
        public SpecializationFormViewModel(ISpecializationService specializationService, SpecializationFormValidator specializationFormValidator)
        {
            _specializationService = specializationService;
            _specializationFormValidator = specializationFormValidator;

            SubmitSpecializationFormCommand = new AsyncRelayCommand(SubmitSpecializationForm, CanSubmitSpecializationForm, (ex) => throw ex);
            CancelSpecializationFormCommand = new RelayCommand(CancelSpecializationForm);
        }

        


        #endregion

        // Methods
        #region Methods

        private bool CanSubmitSpecializationForm(object obj)
        {
            return _canSubmit;
        }

        private async Task SubmitSpecializationForm(object arg)
        {
            await _specializationService.Create(SpecializationCode, SpecializationName);
            SpecializationAdded?.Invoke();
        }

        private void CancelSpecializationForm(object obj)
        {
            SpecializationFormClosed?.Invoke();
        }

       
        #endregion

    }
}
