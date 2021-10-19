using ONTWiFiMaster.Function.Excute;
using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.Function.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using UtilityPack.Converter;

namespace ONTWiFiMaster.uCtrl.Sub {
    /// <summary>
    /// Interaction logic for ucVerify.xaml
    /// </summary>
    public partial class ucVerify : UserControl {

        int time_count = 0;
        DispatcherTimer timer = null;

        public ucVerify() {
            InitializeComponent();
            setItemSourceCombobox();
            this.sp_setting.DataContext = myGlobal.mySetting;
            this.DataContext = myGlobal.myVerify;
            this.dg_per2g.ItemsSource = myGlobal.collectionPerResult2G;
            this.dg_per5g.ItemsSource = myGlobal.collectionPerResult5G;
            this.dg_signal2g.ItemsSource = myGlobal.collectionSignalResult2G;
            this.dg_signal5g.ItemsSource = myGlobal.collectionSignalResult5G;

            loadConfigFile();

            timer = new DispatcherTimer();
            int delay_time = 500;
            timer.Interval = TimeSpan.FromMilliseconds(delay_time);
            timer.Tick += (s, e) => {
                time_count++;
                myGlobal.myVerify.totalTime = myConverter.intToTimeSpan(time_count * delay_time);
                this.scrl_logsystem.ScrollToEnd();
                this.scrl_loguart.ScrollToEnd();
                this.scrl_loginstrument.ScrollToEnd();
                this.scrl_logtable.ScrollToEnd();
            };
        }

        private bool loadConfigFile() {
            bool ret = true, r = false;
            ChannelManagementFile cmf = new ChannelManagementFile();
            r = cmf.loadDataFromFile(); if (!r) ret = false;
            AttenuatorConfigFile acf = new AttenuatorConfigFile();
            r = acf.loadDataFromFile(); if (!r) ret = false;
            WaveformConfigFile wcf = new WaveformConfigFile();
            r = wcf.loadDataFromFile(); if (!r) ret = false;
            RxLimitConfigFile rcf = new RxLimitConfigFile();
            r = rcf.loadDataFromFile(); if (!r) ret = false;
            TxLimitConfigFile tcf = new TxLimitConfigFile();
            r = tcf.loadDataFromFile(); if (!r) ret = false;
            TestCaseConfigFile tccf = new TestCaseConfigFile();
            r = tccf.loadDataFromFile(); if (!r) ret = false;
            return ret;
        }

        private void setItemSourceCombobox() {
            this.cbb_Port1.ItemsSource = this.cbb_Port2.ItemsSource = myParameter.listPort;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string bTag = b.Tag as string;
            time_count = 0;
            if (loadConfigFile() == false) {
                MessageBox.Show("Không thể load được file config. Vui lòng kiểm tra lại.", "Load File Config", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            myGlobal.myVerify.Init();

            switch (bTag.ToLower()) {
                case "start": {
                        Thread t = new Thread(new ThreadStart(() => {
                            timer.Start();
                            myGlobal.myVerify.buttonContent = "Stop";
                            myGlobal.myTesting.verifyResult = "Waiting...";
                            VerifyWiFiSignal vwf = new VerifyWiFiSignal();
                            bool r = vwf.Excuted();
                            myGlobal.myTesting.verifyResult = r ? "Passed" : "Failed";
                            VerifyLogFile vlf = new VerifyLogFile();
                            vlf.SaveToFile(myGlobal.myVerify);
                            timer.Stop();
                            myGlobal.myVerify.buttonContent = "Start";
                            MessageBox.Show(r ? "Success" : "Failured", "Verify WiFi", MessageBoxButton.OK, r ? MessageBoxImage.Information : MessageBoxImage.Error);
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
            }

        }
    }
}
