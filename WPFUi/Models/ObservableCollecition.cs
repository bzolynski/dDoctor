using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WPFUi.Models
{
    public class ObservableCollecition : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
