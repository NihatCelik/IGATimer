using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
