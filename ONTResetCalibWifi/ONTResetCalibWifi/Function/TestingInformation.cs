using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTResetCalibWifi.Function {
    public class TestingInformation : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
                Properties.Settings.Default.Save();
            }
        }

        public TestingInformation() {
            Init();
        }

        public void Init() {
            LogSystem = "";
            LogUart = "";
            TotalResult = "-";
            MacAddress = "";
        }

        public void Wait() {
            LogSystem = "";
            LogUart = "";
            MacAddress = "";
            TotalResult = "Waiting...";
        }

        public bool Passed() {
            TotalResult = "Passed";
            return true;
        } 

        public bool Failed() {
            TotalResult = "Failed";
            return true;
        }


        string _log_system;
        public string LogSystem {
            get { return _log_system; }
            set {
                _log_system = value;
                OnPropertyChanged(nameof(LogSystem));
            }
        }
        string _log_uart;
        public string LogUart {
            get { return _log_uart; }
            set {
                _log_uart = value;
                OnPropertyChanged(nameof(LogUart));
            }
        }
        string _total_result;
        public string TotalResult {
            get { return _total_result; }
            set {
                _total_result = value;
                OnPropertyChanged(nameof(TotalResult));
            }
        }
        string _mac_address;
        public string MacAddress {
            get { return _mac_address; }
            set {
                _mac_address = value;
                OnPropertyChanged(nameof(MacAddress));
            }
        }
        
    }
}
