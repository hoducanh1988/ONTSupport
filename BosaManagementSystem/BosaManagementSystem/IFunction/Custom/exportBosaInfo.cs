using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BosaManagementSystem.IFunction.Custom {
    public class exportBosaInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public exportBosaInfo() {
            Init();
        }

        public void Init() {
            bosaSerial = "";
            Ith = "";
            Vbr = "";
            bosaFile = "";
            exportFolderPath = "";
            exportFileName = "";
            totalTime = "00:00:00";
            progressValue = 0;
            progressMax = 1;
            totalResult = "";
        }

        string _bosa_serial;
        public string bosaSerial {
            get { return _bosa_serial; }
            set {
                _bosa_serial = value;
                OnPropertyChanged(nameof(bosaSerial));
            }
        }
        string _ith;
        public string Ith {
            get { return _ith; }
            set {
                _ith = value;
                OnPropertyChanged(nameof(Ith));
            }
        }
        string _vbr;
        public string Vbr {
            get { return _vbr; }
            set {
                _vbr = value;
                OnPropertyChanged(nameof(Vbr));
            }
        }
        string _bosa_file;
        public string bosaFile {
            get { return _bosa_file; }
            set {
                _bosa_file = value;
                OnPropertyChanged(nameof(bosaFile));
            }
        }
        string _export_folder_path;
        public string exportFolderPath {
            get { return _export_folder_path; }
            set {
                _export_folder_path = value;
                OnPropertyChanged(nameof(exportFolderPath));
            }
        }
        string _export_file_name;
        public string exportFileName {
            get { return _export_file_name; }
            set {
                _export_file_name = value;
                OnPropertyChanged(nameof(exportFileName));
            }
        }
        int _split_value;
        public int splitValue {
            get { return _split_value; }
            set {
                _split_value = value;
                OnPropertyChanged(nameof(splitValue));
            }
        }
        int _progress_value;
        public int progressValue {
            get { return _progress_value; }
            set {
                _progress_value = value;
                OnPropertyChanged(nameof(progressValue));
            }
        }
        int _progress_max;
        public int progressMax {
            get { return _progress_max; }
            set {
                _progress_max = value;
                OnPropertyChanged(nameof(progressMax));
            }
        }
        string _total_time;
        public string totalTime {
            get { return _total_time; }
            set {
                _total_time = value;
                OnPropertyChanged(nameof(totalTime));
            }
        }
        string _total_result;
        public string totalResult {
            get { return _total_result; }
            set {
                _total_result = value;
                OnPropertyChanged(nameof(totalResult));
            }
        }
    }
}
