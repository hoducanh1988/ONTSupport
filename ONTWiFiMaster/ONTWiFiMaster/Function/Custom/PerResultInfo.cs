using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class PerResultInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public PerResultInfo() {
            Range = Antenna = WiFi = MCS = Channel = Attenuator = powerTransmit = waveForm = packetSent = packetReceived = PER = Result = "";
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
        string _powertransmit;
        public string powerTransmit {
            get { return _powertransmit; }
            set {
                _powertransmit = value;
                OnPropertyChanged(nameof(powerTransmit));
            }
        }
        string _waveform;
        public string waveForm {
            get { return _waveform; }
            set {
                _waveform = value;
                OnPropertyChanged(nameof(waveForm));
            }
        }
        string _packetsent;
        public string packetSent {
            get { return _packetsent; }
            set {
                _packetsent = value;
                OnPropertyChanged(nameof(packetSent));
            }
        }
        string _packetreceived;
        public string packetReceived {
            get { return _packetreceived; }
            set {
                _packetreceived = value;
                OnPropertyChanged(nameof(packetReceived));
            }
        }
        string _per;
        public string PER {
            get { return _per; }
            set {
                _per = value;
                OnPropertyChanged(nameof(PER));
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
            return $"{Range.PadLeft(15, ' ')}{Antenna.PadLeft(15, ' ')}{WiFi.PadLeft(15, ' ')}{MCS.PadLeft(15, ' ')}{Channel.PadLeft(15, ' ')}{Attenuator.PadLeft(15, ' ')}{powerTransmit.PadLeft(15, ' ')}{packetSent.PadLeft(15, ' ')}{packetReceived.PadLeft(15, ' ')}{PER.PadLeft(15, ' ')}{Result.PadLeft(15, ' ')}";
        }

    }
}
