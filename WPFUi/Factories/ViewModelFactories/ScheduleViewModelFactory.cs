using System;
using System.Collections.Generic;
using System.Text;
using WPFUi.ViewModels;

namespace WPFUi.Factories.ViewModelFactories
{
    public class ScheduleViewModelFactory : IViewModelFactory<ScheduleViewModel>
    {
        public ScheduleViewModelFactory()
        {

        }
        public ScheduleViewModel CreateViewModel()
        {
            return new ScheduleViewModel();
        }
    }
}
