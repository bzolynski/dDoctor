using Application.Services.SpecializationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        private bool _canSubmit;

        public string Error => null;

        public string this[string propertyName] 
        { 
            get
            {
                var errorList = _specializationFormValidator.Validate(this).Errors;

                _canSubmit = errorList.Count > 0 ? false : true;

                var error = errorList.FirstOrDefault(e => e.PropertyName == propertyName);

                return error != null ? error.ErrorMessage : null;
            } 
        }

        #endregion


        // Private fields
        #region Private fields

        private readonly ISpecializationService _specializationService;
        private readonly SpecializationFormValidator _specializationFormValidator;
        private string _specializationCode;
        private string _specializationName;

        #endregion

        // Properties
        #region Properties



        public string SpecializationCode
        {
            get { return _specializationCode; }
            set
            {
                _specializationCode = value;
                OnPropertyChanged(nameof(SpecializationCode));
            }
        }
        public string SpecializationName
        {
            get { return _specializationName; }
            set
            {
                _specializationName = value;
                OnPropertyChanged(nameof(SpecializationName));
            }
        }

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
            ResetForm();
            SpecializationAdded?.Invoke();
        }

        private void CancelSpecializationForm(object obj)
        {
            ResetForm();
            SpecializationFormClosed?.Invoke();
        }

        private void ResetForm()
        {
            SpecializationCode = string.Empty;
            SpecializationName = string.Empty;
            OnPropertyChanged(nameof(SpecializationCode));
            OnPropertyChanged(nameof(SpecializationName));
        }

       
        #endregion

    }
}
