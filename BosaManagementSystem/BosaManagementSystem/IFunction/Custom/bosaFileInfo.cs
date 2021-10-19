using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BosaManagementSystem.IFunction.Custom {
    public class bosaFileInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public bosaFileInfo() {
            Index = "";
            FileName = "";
            FileMD5 = "";
            InputMD5 = "";
            BosaCounted = "";
            Status = "";
            Result = "";
        }

        string _index;
        public string Index {
            get { return _index; }
            set {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }
        string _file_name;
        public string FileName {
            get { return _file_name; }
            set {
                _file_name = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        string _file_md5;
        public string FileMD5 {
            get { return _file_md5; }
            set {
                _file_md5 = value;
                OnPropertyChanged(nameof(FileMD5));
            }
        }
        string _input_md5;
        public string InputMD5 {
            get { return _input_md5; }
            set {
                _input_md5 = value;
                OnPropertyChanged(nameof(InputMD5));
            }
        }
        string _bosa_counted;
        public string BosaCounted {
            get { return _bosa_counted; }
            set {
                _bosa_counted = value;
                OnPropertyChanged(nameof(BosaCounted));
            }
        }
        string _status;
        public string Status {
            get { return _status; }
            set {
                _status = value;
                OnPropertyChanged(nameof(Status));
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
    }
}
