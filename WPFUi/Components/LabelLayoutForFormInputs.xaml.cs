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
    /// Interaction logic for LabelLayoutForFormInputs.xaml
    /// </summary>
    public partial class LabelLayoutForFormInputs : UserControl
    {
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(LabelLayoutForFormInputs), new PropertyMetadata(string.Empty));

        public LabelLayoutForFormInputs()
        {
            InitializeComponent();
        }
    }
}
