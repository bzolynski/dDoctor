using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUi.Components
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
