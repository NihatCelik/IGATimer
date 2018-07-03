using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        TimeSpan LastCountdown { get; set; }
        TimerState timerState;
        DateTime timerTime;
        DispatcherTimer timer1 = new DispatcherTimer();
        Brush DefaultBackgroundBrush { get; set; }
        Brush PreviousForegroundBrush { get; set; }
        Brush halfTimeBrush = new SolidColorBrush(Color.FromRgb(255, 50, 50));

        BrushConverter bc;
        public MainWindow()
        {
            InitializeComponent();
            timer1.Interval = TimeSpan.FromMilliseconds(1);
            timer1.Tick += Timer1_Tick;
            timerState = TimerState.Play;
            DefaultBackgroundBrush = Background;
            //bc = new BrushConverter();
            //Background = (Brush)bc.ConvertFrom(Session.BgColorCode);
            GetDefaultSettings();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = timerTime.Subtract(DateTime.Now);

            var halfTime = new TimeSpan(LastCountdown.Ticks / 2);

            if (remainingTime < halfTime)
            {
                if (halfTimeBrush != lblRemainingTime.Foreground)
                    PreviousForegroundBrush = lblRemainingTime.Foreground;
                lblRemainingTime.Foreground = halfTimeBrush;
            }

            if (remainingTime.TotalSeconds > 0)
            {
                lblRemainingTime.Content = remainingTime.ToString(@"hh\:mm\:ss");
            }
            else
            {
                lblRemainingTime.Foreground = PreviousForegroundBrush;
                timerState = TimerState.Play;
                PlayPauseSetBackround();
                timer1.IsEnabled = false;
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
                    LastCountdown = frm.Timer;
                    lblRemainingTime.Content = LastCountdown.ToString();
                    return;
                }
            }

            if (timerState == TimerState.Play)
            {
                if (activeTime.TotalSeconds == 0) return;
                timerState = TimerState.Pause;
                PlayPauseSetBackround();
                timer1.IsEnabled = true;
                timerTime = Convert.ToDateTime(DateTime.Now.Add(activeTime));
            }
            else
            {
                timerState = TimerState.Play;
                PlayPauseSetBackround();
                timer1.IsEnabled = false;
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            lblRemainingTime.Content = LastCountdown.ToString(@"hh\:mm\:ss");
            TimeSpan activeTime = TimeSpan.Parse(lblRemainingTime.Content.ToString());
            timerTime = Convert.ToDateTime(DateTime.Now.Add(activeTime));

            if (timerState == TimerState.Pause)
            {
                GetDefaultSettings();
                PlayPause();
            }
        }

        private void BtnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            PlayPause();
        }

        private void BtnSetTimer_Click(object sender, RoutedEventArgs e)
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
                LastCountdown = frm.Timer;
                lblRemainingTime.Content = LastCountdown.ToString();
            }
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            WindowSettings frm = new WindowSettings();
            Session.IsSetColor = false;
            frm.ShowDialog();

            GetDefaultSettings();
        }

        private void GetDefaultSettings()
        {
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
                        this.Background = DefaultBackgroundBrush;
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
                    imgSettings.Source = Session.IsButtonColorBlack ?
                        new BitmapImage(ResourceAccessor.Get("images/icon/bg-settings.png"))
                        : imgSettings.Source = new BitmapImage(ResourceAccessor.Get("images/icon/bg-settings-white.png"));

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

                    PlayPauseSetBackround();

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

        private void PlayPauseSetBackround()
        {
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
                    imgPlayPause.Source = new BitmapImage(ResourceAccessor.Get("images/icon/play-white.png"));
                }
                else
                {
                    imgPlayPause.Source = new BitmapImage(ResourceAccessor.Get("images/icon/pause-white.png"));
                }
            }
        }
    }
}
