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
            totalResult = "--";
            logBinding = "";
            startTime = "";
            endTime = "";
        }

        public void passParams() {
            totalResult = "Passed";
            endTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public void failParams() {
            totalResult = "Failed";
            endTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public void waitParams() {
            totalResult = "Waiting...";
            startTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public void abortParams() {
            totalResult = "--";
            endTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }


        string _logdut;
        public string logDut {
            get { return _logdut; }
            set {
                _logdut = value;
                OnPropertyChanged(nameof(logDut));
            }
        }
        string _logbinding;
        public string logBinding {
            get { return _logbinding; }
            set {
                _logbinding = value;
                OnPropertyChanged(nameof(logBinding));
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
        int _onlineremaining;
        public int onlineRemaining {
            get { return _onlineremaining; }
            set {
                _onlineremaining = value;
                OnPropertyChanged(nameof(onlineRemaining));
            }
        }
        double _timeremaining;
        public double timeRemaining {
            get { return _timeremaining; }
            set {
                _timeremaining = value;
                OnPropertyChanged(nameof(timeRemaining));
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
        string _starttime;
        public string startTime {
            get { return _starttime; }
            set {
                _starttime = value;
                OnPropertyChanged(nameof(startTime));
            }
        }
        string _endtime;
        public string endTime {
            get { return _endtime; }
            set {
                _endtime = value;
                OnPropertyChanged(nameof(endTime));
            }
        }
    }
}
