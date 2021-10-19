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
using System.Windows.Shapes;
using GW040Hv2Project.Function;
using Microsoft.Win32;
using System.IO;

namespace GW040Hv2Project.SubWindow {
    /// <summary>
    /// Interaction logic for resetRegisterWindow.xaml
    /// </summary>
    public partial class resetRegisterWindow : Window {

        class registerInfo {
            public string Address { get; set; }
            public string Value { get; set; }
        }
        List<registerInfo> list_register = null;

        public resetRegisterWindow() {
            InitializeComponent();
            GlobalData.rrBinding.Init();
            this.DataContext = GlobalData.rrBinding;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_tag = (string)b.Tag;

            switch (b_tag) {
                case "browser": {
                        OpenFileDialog dlg = new OpenFileDialog();
                        dlg.Filter = "*.csv|*.csv";
                        if (dlg.ShowDialog() == true) {
                            GlobalData.rrBinding.fileConfig = dlg.FileName;
                        }
                        break;
                    }
                case "start": {
                        Thread t = new Thread(new ThreadStart(() => {
                            Dispatcher.Invoke(new Action(() => { b.IsEnabled = false; }));
                            GlobalData.rrBinding.totalResult = "Waiting...";
                            loadFileConfig();
                            loginToOnt();
                            writeRegister();
                            saveFlash();
                            readRegister();
                            closeOnt();
                            GlobalData.rrBinding.totalResult = "Passed";
                            Dispatcher.Invoke(new Action(() => { b.IsEnabled = true; }));
                        }));
                        t.IsBackground = true;
                        t.Start();

                        break;
                    }
                default: break;
            }
        }


        #region reset register

        private void loadFileConfig() {
            GlobalData.rrBinding.logSystem += "Load data from config file: \n";
            GlobalData.rrBinding.logSystem += "---------------------------------------------------------\n";
            if (File.Exists(GlobalData.rrBinding.fileConfig) == false) return;
            list_register = new List<registerInfo>();
            string[] lines = File.ReadAllLines(GlobalData.rrBinding.fileConfig);
            for (int i = 1; i < lines.Length; i++) {
                string data = lines[i];
                if (string.IsNullOrEmpty(data) == false && string.IsNullOrWhiteSpace(data) == false) {
                    string[] buffer = data.Split(',');
                    registerInfo info = new registerInfo() { Address = buffer[4].ToUpper().Replace("0X", ""), Value = buffer[5] };
                    list_register.Add(info);
                }
            }

            GlobalData.rrBinding.logSystem += "Register".PadLeft(10, ' ') + "Value".PadLeft(10, ' ') + "\n";
            foreach (var item in list_register) {
                GlobalData.rrBinding.logSystem += string.Format("{0}{1}\n", item.Address.PadLeft(10, ' '), item.Value.PadLeft(10, ' '));
            }
        }

        private bool loginToOnt() {
            GlobalData.rrBinding.logSystem += "Login to ONT: \n";
            GlobalData.rrBinding.logSystem += "---------------------------------------------------------\n";
            GlobalData.rrBinding.logSystem += string.Format("{0}, user={1}, password={2}\n", GlobalData.initSetting.SERIALPORT, GlobalData.initSetting.ONTUSER, GlobalData.initSetting.ONTPASS);
            GlobalData.MODEM = new ModemTelnet("", 23, GlobalData.initSetting.SERIALPORT);
            bool r = GlobalData.MODEM.Login(GlobalData.initSetting.ONTUSER, GlobalData.initSetting.ONTPASS, 3000);
            GlobalData.rrBinding.logSystem += string.Format("Result = {0}\n", r ? "Passed" : "Failed");
            return r;
        }

        private bool writeRegister() {
            try {
                GlobalData.rrBinding.logSystem += "Write data to register: \n";
                GlobalData.rrBinding.logSystem += "---------------------------------------------------------\n";
                foreach (var item in list_register) {
                    GlobalData.MODEM.Write_Register(item.Address, item.Value);
                    Thread.Sleep(200);
                }
                GlobalData.rrBinding.logSystem += "Completed\n";
                return true;
            }
            catch { return false; }
        }

        private bool saveFlash() {
            try {
                GlobalData.rrBinding.logSystem += "Save flash: \n";
                GlobalData.rrBinding.logSystem += "---------------------------------------------------------\n";
                GlobalData.MODEM.WriteLine("iwpriv rai0 set efuseBufferModeWriteBack=1");
                Thread.Sleep(200);
                GlobalData.MODEM.WriteLine("tcapi set WLan11ac_Common WriteBinToFlash 1");
                Thread.Sleep(500);
                GlobalData.MODEM.WriteLine("tcapi commit WLan11ac");
                Thread.Sleep(20000);
                GlobalData.MODEM.WriteLine("ifconfig ra0 down");
                Thread.Sleep(200);
                GlobalData.MODEM.WriteLine("ifconfig rai0 down");
                Thread.Sleep(500);
                GlobalData.MODEM.WriteLine("rmmod mt7615_ap");
                Thread.Sleep(5000);
                GlobalData.MODEM.WriteLine("insmod lib/modules/mt7615_ap.ko");
                Thread.Sleep(1000);
                GlobalData.MODEM.WriteLine("ifconfig rai0 up");
                Thread.Sleep(20000);
                GlobalData.MODEM.WriteLine("ifconfig ra0 up");
                Thread.Sleep(1000);
                GlobalData.rrBinding.logSystem += "Completed\n";
                return true;
            }
            catch { return false; }
        }

        private bool readRegister() {
            GlobalData.rrBinding.logSystem += "Read data from register: \n";
            GlobalData.rrBinding.logSystem += "---------------------------------------------------------\n";
            foreach (var item in list_register) {
                string data = GlobalData.MODEM.Read_Register(item.Address);
                Thread.Sleep(200);
                GlobalData.rrBinding.logSystem += string.Format("{0}{1}\n", item.Address.PadLeft(10, ' '), data.PadLeft(10, ' '));
            }
            GlobalData.rrBinding.logSystem += "Completed\n";
            return true;
        }

        private bool closeOnt() {
            GlobalData.MODEM.Close();
            return true;
        }


        #endregion









    }
}
