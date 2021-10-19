using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class AttenuatorInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public AttenuatorInfo() {
            Init();
        }

        public void Init() {
            Channel = Frequency = Antenna1 = Antenna2 = Result = "";
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
        string _antenna1;
        public string Antenna1 {
            get { return _antenna1; }
            set {
                _antenna1 = value;
                OnPropertyChanged(nameof(Antenna1));
            }
        }
        string _antenna2;
        public string Antenna2 {
            get { return _antenna2; }
            set {
                _antenna2 = value;
                OnPropertyChanged(nameof(Antenna2));
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
            return $"{Channel},{Frequency},{Antenna1},{Antenna2}";
        }

        public string getLogTable() {
            return $"{Channel.PadLeft(15,' ')}{Frequency.PadLeft(15, ' ')}{Antenna1.PadLeft(15, ' ')}{Antenna2.PadLeft(15, ' ')}{Result.PadLeft(15, ' ')}";
        }

    }
}
