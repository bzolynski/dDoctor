using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFUi.Views
{
    /// <summary>
    /// Interaction logic for AppointmentsView.xaml
    /// </summary>
    public partial class AppointmentsView : UserControl
    {
        public AppointmentsView()
        {
            InitializeComponent();
        }

        private void calendar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            calendar.IsHitTestVisible = true;
        }

        private void calendar_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            calendar.IsHitTestVisible = false;
        }

        private void calendar_MouseLeave(object sender, MouseEventArgs e)
        {
            calendar.IsHitTestVisible = true;

        }
    }
}
