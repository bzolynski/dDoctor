using System.Windows;
using System.Windows.Controls;

namespace WPFUi.Components
{
    /// <summary>
    /// Interaction logic for HomeMenuIconComponent.xaml
    /// </summary>
    public partial class HomeMenuIconComponent : UserControl
    {



        public object Command
        {
            get { return (object)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(object), typeof(HomeMenuIconComponent), new PropertyMetadata(null));


        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(HomeMenuIconComponent), new PropertyMetadata(null));



        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(HomeMenuIconComponent), new PropertyMetadata(string.Empty));


        public HomeMenuIconComponent()
        {
            InitializeComponent();
        }
    }
}
