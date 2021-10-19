using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {

    public class MasterResultInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public MasterResultInfo() {
            Channel = Frequency = Antenna = Attenuator = Master = Max = Min = Diff = Result = "";
            Power1 = Power2 = Power3 = Power4 = Power5 = "";
        }

        string _channel;
        public string Channel {
            get { return _channel; }
            set {
                _channel = value;
                OnPropertyChanged(nameof(Channel));
            }
        }
        string _frequency;
        public string Frequency {
            get { return _frequency; }
            set {
                _frequency = value;
                OnPropertyChanged(nameof(Frequency));
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
        string _attenuator;
        public string Attenuator {
            get { return _attenuator; }
            set {
                _attenuator = value;
                OnPropertyChanged(nameof(Attenuator));
            }
        }
        string _power1;
        public string Power1 {
            get { return _power1; }
            set {
                _power1 = value;
                OnPropertyChanged(nameof(Power1));
            }
        }
        string _power2;
        public string Power2 {
            get { return _power2; }
            set {
                _power2 = value;
                OnPropertyChanged(nameof(Power2));
            }
        }
        string _power3;
        public string Power3 {
            get { return _power3; }
            set {
                _power3 = value;
                OnPropertyChanged(nameof(Power3));
            }
        }
        string _power4;
        public string Power4 {
            get { return _power4; }
            set {
                _power4 = value;
                OnPropertyChanged(nameof(Power4));
            }
        }
        string _power5;
        public string Power5 {
            get { return _power5; }
            set {
                _power5 = value;
                OnPropertyChanged(nameof(Power5));
            }
        }
        string _master;
        public string Master {
            get { return _master; }
            set {
                _master = value;
                OnPropertyChanged(nameof(Master));
            }
        }
        string _max;
        public string Max {
            get { return _max; }
            set {
                _max = value;
                OnPropertyChanged(nameof(Max));
            }
        }
        string _min;
        public string Min {
            get { return _min; }
            set {
                _min = value;
                OnPropertyChanged(nameof(Min));
            }
        }
        string _diff;
        public string Diff {
            get { return _diff; }
            set {
                _diff = value;
                OnPropertyChanged(nameof(Diff));
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
            return $"{Frequency.PadLeft(15, ' ')}{Antenna.PadLeft(15, ' ')}{Attenuator.PadLeft(15, ' ')}{Power1.PadLeft(15, ' ')}{Power2.PadLeft(15, ' ')}{Power3.PadLeft(15, ' ')}{Power4.PadLeft(15, ' ')}{Power5.PadLeft(15, ' ')}{Master.PadLeft(15, ' ')}{Max.PadLeft(15, ' ')}{Min.PadLeft(15, ' ')}{Diff.PadLeft(15, ' ')}{Result.PadLeft(15, ' ')}";
        }

    }
}
