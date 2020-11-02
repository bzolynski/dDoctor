using System.Windows;
using System.Windows.Controls;

namespace WPFUi.Components
{
    /// <summary>
    /// Interaction logic for SidePanelComponent.xaml
    /// </summary>
    public partial class SidePanelComponent : UserControl
    {


        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(SidePanelComponent), new PropertyMetadata(string.Empty));



        public SidePanelComponent()
        {
            InitializeComponent();
        }
    }
}
