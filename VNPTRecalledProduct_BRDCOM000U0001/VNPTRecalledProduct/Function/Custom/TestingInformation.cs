using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPTRecalledProduct.Function.Custom {

    public class TestingInformation : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
                Properties.Settings.Default.Save();
            }
        }

        public TestingInformation() {
            InitParam();
        }

        public void InitParam() {
            totalResult = "--";
            inputBarcode = "";
            fileProduct = "";
            fileSetting = "";
            logSystem = "";
            productName = "";
            Station = "";
            appTitle = "";
            Opacity = 1;
            totalTime = "00:00:00";
            totalTestItem = 0;
            testedItemCount = 0;
            errorItemCount = 0;
            errorRate = 0;
            productTested = 0;
            productPassed = 0;
            productFailed = 0;
            productErrorRate = 0;
            continueCheck = false;
            autoStart = false;
        }

        public void Clear() {
            totalResult = "--";
            inputBarcode = "";
            logSystem = "";
            totalTime = "00:00:00";
            testedItemCount = 0;
            errorItemCount = 0;
            errorRate = 0;
        }

        public void waitParam() {
            totalResult = "Waiting...";
            logSystem = "";
            inputBarcode = "";
            totalTime = "00:00:00";
            testedItemCount = 0;
            errorItemCount = 0;
            errorRate = 0;
        }

        public bool passParam() {
            totalResult = "Passed";
            return true;
        }

        public bool failParam() {
            totalResult = "Failed";
            return true;
        }

        bool _show_start_button;
        public bool showStartButton {
            get { return _show_start_button; }
            set {
                _show_start_button = value;
                OnPropertyChanged(nameof(showStartButton));
            }
        }
        bool _auto_start;
        public bool autoStart {
            get { return _auto_start; }
            set {
                _auto_start = value;
                showStartButton = !value;
                OnPropertyChanged(nameof(autoStart));
            }
        }
        bool _continue_check;
        public bool continueCheck {
            get { return _continue_check; }
            set {
                _continue_check = value;
                OnPropertyChanged(nameof(continueCheck));
            }
        }
        double _product_error_rate;
        public double productErrorRate {
            get { return _product_error_rate; }
            set {
                _product_error_rate = value;
                OnPropertyChanged(nameof(productErrorRate));
            }
        }
        int _product_passed;
        public int productPassed {
            get { return _product_passed; }
            set {
                _product_passed = value;
                OnPropertyChanged(nameof(productPassed));
            }
        }
        int _product_failed;
        public int productFailed {
            get { return _product_failed; }
            set {
                _product_failed = value;
                OnPropertyChanged(nameof(productFailed));
            }
        }
        int _product_tested;
        public int productTested {
            get { return _product_tested; }
            set {
                _product_tested = value;
                OnPropertyChanged(nameof(productTested));
            }
        }
        double _error_rate;
        public double errorRate {
            get { return _error_rate; }
            set {
                _error_rate = value;
                OnPropertyChanged(nameof(errorRate));
            }
        }
        int _error_item_count;
        public int errorItemCount {
            get { return _error_item_count; }
            set {
                _error_item_count = value;
                OnPropertyChanged(nameof(errorItemCount));
            }
        }
        int _total_test_item;
        public int totalTestItem {
            get { return _total_test_item; }
            set {
                _total_test_item = value;
                OnPropertyChanged(nameof(totalTestItem));
            }
        }
        int _tested_item_count;
        public int testedItemCount {
            get { return _tested_item_count; }
            set {
                _tested_item_count = value;
                OnPropertyChanged(nameof(testedItemCount));
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
        string _file_product;
        public string fileProduct {
            get { return _file_product; }
            set {
                _file_product = value;
                OnPropertyChanged(nameof(fileProduct));
            }
        }
        string _file_setting;
        public string fileSetting {
            get { return _file_setting; }
            set {
                _file_setting = value;
                OnPropertyChanged(nameof(fileSetting));
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
        string _input_barcode;
        public string inputBarcode {
            get { return _input_barcode; }
            set {
                _input_barcode = value;
                OnPropertyChanged(nameof(inputBarcode));
            }
        }
        string _log_system;
        public string logSystem {
            get { return _log_system; }
            set {
                _log_system = value;
                OnPropertyChanged(nameof(logSystem));
            }
        }
        string _product_name;
        public string productName {
            get { return _product_name; }
            set {
                _product_name = value;
                OnPropertyChanged(nameof(productName));
            }
        }
        string _station;
        public string Station {
            get { return _station; }
            set {
                _station = value;
                OnPropertyChanged(nameof(Station));
            }
        }
        string _app_title;
        public string appTitle {
            get { return _app_title; }
            set {
                _app_title = value;
                OnPropertyChanged(nameof(appTitle));
            }
        }
        double _opacity;
        public double Opacity {
            get { return _opacity; }
            set {
                _opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
    }

}
