using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ONTWiFiMaster.Function.IO;
using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.Function.Excute;
using System.Windows.Threading;
using UtilityPack.Converter;

namespace ONTWiFiMaster.uCtrl.Sub {
    /// <summary>
    /// Interaction logic for ucCalib.xaml
    /// </summary>
    public partial class ucCalib : UserControl {

        int time_count = 0;
        DispatcherTimer timer = null;

        public ucCalib() {
            InitializeComponent();
            setItemSourceCombobox();
            this.sp_setting.DataContext = myGlobal.mySetting;
            this.DataContext = myGlobal.myCalib;
            this.dg_calib2g.ItemsSource = myGlobal.collectionCalibResult2G;
            this.dg_calib5g.ItemsSource = myGlobal.collectionCalibResult5G;
            this.dg_calibfreq.ItemsSource = myGlobal.collectionCalibFreqResult;

            loadConfigFile();

            timer = new DispatcherTimer();
            int delay_time = 500;
            timer.Interval = TimeSpan.FromMilliseconds(delay_time);
            timer.Tick += (s, e) => {
                time_count++;
                myGlobal.myCalib.totalTime = myConverter.intToTimeSpan(time_count * delay_time);
                this.scrl_logsystem.ScrollToEnd();
                this.scrl_loguart.ScrollToEnd();
                this.scrl_loginstrument.ScrollToEnd();
                this.scrl_logtable.ScrollToEnd();
            };
        }

        private void setItemSourceCombobox() {
            this.cbb_Port1.ItemsSource = this.cbb_Port2.ItemsSource = myParameter.listPort;
        }

        private void loadConfigFile() {
            ChannelManagementFile cmf = new ChannelManagementFile();
            cmf.loadDataFromFile();
            CalibConfigFile ccf = new CalibConfigFile();
            ccf.loadDataFromFile();
            AttenuatorConfigFile acf = new AttenuatorConfigFile();
            acf.loadDataFromFile();
            BinFile bf = new BinFile(myGlobal.myMainWindowDataBinding.productName + ".csv");
            bf.loadDataFromFile();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string bTag = b.Tag as string;
            time_count = 0;
            loadConfigFile();
            myGlobal.myCalib.Init();

            switch (bTag.ToLower()) {
                case "start": {
                        Thread t = new Thread(new ThreadStart(() => {
                            timer.Start();
                            myGlobal.myCalib.buttonContent = "Stop";
                            myGlobal.myTesting.calibResult = "Waiting...";
                            CalibrationWiFi cw = new CalibrationWiFi();
                            bool r = cw.Excuted();
                            myGlobal.myTesting.calibResult = r ? "Passed" : "Failed";
                            CalibLogFile clf = new CalibLogFile();
                            clf.SaveToFile(myGlobal.myCalib);
                            timer.Stop();
                            myGlobal.myCalib.buttonContent = "Start";
                            MessageBox.Show(r ? "Success" : "Failured", "Calib WiFi", MessageBoxButton.OK, r ? MessageBoxImage.Information : MessageBoxImage.Error);
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
            }



        }
    }
}
