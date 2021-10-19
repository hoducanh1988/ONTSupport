using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class CalibDataBinding : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public CalibDataBinding() {
            Init();
        }

        public void Init() {
            logSystem = logInstrument = logUart = logError = logRegister = "";
            checkRegisterResult = macAddress = calibFrequencyResult = calib2GResult = calib5GResult = writeBinResult = saveFlashResult = totalResult = "--";
            totalTime = "00:00:00";
            buttonContent = "Start";
            valueCalib2G = maxCalib2G = passedCalib2G = failedCalib2G = 0;
            valueCalib5G = maxCalib5G = passedCalib5G = failedCalib5G = 0;
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
        string _logregister;
        public string logRegister {
            get { return _logregister; }
            set {
                _logregister = value;
                OnPropertyChanged(nameof(logRegister));
            }
        }
        string _logerror;
        public string logError {
            get { return _logerror; }
            set {
                _logerror = value;
                OnPropertyChanged(nameof(logError));
            }
        }
        string _macaddress;
        public string macAddress {
            get { return _macaddress; }
            set {
                _macaddress = value;
                OnPropertyChanged(nameof(macAddress));
            }
        }
        string _calibfreqresult;
        public string calibFrequencyResult {
            get { return _calibfreqresult; }
            set {
                _calibfreqresult = value;
                OnPropertyChanged(nameof(calibFrequencyResult));
            }
        }
        string _calib2gresult;
        public string calib2GResult {
            get { return _calib2gresult; }
            set {
                _calib2gresult = value;
                OnPropertyChanged(nameof(calib2GResult));
            }
        }
        string _calib5gresult;
        public string calib5GResult {
            get { return _calib5gresult; }
            set {
                _calib5gresult = value;
                OnPropertyChanged(nameof(calib5GResult));
            }
        }
        string _writebinresult;
        public string writeBinResult {
            get { return _writebinresult; }
            set {
                _writebinresult = value;
                OnPropertyChanged(nameof(writeBinResult));
            }
        }
        string _saveflashresult;
        public string saveFlashResult {
            get { return _saveflashresult; }
            set {
                _saveflashresult = value;
                OnPropertyChanged(nameof(saveFlashResult));
            }
        }
        string _checkregisterresult;
        public string checkRegisterResult {
            get { return _checkregisterresult; }
            set {
                _checkregisterresult = value;
                OnPropertyChanged(nameof(checkRegisterResult));
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

        int _valuecalib2g;
        public int valueCalib2G {
            get { return _valuecalib2g; }
            set {
                _valuecalib2g = value;
                OnPropertyChanged(nameof(valueCalib2G));
            }
        }
        int _maxcalib2g;
        public int maxCalib2G {
            get { return _maxcalib2g; }
            set {
                _maxcalib2g = value;
                OnPropertyChanged(nameof(maxCalib2G));
            }
        }
        int _passedcalib2g;
        public int passedCalib2G {
            get { return _passedcalib2g; }
            set {
                _passedcalib2g = value;
                OnPropertyChanged(nameof(passedCalib2G));
            }
        }
        int _failedcalib2g;
        public int failedCalib2G {
            get { return _failedcalib2g; }
            set {
                _failedcalib2g = value;
                OnPropertyChanged(nameof(failedCalib2G));
            }
        }
        int _valuecalib5g;
        public int valueCalib5G {
            get { return _valuecalib5g; }
            set {
                _valuecalib5g = value;
                OnPropertyChanged(nameof(valueCalib5G));
            }
        }
        int _maxcalib5g;
        public int maxCalib5G {
            get { return _maxcalib5g; }
            set {
                _maxcalib5g = value;
                OnPropertyChanged(nameof(maxCalib5G));
            }
        }
        int _passedcalib5g;
        public int passedCalib5G {
            get { return _passedcalib5g; }
            set {
                _passedcalib5g = value;
                OnPropertyChanged(nameof(passedCalib5G));
            }
        }
        int _failedcalib5g;
        public int failedCalib5G {
            get { return _failedcalib5g; }
            set {
                _failedcalib5g = value;
                OnPropertyChanged(nameof(failedCalib5G));
            }
        }
    }
}
