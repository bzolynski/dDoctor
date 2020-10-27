using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WPFUi.Commands.Common
{
    public class AsyncRelayCommand : AsyncCommandBase
    {
        private readonly Func<Task> _function;

        public AsyncRelayCommand(Func<Task> function, Action<Exception> onException) : base(onException)
        {
            _function = function;
        }
        protected override async Task ExecuteAsync(object parameter)
        {
            await _function();
        }
    }
}
