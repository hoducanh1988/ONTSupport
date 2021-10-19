using System;
using System.Collections.Generic;
using System.IO;
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
using System.Diagnostics;
using System.Windows.Threading;

namespace ONTResetCalibWifi {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        class registerInfo {
            public string Address { get; set; }
            public string Value { get; set; }
        }
        List<registerInfo> list_register = null;

        Function.TestingInformation myTesting = Function.myGlobal.myTesting;
        Function.SettingInformation mySetting = Function.myGlobal.mySetting;

        public MainWindow() {
            InitializeComponent();

            //load combobox
            loadCombobox();

            //binding data
            this.tbi_runall.DataContext = myTesting;
            this.tbi_setting.DataContext = mySetting;

            //timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) => {
                if (myTesting.TotalResult == "Waiting...") {
                    this._scrollSystem.ScrollToEnd();
                    this._scrollUart.ScrollToEnd();
                }
            };
            timer.Start();
        }

        void loadCombobox() {
            List<string> list_com = new List<string>();
            for (int i = 1; i < 100; i++) list_com.Add($"COM{i}");
            cbb_com.ItemsSource = list_com;
            cbb_pass.ItemsSource = new List<string>() { "ttcn@77CN", "VnT3ch@dm1n" };
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_content = (string)b.Content;

            switch (b_content) {
                case "Start": {
                        b.Content = "Stop";
                        Thread t = new Thread(new ThreadStart(() => {
                            bool r = runAll();
                            Dispatcher.Invoke(new Action(() => { b.Content = "Start"; }));
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "Stop": {
                        b.Content = "Start";
                        break;
                    }
            }
        }
        
        private bool runAll() {
            bool r = false;
            Function.OntEconet ont = null;
            myTesting.Wait();

            //load file config
            r = loadFileConfig();
            if (!r) goto END;

            //login to ont
             ont = new Function.OntEconet(mySetting.Comport);
            r = ont.Login(mySetting.LoginUser, mySetting.LoginPass, 300);
            if (!r) goto END;

            //get mac ethernet
            r = getMacEthernet(ont);
            if (!r) goto END;

            //set default
            r = writeRegister(ont);
            if (!r) goto END;

            //save flash
            r = saveFlash(ont);
            if (!r) goto END;

            //read again
            r = readRegister(ont);

        END:
            //close ont
            ont.Dispose();
            var log = new Function.LogFile();
            bool ___ = r ? myTesting.Passed() : myTesting.Failed();
            log.SaveLogSystem(myTesting.LogSystem, myTesting.MacAddress, myTesting.TotalResult);
            log.SaveLogUart(myTesting.LogUart, myTesting.MacAddress, myTesting.TotalResult);
            return r;
        }

        private bool loadFileConfig() {
            Stopwatch st = new Stopwatch();
            st.Start();
            myTesting.LogSystem += "\n+++++++++++ Load File Config Register\n";
            string f = AppDomain.CurrentDomain.BaseDirectory + "Calib_Default_Value.csv";
            bool r = false;
            if (File.Exists(f) == false) goto END;
            list_register = new List<registerInfo>();
            string[] lines = File.ReadAllLines(f);
            for (int i = 1; i < lines.Length; i++) {
                string data = lines[i];
                if (string.IsNullOrEmpty(data) == false && string.IsNullOrWhiteSpace(data) == false) {
                    string[] buffer = data.Split(',');
                    registerInfo info = new registerInfo() { Address = buffer[4].ToUpper().Replace("0X", ""), Value = buffer[5] };
                    list_register.Add(info);
                }
            }
            r = list_register.Count > 0;
            if (r) {
                foreach (var item in list_register) {
                    myTesting.LogSystem += string.Format("{0}{1}\n", item.Address.PadLeft(10, ' '), item.Value.PadLeft(10, ' '));
                }
            }

        END:
            st.Stop();
            myTesting.LogSystem += string.Format("... result: {0}\n", r ? "Passed" : "Failed");
            myTesting.LogSystem += string.Format("... time elapsed: {0} ms\n", st.ElapsedMilliseconds);
            return r;
        }

        private bool getMacEthernet(Function.OntEconet ont) {
            Stopwatch st = new Stopwatch();
            st.Start();
            myTesting.LogSystem += "\n+++++++++++ Get Mac Ethernet\n";
            bool r = false;

            myTesting.MacAddress = ont.Get_Mac();
            r = myTesting.MacAddress.Length == 12;

            st.Stop();
            myTesting.LogSystem += string.Format("... result: {0}\n", r ? "Passed" : "Failed");
            myTesting.LogSystem += string.Format("... time elapsed: {0} ms\n", st.ElapsedMilliseconds);
            return r;

        }

        private bool writeRegister(Function.OntEconet ont) {
            Stopwatch st = new Stopwatch();
            st.Start();
            myTesting.LogSystem += "\n+++++++++++ Set Wifi Register To Default\n";
            bool r = false;
            try {
                foreach (var item in list_register) {
                    ont.Write_Register(item.Address, item.Value);
                }
                r = true;
            }
            catch { goto END; }

        END:
            st.Stop();
            myTesting.LogSystem += string.Format("... result: {0}\n", r ? "Passed" : "Failed");
            myTesting.LogSystem += string.Format("... time elapsed: {0} ms\n", st.ElapsedMilliseconds);
            return r;
        }


        private bool readRegister(Function.OntEconet ont) {
            Stopwatch st = new Stopwatch();
            st.Start();
            myTesting.LogSystem += "\n+++++++++++ Verify Wifi Register After Save Flash\n";
            bool r = false;
            bool ret = true;

            foreach (var item in list_register) {
                string data = ont.Read_Register(item.Address);
                r = data.ToLower().Equals(item.Value.ToLower());
                if (!r) ret = false;
                myTesting.LogSystem += string.Format("{0}{1}{2}\n", item.Address.PadLeft(10, ' '), item.Value.PadLeft(10, ' '), data.PadLeft(10, ' '));
            }

            st.Stop();
            myTesting.LogSystem += string.Format("... result: {0}\n", ret ? "Passed" : "Failed");
            myTesting.LogSystem += string.Format("... time elapsed: {0} ms\n", st.ElapsedMilliseconds);
            return ret;
        }

        private bool saveFlash(Function.OntEconet ont) {
            Stopwatch st = new Stopwatch();
            st.Start();
            myTesting.LogSystem += "\n+++++++++++ Save Flash \n";
            bool r = ont.Save_Flash();

            st.Stop();
            myTesting.LogSystem += string.Format("... result: {0}\n", r ? "Passed" : "Failed");
            myTesting.LogSystem += string.Format("... time elapsed: {0} ms\n", st.ElapsedMilliseconds);
            return r;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            MenuItem mi = sender as MenuItem;
            string m_header = (string)mi.Header;

            switch (m_header) {
                case "Log System": {
                        string dir_system = $"{AppDomain.CurrentDomain.BaseDirectory}LogSystem\\{DateTime.Now.ToString("yyyy-MM-dd")}";
                        if (Directory.Exists(dir_system) == false) Directory.CreateDirectory(dir_system);
                        Process.Start(dir_system);
                        break;
                    }
                case "Log Uart": {
                        string dir_uart = $"{AppDomain.CurrentDomain.BaseDirectory}LogUart\\{DateTime.Now.ToString("yyyy-MM-dd")}";
                        if (Directory.Exists(dir_uart) == false) Directory.CreateDirectory(dir_uart);
                        Process.Start(dir_uart);
                        break;
                    }
                case "Exit": {
                        this.Close();
                        break;
                    }
            }


        }
    }
}
