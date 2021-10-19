using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {

    public class resetRegisterBinding : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public resetRegisterBinding() {
            Init();
        }

        public void Init() {
            fileConfig = "";
            logSystem = "";
            totalResult = "";
        }



        string _fileconfig;
        public string fileConfig {
            get { return _fileconfig; }
            set {
                _fileconfig = value;
                OnPropertyChanged(nameof(fileConfig));
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
        string _totalresult;
        public string totalResult {
            get { return _totalresult; }
            set {
                _totalresult = value;
                OnPropertyChanged(nameof(totalResult));
            }
        }



    }
}
