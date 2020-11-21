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
