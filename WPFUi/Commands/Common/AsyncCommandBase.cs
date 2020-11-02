using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFUi.Commands.Common
{
    public abstract class AsyncCommandBase : ICommand
    {
        private bool _isExecuting;
        private readonly Action<Exception> _onException;
        private readonly Predicate<object> _canExecute;


        public AsyncCommandBase(Predicate<object> canExecute, Action<Exception> onException)
        {
            _canExecute = canExecute;
            _onException = onException;
        }

        public AsyncCommandBase(Action<Exception> onException): this(null, onException)
        {
        }

        public bool IsExecuting
        {
            get { return _isExecuting; }
            set
            {
                _isExecuting = value;
                
                //CanExecuteChanged?.Invoke(this, new EventArgs());
                
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //public event EventHandler CanExecuteChanged;
                
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null ? true : _canExecute.Invoke(parameter) && !IsExecuting)
                return true;
            return false;
        }

        public async void Execute(object parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);

            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);
    }
}
