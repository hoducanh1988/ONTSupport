using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {

    public class defaultSetting : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public string STATION {
            get { return Properties.Settings.Default.Station; }
            set {
                Properties.Settings.Default.Station = value;
                OnPropertyChanged(nameof(STATION));
                GlobalData.testingData.STATION = value;
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public string ONTIP {
            get { return Properties.Settings.Default.ontIP; }
            set {
                Properties.Settings.Default.ontIP = value;
                OnPropertyChanged(nameof(ONTIP));
            }
        }
        public string ONTUSER {
            get { return Properties.Settings.Default.ontUser; }
            set {
                Properties.Settings.Default.ontUser = value;
                OnPropertyChanged(nameof(ONTUSER));
            }
        }
        public string ONTPASS {
            get { return Properties.Settings.Default.ontPass; }
            set {
                Properties.Settings.Default.ontPass = value;
                OnPropertyChanged(nameof(ONTPASS));
            }
        }
        public string ONTTYPE {
            get { return Properties.Settings.Default.ontType; }
            set {
                Properties.Settings.Default.ontType = value;
                OnPropertyChanged(nameof(ONTTYPE));
                GlobalData.testingData.APPTITLE = STATION == "Trước đóng vỏ" ? "PHẦN MỀM CALIBRATION + LÀM MẠCH MASTER ONT " : "PHẦN MỀM TEST ANTEN WIFI ONT ";
                GlobalData.testingData.APPTITLE += value;



            }
        }
        public string SERIALPORT {
            get { return Properties.Settings.Default.serialPort; }
            set {
                Properties.Settings.Default.serialPort = value;
                OnPropertyChanged(nameof(SERIALPORT));
            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public int CALIBTIME {
            get { return Properties.Settings.Default.calibtime; }
            set {
                Properties.Settings.Default.calibtime = value;
                OnPropertyChanged(nameof(CALIBTIME));
            }
        }
        public int CALIBTOTAL {
            get { return Properties.Settings.Default.calibtotal; }
            set {
                Properties.Settings.Default.calibtotal = value;
                OnPropertyChanged(nameof(CALIBTOTAL));
            }
        }
        public int CALIBCOUNTED {
            get { return Properties.Settings.Default.calibcount; }
            set {
                Properties.Settings.Default.calibcount = value;
                OnPropertyChanged(nameof(CALIBCOUNTED));
                GlobalData.testingData.CALIBCOUNTED = value.ToString();
            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public string INSTRUMENT {
            get { return Properties.Settings.Default.Instrument; }
            set {
                Properties.Settings.Default.Instrument = value;
                OnPropertyChanged(nameof(INSTRUMENT));
            }
        }
        public string VISAADDRESS {
            get { return Properties.Settings.Default.visaAddress; }
            set {
                Properties.Settings.Default.visaAddress = value;
                OnPropertyChanged(nameof(VISAADDRESS));
            }
        }
        public string RFPORT1 {
            get { return Properties.Settings.Default.RFPort1; }
            set {
                Properties.Settings.Default.RFPort1 = value;
                OnPropertyChanged(nameof(RFPORT1));
            }
        }
        public string RFPORT2 {
            get { return Properties.Settings.Default.RFPort2; }
            set {
                Properties.Settings.Default.RFPort2 = value;
                OnPropertyChanged(nameof(RFPORT2));
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public string TARGETPOWER2G {
            get { return Properties.Settings.Default.TargetPw2G; }
            set {
                Properties.Settings.Default.TargetPw2G = value;
                OnPropertyChanged(nameof(TARGETPOWER2G));
            }
        }
        public string TARGETPOWER5G {
            get { return Properties.Settings.Default.TargetPw5G; }
            set {
                Properties.Settings.Default.TargetPw5G = value;
                OnPropertyChanged(nameof(TARGETPOWER5G));
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public bool ENCALIBFREQ {
            get { return Properties.Settings.Default.enCalibFreq; }
            set {
                Properties.Settings.Default.enCalibFreq = value;
                OnPropertyChanged(nameof(ENCALIBFREQ));
            }
        }

        public bool ENCALIBPW2G {
            get { return Properties.Settings.Default.enCalibPw2G; }
            set {
                Properties.Settings.Default.enCalibPw2G = value;
                OnPropertyChanged(nameof(ENCALIBPW2G));
            }
        }
        public bool ENCALIBPW5G {
            get { return Properties.Settings.Default.enCalibPw5G; }
            set {
                Properties.Settings.Default.enCalibPw5G = value;
                OnPropertyChanged(nameof(ENCALIBPW5G));
            }
        }
        public bool ENWRITEBIN {
            get { return Properties.Settings.Default.enWriteBIN; }
            set {
                Properties.Settings.Default.enWriteBIN = value;
                OnPropertyChanged(nameof(ENWRITEBIN));
            }
        }
        public bool ENTESTRX2G {
            get { return Properties.Settings.Default.enTestRx2G; }
            set {
                Properties.Settings.Default.enTestRx2G = value;
                OnPropertyChanged(nameof(ENTESTRX2G));
            }
        }
        public bool ENTESTRX5G {
            get { return Properties.Settings.Default.enTestRx5G; }
            set {
                Properties.Settings.Default.enTestRx5G = value;
                OnPropertyChanged(nameof(ENTESTRX5G));
            }
        }
        public bool ENTESTTX2G {
            get { return Properties.Settings.Default.enTestTx2G; }
            set {
                Properties.Settings.Default.enTestTx2G = value;
                OnPropertyChanged(nameof(ENTESTTX2G));
            }
        }
        public bool ENTESTTX5G {
            get { return Properties.Settings.Default.enTestTx5G; }
            set {
                Properties.Settings.Default.enTestTx5G = value;
                OnPropertyChanged(nameof(ENTESTTX5G));
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public bool ENTESTANTEN1 {
            get { return Properties.Settings.Default.enTestAnten1; }
            set {
                Properties.Settings.Default.enTestAnten1 = value;
                OnPropertyChanged(nameof(ENTESTANTEN1));
            }
        }
        public bool ENTESTANTEN2 {
            get { return Properties.Settings.Default.enTestAnten2; }
            set {
                Properties.Settings.Default.enTestAnten2 = value;
                OnPropertyChanged(nameof(ENTESTANTEN2));
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public string STDPWANTEN1 {
            get { return Properties.Settings.Default.stdPwAnten1; }
            set {
                Properties.Settings.Default.stdPwAnten1 = value;
                OnPropertyChanged(nameof(STDPWANTEN1));
            }
        }
        public string STDPWANTEN2 {
            get { return Properties.Settings.Default.stdPwAnten2; }
            set {
                Properties.Settings.Default.stdPwAnten2 = value;
                OnPropertyChanged(nameof(STDPWANTEN2));
            }
        }
    }
    //---------------------------------------------------------------------------------//
    public class WifiTestingInfor : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        string _frequency;
        public string Frequency {
            get { return _frequency; }
            set {
                _frequency = value;
                OnPropertyChanged(nameof(Frequency));
            }
        }
        string _antena;
        public string Antena {
            get { return _antena; }
            set {
                _antena = value;
                OnPropertyChanged(nameof(Antena));
            }
        }
        string _attenuator;
        public string Attenuator {
            get { return _attenuator; }
            set {
                _attenuator = value;
                OnPropertyChanged(nameof(Attenuator));
            }
        }
        string _upperLimit;
        public string UpperLimit {
            get { return _upperLimit; }
            set {
                _upperLimit = value;
                OnPropertyChanged(nameof(UpperLimit));
            }
        }
        string _lowerLimit;
        public string LowerLimit {
            get { return _lowerLimit; }
            set {
                _lowerLimit = value;
                OnPropertyChanged(nameof(LowerLimit));
            }
        }
        public WifiTestingInfor() {
            Frequency = ""; Attenuator = ""; UpperLimit = ""; LowerLimit = ""; Antena = "";
        }

    }

    public class LstWifiTestingInfor : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        List<WifiTestingInfor> _lstWifiTesting;
        public List<WifiTestingInfor> LstWifiTesting {
            get { return _lstWifiTesting; }
            set {
                _lstWifiTesting = value;
                OnPropertyChanged(nameof(LstWifiTesting));
            }
        }
        public List<WifiTestingInfor> LstWifiTestingTmp;
        public LstWifiTestingInfor() { LstWifiTesting = new List<WifiTestingInfor>(); LstWifiTestingTmp = new List<WifiTestingInfor>(); }
    }
    //----------------------------------------------------------------------------------//

    public class testinginfo : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public testinginfo() {
            Initialize();
            BUTTONCONTENT = "START";
        }

        public void Initialize() {
            STATION = GlobalData.initSetting.STATION;
            TimeCount = 0;
            LOGSYSTEM = "";
            LOGERROR = "";
            LOGUART = "";
            LOGINSTRUMENT = "";
            LOGREGISTER = "";
            LOGTELNET = "";
            //BUTTONCONTENT = "START";
            MACADDRESS = "--";
            CALIBFREQRESULT = InitParameters.Statuses.None;
            CALIBPW2GRESULT = InitParameters.Statuses.None;
            CALIBPW5GRESULT = InitParameters.Statuses.None;
            SAVEBIN = InitParameters.Statuses.None;
            TESTRX2GRESULT = InitParameters.Statuses.None;
            TESTRX5GRESULT = InitParameters.Statuses.None;
            TESTTX2GRESULT = InitParameters.Statuses.None;
            TESTTX5GRESULT = InitParameters.Statuses.None;
            TESTANTEN1RESULT2G = InitParameters.Statuses.None;
            TESTANTEN1RESULT5G = InitParameters.Statuses.None;

            TESTANTEN2RESULT2G = InitParameters.Statuses.None;
            TESTANTEN2RESULT5G = InitParameters.Statuses.None;

            TOTALRESULT = InitParameters.Statuses.None;
            ERRORCODE = "";
            CALIBCOUNTED = GlobalData.initSetting.CALIBCOUNTED.ToString();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        string _station;
        public string STATION {
            get { return _station; }
            set {
                _station = value;
                OnPropertyChanged(nameof(STATION));
            }
        }
        string _apptitle;
        public string APPTITLE {
            get { return _apptitle; }
            set {
                _apptitle = value;
                OnPropertyChanged(nameof(APPTITLE));
            }
        }
        string _rdContent;
        public string RDContent {
            get { return _rdContent; }
            set {
                _rdContent = value;
                OnPropertyChanged(nameof(RDContent));
            }
        }

        string _calibcounted;
        public string CALIBCOUNTED {
            get { return _calibcounted; }
            set {
                _calibcounted = value;
                OnPropertyChanged(nameof(CALIBCOUNTED));
            }
        }

        string _logsystem;
        public string LOGSYSTEM {
            get { return _logsystem; }
            set {
                _logsystem = value;
                OnPropertyChanged(nameof(LOGSYSTEM));
            }
        }
        string _logerror;
        public string LOGERROR {
            get { return _logerror; }
            set {
                _logerror = value;
                OnPropertyChanged(nameof(LOGERROR));
            }
        }//LOGERROR
        string _loguart;
        public string LOGUART {
            get { return _loguart; }
            set {
                _loguart = value;
                OnPropertyChanged(nameof(LOGUART));
            }
        }
        string _logtelnet;
        public string LOGTELNET {
            get { return _logtelnet; }
            set {
                _logtelnet = value;
                OnPropertyChanged(nameof(LOGTELNET));
            }
        }
        string _loginstrument;
        public string LOGINSTRUMENT {
            get { return _loginstrument; }
            set {
                _loginstrument = value;
                OnPropertyChanged(nameof(LOGINSTRUMENT));
            }
        }
        string _logregister;
        public string LOGREGISTER {
            get { return _logregister; }
            set {
                _logregister = value;
                OnPropertyChanged(nameof(LOGREGISTER));
            }
        }
        int _timeCount;
        public int TimeCount {
            get { return _timeCount; }
            set {
                _timeCount = value;
                OnPropertyChanged(nameof(TimeCount));
            }
        }
        string _macaddress;
        public string MACADDRESS {
            get { return _macaddress; }
            set {
                _macaddress = value;
                OnPropertyChanged(nameof(MACADDRESS));
            }
        }

        string _errorcode;
        public string ERRORCODE {
            get { return _errorcode; }
            set {
                _errorcode = value;
                OnPropertyChanged(nameof(ERRORCODE));
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        string _buttoncontent;
        public string BUTTONCONTENT {
            get { return _buttoncontent; }
            set {
                _buttoncontent = value;
                OnPropertyChanged(nameof(BUTTONCONTENT));
            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        string _calibfreqresult;
        public string CALIBFREQRESULT {
            get { return _calibfreqresult; }
            set {
                _calibfreqresult = value;
                OnPropertyChanged(nameof(CALIBFREQRESULT));
            }
        }

        string _calibpw2gresult;
        public string CALIBPW2GRESULT {
            get { return _calibpw2gresult; }
            set {
                _calibpw2gresult = value;
                OnPropertyChanged(nameof(CALIBPW2GRESULT));
            }
        }

        string _calibpw5gresult;
        public string CALIBPW5GRESULT {
            get { return _calibpw5gresult; }
            set {
                _calibpw5gresult = value;
                OnPropertyChanged(nameof(CALIBPW5GRESULT));
            }
        }//SAVEFLASH

        string _savebin;
        public string SAVEBIN {
            get { return _savebin; }
            set {
                _savebin = value;
                OnPropertyChanged(nameof(SAVEBIN));
            }
        }//SAVEFLASH

        string _testrx2gresult;
        public string TESTRX2GRESULT {
            get { return _testrx2gresult; }
            set {
                _testrx2gresult = value;
                OnPropertyChanged(nameof(TESTRX2GRESULT));
            }
        }

        string _testrx5gresult;
        public string TESTRX5GRESULT {
            get { return _testrx5gresult; }
            set {
                _testrx5gresult = value;
                OnPropertyChanged(nameof(TESTRX5GRESULT));
            }
        }

        string _testtx2gresult;
        public string TESTTX2GRESULT {
            get { return _testtx2gresult; }
            set {
                _testtx2gresult = value;
                OnPropertyChanged(nameof(TESTTX2GRESULT));
            }
        }

        string _testtx5gresult;
        public string TESTTX5GRESULT {
            get { return _testtx5gresult; }
            set {
                _testtx5gresult = value;
                OnPropertyChanged(nameof(TESTTX5GRESULT));
            }
        }
        string _testanten1result2G;
        public string TESTANTEN1RESULT2G {
            get { return _testanten1result2G; }
            set {
                _testanten1result2G = value;
                OnPropertyChanged(nameof(TESTANTEN1RESULT2G));
            }
        }
        string _testanten1result5G;
        public string TESTANTEN1RESULT5G {
            get { return _testanten1result5G; }
            set {
                _testanten1result5G = value;
                OnPropertyChanged(nameof(TESTANTEN1RESULT5G));
            }
        }

        string _testanten2result2G;
        public string TESTANTEN2RESULT2G {
            get { return _testanten2result2G; }
            set {
                _testanten2result2G = value;
                OnPropertyChanged(nameof(TESTANTEN2RESULT2G));
            }
        }
        string _testanten2result5G;
        public string TESTANTEN2RESULT5G {
            get { return _testanten2result5G; }
            set {
                _testanten2result5G = value;
                OnPropertyChanged(nameof(TESTANTEN2RESULT5G));
            }
        }

        string _totalresult;
        public string TOTALRESULT {
            get { return _totalresult; }
            set {
                _totalresult = value;
                OnPropertyChanged(nameof(TOTALRESULT));
            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

    }

    public class waveformInfo {
        public string instrument { get; set; }
        public string wifi { get; set; }
        public string mcs { get; set; }
        public string bandwidth { get; set; }
        public string waveform { get; set; }
    }

    public class attenuatorInfo {
        public string channelnumber { get; set; }
        public string channelfreq { get; set; }
        public double at1_attenuator { get; set; }
        public double at2_attenuator { get; set; }
    }

    public class calibpower {
        public string carrierfreq { get; set; }
        public string anten { get; set; }
        public string channelfreq { get; set; }
        public string register { get; set; }
        public string calibflag { get; set; }
        public string refference { get; set; }

    }

    public class channelmanagement {
        public string rangefreq { get; set; }
        public string channel { get; set; }
        public string channelfreq { get; set; }
    }

    public class verifysignal {
        public string wifi { get; set; } //0=b, 1=g, 3=n
        public string rate { get; set; }
        public string bandwidth { get; set; } //0=20M, 1=40M
        public string anten { get; set; }
        public string channelfreq { get; set; }

        public override string ToString() {
            return string.Format("{0}|{1}|{2}|{3}|{4}", wifi, rate, bandwidth, anten, channelfreq);
        }
    }

    public class sensivitity {
        public string wifi { get; set; } //0=b, 1=g, 3=n
        public string rate { get; set; }
        public string bandwidth { get; set; } //0=20M, 1=40M
        public string anten { get; set; }
        public string channelfreq { get; set; }
        public double powertransmit { get; set; }
        public int packet { get; set; } //so goi tin 

        public override string ToString() {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}", wifi, rate, bandwidth, anten, channelfreq, packet);
        }
    }

    public class limittx {
        public string rangefreq { get; set; }
        public string wifi { get; set; }
        public string mcs { get; set; }
        public string power_MAX { get; set; }
        public string power_MIN { get; set; }
        public string evm_MAX { get; set; }
        public string evm_MIN { get; set; }
        public string freqError_MAX { get; set; }
        public string freqError_MIN { get; set; }
        public string symclock_MAX { get; set; }
        public string symclock_MIN { get; set; }
    }

    public class limitrx {
        public string rangefreq { get; set; }
        public string wifi { get; set; }
        public string mcs { get; set; }
        public string power_Transmit { get; set; }
        public string PER { get; set; }
    }

    public class logdata {

        public string date { get; set; }
        public string mac { get; set; }
        public string calibFreqResult { get; set; }
        public string calibPower2GResult { get; set; }
        public string calibPower5GResult { get; set; }
        public string testSens2GResult { get; set; }
        public string testSens5GResult { get; set; }
        public string verify2GResult { get; set; }
        public string verify5GResult { get; set; }

        public string testAnten1Result2g { get; set; }
        public string testAnten1Result5g { get; set; }

        public string testAnten2Result2g { get; set; }
        public string testAnten2Result5g { get; set; }


        public string errorCode { get; set; }
        public string totalResult { get; set; }

        public logdata() {
            date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            mac = "";
            calibFreqResult = "-";
            calibPower2GResult = "-";
            calibPower5GResult = "-";
            testSens2GResult = "-";
            testSens5GResult = "-";
            verify2GResult = "-";
            verify5GResult = "-";
            testAnten1Result2g = "-";
            testAnten1Result5g = "-";

            testAnten2Result2g = "-";
            testAnten2Result5g = "-";

            errorCode = "";
            totalResult = "";
        }

        public override string ToString() {
            if (GlobalData.initSetting.STATION == "Trước đóng vỏ")
                return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", date, mac, calibFreqResult, calibPower2GResult, calibPower5GResult, testSens2GResult, testSens5GResult, verify2GResult, verify5GResult, errorCode, totalResult);
            else
                return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", date, mac, testAnten1Result2g, testAnten1Result5g, testAnten2Result2g, testAnten2Result5g, errorCode, totalResult);
        }
    }

    public class logregister {
        public string macaddress { get; set; }
        public string totalresult { get; set; }

        //2G - Register
        public string _0x59 { get; set; }
        public string _0x5A { get; set; }
        public string _0x5B { get; set; }
        public string _0x5F { get; set; }
        public string _0x60 { get; set; }
        public string _0x61 { get; set; }

        //5G - Register
        public string _0x143 { get; set; }
        public string _0x144 { get; set; }
        public string _0x148 { get; set; }
        public string _0x149 { get; set; }
        public string _0x14D { get; set; }
        public string _0x14E { get; set; }
        public string _0x157 { get; set; }
        public string _0x158 { get; set; }
        public string _0x15C { get; set; }
        public string _0x15D { get; set; }
        public string _0x161 { get; set; }
        public string _0x162 { get; set; }
        public string _0x166 { get; set; }
        public string _0x167 { get; set; }
        public string _0x16B { get; set; }
        public string _0x16C { get; set; }
        public string _0x170 { get; set; }
        public string _0x171 { get; set; }
        public string _0x175 { get; set; }
        public string _0x176 { get; set; }
        public string _0x17F { get; set; }
        public string _0x180 { get; set; }
        public string _0x184 { get; set; }
        public string _0x185 { get; set; }
        public string _0x189 { get; set; }
        public string _0x18A { get; set; }
        public string _0x18E { get; set; }
        public string _0x18F { get; set; }

        public override string ToString() {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36}",
                                 DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                 macaddress,
                                 _0x59, _0x5A, _0x5B,
                                 _0x5F, _0x60, _0x61,
                                 _0x143, _0x144, _0x148, _0x149, _0x14D, _0x14E, _0x157, _0x158, _0x15C, _0x15D, _0x161, _0x162, _0x166, _0x167,
                                 _0x16B, _0x16C, _0x170, _0x171, _0x175, _0x176, _0x17F, _0x180, _0x184, _0x185, _0x189, _0x18A, _0x18E, _0x18F,
                                 totalresult
                                 );
        }
    }

    public class logreviewregister {

        public string rangeFreq { get; set; }
        public string Anten { get; set; }
        public string groupChannel { get; set; }
        public string Register { get; set; }
        public string registerValue { get; set; }
        public string diffPower { get; set; }
        public string currentPower { get; set; }
    }

    public class logreviewtx {

        public string rangeFreq { get; set; }
        public string Anten { get; set; }
        public string wifiStandard { get; set; }
        public string Rate { get; set; }
        public string Bandwidth { get; set; }
        public string Channel { get; set; }
        public string averagePower { get; set; }
        public string powerMAX { get; set; }
        public string powerMIN { get; set; }
        public string powerStd { get; set; }
        public string Evm { get; set; }
        public string evmMAX { get; set; }
        public string centerFreqError { get; set; }
        public string Result { get; set; }

        public override string ToString() {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", rangeFreq, Anten, wifiStandard, Rate, Bandwidth, Channel, powerStd, averagePower, evmMAX, Evm, centerFreqError, Result);
        }
    }

    public class logreviewrx {

        public string rangeFreq { get; set; }
        public string Anten { get; set; }
        public string wifiStandard { get; set; }
        public string Rate { get; set; }
        public string Bandwidth { get; set; }
        public string Channel { get; set; }
        public string transmitPower { get; set; }
        public string Sent { get; set; }
        public string Received { get; set; }
        public string Per { get; set; }
        public string Result { get; set; }
    }

    public class autoattenuator {

        public string Channel { get; set; }
        public string Frequency { get; set; }
        public string Anten { get; set; }
        public string PowerMaster { get; set; }
        public string measuredPower { get; set; }
        public string Attenuator { get; set; }
    }

    public class formattinfo : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        string _filename = string.Format("{0}MasterData\\Master.csv", System.AppDomain.CurrentDomain.BaseDirectory);
        public string FILENAME {
            get { return _filename; }
            set {
                _filename = value;
                OnPropertyChanged(nameof(FILENAME));
            }
        }

        string _logdata;
        public string LOGDATA {
            get { return _logdata; }
            set {
                _logdata = value;
                OnPropertyChanged(nameof(LOGDATA));
            }
        }

    }

    public class masterinformation {

        public string Channel { get; set; }
        public string Frequency { get; set; }
        public string pwAnten1 { get; set; }
        public string pwAnten2 { get; set; }

        public override string ToString() {
            return string.Format("{0},{1},{2},{3}", Channel, Frequency, pwAnten1, pwAnten2);
        }
    }

    public class calmaster {

        public string Channel { get; set; }
        public string Frequency { get; set; }
        public string Anten { get; set; }
        public string wirePower { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string masterPower { get; set; }
        public string minValue { get; set; }
        public string maxValue { get; set; }
        public string Difference { get; set; }
        public string Result { get; set; }

        public calmaster() {
            Value1 = "";
            Value2 = "";
            Value3 = "";
            Value4 = "";
            Value5 = "";
            masterPower = "";
            maxValue = "";
            minValue = "";
            Difference = "";
            Result = "";
        }

        public override string ToString() {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}", Channel, Frequency, Anten, Value1, Value2, Value3, Value4, Value5, masterPower, minValue, maxValue, Difference, Result);
        }
    }

    public class binregister {
        public string Address { get; set; }
        public string oldValue { get; set; }
        public string newValue { get; set; }
    }
}
