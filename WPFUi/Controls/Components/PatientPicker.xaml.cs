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

        // Using a DependencyProperty as the backing store for MaximumHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumHeightProperty =
            DependencyProperty.Register("MaximumHeight", typeof(int), typeof(PatientPicker), new PropertyMetadata(1000));



        public ICommand SelectedPatientChangedCommand
        {
            get { return (ICommand)GetValue(SelectedPatientChangedProperty); }
            set { SetValue(SelectedPatientChangedProperty, value); }
        }

        public static readonly DependencyProperty SelectedPatientChangedProperty =
            DependencyProperty.Register("SelectedPatientChangedCommand", typeof(ICommand), typeof(PatientPicker), new PropertyMetadata(null));



        public PatientPicker()
        {
            InitializeComponent();
        }

        private void patientPicker_SelectedPatientChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (patientPicker.SelectedItem != null)
            {
                SelectedPatientChangedCommand?.Execute(null);
            }
        }
    }
}
