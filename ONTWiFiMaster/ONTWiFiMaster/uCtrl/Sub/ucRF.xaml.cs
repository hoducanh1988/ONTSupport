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
using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.Function.IO;
using ONTWiFiMaster.Function.Excute;
using System.Windows.Threading;
using UtilityPack.Converter;

namespace ONTWiFiMaster.uCtrl.Sub {
    /// <summary>
    /// Interaction logic for ucRF.xaml
    /// </summary>
    public partial class ucRF : UserControl {

        int time_count = 0;
        DispatcherTimer timer = null;

        public ucRF() {
            InitializeComponent();
            setItemSourceCombobox();
            this.sp_setting1.DataContext = myGlobal.mySetting;
            this.sp_setting2.DataContext = myGlobal.mySetting;

            this.DataContext = myGlobal.myCable;
            AttenuatorConfigFile atc = new AttenuatorConfigFile();
            if (atc.date_diff < 3) { 
                atc.loadFileForShow();
                myGlobal.myTesting.cableResult = "Passed";
            }
            else { atc.loadFileForMeasure(); }

            this.dgAttenuator.ItemsSource = myGlobal.collectionAttenuator;

            timer = new DispatcherTimer();
            int delay_time = 500;
            timer.Interval = TimeSpan.FromMilliseconds(delay_time);
            timer.Tick += (s, e) => {
                time_count++;
                scrl_loginstrument.ScrollToEnd();
                scrl_logsystem.ScrollToEnd();
                if (myGlobal.myCable.buttonContent1.Equals("Stop")) {
                    myGlobal.myCable.totalTime1 = myConverter.intToTimeSpan(time_count * delay_time);
                }
                if (myGlobal.myCable.buttonContent2.Equals("Stop")) {
                    myGlobal.myCable.totalTime2 = myConverter.intToTimeSpan(time_count * delay_time);
                }

                try {
                    this.dgAttenuator.Items.Refresh();
                    this.dgAttenuator.ScrollIntoView(this.dgAttenuator.Items.GetItemAt(myGlobal.myCable.currentIndex));
                }
                catch { }
            };
        }

