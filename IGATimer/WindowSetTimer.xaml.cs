using System;
using System.Windows;

namespace IGATimer
{
    /// <summary>
    /// Interaction logic for WindowSetTimer.xaml
    /// </summary>
    public partial class WindowSetTimer : Window
    {
        public TimeSpan Timer { get; set; }
        public WindowSetTimer(TimeSpan activeTime)
        {
            InitializeComponent();
            nudHours.MaxValue = 23;
            nudMinutes.MaxValue = 59;
            nudHours.StartValue = activeTime.Hours;
            nudMinutes.StartValue = activeTime.Minutes;
        }

        public WindowSetTimer()
        {
        }

        private void btnSetTimer_Click(object sender, RoutedEventArgs e)
        {
            int hours = Convert.ToInt16(nudHours.StartValue);
            int minutes = Convert.ToInt16(nudMinutes.StartValue);
            TimeSpan ts = new TimeSpan(hours, minutes, 0);
            Timer = ts;
            Session.IsSetTimer = true;
            Close();
        }
    }
}
