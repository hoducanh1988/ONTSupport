using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class CalibPowerResultInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public CalibPowerResultInfo() {
            Init();
        }

        public void Init() {
            Range = Antenna = Channel = Attenuator = registerAddress = 
                currentValue = newValue = differencePower = powerValue1 = powerValue2 = powerValue3 = Result = "";
        }

        string _range;
        public string Range {
            get { return _range; }
            set {
                _range = value;
                OnPropertyChanged(nameof(Range));
            }
        }
        string _antenna;
        public string Antenna {
            get { return _antenna; }
            set {
                _antenna = value;
                OnPropertyChanged(nameof(Antenna));
            }
        }
        string _channel;
        public string Channel {
            get { return _channel; }
            set {
                _channel = value;
                OnPropertyChanged(nameof(Channel));
            }
        }
        string _attenuator;
        public string Attenuator {
            get { return _attenuator; }
            set {
                _attenuator = value;
                OnPropertyChanged(nameof(Attenuator));
            }
        }
        string _registeraddress;
        public string registerAddress {
            get { return _registeraddress; }
            set {
                _registeraddress = value;
                OnPropertyChanged(nameof(registerAddress));
            }
        }
        string _currentvalue;
        public string currentValue {
            get { return _currentvalue; }
            set {
                _currentvalue = value;
                OnPropertyChanged(nameof(currentValue));
            }
        }
        string _newvalue;
        public string newValue {
            get { return _newvalue; }
            set {
                _newvalue = value;
                OnPropertyChanged(nameof(newValue));
            }
        }
        string _differencepower;
        public string differencePower {
            get { return _differencepower; }
            set {
                _differencepower = value;
                OnPropertyChanged(nameof(differencePower));
            }
        }
        string _powervalue1;
        public string powerValue1 {
            get { return _powervalue1; }
            set {
                _powervalue1 = value;
                OnPropertyChanged(nameof(powerValue1));
            }
        }
        string _powervalue2;
        public string powerValue2 {
            get { return _powervalue2; }
            set {
                _powervalue2 = value;
                OnPropertyChanged(nameof(powerValue2));
            }
        }
        string _powervalue3;
        public string powerValue3 {
            get { return _powervalue3; }
            set {
                _powervalue3 = value;
                OnPropertyChanged(nameof(powerValue3));
            }
        }
        string _result;
        public string Result {
            get { return _result; }
            set {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public override string ToString() {
            return $"{Range.PadLeft(15, ' ')}{Antenna.PadLeft(15, ' ')}{Channel.PadLeft(15, ' ')}{Attenuator.PadLeft(15, ' ')}{registerAddress.PadLeft(15, ' ')}{currentValue.PadLeft(15, ' ')}{powerValue1.PadLeft(15, ' ')}{powerValue2.PadLeft(15, ' ')}{powerValue3.PadLeft(15, ' ')}{differencePower.PadLeft(15, ' ')}{newValue.PadLeft(15, ' ')}{Result.PadLeft(15, ' ')}";
        }

    }
}
