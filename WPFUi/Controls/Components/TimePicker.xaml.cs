using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUi.Controls.Components
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(new TimeSpan(hours: 8, minutes: 0, seconds: 0)));
        
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        private byte _hours = 8;
        private byte _minutes = 0;

        public TimePicker()
        {
            InitializeComponent();
            HourBox.MaxLength = 2;
            MinuteBox.MaxLength = 2;
            SetTime();

        }

        private void TextBox_SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && tb != null)
            {
                if (!tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;

                    tb.Focus();
                }
            }

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb != null)
            {
                tb.SelectAll();
            }
        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            bool result = true;

            if (!regex.IsMatch(e.Text))
            {
                result = false;
            }

            e.Handled = result;
        }

        private void HourBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb.Text[0] != '0' && int.Parse(tb.Text) < 10)
            {
                tb.Text = $"0{tb.Text}";
            }
            _hours = byte.Parse(tb.Text);

            SetTime();

        }

        private void MinuteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (tb.Text[0] != '0' && int.Parse(tb.Text) < 10)
            {
                tb.Text = $"0{tb.Text}";
            }
            _minutes = byte.Parse(tb.Text);

            SetTime();
        }


        private void HourBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.IsNullOrWhiteSpace(tb.Text) && !(int.Parse(tb.Text) > 24 || int.Parse(tb.Text) < 0))
            {
            }
            else
            {
                tb.Text = "08";
                tb.SelectAll();
            }
            
        }

        private void MinuteBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!String.IsNullOrWhiteSpace(tb.Text) && !(int.Parse(tb.Text) > 59 || int.Parse(tb.Text) < 0))
            {
            }
            else
            {
                tb.Text = "00";
                tb.SelectAll();
            }
            
        }

        private void SetTime()
        {
            Time = new TimeSpan(_hours, _minutes, 0);
        }

    }
}
