using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class SettingDataBinding : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public SettingDataBinding() {
            serialPortName = "COM1";
            loginUser = "admin";
            loginPassword = "VnT3ch@dm1n";
            instrumentType = "E6640A";
            gpibAddress = "";

            Port1 = "PORT1";
            Port2 = "PORT2";

            portTransmitter1 = "PORT1";
            portReceiver1 = "PORT3";
            powerTransmit1 = "-20";
            Connector1 = "0.3";
            portTransmitter2 = "PORT2";
            portReceiver2 = "PORT3";
            powerTransmit2 = "-20";
            Connector2 = "0.3";

            calibFreq = true;
            calib2G = true;
            calib5G = true;
            writeBIN = true;
            saveFlash = true;
            checkRegister = true;
            target2G = "19.5";
            target5G = "18";
            lowerLimit = "10";
            upperLimit = "30";

            testPer2G = true;
            testPer5G = true;
            testSignal2G = true;
            testSignal5G = true;

             measureTime = "3";
            Difference = "0.5";
        }

        #region Setting

        string _serialportname;
        public string serialPortName {
            get { return _serialportname; }
            set {
                _serialportname = value;
                OnPropertyChanged(nameof(serialPortName));
            }
        }
        string _loginuser;
        public string loginUser {
            get { return _loginuser; }
            set {
                _loginuser = value;
                OnPropertyChanged(nameof(loginUser));
            }
        }
        string _loginpassword;
        public string loginPassword {
            get { return _loginpassword; }
            set {
                _loginpassword = value;
                OnPropertyChanged(nameof(loginPassword));
            }
        }
        string _instrumenttype;
        public string instrumentType {
            get { return _instrumenttype; }
            set {
                _instrumenttype = value;
                OnPropertyChanged(nameof(instrumentType));
            }
        }
        string _gpibaddress;
        public string gpibAddress {
            get { return _gpibaddress; }
            set {
                _gpibaddress = value;
                OnPropertyChanged(nameof(gpibAddress));
            }
        }

        #endregion

        #region Common

        string _port1;
        public string Port1 {
            get { return _port1; }
            set {
                _port1 = value;
                OnPropertyChanged(nameof(Port1));
            }
        }
        string _port2;
        public string Port2 {
            get { return _port2; }
            set {
                _port2 = value;
                OnPropertyChanged(nameof(Port2));
            }
        }
        #endregion

        #region Do suy hao RF

        string _porttransmitter1;
        public string portTransmitter1 {
            get { return _porttransmitter1; }
            set {
                _porttransmitter1 = value;
                OnPropertyChanged(nameof(portTransmitter1));
            }
        }
        string _portreceiver1;
        public string portReceiver1 {
            get { return _portreceiver1; }
            set {
                _portreceiver1 = value;
                OnPropertyChanged(nameof(portReceiver1));
            }
        }
        string _powertransmit1;
        public string powerTransmit1 {
            get { return _powertransmit1; }
            set {
                _powertransmit1 = value;
                OnPropertyChanged(nameof(powerTransmit1));
            }
        }


        string _porttransmitter2;
        public string portTransmitter2 {
            get { return _porttransmitter2; }
            set {
                _porttransmitter2 = value;
                OnPropertyChanged(nameof(portTransmitter2));
            }
        }
        string _portreceiver2;
        public string portReceiver2 {
            get { return _portreceiver2; }
            set {
                _portreceiver2 = value;
                OnPropertyChanged(nameof(portReceiver2));
            }
        }
        string _powertransmit2;
        public string powerTransmit2 {
            get { return _powertransmit2; }
            set {
                _powertransmit2 = value;
                OnPropertyChanged(nameof(powerTransmit2));
            }
        }
        string _connector1;
        public string Connector1 {
            get { return _connector1; }
            set {
                _connector1 = value;
                OnPropertyChanged(nameof(Connector1));
            }
        }
        string _connector2;
        public string Connector2 {
            get { return _connector2; }
            set {
                _connector2 = value;
                OnPropertyChanged(nameof(Connector2));
            }
        }

        #endregion

        #region Calib wifi

        bool _calibfreq;
        public bool calibFreq {
            get { return _calibfreq; }
            set {
                _calibfreq = value;
                OnPropertyChanged(nameof(calibFreq));
            }
        }
        bool _calib2g;
        public bool calib2G {
            get { return _calib2g; }
            set {
                _calib2g = value;
                OnPropertyChanged(nameof(calib2G));
            }
        }
        bool _calib5g;
        public bool calib5G {
            get { return _calib5g; }
            set {
                _calib5g = value;
                OnPropertyChanged(nameof(calib5G));
            }
        }
        bool _writebin;
        public bool writeBIN {
            get { return _writebin; }
            set {
                _writebin = value;
                OnPropertyChanged(nameof(writeBIN));
            }
        }
        bool _saveflash;
        public bool saveFlash {
            get { return _saveflash; }
            set {
                _saveflash = value;
                OnPropertyChanged(nameof(saveFlash));
            }
        }
        bool _checkregister;
        public bool checkRegister {
            get { return _checkregister; }
            set {
                _checkregister = value;
                OnPropertyChanged(nameof(checkRegister));
            }
        }
        string _target2g;
        public string target2G {
            get { return _target2g; }
            set {
                _target2g = value;
                OnPropertyChanged(nameof(target2G));
            }
        }
        string _target5g;
        public string target5G {
            get { return _target5g; }
            set {
                _target5g = value;
                OnPropertyChanged(nameof(target5G));
            }
        }
        string _lowerlimit;
        public string lowerLimit {
            get { return _lowerlimit; }
            set {
                _lowerlimit = value;
                OnPropertyChanged(nameof(lowerLimit));
            }
        }
        string _upperlimit;
        public string upperLimit {
            get { return _upperlimit; }
            set {
                _upperlimit = value;
                OnPropertyChanged(nameof(upperLimit));
            }
        }
        #endregion

        #region Verify wifi

        bool _testper2g;
        public bool testPer2G {
            get { return _testper2g; }
            set {
                _testper2g = value;
                OnPropertyChanged(nameof(testPer2G));
            }
        }
        bool _testper5g;
        public bool testPer5G {
            get { return _testper5g; }
            set {
                _testper5g = value;
                OnPropertyChanged(nameof(testPer5G));
            }
        }
        bool _testsignal2g;
        public bool testSignal2G {
            get { return _testsignal2g; }
            set {
                _testsignal2g = value;
                OnPropertyChanged(nameof(testSignal2G));
            }
        }
        bool _testsignal5g;
        public bool testSignal5G {
            get { return _testsignal5g; }
            set {
                _testsignal5g = value;
                OnPropertyChanged(nameof(testSignal5G));
            }
        }

        #endregion

        #region Do master

        string _measuretime;
        public string measureTime {
            get { return _measuretime; }
            set {
                _measuretime = value;
                OnPropertyChanged(nameof(measureTime));
            }
        }
        string _difference;
        public string Difference {
            get { return _difference; }
            set {
                _difference = value;
                OnPropertyChanged(nameof(Difference));
            }
        }

        #endregion
    }
}
