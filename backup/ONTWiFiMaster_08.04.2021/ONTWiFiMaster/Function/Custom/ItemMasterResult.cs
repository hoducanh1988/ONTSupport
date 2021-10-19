using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class ItemMasterResult : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public ItemMasterResult() {
            labelTitle = "";
            Result = "";
            valueProgress = 0;
            maxProgress = 1;
        }

        string _labeltitle;
        public string labelTitle {
            get { return _labeltitle; }
            set {
                _labeltitle = value;
                OnPropertyChanged(nameof(labelTitle));
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
        int _valueprogress;
        public int valueProgress {
            get { return _valueprogress; }
            set {
                _valueprogress = value;
                OnPropertyChanged(nameof(valueProgress));
            }
        }
        int _maxprogress;
        public int maxProgress {
            get { return _maxprogress; }
            set {
                _maxprogress = value;
                OnPropertyChanged(nameof(maxProgress));
            }
        }
    }
}
