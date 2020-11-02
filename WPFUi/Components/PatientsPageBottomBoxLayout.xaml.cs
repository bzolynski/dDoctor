using System.Windows;
using System.Windows.Controls;

namespace WPFUi.Components
{
    /// <summary>
    /// Interaction logic for PatientsPageBottomBoxLayout.xaml
    /// </summary>
    public partial class PatientsPageBottomBoxLayout : UserControl
    {


        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(PatientsPageBottomBoxLayout), new PropertyMetadata(string.Empty));


        public PatientsPageBottomBoxLayout()
        {
            InitializeComponent();
        }
    }
}
