using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BosaManagementSystem.IFunction.Global;

namespace BosaManagementSystem.IFunction.Custom {

    public class importBosaInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public importBosaInfo() {
            Init();
        }

        public void Init() {
            bosaSerialProgressValue = 0;
            bosaSerialMaxValue = 1;
            bosaFileProgressValue = 0;
            bosaFileMaxValue = 1;
            logImport = "";
            importFileName = "";
            totalTime = "00:00:00";
        }

        string _bosa_folder_path;
        public string bosaFolderPath {
            get { return _bosa_folder_path; }
            set {
                _bosa_folder_path = value;
                if (value.Trim().Length == 0) {
                    if (myGlobal.datagridBosaFile != null) myGlobal.datagridBosaFile.Clear();
                }
                OnPropertyChanged(nameof(bosaFolderPath));
            }
        }
        string _import_file_name;
        public string importFileName {
            get { return _import_file_name; }
            set {
                _import_file_name = value;
                OnPropertyChanged(nameof(importFileName));
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
        int _bosa_file_progess_value;
        public int bosaFileProgressValue {
            get { return _bosa_file_progess_value; }
            set {
                _bosa_file_progess_value = value;
                OnPropertyChanged(nameof(bosaFileProgressValue));
            }
        }
        int _bosa_file_max_value;
        public int bosaFileMaxValue {
            get { return _bosa_file_max_value; }
            set {
                _bosa_file_max_value = value;
                OnPropertyChanged(nameof(bosaFileMaxValue));
            }
        }
        int _bosa_serial_progess_value;
        public int bosaSerialProgressValue {
            get { return _bosa_serial_progess_value; }
            set {
                _bosa_serial_progess_value = value;
                OnPropertyChanged(nameof(bosaSerialProgressValue));
            }
        }
        int _bosa_serial_max_value;
        public int bosaSerialMaxValue {
            get { return _bosa_serial_max_value; }
            set {
                _bosa_serial_max_value = value;
                OnPropertyChanged(nameof(bosaSerialMaxValue));
            }
        }
        string _log_import;
        public string logImport {
            get { return _log_import; }
            set {
                _log_import = value;
                OnPropertyChanged(nameof(logImport));
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
        
    }
}
