using System.Windows;
using WPFUi.ViewModels;

namespace WPFUi.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();

            Loaded += ShellView_Loaded;
        }

        private void ShellView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ShellViewModel viewModel)
            {
                viewModel.Close += () => this.Close();
            }
        }
    }
}
