using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class CalibFrequencyResultInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public CalibFrequencyResultInfo() {
            F4Dec = F6DecOld = offsetOld = freqErrorOld = offsetNew = F6DecNew = freqErrorNew = "";
        }

        string _f4dec;
        public string F4Dec {
            get { return _f4dec; }
            set {
                _f4dec = value;
                OnPropertyChanged(nameof(F4Dec));
            }
        }
        string _f6decold;
        public string F6DecOld {
            get { return _f6decold; }
            set {
                _f6decold = value;
                OnPropertyChanged(nameof(F6DecOld));
            }
        }
        string _offsetold;
        public string offsetOld {
            get { return _offsetold; }
            set {
                _offsetold = value;
                OnPropertyChanged(nameof(offsetOld));
            }
        }
        string _freqerrorold;
        public string freqErrorOld {
            get { return _freqerrorold; }
            set {
                _freqerrorold = value;
                OnPropertyChanged(nameof(freqErrorOld));
            }
        }
        string _offsetnew;
        public string offsetNew {
            get { return _offsetnew; }
            set {
                _offsetnew = value;
                OnPropertyChanged(nameof(offsetNew));
            }
        }
        string _f6decnew;
        public string F6DecNew {
            get { return _f6decnew; }
            set {
                _f6decnew = value;
                OnPropertyChanged(nameof(F6DecNew));
            }
        }
        string _freqerrornew;
        public string freqErrorNew {
            get { return _freqerrornew; }
            set {
                _freqerrornew = value;
                OnPropertyChanged(nameof(freqErrorNew));
            }
        }

        public override string ToString() {
            return $"{F4Dec.PadLeft(15, ' ')}{F6DecOld.PadLeft(15, ' ')}{offsetOld.PadLeft(15, ' ')}{freqErrorOld.PadLeft(15, ' ')}{offsetNew.PadLeft(15, ' ')}{F6DecNew.PadLeft(15, ' ')}{freqErrorNew.PadLeft(15, ' ')}";
        }

    }
}
