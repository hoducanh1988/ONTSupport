using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class MasterDataBinding : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public MasterDataBinding() {
            Init();
        }

        public void Init() {
            logSystem = logInstrument = logUart = "";
            macAddress = totalResult = "--";
            totalTime = "00:00:00";
            buttonContent = "Start";
            currentIndex = 0;
        }

        string _macaddress;
        public string macAddress {
            get { return _macaddress; }
            set {
                _macaddress = value;
                OnPropertyChanged(nameof(macAddress));
            }
        }
        string _totalresult;
        public string totalResult {
            get { return _totalresult; }
            set {
                _totalresult = value;
                OnPropertyChanged(nameof(totalResult));
            }
        }
        string _totaltime;
        public string totalTime {
            get { return _totaltime; }
            set {
                _totaltime = value;
                OnPropertyChanged(nameof(totalTime));
            }
        }
        string _buttoncontent;
        public string buttonContent {
            get { return _buttoncontent; }
            set {
                _buttoncontent = value;
                OnPropertyChanged(nameof(buttonContent));
            }
        }
        string _logsystem;
        public string logSystem {
            get { return _logsystem; }
            set {
                _logsystem = value;
                OnPropertyChanged(nameof(logSystem));
            }
        }
        string _loginstrument;
        public string logInstrument {
            get { return _loginstrument; }
            set {
                _loginstrument = value;
                OnPropertyChanged(nameof(logInstrument));
            }
        }
        string _loguart;
        public string logUart {
            get { return _loguart; }
            set {
                _loguart = value;
                OnPropertyChanged(nameof(logUart));
            }
        }
        int _currentindex;
        public int currentIndex {
            get { return _currentindex; }
            set {
                _currentindex = value;
                OnPropertyChanged(nameof(currentIndex));
            }
        }
    }
}