        private void setItemSourceCombobox() {
            this.cbb_Port11.ItemsSource = this.cbb_Port21.ItemsSource = this.cbb_Port12.ItemsSource = this.cbb_Port22.ItemsSource = myParameter.listPort;
            this.cbb_Power1.ItemsSource = this.cbb_Power2.ItemsSource = myParameter.listPower;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string bTag = b.Tag as string;
            string port1 = "", port2 = "", power = "";
            time_count = 0;
            myGlobal.myCable.Init();

            switch (bTag.ToLower()) {
                case "antenna1": {
                        port1 = myGlobal.mySetting.portTransmitter1;
                        port2 = myGlobal.mySetting.portReceiver1;
                        power = myGlobal.mySetting.powerTransmit1;
                        if (MessageBox.Show($"Xác nhận thông tin máy đo:\n---------------------------------\nPort phát: {port1}\nPort thu: {port2}\nCông suất phát: {power} dBm\n---------------------------------\n", "Đo suy hao dây RF Antenna 1", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                            Thread t = new Thread(new ThreadStart(() => {
                                timer.Start();
                                myGlobal.myCable.buttonContent1 = "Stop";
                                myGlobal.myCable.Result1 = "";
                                foreach (var item in myGlobal.collectionAttenuator) item.Antenna1 = "";
                                MeasureAttenuator matt = new MeasureAttenuator();
                                bool r = matt.Excute(myGlobal.collectionAttenuator, "1", port1, port2, power);
                                myGlobal.myCable.logSystem += $"...Total time: {myGlobal.myCable.totalTime1}\n";
                                myGlobal.myCable.buttonContent1 = "Start";
                                myGlobal.myCable.Result1 = r ? "Passed" : "Failed";
                                RFLogFile rfl = new RFLogFile();
                                rfl.SaveToFile(myGlobal.myCable, "1");
                                switch (myGlobal.myCable.Result2) {
                                    case "": { myGlobal.myTesting.cableResult = "Waiting..."; break; }
                                    case "Passed": {
                                            var it = myGlobal.collectionAttenuator.Where(x => x.Result.Equals("Failed")).FirstOrDefault();
                                            myGlobal.myTesting.cableResult = it == null ? "Passed" : "Failed";
                                            //save data to file
                                            if (myGlobal.myTesting.cableResult == "Passed") {
                                                AttenuatorConfigFile att = new AttenuatorConfigFile();
                                                bool ret1 = att.saveDataToFile("1");
                                                myGlobal.myCable.logSystem += $"...Lưu dữ liệu suy hao dây RF Antenna1 vào file {att.fileName}\n";
                                                myGlobal.myCable.logSystem += $"...Kết quả lưu file là: {ret1}\n";
                                                bool ret2 = att.saveDataToFile("2");
                                                myGlobal.myCable.logSystem += $"...Lưu dữ liệu suy hao dây RF Antenna2 vào file {att.fileName}\n";
                                                myGlobal.myCable.logSystem += $"...Kết quả lưu file là: {ret2}\n";
                                            }
                                            break;
                                        }
                                    case "Failed": { myGlobal.myTesting.cableResult = "Failed"; break; }
                                }
                                timer.Stop();
                                MessageBox.Show(r ? "Success" : "Failured", "Đo suy hao dây RF Antenna 1", MessageBoxButton.OK, r ? MessageBoxImage.Information : MessageBoxImage.Error);
                            }));
                            t.IsBackground = true;
                            t.Start();
                        }
                        break;
                    }
                case "antenna2": {
                        port1 = myGlobal.mySetting.portTransmitter2;
                        port2 = myGlobal.mySetting.portReceiver2;
                        power = myGlobal.mySetting.powerTransmit2;
                        if (MessageBox.Show($"Xác nhận thông tin máy đo:\n---------------------------------\nPort phát: {port1}\nPort thu: {port2}\nCông suất phát: {power} dBm\n---------------------------------\n", "Đo suy hao dây RF Antenna 2", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                            Thread t = new Thread(new ThreadStart(() => {
                                timer.Start();
                                myGlobal.myCable.buttonContent2 = "Stop";
                                myGlobal.myCable.Result2 = "";
                                foreach (var item in myGlobal.collectionAttenuator) item.Antenna2 = "";
                                MeasureAttenuator matt = new MeasureAttenuator();
                                bool r = matt.Excute(myGlobal.collectionAttenuator, "2", port1, port2, power);
                                myGlobal.myCable.logSystem += $"...Total time: {myGlobal.myCable.totalTime2}\n";
                                myGlobal.myCable.buttonContent2 = "Start";
                                myGlobal.myCable.Result2 = r ? "Passed" : "Failed";
                                RFLogFile rfl = new RFLogFile();
                                rfl.SaveToFile(myGlobal.myCable, "2");
                                switch (myGlobal.myCable.Result1) {
                                    case "": { myGlobal.myTesting.cableResult = "Waiting..."; break; }
                                    case "Passed": {
                                            var it = myGlobal.collectionAttenuator.Where(x => x.Result.Equals("Failed")).FirstOrDefault();
                                            myGlobal.myTesting.cableResult = it == null ? "Passed" : "Failed";
                                            //save data to file
                                            if (myGlobal.myTesting.cableResult == "Passed") {
                                                AttenuatorConfigFile att = new AttenuatorConfigFile();
                                                bool ret1 = att.saveDataToFile("1");
                                                myGlobal.myCable.logSystem += $"...Lưu dữ liệu suy hao dây RF Antenna1 vào file {att.fileName}\n";
                                                myGlobal.myCable.logSystem += $"...Kết quả lưu file là: {ret1}\n";
                                                bool ret2 = att.saveDataToFile("2");
                                                myGlobal.myCable.logSystem += $"...Lưu dữ liệu suy hao dây RF Antenna2 vào file {att.fileName}\n";
                                                myGlobal.myCable.logSystem += $"...Kết quả lưu file là: {ret2}\n";
                                            }
                                            break;
                                        }
                                    case "Failed": { myGlobal.myTesting.cableResult = "Failed"; break; }
                                }
                                timer.Stop();
                                MessageBox.Show(r ? "Success" : "Failured", "Đo suy hao dây RF Antenna 2", MessageBoxButton.OK, r ? MessageBoxImage.Information : MessageBoxImage.Error);
                            }));
                            t.IsBackground = true;
                            t.Start();
                        }

                        break;
                    }
            }

        }

    }
}
