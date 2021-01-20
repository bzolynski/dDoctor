using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFUi.Controls.Components
{
    /// <summary>
    /// Interaction logic for PatientPicker.xaml
    /// </summary>
    public partial class PatientPicker : UserControl
    {
        public int MaximumHeight
        {
            get { return (int)GetValue(MaximumHeightProperty); }
            set { SetValue(MaximumHeightProperty, value); }
        }

        public static readonly DependencyProperty MaximumHeightProperty =
            DependencyProperty.Register("MaximumHeight", typeof(int), typeof(PatientPicker), new PropertyMetadata(1000));

        public PatientPicker()
        {
            InitializeComponent();
        }

        
    }
}
