using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class TestingDataBinding : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public TestingDataBinding() {
            Init();
        }

        public void Init() {
            cableResult = calibResult = verifyResult = masterResult = "--";
            progressValue = 0;
            progressMax = 4;
            showIndex = 0;
        }


        string _cableresult;
        public string cableResult {
            get { return _cableresult; }
            set {
                _cableresult = value;
                OnPropertyChanged(nameof(cableResult));
            }
        }
        string _calibresult;
        public string calibResult {
            get { return _calibresult; }
            set {
                _calibresult = value;
                OnPropertyChanged(nameof(calibResult));
            }
        }
        string _verifyresult;
        public string verifyResult {
            get { return _verifyresult; }
            set {
                _verifyresult = value;
                OnPropertyChanged(nameof(verifyResult));
            }
        }
        string _masterresult;
        public string masterResult {
            get { return _masterresult; }
            set {
                _masterresult = value;
                OnPropertyChanged(nameof(masterResult));
            }
        }
        int _progressvalue;
        public int progressValue {
            get { return _progressvalue; }
            set {
                _progressvalue = value;
                OnPropertyChanged(nameof(progressValue));
            }
        }
        int _progressmax;
        public int progressMax {
            get { return _progressmax; }
            set {
                _progressmax = value;
                OnPropertyChanged(nameof(progressMax));
            }
        }
        int _showindex;
        public int showIndex {
            get { return _showindex; }
            set {
                _showindex = value;
                OnPropertyChanged(nameof(showIndex));
            }
        }

    }
}
