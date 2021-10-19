using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTResetCalibWifi.Function {
    public class SettingInformation : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
                Properties.Settings.Default.Save();
            }
        }

        public string Comport {
            get { return Properties.Settings.Default.comport; }
            set {
                Properties.Settings.Default.comport = value;
                OnPropertyChanged(nameof(Comport));
            }
        }
        public string LoginUser {
            get { return Properties.Settings.Default.user; }
            set {
                Properties.Settings.Default.user = value;
                OnPropertyChanged(nameof(LoginUser));
            }
        }
        public string LoginPass {
            get { return Properties.Settings.Default.pass; }
            set {
                Properties.Settings.Default.pass = value;
                OnPropertyChanged(nameof(LoginPass));
            }
        }

    }
}
