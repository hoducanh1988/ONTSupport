using ONTWiFiMaster.Function.Base;
using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Dut;
using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.Function.Instrument;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Excute {
    public class VerifyWiFiSignal {

        public bool Excuted() {
            bool _flag = false;
            bool _flagconnection = true, _flagtestper2g = true, _flagtestper5g = true, _flagtestsignal2g = true, _flagtestsignal5g = true;
            OntEconet<VerifyDataBinding> ont = null;
            IInstrument equipment = null;

            App.Current.Dispatcher.Invoke(new Action(() => {
                myGlobal.collectionPerResult2G.Clear();
                myGlobal.collectionPerResult5G.Clear();
                myGlobal.collectionSignalResult2G.Clear();
                myGlobal.collectionSignalResult5G.Clear();
            }));

            try {
                myGlobal.myVerify.totalResult = "Waiting...";

                //0. Kết nối telnet tới ONT và máy đo
                _flagconnection = Connect_ONT(ref ont, myGlobal.mySetting.loginUser, myGlobal.mySetting.loginPassword);
                if (!_flagconnection) goto Finished;
                _flagconnection = Connect_Instrument(ref equipment);
                if (!_flagconnection) goto Finished;


                //1. test PER 2G
                if (myGlobal.mySetting.testPer2G == true) {
                    myGlobal.myVerify.per2GResult = "Waiting...";
                    bool ret = Test_Sensitivity_2G(myGlobal.myVerify, ont, equipment);
                    myGlobal.myVerify.per2GResult = ret ? "Passed" : "Failed";
                    if (ret == false) {
                        _flagtestper2g = false;
                    }
                }

                //2. test PER 5G 
                if (myGlobal.mySetting.testPer5G == true) {
                    myGlobal.myVerify.per5GResult = "Waiting...";
                    bool ret = Test_Sensitivity_5G(myGlobal.myVerify, ont, equipment);
                    myGlobal.myVerify.per5GResult = ret ? "Passed" : "Failed";
                    if (ret == false) {
                        _flagtestper5g = false;
                    }
                }

                //3. test signal 2G
                if (myGlobal.mySetting.testSignal2G == true) {
                    myGlobal.myVerify.signal2GResult = "Waiting...";
                    bool ret = Verify_2G(myGlobal.myVerify, ont, equipment);
                    myGlobal.myVerify.signal2GResult = ret ? "Passed" : "Failed";
                    if (ret == false) {
                        _flagtestsignal2g = false;
                    }
                }

                //4. test signal 5G
                if (myGlobal.mySetting.testSignal5G == true) {
                    myGlobal.myVerify.signal5GResult = "Waiting...";
                    bool ret = Verify_5G(myGlobal.myVerify, ont, equipment);
                    myGlobal.myVerify.signal5GResult = ret ? "Passed" : "Failed";
                    if (ret == false) {
                        _flagtestsignal5g = false;
                    }
                }

                goto Finished;

            }
            catch {
                goto Finished;
            }

        //9. Hiển thị kết quả
        Finished:
            _flag = _flagconnection && _flagtestper2g && _flagtestper5g && _flagtestsignal2g && _flagtestsignal5g;
            Close_ONT(ref ont);
            Close_Instrument(ref equipment);
            myGlobal.myVerify.totalResult = _flag ? "Passed" : "Failed";
            return _flag;
        }

        #region Connection

        bool Connect_ONT(ref OntEconet<VerifyDataBinding> ont, string user, string pass) {
            try {
                int count = 0;
                bool r = false;

            RE:
                count++;
                ont = new OntEconet<VerifyDataBinding>(myGlobal.myVerify, myGlobal.mySetting.serialPortName);
                r = ont.Login(user, pass, 400);
                if (!r) {
                    myGlobal.myVerify.logSystem += string.Format("[FAIL] Connect to ONT FAIL => Retry {0}\r\n", count);
                    ont.Close();
                    if (count < 20) goto RE;
                }
                myGlobal.myVerify.logSystem += "[OK] Connect to ONT Successful.\r\n";
                if (r) {
                    myGlobal.myVerify.macAddress = ont.getMAC();
                    myGlobal.myVerify.logSystem += string.Format("ONT MAC Address: {0}\r\n", myGlobal.myVerify.macAddress);
                }
                return r;
            }
            catch {
                return false;
            }
        }

        bool Close_ONT(ref OntEconet<VerifyDataBinding> ont) {
            try {
                if (ont != null) ont.Close();
                return true;
            }
            catch { return false; }

        }

        bool Connect_Instrument(ref IInstrument equipment) {
            bool ret = false;
            try {
                if (myGlobal.mySetting.instrumentType == "E6640A") {
                    equipment = new E6640A<VerifyDataBinding>(myGlobal.myVerify, myGlobal.mySetting.gpibAddress, out ret);
                }
                else {
                    equipment = new MT8870A<VerifyDataBinding>(myGlobal.myVerify, myGlobal.mySetting.gpibAddress, out ret);
                }
                goto END;
            }
            catch {
                ret = false;
                goto END;
            }

        END:
            myGlobal.myVerify.logSystem += ret ? "[OK] Kết nối tới máy đo thành công.\n" : "FAIL: Lỗi kết nối máy đo.\n";
            return ret;
        }

        bool Close_Instrument(ref IInstrument equipment) {
            try {
                if (equipment != null) equipment.Close();
                return true;
            }
            catch { return false; }

        }

        #endregion


        string Name_measurement = myGlobal.mySetting.instrumentType;
        string RF_Port1 = myGlobal.mySetting.Port1;
        string RF_Port2 = myGlobal.mySetting.Port2;


        /// <summary>
        /// XÁC NHẬN WIFI-RX *******************************************
        /// 1. Test_Sensivitity_Detail -------//Core
        /// 2. AutoTestSensivitity -----------//Hỗ trợ tự động test nhiều
        /// 3. Test_Sensitivity_2G -----------//
        /// 4. Test_Sensitivity_5G -----------//
        /// 
        /// </summary>
        #region VERIFY RX

        private bool Test_Sensitivity_2G(VerifyDataBinding _ti, OntEconet<VerifyDataBinding> _mt, IInstrument _it) {
            myGlobal.myVerify.maxPER2G = myGlobal.listSensivitity2G.Count;
            return AutoTestSensivitity(_ti, _mt, _it, "2G");
        }

        private bool Test_Sensitivity_5G(VerifyDataBinding _ti, OntEconet<VerifyDataBinding> _mt, IInstrument _it) {
            myGlobal.myVerify.maxPER5G = myGlobal.listSensivitity5G.Count;
            return AutoTestSensivitity(_ti, _mt, _it, "5G");
        }

        private bool AutoTestSensivitity(VerifyDataBinding _ti, OntEconet<VerifyDataBinding> ModemTelnet, IInstrument instrument, string _CarrierFreq) {
            List<SensitivityConfigInfo> list = null;
            _ti.logSystem += "------------------------------------------------------------\r\n";
            _ti.logSystem += string.Format("Bắt đầu thực hiện Test Sensitivity {0}.\r\n", _CarrierFreq);
            list = _CarrierFreq == "2G" ? myGlobal.listSensivitity2G : myGlobal.listSensivitity5G;

            if (list.Count == 0) return false;
            bool result = true;
            string _error = "";
            string _antenna = "0";

            foreach (var item in list) {
                switch (_CarrierFreq) {
                    case "2G": { myGlobal.myVerify.valuePER2G++; break; }
                    case "5G": { myGlobal.myVerify.valuePER5G++; break; }
                }
                Stopwatch st = new Stopwatch();
                st.Start();

                string _channelNo = myGlobal.listAttenuator.Where(x => x.Frequency.Equals(item.channelfreq)).FirstOrDefault().Channel;

                //Switch RFIO Port
                if ((item.anten != _antenna) && (RF_Port1 != RF_Port2)) {
                    _ti.logSystem += string.Format("Switch From {0} to {1}\r\n", item.anten == "1" ? RF_Port2 : RF_Port1, item.anten == "1" ? RF_Port1 : RF_Port2);
                    int c = 0;
                RE:
                    c++;
                    bool ret = instrument.config_Instrument_OutputPort(item.anten == "1" ? RF_Port1 : RF_Port2, ref _error);
                    if (ret == false) if (c < 5) goto RE;
                    _antenna = item.anten;
                }

                _ti.logSystem += "*************************************************************************\r\n";
                _ti.logSystem += string.Format("{0} - {1} - {2} - MCS{3} - BW{4} - Anten {5} - Channel {6}\r\n", _CarrierFreq, item.anten == "1" ? RF_Port1 : RF_Port2, FunctionSupport.Get_WifiStandard_By_Mode(item.wifi, item.bandwidth), item.rate, 20 * Math.Pow(2, double.Parse(item.bandwidth)), item.anten, _channelNo);
                int count = 0;
                string pw_transmited = "";
                string rx_counted = "";
                string rx_per = "";
            REP:
                count++; string logError = "";
                PerResultInfo pri = null;
                if (!Test_Sensivitity_Detail(_ti, ModemTelnet, instrument, _CarrierFreq, item.wifi, item.rate, item.bandwidth, item.channelfreq, item.anten, item.packet, out pw_transmited, out rx_counted, out rx_per, out logError, out pri)) {
                    if (count < 9) {
                        _ti.logSystem += string.Format("RETRY = {0}\r\n", count);
                        goto REP;
                    }
                    else {
                        _ti.logSystem += string.Format("Phán định = {0}", "FAIL\r\n");
                        result = false;
                    }
                }
                else _ti.logSystem += string.Format("Phán định = {0}", "PASS\r\n");

                App.Current.Dispatcher.Invoke(new Action(() => {
                    if (_CarrierFreq.Equals("5G")) {
                        myGlobal.collectionPerResult5G.Add(pri);
                        if (result) myGlobal.myVerify.passedPER5G++;
                        else myGlobal.myVerify.failedPER5G++;
                    }
                    else {
                        myGlobal.collectionPerResult2G.Add(pri);
                        if (result) myGlobal.myVerify.passedPER2G++;
                        else myGlobal.myVerify.failedPER2G++;
                    }
                }));

                st.Stop();
                _ti.logSystem += string.Format("Thời gian test độ nhạy thu : {0} ms\r\n", st.ElapsedMilliseconds);
                _ti.logSystem += "\r\n";
            }
            return result;
        }


        private bool Test_Sensivitity_Detail(VerifyDataBinding _ti, OntEconet<VerifyDataBinding> ModemTelnet, IInstrument instrument, string standard_2G_5G, string Mode, string MCS, string BW, string Channel_Freq, string Anten, int packet, out string pw_tran, out string received, out string per, out string error, out PerResultInfo pri) {
            pri = new PerResultInfo() {
                Range = standard_2G_5G,
                Antenna = Anten,
                MCS = MCS,
                Channel = Channel_Freq.Replace("000000", ""),
                WiFi = convertWiFiStandard(Mode, BW),
                packetSent = packet.ToString()
            };

            pw_tran = received = per = ""; error = "";
            bool ret = false;
            try {

                string wave_form_name = "";
                string _channelNo = myGlobal.listAttenuator.Where(x => x.Frequency.Equals(Channel_Freq)).FirstOrDefault().Channel;
                double _attenuator = 0;
                if (Anten == "1") _attenuator = double.Parse(myGlobal.listAttenuator.Where(x => x.Frequency.Equals(Channel_Freq)).FirstOrDefault().Antenna1);
                if (Anten == "2") _attenuator = double.Parse(myGlobal.listAttenuator.Where(x => x.Frequency.Equals(Channel_Freq)).FirstOrDefault().Antenna2);
                pri.Attenuator = _attenuator.ToString();

                double power_transmit = -1000;
                double stPER = 0.0;
                double PER = 0.0;
                int RXCounter = 0;

                //Đọc giá trị PER, POWER TRANSMIT
                RxLimitInfo _limit = myGlobal.listLimitWifiRX.Where(x => x.rangefreq.Equals(standard_2G_5G) && x.wifi.Equals(FunctionSupport.Get_WifiStandard_By_Mode(Mode, BW)) && x.mcs.Equals(MCS)).FirstOrDefault();
                power_transmit = _limit.power_Transmit.Trim() == "-" ? -1000 : double.Parse(_limit.power_Transmit);
                pw_tran = power_transmit.ToString();
                stPER = _limit.PER.Trim() == "-" ? 0 : double.Parse(_limit.PER.Trim().Replace("%", ""));
                pri.powerTransmit = pw_tran;


                //Lấy tên file wave form
                wave_form_name = myGlobal.listWaveForm.Where(x => x.instrument.Equals(Name_measurement) && x.mcs.Equals(MCS) && x.bandwidth.Equals(BW) && x.wifi.Equals(Mode)).FirstOrDefault().waveform;
                pri.waveForm = wave_form_name;

                //Cấu hình ONT về chế độ WIFI RX
                _ti.logSystem += "Cấu hình ONT...\r\n";
                string _message = "";
                ModemTelnet.TestSensitivity_SendCommand(standard_2G_5G, Mode, MCS, BW, _channelNo, Anten, ref _message);

                //Điều khiển máy đo phát gói tin
                _ti.logSystem += string.Format("Cấu hình máy đo phát tín hiệu: Power={0} dBm, waveform={1}\r\n", power_transmit, wave_form_name);
                _ti.logSystem += string.Format("Attenuator: {0}(db)\n", _attenuator);

                instrument.config_HT20_RxTest_MAC(Channel_Freq, (power_transmit + _attenuator).ToString(), packet.ToString(), wave_form_name, Anten == "1" ? RF_Port1 : RF_Port2);
                Thread.Sleep(1000);

                //Đọc số gói tin nhận được từ ONT
                if (standard_2G_5G == "2G") Thread.Sleep(2000);
                RXCounter = int.Parse(ModemTelnet.TestSensitivity_ReadPER_SendCommand(standard_2G_5G, ref _message));
                received = RXCounter.ToString();
                pri.packetReceived = received;

                //Tính PER và hiển thị
                PER = Math.Round(((packet - RXCounter) * 100.0) / packet, 2);
                per = PER.ToString();
                _ti.logSystem += string.Format("PER = {0}%, Sent={1}, Received={2}\r\n", PER, packet, RXCounter);
                pri.PER = per;

                //So sánh PER với tiêu chuẩn
                bool _result = false;
                _result = PER <= stPER;

                ret = _result;
                goto END;
            }
            catch {
                ret = false;
                goto END;
            }

        END:
            pri.Result = ret ? "Passed" : "Failed";
            return ret;

        }
        #endregion


        #region VERIFY TX

        private bool Verify_2G(VerifyDataBinding _ti, OntEconet<VerifyDataBinding> _mt, IInstrument _it) {
            myGlobal.myVerify.maxSignal2G = myGlobal.listVerifySignal2G.Count;
            return AutoVerifySignal(_ti, _mt, _it, "2G");
        }

        private bool Verify_5G(VerifyDataBinding _ti, OntEconet<VerifyDataBinding> _mt, IInstrument _it) {
            myGlobal.myVerify.maxSignal5G = myGlobal.listVerifySignal5G.Count;
            return AutoVerifySignal(_ti, _mt, _it, "5G");
        }

        private bool AutoVerifySignal(VerifyDataBinding _ti, OntEconet<VerifyDataBinding> ModemTelnet, IInstrument instrument, string _CarrierFreq) {
            List<SignalConfigInfo> list = null;
            _ti.logSystem += "------------------------------------------------------------\r\n";
            _ti.logSystem += string.Format("Bắt đầu thực hiện quá trình Verify tín hiệu {0}.\r\n", _CarrierFreq);
            list = _CarrierFreq == "2G" ? myGlobal.listVerifySignal2G : myGlobal.listVerifySignal5G;

            if (list.Count == 0) return false;
            bool result = true;
            string _oldwifi = "";
            string _antenna = "0";
            string _error = "";

            foreach (var item in list) {
                switch (_CarrierFreq) {
                    case "5G": { myGlobal.myVerify.valueSignal5G++; break; }
                    case "2G": { myGlobal.myVerify.valueSignal2G++; break; }
                }
                
                Stopwatch st = new Stopwatch();
                st.Start();

                string _eqChannel = string.Format("{0}000000", item.channelfreq);
                string _channelNo = myGlobal.listAttenuator.Where(x => x.Frequency.Equals(item.channelfreq)).FirstOrDefault().Channel;
                double _attenuator = 0;
                if (item.anten == "1") _attenuator = double.Parse(myGlobal.listAttenuator.Where(x => x.Frequency.Equals(item.channelfreq)).FirstOrDefault().Antenna1);
                if (item.anten == "2") _attenuator = double.Parse(myGlobal.listAttenuator.Where(x => x.Frequency.Equals(item.channelfreq)).FirstOrDefault().Antenna2);
                string _wifi = "";
                switch (item.wifi) {
                    case "0": { _wifi = "b"; break; }
                    case "1": { _wifi = "g"; break; }
                    case "3": { _wifi = string.Format("n{0}", item.bandwidth == "0" ? "20" : "40"); break; }
                    case "4": {
                            switch (item.bandwidth) {
                                case "0": { _wifi = "ac20"; break; }
                                case "1": { _wifi = "ac40"; break; }
                                case "2": { _wifi = "ac80"; break; }
                                case "3": { _wifi = "ac160"; break; }
                            }
                            break;
                        }
                }
                if (_oldwifi != _wifi) {
                    _error = "";
                    instrument.config_Instrument_Total(item.anten == "1" ? RF_Port1 : RF_Port2, _wifi, ref _error);
                    _oldwifi = _wifi;
                }

                //Switch RFIO Port
                if ((item.anten != _antenna) && (RF_Port1 != RF_Port2)) {
                    _ti.logSystem += string.Format("Switch From {0} to {1}\r\n", item.anten == "1" ? RF_Port2 : RF_Port1, item.anten == "1" ? RF_Port1 : RF_Port2);
                    int c = 0;
                RE:
                    c++;
                    bool ret = instrument.config_Instrument_Port(item.anten == "1" ? RF_Port1 : RF_Port2, ref _error);
                    if (ret == false) if (c < 5) goto RE;
                    _antenna = item.anten;
                }

                //Verify Signal
                _ti.logSystem += "*************************************************************************\r\n";
                _ti.logSystem += string.Format("{0} - {1} - {2} - MCS{3} - BW{4} - Anten {5} - Channel {6}\r\n", _CarrierFreq, item.anten == "1" ? RF_Port1 : RF_Port2, FunctionSupport.Get_WifiStandard_By_Mode(item.wifi, item.bandwidth), item.rate, 20 * Math.Pow(2, double.Parse(item.bandwidth)), item.anten, _channelNo);
                _ti.logSystem += string.Format("Attenuator: {0}(dbm)\n", _attenuator);

                int count = 0;
                string _Power = "", _Evm = "", _FreqErr = "", _pStd = "", _eMax = "";
                bool _kq = true;
            REP:
                count++;
                string errorlog = "";
                SignalResultInfo sri = null;
                if (!Verify_Signal(_ti, ModemTelnet, instrument, _CarrierFreq, item.wifi, item.rate, item.bandwidth, _eqChannel, item.anten, _attenuator, ref _Power, ref _Evm, ref _FreqErr, ref _pStd, ref _eMax, out errorlog, out sri)) {
                    if (count < 2) {
                        _ti.logSystem += string.Format("RETRY = {0}\r\n", count);
                        _kq = false;
                        goto REP;
                    }
                    else {
                        _ti.logSystem += string.Format("Phán định = {0}", "FAIL\r\n");
                        _kq = false;
                        result = false;
                    }
                }
                else {
                    _ti.logSystem += string.Format("Phán định = {0}\r\n", "PASS");
                    _kq = true;
                }

                App.Current.Dispatcher.Invoke(new Action(() => {
                    if (_CarrierFreq.Equals("5G")) {
                        myGlobal.collectionSignalResult5G.Add(sri);
                        if (_kq) myGlobal.myVerify.passedSignal5G++;
                        else myGlobal.myVerify.failedSignal5G++;
                    }
                    else {
                        myGlobal.collectionSignalResult2G.Add(sri);
                        if (_kq) myGlobal.myVerify.passedSignal2G++;
                        else myGlobal.myVerify.failedSignal2G++;
                    }
                }));

                st.Stop();
                _ti.logSystem += string.Format("Thời gian verify : {0} ms\r\n", st.ElapsedMilliseconds);
                _ti.logSystem += "\r\n";
            }
            return result;
        }

        private bool Verify_Signal(VerifyDataBinding _ti, OntEconet<VerifyDataBinding> ModemTelnet, IInstrument instrument, string standard_2G_5G, string Mode, string MCS, string BW, string Channel_Freq, string Anten, double Attenuator, ref string _pw, ref string _evm, ref string _freqerr, ref string _pstd, ref string _evmmax, out string error, out SignalResultInfo sri) {
            sri = new SignalResultInfo() {
                Range = standard_2G_5G,
                WiFi = convertWiFiStandard(Mode, BW),
                MCS = MCS,
                Antenna = Anten,
                Attenuator = Attenuator.ToString(),
                Channel = Channel_Freq.Replace("000000", "")
            };
            bool ret = false;

            string Result_Measure_temp = ""; error = "";
            decimal Pwr_measure_temp, EVM_measure_temp, FreqErr_measure_temp;
            string _wifi = "";
            try {
                switch (Mode) {
                    case "0": { _wifi = "b"; break; }
                    case "1": { _wifi = "g"; break; }
                    case "3": { _wifi = string.Format("n{0}", BW == "0" ? "20" : "40"); break; }
                    case "4": {
                            switch (BW) {
                                case "0": { _wifi = "ac20"; break; }
                                case "1": { _wifi = "ac40"; break; }
                                case "2": { _wifi = "ac80"; break; }
                                case "3": { _wifi = "ac160"; break; }
                            }
                            break;
                        }
                }

                //Đọc giá trị tiêu chuẩn
                TxLimitInfo _limit = myGlobal.listLimitWifiTX.Where(x => x.rangefreq.Equals(standard_2G_5G) && x.wifi.Equals(FunctionSupport.Get_WifiStandard_By_Mode(Mode, BW)) && x.mcs.Equals(MCS)).FirstOrDefault();

                //Thiết lập tần số máy đo
                string _error = "";
                instrument.config_Instrument_Channel(Channel_Freq, ref _error);

                //Gửi lệnh yêu cầu ONT phát WIFI TX
                string _message = "";
                ModemTelnet.Verify_Signal_SendCommand(standard_2G_5G, Mode, MCS, BW, Channel_Freq, Anten, ref _message);

                int count = 0;
                List<decimal> list_Power = new List<decimal>();
                List<decimal> list_Evm = new List<decimal>();
                List<decimal> list_FreqErr = new List<decimal>();
            RE:
                count++;
                //Đọc kết quả từ máy đo
                Result_Measure_temp = instrument.config_Instrument_get_TotalResult("RFB", _wifi);

                //Lấy dữ liệu Power
                Pwr_measure_temp = Decimal.Parse(Result_Measure_temp.Split(',')[19], System.Globalization.NumberStyles.Float) + Convert.ToDecimal(Attenuator);
                if (Pwr_measure_temp < 15) {
                    instrument.config_Instrument_get_TotalResult("VID", _wifi);
                    Result_Measure_temp = instrument.config_Instrument_get_TotalResult("VID", _wifi);
                    Pwr_measure_temp = Decimal.Parse(Result_Measure_temp.Split(',')[19], System.Globalization.NumberStyles.Float) + Convert.ToDecimal(Attenuator);
                }
                list_Power.Add(Pwr_measure_temp);

                //Lấy dữ liệu EVM
                EVM_measure_temp = Decimal.Parse(Result_Measure_temp.Split(',')[1], System.Globalization.NumberStyles.Float);
                list_Evm.Add(EVM_measure_temp);

                //Lấy dữ liệu Frequency Error
                FreqErr_measure_temp = Decimal.Parse(Result_Measure_temp.Split(',')[7], System.Globalization.NumberStyles.Float);
                list_FreqErr.Add(FreqErr_measure_temp);

                //Hiển thị kết quả đo lên giao diện phần mềm
                _pw = Pwr_measure_temp.ToString("0.##");
                _evm = EVM_measure_temp.ToString("0.##");
                _freqerr = FreqErr_measure_temp.ToString("0.##");
                _pstd = string.Format("{0}~{1}", _limit.power_MIN, _limit.power_MAX);
                _evmmax = string.Format("{0}", _limit.evm_MAX);

                _ti.logSystem += "Average Power = " + _pw + " dBm ... ";
                _ti.logSystem += string.Format("EVM All Carriers = {0} {1} ... ", _evm, _wifi == "b" ? " %" : " dB");
                _ti.logSystem += "Center Frequency Error = " + _freqerr + " Hz\r\n";

                if (count < 1) goto RE;

                Pwr_measure_temp = list_Power.Average();
                EVM_measure_temp = list_Evm.Average();
                FreqErr_measure_temp = list_FreqErr.Average();

                sri.Power = Math.Round(Pwr_measure_temp, 2).ToString();
                sri.EVM = Math.Round(EVM_measure_temp, 2).ToString();
                sri.freqError = Math.Round(FreqErr_measure_temp, 2).ToString();

                //So sánh kết quả đo với giá trị tiêu chuẩn
                bool _result = false, _powerOK = false, _evmOK = false, _freqerrOK = true;
                _powerOK = FunctionSupport.Compare_TXMeasure_With_Standard(_limit.power_MAX, _limit.power_MIN, Pwr_measure_temp);
                _evmOK = FunctionSupport.Compare_TXMeasure_With_Standard(_limit.evm_MAX, _limit.evm_MIN, EVM_measure_temp);
                _freqerrOK = FunctionSupport.Compare_TXMeasure_With_Standard(_limit.freqError_MAX, _limit.freqError_MIN, FreqErr_measure_temp);

                if (_powerOK == false) {
                    _ti.logSystem += "FAIL: Power\r\n";
                    error += string.Format($"[FAIL] Verify tín hiệu TX Power = {_pw} =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");
                }
                else if (_evmOK == false) {
                    error += string.Format($"[FAIL] Verify tín hiệu TX EVM = {_evm} =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");
                    _ti.logSystem += "FAIL: EVM\r\n";
                }
                else if (_freqerrOK == false) {
                    _ti.logSystem += "FAIL: Frequency Error\r\n";
                    error += string.Format($"[FAIL] Verify tín hiệu TX Frequency Error = {_freqerr} =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");
                }
                _result = _powerOK && _evmOK && _freqerrOK;
                ret = _result;
                goto END;
            }
            catch {
                error += string.Format($"[FAIL] Verify tín hiệu TX  =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");
                ret = false;

            }

        END:
            sri.Result = ret ? "Passed" : "Failed";
            return ret;
        }

        private string convertWiFiStandard(string mode, string bandwidth) {
            string w = "", bw = "";
            switch (mode) {
                case "0": { w = "802.11b"; break; }
                case "1": { w = "802.11g"; break; }
                case "2": { w = "802.11a"; break; }
                case "3": { w = "802.11n"; break; }
                case "4": { w = "802.11ac"; break; }
            }
            switch (bandwidth) {
                case "0": { bw = "20"; break; }
                case "1": { bw = "40"; break; }
                case "2": { bw = "80"; break; }
                case "3": { bw = "160"; break; }
            }

            return $"{w}{bw}";
        }

        #endregion

    }
}
