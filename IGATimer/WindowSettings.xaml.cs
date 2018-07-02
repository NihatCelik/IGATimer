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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class WindowSettings : Window
    {
        public WindowSettings()
        {
            InitializeComponent();

            if (Session.IsBgColor)
            {
                txtBgColorCode.Text = Session.BgColorCode;
                rbBgColorCode.IsChecked = true;
            }
            else
            {
                string bgColor = Session.SelectedBgPath;
                if (bgColor.Contains("bg1.jpg"))
                {
                    rbBg1.IsChecked = true;
                }
                else if (bgColor.Contains("bg2.jpg"))
                {
                    rbBg2.IsChecked = true;
                }
                else if (bgColor.Contains("bg3.jpg"))
                {
                    rbBg3.IsChecked = true;
                }
            }

            rbButtonColorWhite.IsChecked = !(rbButtonColorBlack.IsChecked = Session.IsButtonColorBlack);
            txtFontColorCode.Text = Session.RemaniningTimeColorCode;
        }

        private void btnCheckRbColorCode_Click(object sender, RoutedEventArgs e)
        {
            rbBgColorCode.IsChecked = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(rbBg1.IsChecked))
                {
                    Session.SelectedBgPath = "image/bg/bg1.jpg";
                    Session.IsBgColor = false;
                }
                else if (Convert.ToBoolean(rbBg2.IsChecked))
                {
                    Session.SelectedBgPath = "image/bg/bg2.jpg";
                    Session.IsBgColor = false;
                }
                else if (Convert.ToBoolean(rbBg3.IsChecked))
                {
                    Session.SelectedBgPath = "image/bg/bg3.jpg";
                    Session.IsBgColor = false;
                }
                else
                {
                    string bgColorCode = txtBgColorCode.Text.Trim();
                    if (bgColorCode == "")
                    {
                        System.Windows.Forms.MessageBox.Show("Renk Kodu Girin!");
                        return;
                    }
                    Session.BgColorCode = bgColorCode;
                    Session.IsBgColor = true;
                }

                Session.IsButtonColorBlack = Convert.ToBoolean(rbButtonColorBlack.IsChecked) ? true : false;

                string fontColorCode = txtFontColorCode.Text.Trim();
                if (fontColorCode == "")
                {
                    System.Windows.Forms.MessageBox.Show("Yazı Tipinin Renk Kodunu Girin!");
                    return;
                }
                Session.RemaniningTimeColorCode = fontColorCode;
                Session.IsSetColor = true;
                Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void txtBgColorCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rbBgColorCode == null) return;
            rbBgColorCode.IsChecked = true;

            try
            {
                BrushConverter bc = new BrushConverter();
                spHexColorCode.Background = (Brush)bc.ConvertFrom(txtBgColorCode.Text);
                imgErrorBgColorCode.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
                imgErrorBgColorCode.Visibility = Visibility.Visible;
            }
        }
    }
}