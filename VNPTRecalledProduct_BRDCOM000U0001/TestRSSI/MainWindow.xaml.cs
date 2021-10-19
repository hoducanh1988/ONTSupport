using NativeWifi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace TestRSSI {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        volatile bool total_result = false;
        volatile List<double> listRSSI = new List<double>();


        public MainWindow() {
            InitializeComponent();

            Label lb1 = new Label();
            lb1.Content = "Phần mềm đang kết nối wifi tới ONT.";
            lb1.Margin = new Thickness(-20, -50, 0, 0);
            lb1.FontSize = 25;
            lb1.FontWeight = FontWeights.SemiBold;
            lb1.Foreground = Brushes.Black;
            this.cvs_main.Children.Add(lb1);

            Label lb2 = new Label();
            lb2.Content = "Vui lòng đợi trong giây lát...";
            lb2.Margin = new Thickness(-20, -10, 0, 0);
            lb2.FontSize = 20;
            lb2.FontWeight = FontWeights.SemiBold;
            lb2.Foreground = Brushes.Black;
            this.cvs_main.Children.Add(lb2);

            string[] data = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}rssiin.txt");
            string ssid = data[0].Split('=')[1];
            string wifi_pass = data[1].Split('=')[1];
            string sleep_connection = data[2].Split('=')[1];
            string antenna = data[3].Split('=')[1];
            string max_time = data[4].Split('=')[1];
            string lower_limit = data[5].Split('=')[1];
            string min_dbm = data[6].Split('=')[1];

            Thread t = new Thread(new ThreadStart(() => {
                using (var sw = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}connectin.txt")) {
                    sw.WriteLine($"ssid_name={ssid}");
                    sw.WriteLine($"wifi_password={wifi_pass}");
                    sw.WriteLine($"sleep={sleep_connection}");


                }
                File.Delete($"{AppDomain.CurrentDomain.BaseDirectory}connectout.txt");
                Thread.Sleep(100);
                Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}connectwifi.exe");

            STA:
                if (!File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}connectout.txt")) {
                    Thread.Sleep(100);
                    goto STA;
                }

                string conn_result = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}connectout.txt");
                if (conn_result.ToLower().Contains("failed")) goto END;

                WlanClient client = new WlanClient();
                int count = 0;

            RE:
                count++;
                Thread.Sleep(1000);
                WiFiHelper wifi = new WiFiHelper(client, ssid);
                string rssi = wifi.getRSSID();
                if (rssi == null) goto RE;
                listRSSI.Add(double.Parse(rssi));
                Dispatcher.Invoke(new Action(() => {
                    RSSIChartHelper rssi_chart = new RSSIChartHelper(this.cvs_main, ssid, listRSSI, max_time, min_dbm, lower_limit, antenna);
                    rssi_chart.drawingRSSIChart();
                    total_result = rssi_chart.totalResult.Contains("Passed") ? true : false;
                }));

                if (count <= (int.Parse(max_time))) goto RE;

                END:
                Dispatcher.Invoke(new Action(() => { this.Close(); }));

            }));
            t.IsBackground = true;
            t.Start();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            using (var sw = new StreamWriter($"{AppDomain.CurrentDomain.BaseDirectory}rssiout.txt", false, Encoding.Unicode)) {
                sw.WriteLine(total_result ? "Passed" : "Failed");
                sw.WriteLine(Math.Round(listRSSI.Average(), 2).ToString());
            }
        }
    }
}
