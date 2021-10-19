using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class CableDataBinding : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public CableDataBinding() {
            Init();
            Result1 = "";
            Result2 = "";
        }

        public void Init() {
            logSystem = logInstrument = "";
            progressValue1 = 0;
            progressMax1 = 1;
            progressValue2 = 0;
            progressMax2 = 1;
            totalTime1 = totalTime2 = "00:00:00";
            buttonContent1 = buttonContent2 = "Start";
            currentIndex = 0;
        }

        int _progressvalue1;
        public int progressValue1 {
            get { return _progressvalue1; }
            set {
                _progressvalue1 = value;
                OnPropertyChanged(nameof(progressValue1));
            }
        }
        int _progressmax1;
        public int progressMax1 {
            get { return _progressmax1; }
            set {
                _progressmax1 = value;
                OnPropertyChanged(nameof(progressMax1));
            }
        }
        int _progressvalue2;
        public int progressValue2 {
            get { return _progressvalue2; }
            set {
                _progressvalue2 = value;
                OnPropertyChanged(nameof(progressValue2));
            }
        }
        int _progressmax2;
        public int progressMax2 {
            get { return _progressmax2; }
            set {
                _progressmax2 = value;
                OnPropertyChanged(nameof(progressMax2));
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
        string _totaltime1;
        public string totalTime1 {
            get { return _totaltime1; }
            set {
                _totaltime1 = value;
                OnPropertyChanged(nameof(totalTime1));
            }
        }
        string _totaltime2;
        public string totalTime2 {
            get { return _totaltime2; }
            set {
                _totaltime2 = value;
                OnPropertyChanged(nameof(totalTime2));
            }
        }
        string _buttoncontent1;
        public string buttonContent1 {
            get { return _buttoncontent1; }
            set {
                _buttoncontent1 = value;
                OnPropertyChanged(nameof(buttonContent1));
            }
        }
        string _buttoncontent2;
        public string buttonContent2 {
            get { return _buttoncontent2; }
            set {
                _buttoncontent2 = value;
                OnPropertyChanged(nameof(buttonContent2));
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
        string _result1;
        public string Result1 {
            get { return _result1; }
            set {
                _result1 = value;
                OnPropertyChanged(nameof(Result1));
            }
        }
        string _result2;
        public string Result2 {
            get { return _result2; }
            set {
                _result2 = value;
                OnPropertyChanged(nameof(Result2));
            }
        }
    }
}
