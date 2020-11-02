using System;
using System.Threading.Tasks;

namespace WPFUi.Commands.Common
{
    public class AsyncRelayCommand : AsyncCommandBase
    {
        private readonly Func<object, Task> _function;

        public AsyncRelayCommand(Func<object, Task> function, Predicate<object> canExecute, Action<Exception> onException) : base(canExecute, onException)
        {
            _function = function;
        }

        public AsyncRelayCommand(Func<object, Task> function, Action<Exception> onException) : base(onException)
        {
            _function = function;
        }
        protected override async Task ExecuteAsync(object parameter)
        {
            await _function(parameter);
        }
    }
}
