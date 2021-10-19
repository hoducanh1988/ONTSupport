using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {

    public class MainWindowDataBinding : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public MainWindowDataBinding() {
            appInfo = "Version: ONT001VN0U0001 - Build time: 19/01/2021 14:20 - Copyright of VNPT Technology 2021";
            productName = "";
            productStation = "ĐO DỮ LIỆU WIFI MASTER SẢN PHẨM ONT";
            modeRD = false;
        }

        string _appinfo;
        public string appInfo {
            get { return _appinfo; }
            set {
                _appinfo = value;
                OnPropertyChanged(nameof(appInfo));
            }
        }
        string _productname;
        public string productName {
            get { return _productname; }
            set {
                _productname = value;
                OnPropertyChanged(nameof(productName));
            }
        }
        string _productstation;
        public string productStation {
            get { return _productstation; }
            set {
                _productstation = value;
                OnPropertyChanged(nameof(productStation));
            }
        }
        bool _moderd;
        public bool modeRD {
            get { return _moderd; }
            set {
                _moderd = value;
                OnPropertyChanged(nameof(modeRD));
            }
        }

    }
}
