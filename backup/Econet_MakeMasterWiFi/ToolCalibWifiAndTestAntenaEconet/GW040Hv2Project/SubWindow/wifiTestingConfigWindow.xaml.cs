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
using GW040Hv2Project.Function;
using GW040Hv2Project.Function.IO;

namespace GW040Hv2Project.SubWindow
{
    /// <summary>
    /// Interaction logic for wifiTestingConfigWindow.xaml
    /// </summary>
    public partial class wifiTestingConfigWindow : Window
    {
        public wifiTestingConfigWindow()
        {
            InitializeComponent();
            this.dgtxTestWifi.DataContext = GlobalData.lstWifiTestingInfor;
            ShowWifiTesting();
        }
        void ShowWifiTesting()
        {
            //----------------Init value ----------------------------//
            GlobalData.lstWifiTestingInfor.LstWifiTestingTmp = new List<WifiTestingInfor>();
            GlobalData.lstWifiTestingInfor.LstWifiTesting = new List<WifiTestingInfor>();


            //-----------------Check Frequency List------------------------//
            foreach (var tmp1 in GlobalData.listTestAnten1)
            {
                GlobalData.lstWifiTestingInfor.LstWifiTestingTmp.Add(new WifiTestingInfor() {  Frequency = tmp1.channelfreq,Antena=tmp1.anten,Attenuator = "", LowerLimit = "", UpperLimit = "" });
            }
             
            foreach (var tmp1 in GlobalData.listTestAnten2)
            {
                GlobalData.lstWifiTestingInfor.LstWifiTestingTmp.Add(new WifiTestingInfor() { Frequency = tmp1.channelfreq, Antena = tmp1.anten, Attenuator = "", LowerLimit = "", UpperLimit = "" });
            }

            //----------------Load from File -------------------------//
            LimitWifiFinal.readFromFile();// sẽ lưu vào GlobalData.lstWifiTestingInfor.LstWifiTesting
            foreach (var tmp4 in GlobalData.lstWifiTestingInfor.LstWifiTestingTmp)// check xem có bài test mới
            {
                foreach (var tmp5 in GlobalData.lstWifiTestingInfor.LstWifiTesting)
                {
                    if (tmp4.Frequency == tmp5.Frequency && tmp4.Antena == tmp5.Antena)
                    {
                        tmp4.Attenuator = tmp5.Attenuator;
                        tmp4.UpperLimit = tmp5.UpperLimit;
                        tmp4.LowerLimit = tmp5.LowerLimit;
                    }
                }
            }
            GlobalData.lstWifiTestingInfor.LstWifiTesting = GlobalData.lstWifiTestingInfor.LstWifiTestingTmp;
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            foreach (var tmp in GlobalData.lstWifiTestingInfor.LstWifiTesting)
            {
                float a;
                if (!float.TryParse(tmp.Frequency.Trim(), out a) || !float.TryParse(tmp.Attenuator.Trim(), out a) || !float.TryParse(tmp.UpperLimit.Trim(), out a) || !float.TryParse(tmp.LowerLimit.Trim(), out a) || !float.TryParse(tmp.Antena.Trim(), out a))//
                {
                    MessageBox.Show("Có Trường Dữ Liệu Để Trống Hoặc Không Đúng Định Dạng!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (LimitWifiFinal.SaveFile()) { MessageBox.Show("Lưu Cấu Hình Thành Công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information); this.Close(); }
            else
            {
                MessageBox.Show("Lỗi Trong Quá Trình Lưu Cấu Hình!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }
    }
}
