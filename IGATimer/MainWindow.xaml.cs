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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IGATimer
{
    enum TimerState
    {
        Play,
        Pause
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TimeSpan lastSetUserTime;
        TimerState timerState;
        DateTime timerTime;
        DispatcherTimer timer1 = new DispatcherTimer();
        Brush defaultBrush;

        BrushConverter bc;
        public MainWindow()
        {
            InitializeComponent();
            timer1.Interval = TimeSpan.FromMilliseconds(1);
            timer1.Tick += Timer1_Tick;
            timerState = TimerState.Play;
            defaultBrush = this.Background;
            bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom(Session.BgColorCode);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = timerTime.Subtract(DateTime.Now);
            if (remainingTime.TotalSeconds >= 0)
            {
                lblRemainingTime.Content = remainingTime.ToString(@"hh\:mm\:ss");
            }
            else
            {
                timer1.IsEnabled = false;
                MessageBox.Show("Süre Doldu!");
            }
        }

        private void PlayPause()
        {
            TimeSpan activeTime = TimeSpan.Parse(lblRemainingTime.Content.ToString());
            if (activeTime.TotalSeconds == 0)
            {
                WindowSetTimer frm = new WindowSetTimer(activeTime);
                Session.IsSetTimer = false;
                frm.ShowDialog();
                if (Session.IsSetTimer)
                {
                    lastSetUserTime = frm.Timer;
                    lblRemainingTime.Content = lastSetUserTime.ToString();
                    return;
                }
            }

            if (timerState == TimerState.Play)
            {
                if (activeTime.TotalSeconds == 0) return;

                imgPlayPause.Source = new BitmapImage(ResourceAccessor.Get("images/icon/white-pause.png"));
                timerState = TimerState.Pause;
                timer1.IsEnabled = true;

                timerTime = Convert.ToDateTime(DateTime.Now.Add(activeTime));
            }
            else
            {
                imgPlayPause.Source = new BitmapImage(ResourceAccessor.Get("images/icon/white-play.png"));
                timerState = TimerState.Play;
                timer1.IsEnabled = false;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lblRemainingTime.Content = lastSetUserTime.ToString(@"hh\:mm\:ss");
            TimeSpan activeTime = TimeSpan.Parse(lblRemainingTime.Content.ToString());
            timerTime = Convert.ToDateTime(DateTime.Now.Add(activeTime));
            if (timerState == TimerState.Pause)
            {
                PlayPause();
            }
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            PlayPause();
        }

        private void btnSetTimer_Click(object sender, RoutedEventArgs e)
        {
            SetTimer();
        }

        private void SetTimer()
        {
            if (timerState == TimerState.Pause)
            {
                PlayPause();
            }

            TimeSpan activeTime = TimeSpan.Parse(lblRemainingTime.Content.ToString());
            WindowSetTimer frm = new WindowSetTimer(activeTime);
            Session.IsSetTimer = false;
            frm.ShowDialog();
            if (Session.IsSetTimer)
            {
                lastSetUserTime = frm.Timer;
                lblRemainingTime.Content = lastSetUserTime.ToString();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                imgMaximize.Source = new BitmapImage(ResourceAccessor.Get("images/icon/expand-button-white.png"));
            }
            else
            {
                WindowState = WindowState.Maximized;
                imgMaximize.Source = new BitmapImage(ResourceAccessor.Get("images/icon/compress-square-white.png"));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            WindowSettings frm = new WindowSettings();
            Session.IsSetColor = false;
            frm.ShowDialog();
            try
            {
                if (Session.IsSetColor)
                {
                    if (Session.IsBgColor)
                    {
                        bc = new BrushConverter();
                        this.Background = (Brush)bc.ConvertFrom(Session.BgColorCode);
                    }
                    else
                    {
                        bc = new BrushConverter();
                        this.Background = defaultBrush;
                        string bgColor = Session.SelectedBgPath;
                        if (bgColor.Contains("bg1.jpg"))
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(ResourceAccessor.Get("images/bg/bg1.jpg"));
                            imgBrush.ImageSource = image.Source;
                        }
                        else if (bgColor.Contains("bg2.jpg"))
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(ResourceAccessor.Get("images/bg/bg2.jpg"));
                            imgBrush.ImageSource = image.Source;
                        }
                        else if (bgColor.Contains("bg3.jpg"))
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(ResourceAccessor.Get("images/bg/bg3.jpg"));
                            imgBrush.ImageSource = image.Source;
                        }
                    }

                    imgClose.Source = Session.IsButtonColorBlack ?
                          new BitmapImage(ResourceAccessor.Get("images/icon/cancel-button.png"))
                          : imgClose.Source = new BitmapImage(ResourceAccessor.Get("images/icon/cancel-button-white.png"));
                    imgMinimize.Source = Session.IsButtonColorBlack
                        ? new BitmapImage(ResourceAccessor.Get("images/icon/minimize.png"))
                        : imgMinimize.Source = new BitmapImage(ResourceAccessor.Get("images/icon/minimize-white.png"));

                    if (Session.IsButtonColorBlack)
                    {
                        if (WindowState == WindowState.Maximized)
                        {
                            imgMaximize.Source = new BitmapImage(ResourceAccessor.Get("images/icon/expand-button.png"));
                        }
                        else
                        {
                            imgMaximize.Source = new BitmapImage(ResourceAccessor.Get("images/icon/compress-square.png"));
                        }
                    }
                    else
                    {
                        if (WindowState == WindowState.Maximized)
                        {
                            imgMaximize.Source = new BitmapImage(ResourceAccessor.Get("images/icon/expand-button-white.png"));
                        }
                        else
                        {
                            imgMaximize.Source = new BitmapImage(ResourceAccessor.Get("images/icon/compress-square-white.png"));
                        }
                    }

                    imgReset.Source = Session.IsButtonColorBlack ?
                      new BitmapImage(ResourceAccessor.Get("images/icon/reset.png"))
                      : imgReset.Source = new BitmapImage(ResourceAccessor.Get("images/icon/white-reset.png"));


                    if (Session.IsButtonColorBlack)
                    {
                        if (timerState == TimerState.Play)
                        {
                            imgPlayPause.Source = new BitmapImage(ResourceAccessor.Get("images/icon/play.png"));
                        }
                        else
                        {
                            imgPlayPause.Source = new BitmapImage(ResourceAccessor.Get("images/icon/pause.png"));
                        }
                    }
                    else
                    {
                        if (timerState == TimerState.Play)
                        {
                            imgPlayPause.Source = new BitmapImage(ResourceAccessor.Get("images/icon/white-play.png"));
                        }
                        else
                        {
                            imgPlayPause.Source = new BitmapImage(ResourceAccessor.Get("images/icon/white-pause.png"));
                        }
                    }

                    imgSetTimer.Source = Session.IsButtonColorBlack ?
                      new BitmapImage(ResourceAccessor.Get("images/icon/set-timer.png"))
                      : imgSetTimer.Source = new BitmapImage(ResourceAccessor.Get("images/icon/white-set-timer.png"));
                }

                bc = new BrushConverter();
                lblRemainingTime.Foreground = (Brush)bc.ConvertFrom(Session.RemaniningTimeColorCode);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
