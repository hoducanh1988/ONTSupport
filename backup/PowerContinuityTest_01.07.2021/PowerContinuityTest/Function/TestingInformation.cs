using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerContinuityTest.Function {

    public class TestingInformation : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public TestingInformation() {

        }

        public void initParams() {
            logDut = "";
            logController = "";
        }


        string _logdut;
        public string logDut {
            get { return _logdut; }
            set {
                _logdut = value;
                OnPropertyChanged(nameof(logDut));
            }
        }
        string _logcontroller;
        public string logController {
            get { return _logcontroller; }
            set {
                _logcontroller = value;
                OnPropertyChanged(nameof(logController));
            }
        }
        int _loopremaining;
        public int loopRemaining {
            get { return _loopremaining; }
            set {
                _loopremaining = value;
                OnPropertyChanged(nameof(loopRemaining));
            }
        }
        int _timeremaining;
        public int timeRemaining {
            get { return _timeremaining; }
            set {
                _timeremaining = value;
                OnPropertyChanged(nameof(timeRemaining));
            }
        }
    }
}
