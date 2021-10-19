using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {
    public class LogFile {
        string root_path = "";
        string dir_log = "";
        string dir_total = "";
        string mac_address = "";
        string result = "";

        public LogFile(string _mac, string _result) {
            _mac = _mac.Replace(":", "").Replace("-", "");
            this.mac_address = _mac;
            this.result = _result;

            root_path = AppDomain.CurrentDomain.BaseDirectory.Replace($"{GlobalData.initSetting.ONTTYPE}\\", "").Replace($"{GlobalData.initSetting.ONTTYPE.ToLower()}\\", "");
            dir_log = string.Format("{0}LogData\\{1}\\{2}\\{3}\\{4}",
                                              root_path,
                                              GlobalData.initSetting.STATION == "Trước đóng vỏ" ? "CalibWiFi" : "TestAntenna",
                                              GlobalData.initSetting.ONTTYPE,
                                              DateTime.Now.ToString("yyyyMMdd"),
                                              string.Format("{0}_{1}_{2}", _mac, DateTime.Now.ToString("HHmmss"), _result));

            dir_total = string.Format("{0}LogData\\{1}\\{2}\\{3}",
                                      root_path,
                                      GlobalData.initSetting.STATION == "Trước đóng vỏ" ? "CalibWiFi" : "TestAntenna",
                                      GlobalData.initSetting.ONTTYPE,
                                      DateTime.Now.ToString("yyyyMMdd"));

            if (Directory.Exists(dir_log) == false) Directory.CreateDirectory(dir_log);
            if (Directory.Exists(dir_total) == false) Directory.CreateDirectory(dir_total);
        }

        public bool saveTestLog(logdata _log) {
            try {
                string _logfile = string.Format("{0}\\{2}_{1}.csv", dir_total, DateTime.Now.ToString("yyyyMMdd"), GlobalData.initSetting.ONTTYPE);

                string _title = "";
                if (GlobalData.initSetting.STATION == "Trước đóng vỏ")
                    _title = "DATE-TIME,MAC-ADDRESS,CALIB-FREQ,CALIB-POWER-2G,CALIB-POWER-5G,TEST-SENS-2G,TEST-SENS-5G,VERIFY-SIGNAL-2G,VERIFY-SIGNAL-5G,ERROR-CODE,TOTAL-RESULT";
                else
                    _title = "DATE-TIME,MAC-ADDRESS,TEST-ANTEN1-2G,TEST-ANTEN1-5G,TEST-ANTEN2-2G,TEST-ANTEN2-5G,ERROR-CODE,TOTAL-RESULT";

                StreamWriter st = null;
                if (File.Exists(_logfile) == false) {
                    st = new StreamWriter(_logfile, true);
                    st.WriteLine(_title);
                }
                else st = new StreamWriter(_logfile, true);

                st.WriteLine(_log.ToString());
                st.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool saveDetailLog(string _data) {
            try {
                string _logfile = string.Format("{0}\\{4}_{1}_{2}_{3}_logDetail.txt",
                                                dir_log,
                                                this.mac_address,
                                                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                                                this.result,
                                                GlobalData.initSetting.ONTTYPE);

                StreamWriter st = new StreamWriter(_logfile, true);
                st.WriteLine(_data);
                st.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool saveErrorLog(string _data) {
            try {
                string _logfile = string.Format("{0}\\{4}_{1}_{2}_{3}_logError.txt",
                                                dir_log,
                                                this.mac_address,
                                                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                                                this.result,
                                                GlobalData.initSetting.ONTTYPE);

                StreamWriter st = new StreamWriter(_logfile, true);
                st.WriteLine(_data);
                st.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool saveUartLog(string _data) {
            try {
                string _logfile = string.Format("{0}\\{4}_{1}_{2}_{3}_logUart.txt",
                                                dir_log,
                                                this.mac_address,
                                                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                                                this.result,
                                                GlobalData.initSetting.ONTTYPE);

                StreamWriter st = new StreamWriter(_logfile, true);
                st.WriteLine(_data);
                st.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool saveTelnetLog(string _data) {
            try {
                string _logfile = string.Format("{0}\\{4}_{1}_{2}_{3}_logTelnet.txt",
                                                dir_log,
                                                this.mac_address,
                                                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                                                this.result,
                                                GlobalData.initSetting.ONTTYPE);

                StreamWriter st = new StreamWriter(_logfile, true);
                st.WriteLine(_data);
                st.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool saveInstrumentlog(string _data) {
            try {
                string _logfile = string.Format("{0}\\{4}_{1}_{2}_{3}_logInstrument.txt",
                                                dir_log,
                                                this.mac_address,
                                                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                                                this.result,
                                                GlobalData.initSetting.ONTTYPE);

                StreamWriter st = new StreamWriter(_logfile, true);
                st.WriteLine(_data);
                st.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool savereViewLog(string _result, string _resultRX) {
            try {

                if (GlobalData.datagridlogTX.Count > 0) {
                    string _logfile = string.Format("{0}\\{4}_{1}_{2}_{3}_logViewTX.csv", 
                                                    dir_log,
                                                    this.mac_address,
                                                    DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                                                    _result,
                                                    GlobalData.initSetting.ONTTYPE);

                    string _title = "RANGEFREQ,ANTEN,WIFI,RATE,BANDWIDTH,CHANNEL,POWER-LIMIT,POWER-ACTUAL,EVM-MAX,EVM-ACTUAL,FREQ-ERROR,RESULT";
                    StreamWriter st = new StreamWriter(_logfile, true);
                    st.WriteLine(_title);
                    foreach (var item in GlobalData.datagridlogTX) {
                        st.WriteLine(item.ToString());
                    }
                    st.Dispose();
                }

                //-----------------------logRX 
                if (GlobalData.datagridlogRX.Count > 0) {
                    string _logfile = _logfile = string.Format("{0}\\{4}_{1}_{2}_{3}_logViewRX.csv", 
                                                                dir_log,
                                                                this.mac_address,
                                                                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                                                                _resultRX,
                                                                GlobalData.initSetting.ONTTYPE);

                    string _title = "RANGEFREQ,ANTEN,WIFI,RATE,BANDWIDTH,CHANNEL,PW-Transmit,Sent,Recieved,PER,RESULT";
                    StreamWriter st = new StreamWriter(_logfile, true);
                    st.WriteLine(_title);
                    foreach (var item in GlobalData.datagridlogRX) {
                        string tmp = $"{item.rangeFreq},{item.Anten},{item.wifiStandard},{item.Rate},{item.Bandwidth},{item.Channel},{item.transmitPower},{item.Sent},{item.Received},{item.Per},{item.Result}";
                        st.WriteLine(tmp);
                    }
                    st.Dispose();
                }

                return true;
            }
            catch {
                return false;
            }
        }
    }
}
