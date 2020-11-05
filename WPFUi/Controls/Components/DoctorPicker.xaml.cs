using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace WPFUi.Controls.Components
{
    /// <summary>
    /// Interaction logic for DoctorPicker.xaml
    /// </summary>
    public partial class DoctorPicker : UserControl
    {
        public List<TimeSpan> TimeSpans = new List<TimeSpan>
        {
            new TimeSpan(0, 10, 0),
            new TimeSpan(0, 15, 0),
            new TimeSpan(0, 20, 0),
            new TimeSpan(0, 30, 0)
        };

        public DoctorPicker()
        {
            InitializeComponent();
        }
    }
}
