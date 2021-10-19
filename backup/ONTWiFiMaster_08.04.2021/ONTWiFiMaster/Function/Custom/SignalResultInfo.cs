using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class SignalResultInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public SignalResultInfo() {
            Range = Antenna = WiFi = MCS = Channel = Attenuator = Result = "";
            Power = EVM = freqError = "";
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
        string _wifi;
        public string WiFi {
            get { return _wifi; }
            set {
                _wifi = value;
                OnPropertyChanged(nameof(WiFi));
            }
        }
        string _mcs;
        public string MCS {
            get { return _mcs; }
            set {
                _mcs = value;
                OnPropertyChanged(nameof(MCS));
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
        string _power;
        public string Power {
            get { return _power; }
            set {
                _power = value;
                OnPropertyChanged(nameof(Power));
            }
        }
        string _evm;
        public string EVM {
            get { return _evm; }
            set {
                _evm = value;
                OnPropertyChanged(nameof(EVM));
            }
        }
        string _freqerror;
        public string freqError {
            get { return _freqerror; }
            set {
                _freqerror = value;
                OnPropertyChanged(nameof(freqError));
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
            return $"{Range.PadLeft(15, ' ')}{Antenna.PadLeft(15, ' ')}{WiFi.PadLeft(15, ' ')}{MCS.PadLeft(15, ' ')}{Channel.PadLeft(15, ' ')}{Attenuator.PadLeft(15, ' ')}{Power.PadLeft(15, ' ')}{EVM.PadLeft(15, ' ')}{freqError.PadLeft(15, ' ')}{Result.PadLeft(15, ' ')}";
        }
    }
}
