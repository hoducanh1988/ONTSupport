using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.Function.IO;
using ONTWiFiMaster.Function.Custom;
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
using System.Globalization;
using ONTWiFiMaster.Function.Excute;
using System.Diagnostics;

namespace ONTWiFiMaster.uCtrl.Sub {
    /// <summary>
    /// Interaction logic for ucMaster.xaml
    /// </summary>
    public partial class ucMaster : UserControl {

        int time_count = 0;
        DispatcherTimer timer = null;

        public ucMaster() {
            InitializeComponent();
            setItemSourceCombobox();
            this.sp_setting.DataContext = myGlobal.mySetting;
            this.DataContext = myGlobal.myMaster;
            this.dgMaster.ItemsSource = myGlobal.collectionMaster;
            loadConfigFile();

            timer = new DispatcherTimer();
            int delay_time = 500;
            timer.Interval = TimeSpan.FromMilliseconds(delay_time);
            timer.Tick += (s, e) => {
                time_count++;
                myGlobal.myMaster.totalTime = myConverter.intToTimeSpan(time_count * delay_time);
                this.scrl_logsystem.ScrollToEnd();
                this.scrl_loguart.ScrollToEnd();
                this.scrl_loginstrument.ScrollToEnd();

                try {
                    this.dgMaster.Items.Refresh();
                    this.dgMaster.ScrollIntoView(this.dgMaster.Items.GetItemAt(myGlobal.myMaster.currentIndex));
                }
                catch { }
            };
        }

        private void setItemSourceCombobox() {
            this.cbb_Port1.ItemsSource = this.cbb_Port2.ItemsSource = myParameter.listPort;
            this.cbb_Time.ItemsSource = new List<int>() { 2, 3, 4, 5};
        }

        private bool loadConfigFile() {
            bool ret = true, r = false;
            ChannelManagementFile cmf = new ChannelManagementFile();
            r = cmf.loadDataFromFile(); if (!r) ret = false;
            AttenuatorConfigFile acf = new AttenuatorConfigFile();
            r = acf.loadDataFromFile(); if (!r) ret = false;
            return ret;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string bTag = b.Tag as string;
            time_count = 0;
            if (loadConfigFile() == false) {
                MessageBox.Show("Không thể load được file config. Vui lòng kiểm tra lại.", "Load File Config", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            loadData(this.cbb_Time);
            myGlobal.myMaster.Init();

            switch (bTag.ToLower()) {
                case "start": {
                        Thread t = new Thread(new ThreadStart(() => {
                            //check trang calib & verify mach
                            if (myGlobal.myTesting.calibResult.Equals("Passed") == false || myGlobal.myTesting.verifyResult.Equals("Passed") == false) {
                                MessageBox.Show("Mạch chưa hoàn thành calib wifi hoặc verify tín hiệu wifi.\nVui lòng kiểm tra lại.", "Lỗi đo mạch master", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            timer.Start();
                            myGlobal.myMaster.buttonContent = "Stop";
                            myGlobal.myTesting.masterResult = "Waiting...";
                            MeasureMasterData mmd = new MeasureMasterData();
                            bool r = mmd.Excuted(myGlobal.listItemMaster);
                            myGlobal.myTesting.masterResult = r ? "Passed" : "Failed";
                            MasterLogFile mlf = new MasterLogFile();
                            mlf.SaveToFile(myGlobal.myMaster);
                            string fname = "";
                            if (r) {
                                MasterFile mf = new MasterFile();
                                bool ret = mf.SaveToFile(myGlobal.myMaster.macAddress, out fname);
                                myGlobal.myMaster.logSystem += $"...Save dữ liệu master ra file: {fname}\n";
                                myGlobal.myMaster.logSystem += $"...Kết quả lưu file: {r}\n";
                            }
                            myGlobal.myMaster.logSystem += $"...Total result: {myGlobal.myMaster.totalResult}\n";
                            myGlobal.myMaster.logSystem += $"...Total time: {myGlobal.myMaster.totalTime}\n";
                            
                            timer.Stop();
                            myGlobal.myMaster.buttonContent = "Start";
                            MessageBox.Show(r ? "Success" : "Failured", "Measure Master Data", MessageBoxButton.OK, r ? MessageBoxImage.Information : MessageBoxImage.Error);
                            if (fname != "") Process.Start(fname);
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
            }
        }

        private void cbb_Time_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox cbb = sender as ComboBox;
            if (cbb.SelectedValue == null) return;
            loadData(cbb);

        }


        private void loadData(ComboBox cbb) {
            int max = int.Parse(cbb.SelectedValue.ToString());
            if (max <= 0) return;

            myGlobal.listItemMaster.Clear();
            myGlobal.collectionMaster.Clear();
            myGlobal.listCalMaster = new List<SignalConfigInfo>();
            this.sp_Main.Children.Clear();

            for (int i = 0; i < max; i++) {
                ItemMasterResult imr = new ItemMasterResult() {
                    labelTitle = $"Đo dữ liệu lần {i + 1}"
                };
                myGlobal.listItemMaster.Add(imr);

                ucItemMaster uim = new ucItemMaster(imr);
                this.sp_Main.Children.Add(uim);
            }

            //add data to collection & list calib
            foreach (var item in myGlobal.listAttenuator) {
                MasterResultInfo mri = new MasterResultInfo() {
                    Frequency = item.Frequency,
                    Antenna = "1",
                    Attenuator = item.Antenna1,
                };
                myGlobal.collectionMaster.Add(mri);
                SignalConfigInfo sci = new SignalConfigInfo() { wifi = "1", rate = "7", anten = "1", bandwidth = "0", channelfreq = item.Frequency };
                myGlobal.listCalMaster.Add(sci);
            }
            foreach (var item in myGlobal.listAttenuator) {
                MasterResultInfo mri = new MasterResultInfo() {
                    Frequency = item.Frequency,
                    Antenna = "2",
                    Attenuator = item.Antenna2,
                };
                myGlobal.collectionMaster.Add(mri);
                SignalConfigInfo sci = new SignalConfigInfo() { wifi = "1", rate = "7", anten = "2", bandwidth = "0", channelfreq = item.Frequency };
                myGlobal.listCalMaster.Add(sci);
            }

            this.dgMaster.Columns[4].Visibility = max >= 2 ? Visibility.Visible : Visibility.Collapsed;
            this.dgMaster.Columns[5].Visibility = max >= 3 ? Visibility.Visible : Visibility.Collapsed;
            this.dgMaster.Columns[6].Visibility = max >= 4 ? Visibility.Visible : Visibility.Collapsed;
            this.dgMaster.Columns[7].Visibility = max >= 5 ? Visibility.Visible : Visibility.Collapsed;
        }
    }


}
