using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class TestCaseConfigFile {

        Dictionary<string, string> dictWifi = new Dictionary<string, string>() {
            { "802.11b", "0" },
            { "802.11g", "1" },
            { "802.11a", "2" },
            { "802.11n", "3" },
            { "802.11ac", "4" }
        };

        Dictionary<string, string> dictRate = new Dictionary<string, string>() {
            { "MCS0", "0" },
            { "MCS1", "1" },
            { "MCS2", "2" },
            { "MCS3", "3" },
            { "MCS4", "4" },
            { "MCS5", "5" },
            { "MCS6", "6" },
            { "MCS7", "7" },
            { "MCS8", "8" },
            { "MCS9", "9" }
        };

        Dictionary<string, string> dictBandwidth = new Dictionary<string, string>() {
            { "20", "0" },
            { "40", "1" },
            { "80", "2" },
            { "160", "3" }
        };

        string _dirName = string.Format("{0}TestCase", System.AppDomain.CurrentDomain.BaseDirectory);
        string _filename_txwifi2 = "", _filename_txwifi5 = "", _filename_rxwifi2 = "", _filename_rxwifi5 = "";

        public TestCaseConfigFile() {
            _filename_txwifi2 = myGlobal.myMainWindowDataBinding.modeRD == false ? $"{_dirName}\\SX\\txWifi-2G.csv" : $"{_dirName}\\RD\\txWifi-2G.csv";
            _filename_txwifi5 = myGlobal.myMainWindowDataBinding.modeRD == false ? $"{_dirName}\\SX\\txWifi-5G.csv" : $"{_dirName}\\RD\\txWifi-5G.csv";
            _filename_rxwifi2 = myGlobal.myMainWindowDataBinding.modeRD == false ? $"{_dirName}\\SX\\rxWifi-2G.csv" : $"{_dirName}\\RD\\rxWifi-2G.csv";
            _filename_rxwifi5 = myGlobal.myMainWindowDataBinding.modeRD == false ? $"{_dirName}\\SX\\rxWifi-5G.csv" : $"{_dirName}\\RD\\rxWifi-5G.csv";
        }

        //Load dữ liệu từ file vào tmpList, chuyển dữ liệu từ tmpList sang List test
        public bool loadDataFromFile() {
            try {
                //Load dữ liệu từ file vào tmpList
                _load_File_To_tmpList(_filename_txwifi2, out myGlobal.tmplisttxWifi2G);
                _load_File_To_tmpList(_filename_txwifi5, out myGlobal.tmplisttxWifi5G);
                _load_File_To_tmpList(_filename_rxwifi2, out myGlobal.tmplistrxWifi2G);
                _load_File_To_tmpList(_filename_rxwifi5, out myGlobal.tmplistrxWifi5G);


                //Chuyển dữ liệu từ tmpList sang List test
                _convert_tmpList_To_testList(myGlobal.tmplisttxWifi2G, out myGlobal.listVerifySignal2G);
                _convert_tmpList_To_testList(myGlobal.tmplisttxWifi5G, out myGlobal.listVerifySignal5G);
                _convert_tmpList_To_testList(myGlobal.tmplistrxWifi2G, out myGlobal.listSensivitity2G);
                _convert_tmpList_To_testList(myGlobal.tmplistrxWifi5G, out myGlobal.listSensivitity5G);

                return true;
            }
            catch {
                return false;
            }
        }


        //Load dữ liệu từ file vào tmpList
        bool _load_File_To_tmpList(string _filename, out List<SignalConfigInfo> _lo) {
            _lo = new List<SignalConfigInfo>();
            if (File.Exists(_filename) == false) return false;
            try {
                string line;
                StreamReader sr = new StreamReader(_filename);
                while ((line = sr.ReadLine()) != null) {
                    if (line.Trim() != "" || line.Trim() != string.Empty) {
                        string[] buffer = line.Split('|');
                        _lo.Add(new SignalConfigInfo() { wifi = buffer[0], rate = buffer[1], bandwidth = buffer[2], anten = buffer[3], channelfreq = buffer[4] });
                    }
                }
                sr.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

        bool _load_File_To_tmpList(string _filename, out List<SensitivityConfigInfo> _lo) {
            _lo = new List<SensitivityConfigInfo>();
            if (File.Exists(_filename) == false) return false;
            try {
                string line;
                StreamReader sr = new StreamReader(_filename);
                while ((line = sr.ReadLine()) != null) {
                    if (line.Trim() != "" || line.Trim() != string.Empty) {
                        string[] buffer = line.Split('|');
                        _lo.Add(new SensitivityConfigInfo() { wifi = buffer[0], rate = buffer[1], bandwidth = buffer[2], anten = buffer[3], channelfreq = buffer[4], packet = int.Parse(buffer[5]) });
                    }
                }
                sr.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

        //chuyển dữ liệu từ tmpList sang List test
        bool _convert_tmpList_To_testList(List<SignalConfigInfo> _li, out List<SignalConfigInfo> _lo) {
            _lo = new List<SignalConfigInfo>();
            if (_li == null || _li.Count == 0) return false;

            try {
                // -- 802.11b | MCS0 | 20 | 1,2 | 2412,2437,2462 --
                foreach (var item in _li) {
                    if (item.ToString() != "||||") {

                        bool _ret = false;

                        //Wifi Standard
                        string _wifi = "";
                        _ret = dictWifi.TryGetValue(item.wifi, out _wifi);

                        //Rate
                        string _mcs = "";
                        _ret = dictRate.TryGetValue(item.rate, out _mcs);

                        //Bandwidth
                        string _bw = "";
                        _ret = dictBandwidth.TryGetValue(item.bandwidth, out _bw);

                        //Channel Frequency
                        List<string> _lisch = new List<string>();
                        if (item.channelfreq.Contains(",")) {
                            string[] buffer = item.channelfreq.Split(',');
                            for (int i = 0; i < buffer.Length; i++) { _lisch.Add(buffer[i]); }
                        }
                        else _lisch.Add(item.channelfreq);

                        //Anten
                        List<string> _lisat = new List<string>();
                        if (item.anten.Contains(",")) {
                            string[] buffer = item.anten.Split(',');
                            for (int i = 0; i < buffer.Length; i++) { _lisat.Add(buffer[i]); }
                        }
                        else _lisat.Add(item.anten);

                        //Input data to list
                        foreach (var i in _lisat) {
                            foreach (var j in _lisch) {
                                _lo.Add(new SignalConfigInfo() { wifi = _wifi, rate = _mcs, bandwidth = _bw, anten = i, channelfreq = j });
                            }
                        }
                    }
                }
                return true;
            }
            catch {
                return false;
            }
        }


        bool _convert_tmpList_To_testList(List<SensitivityConfigInfo> _li, out List<SensitivityConfigInfo> _lo) {
            _lo = new List<SensitivityConfigInfo>();
            if (_li == null || _li.Count == 0) return false;

            try {
                // -- 802.11b | MCS0 | 20 | 1,2 | 2412,2437,2462 | 1000 --
                foreach (var item in _li) {
                    if (item.ToString() != "|||||0") {

                        bool _ret = false;

                        //Wifi Standard
                        string _wifi = "";
                        _ret = dictWifi.TryGetValue(item.wifi, out _wifi);

                        //Rate
                        string _mcs = "";
                        _ret = dictRate.TryGetValue(item.rate, out _mcs);

                        //Bandwidth
                        string _bw = "";
                        _ret = dictBandwidth.TryGetValue(item.bandwidth, out _bw);

                        //Channel Frequency
                        List<string> _lisch = new List<string>();
                        if (item.channelfreq.Contains(",")) {
                            string[] buffer = item.channelfreq.Split(',');
                            for (int i = 0; i < buffer.Length; i++) { _lisch.Add(buffer[i]); }
                        }
                        else _lisch.Add(item.channelfreq);

                        //Anten
                        List<string> _lisat = new List<string>();
                        if (item.anten.Contains(",")) {
                            string[] buffer = item.anten.Split(',');
                            for (int i = 0; i < buffer.Length; i++) { _lisat.Add(buffer[i]); }
                        }
                        else _lisat.Add(item.anten);

                        //Input data to list
                        foreach (var i in _lisat) {
                            foreach (var j in _lisch) {
                                _lo.Add(new SensitivityConfigInfo() { wifi = _wifi, rate = _mcs, bandwidth = _bw, anten = i, channelfreq = j, packet = item.packet });
                            }
                        }
                    }
                }
                return true;
            }
            catch {
                return false;
            }
        }


    }
}
