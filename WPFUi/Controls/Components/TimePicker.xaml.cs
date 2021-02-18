using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class TimePicker : UserControl, INotifyPropertyChanged
    {
        private int _hours;
        private int _minutes;
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty TimeProperty =
           DependencyProperty.Register("Time", typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(new TimeSpan(8, 0, 0), new PropertyChangedCallback(OnTimePropertyChanged)));

        private static void OnTimePropertyChanged(
        DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TimePicker tp)
            {
                tp.OnTimeChanged();
            }
        }

        protected virtual void OnTimeChanged()
        {
            OnPropertyChanged(nameof(Time));
            HourBox.Text = Time.ToString("hh");
            MinuteBox.Text = Time.ToString("mm");
        }

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public TimePicker()
        {
            InitializeComponent();
            HourBox.MaxLength = 2;
            MinuteBox.MaxLength = 2;
            _hours = Time.Hours;
            _minutes = Time.Minutes;
            SetTime();

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

            SetHourText(tb.Text);

            SetTime();
        }

        private void MinuteBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            SetMinuteText(tb.Text);

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

        private void SetHourText(string hourString)
        {
            if (hourString[0] != '0' && int.Parse(hourString) < 10)
            {
                HourBox.Text = $"0{hourString}";
            }
            _hours = byte.Parse(hourString);
        }

        private void SetMinuteText(string minuteString)
        {
            if (minuteString[0] != '0' && int.Parse(minuteString) < 10)
            {
                MinuteBox.Text = $"0{minuteString}";
            }
            _minutes = byte.Parse(minuteString);
        }

        private void SetTime()
        {
            Time = new TimeSpan(_hours, _minutes, 0);
        }

    }

}
