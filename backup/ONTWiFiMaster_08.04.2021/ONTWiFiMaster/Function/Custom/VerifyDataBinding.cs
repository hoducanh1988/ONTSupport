using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class VerifyDataBinding : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public VerifyDataBinding() {
            Init();
        }

        public void Init() {
            logSystem = logInstrument = logUart = "";
            macAddress = per2GResult = per5GResult = signal2GResult = signal5GResult = totalResult = "--";
            totalTime = "00:00:00";
            buttonContent = "Start";
            valueSignal2G = maxSignal2G = passedSignal2G = failedSignal2G = valueSignal5G = maxSignal5G = passedSignal5G = failedSignal5G = 0;
            valuePER2G = maxPER2G = passedPER2G = failedPER2G = valuePER5G = maxPER5G = passedPER5G = failedPER5G = 0;

        }
        string _macaddress;
        public string macAddress {
            get { return _macaddress; }
            set {
                _macaddress = value;
                OnPropertyChanged(nameof(macAddress));
            }
        }
        string _per2gresult;
        public string per2GResult {
            get { return _per2gresult; }
            set {
                _per2gresult = value;
                OnPropertyChanged(nameof(per2GResult));
            }
        }
        string _per5gresult;
        public string per5GResult {
            get { return _per5gresult; }
            set {
                _per5gresult = value;
                OnPropertyChanged(nameof(per5GResult));
            }
        }
        string _signal2gresult;
        public string signal2GResult {
            get { return _signal2gresult; }
            set {
                _signal2gresult = value;
                OnPropertyChanged(nameof(signal2GResult));
            }
        }
        string _signal5gresult;
        public string signal5GResult {
            get { return _signal5gresult; }
            set {
                _signal5gresult = value;
                OnPropertyChanged(nameof(signal5GResult));
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
        int _valuesignal2g;
        public int valueSignal2G {
            get { return _valuesignal2g; }
            set {
                _valuesignal2g = value;
                OnPropertyChanged(nameof(valueSignal2G));
            }
        }
        int _maxsignal2g;
        public int maxSignal2G {
            get { return _maxsignal2g; }
            set {
                _maxsignal2g = value;
                OnPropertyChanged(nameof(maxSignal2G));
            }
        }
        int _passedsignal2g;
        public int passedSignal2G {
            get { return _passedsignal2g; }
            set {
                _passedsignal2g = value;
                OnPropertyChanged(nameof(passedSignal2G));
            }
        }
        int _failedsignal2g;
        public int failedSignal2G {
            get { return _failedsignal2g; }
            set {
                _failedsignal2g = value;
                OnPropertyChanged(nameof(failedSignal2G));
            }
        }
        int _valuesignal5g;
        public int valueSignal5G {
            get { return _valuesignal5g; }
            set {
                _valuesignal5g = value;
                OnPropertyChanged(nameof(valueSignal5G));
            }
        }
        int _maxsignal5g;
        public int maxSignal5G {
            get { return _maxsignal5g; }
            set {
                _maxsignal5g = value;
                OnPropertyChanged(nameof(maxSignal5G));
            }
        }
        int _passedsignal5g;
        public int passedSignal5G {
            get { return _passedsignal5g; }
            set {
                _passedsignal5g = value;
                OnPropertyChanged(nameof(passedSignal5G));
            }
        }
        int _failedsignal5g;
        public int failedSignal5G {
            get { return _failedsignal5g; }
            set {
                _failedsignal5g = value;
                OnPropertyChanged(nameof(failedSignal5G));
            }
        }

        int _valueper2g;
        public int valuePER2G {
            get { return _valueper2g; }
            set {
                _valueper2g = value;
                OnPropertyChanged(nameof(valuePER2G));
            }
        }
        int _maxper2g;
        public int maxPER2G {
            get { return _maxper2g; }
            set {
                _maxper2g = value;
                OnPropertyChanged(nameof(maxPER2G));
            }
        }
        int _passedper2g;
        public int passedPER2G {
            get { return _passedper2g; }
            set {
                _passedper2g = value;
                OnPropertyChanged(nameof(passedPER2G));
            }
        }
        int _failedper2g;
        public int failedPER2G {
            get { return _failedper2g; }
            set {
                _failedper2g = value;
                OnPropertyChanged(nameof(failedPER2G));
            }
        }

        int _valueper5g;
        public int valuePER5G {
            get { return _valueper5g; }
            set {
                _valueper5g = value;
                OnPropertyChanged(nameof(valuePER5G));
            }
        }
        int _maxper5g;
        public int maxPER5G {
            get { return _maxper5g; }
            set {
                _maxper5g = value;
                OnPropertyChanged(nameof(maxPER5G));
            }
        }
        int _passedper5g;
        public int passedPER5G {
            get { return _passedper5g; }
            set {
                _passedper5g = value;
                OnPropertyChanged(nameof(passedPER5G));
            }
        }
        int _failedper5g;
        public int failedPER5G {
            get { return _failedper5g; }
            set {
                _failedper5g = value;
                OnPropertyChanged(nameof(failedPER5G));
            }
        }
    }
}
