using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerContinuityTest.Function {

    public class SettingInformation : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
                Properties.Settings.Default.Save();
            }
        }

        public string controllerPort {
            get { return Properties.Settings.Default.controllerPort; }
            set {
                Properties.Settings.Default.controllerPort = value;
                OnPropertyChanged(nameof(controllerPort));
            }
        }
        public string dutPort {
            get { return Properties.Settings.Default.dutPort; }
            set {
                Properties.Settings.Default.dutPort = value;
                OnPropertyChanged(nameof(dutPort));
            }
        }
        public string opTime {
            get { return Properties.Settings.Default.opTime; }
            set {
                Properties.Settings.Default.opTime = value;
                OnPropertyChanged(nameof(opTime));
            }
        }
        public string clTime {
            get { return Properties.Settings.Default.clTime; }
            set {
                Properties.Settings.Default.clTime = value;
                OnPropertyChanged(nameof(clTime));
            }
        }
            
        public string allowTime {
            get { return Properties.Settings.Default.allowTime; }
            set {
                Properties.Settings.Default.allowTime = value;
                OnPropertyChanged(nameof(allowTime));
            }
        }
        public string loopTime {
            get { return Properties.Settings.Default.loopTime; }
            set {
                Properties.Settings.Default.loopTime = value;
                OnPropertyChanged(nameof(loopTime));
            }
        }
        public string logString {
            get { return Properties.Settings.Default.logString; }
            set {
                Properties.Settings.Default.logString = value;
                OnPropertyChanged(nameof(logString));
            }
        }


    }
}
