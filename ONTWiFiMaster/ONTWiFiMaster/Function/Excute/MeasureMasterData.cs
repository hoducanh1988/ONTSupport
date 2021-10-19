using ONTWiFiMaster.Function.Base;
using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Dut;
using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.Function.Instrument;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Excute {
    public class MeasureMasterData {
        
        int delay_instrument = 0;
        int delay_modem = 100;

        public MeasureMasterData() {
            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}sleep.ini");
            delay_instrument = int.Parse(lines[0].Split('=')[1]);
            delay_modem = int.Parse(lines[1].Split('=')[1]);
        }

        public bool Excuted(List<ItemMasterResult> itemMasters) {
            bool _flag = true;
            OntEconet<MasterDataBinding> ont = null;
            IInstrument equipment = null;
            App.Current.Dispatcher.Invoke(new Action(() => {
                foreach (var item in myGlobal.collectionMaster) {
                    item.Power1 = item.Power2 = item.Power3 = item.Power4 = item.Power5 = item.Master = "";
                    item.Diff = item.Max = item.Min = item.Result = "";
                }
                foreach (var item in myGlobal.listItemMaster) { item.valueProgress = 0; item.maxProgress = 1; }
            }));

            try {
                myGlobal.myMaster.totalResult = "Waiting...";

                //0. Kết nối telnet tới ONT và máy đo
                _flag = Connect_ONT(ref ont, myGlobal.mySetting.loginUser, myGlobal.mySetting.loginPassword);
                if (!_flag) goto Finished;
                _flag = Connect_Instrument(ref equipment);
                if (!_flag) goto Finished;

                //1. tinh suy hao master
                _flag = Verify_Master(itemMasters, myGlobal.myMaster, ont, equipment);
                goto Finished;

            }
            catch {
                goto Finished;
            }

        //9. Hiển thị kết quả
        Finished:
            Close_ONT(ref ont);
            Close_Instrument(ref equipment);
            myGlobal.myMaster.totalResult = _flag ? "Passed" : "Failed";
            return _flag;
        }



        #region Connection

        bool Connect_ONT(ref OntEconet<MasterDataBinding> ont, string user, string pass) {
            try {
                int count = 0;
                bool r = false;

            RE:
                count++;
                ont = new OntEconet<MasterDataBinding>(myGlobal.myMaster, myGlobal.mySetting.serialPortName);
                r = ont.Login(user, pass, 400);
                if (!r) {
                    myGlobal.myMaster.logSystem += string.Format("[FAIL] Connect to ONT FAIL => Retry {0}\r\n", count);
                    ont.Close();
                    if (count < 20) goto RE;
                }
                myGlobal.myMaster.logSystem += "[OK] Connect to ONT Successful.\r\n";
                if (r) {
                    myGlobal.myMaster.macAddress = ont.getMAC();
                    myGlobal.myMaster.logSystem += string.Format("ONT MAC Address: {0}\r\n", myGlobal.myMaster.macAddress);
                }
                return r;
            }
            catch {
                return false;
            }
        }

        bool Close_ONT(ref OntEconet<MasterDataBinding> ont) {
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
                    equipment = new E6640A<MasterDataBinding>(myGlobal.myMaster, myGlobal.mySetting.gpibAddress, out ret);
                }
                else {
                    equipment = new MT8870A<MasterDataBinding>(myGlobal.myMaster, myGlobal.mySetting.gpibAddress, out ret);
                }
                goto END;
            }
            catch {
                ret = false;
                goto END;
            }

        END:
            myGlobal.myMaster.logSystem += ret ? "[OK] Kết nối tới máy đo thành công.\n" : "FAIL: Lỗi kết nối máy đo.\n";
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

        #region tinh du lieu mạch master

        string Name_measurement = myGlobal.mySetting.instrumentType;
        string RF_Port1 = myGlobal.mySetting.Port1;
        string RF_Port2 = myGlobal.mySetting.Port2;
        bool total_result = true;

        private bool Verify_Master(List<ItemMasterResult> itemMasters, MasterDataBinding _fi, OntEconet<MasterDataBinding> _mt, IInstrument _it) {
            total_result = true;
            for (int i = 0; i < itemMasters.Count; i++) {
                myGlobal.myMaster.currentIndex = 0;
                //itemMasters[i].Result = "Waiting...";
                itemMasters[i].maxProgress = myGlobal.listCalMaster.Count;
                itemMasters[i].valueProgress = 0;
                bool r = AutoVerifySignal(_fi, _mt, _it, i, itemMasters[i]);
                if (!r) total_result = false;
                if (total_result == false) break;
                //itemMasters[i].Result = "Completed";
            }
            return total_result;
        }


        private bool AutoVerifySignal(MasterDataBinding _fi, OntEconet<MasterDataBinding> ModemTelnet, IInstrument instrument, int _Id, ItemMasterResult imr) {
            bool result = true;
            Stopwatch st = new Stopwatch();
            st.Start();
            try {
                List<SignalConfigInfo> list = null;
                _fi.logSystem += "------------------------------------------------------------\r\n";
                _fi.logSystem += string.Format("Bắt đầu thực hiện quá trình đo master power.\r\n");
                list = myGlobal.listCalMaster;
                if (list.Count == 0) { result = false; goto END; }
                
                string _oldwifi = "";
                string _antenna = "0";
                string _error = "";

                foreach (var item in list) {
                    imr.valueProgress++;
                    
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
                        _fi.logSystem += string.Format("Switch From {0} to {1}\r\n", item.anten == "1" ? RF_Port2 : RF_Port1, item.anten == "1" ? RF_Port1 : RF_Port2);
                        int c = 0;
                    RE:
                        c++;
                        bool ret = instrument.config_Instrument_Port(item.anten == "1" ? RF_Port1 : RF_Port2, ref _error);
                        if (ret == false) if (c < 5) goto RE;
                        _antenna = item.anten;
                    }

                    _fi.logSystem += "";
                    _fi.logSystem += "*************************************************************************\r\n";
                    _fi.logSystem += string.Format("{0} - {1} - MCS{2} - BW{3} - Anten {4} - Channel {5}\r\n", item.anten == "1" ? RF_Port1 : RF_Port2, FunctionSupport.Get_WifiStandard_By_Mode(item.wifi, item.bandwidth), item.rate, 20 * Math.Pow(2, double.Parse(item.bandwidth)), item.anten, _channelNo);
                    _fi.logSystem += string.Format("Suy hao = {0} dBm\n", _attenuator);
                    int count = 0;
                REP:
                    count++;
                    if (!Verify_Signal(_fi, ModemTelnet, instrument, item.wifi, item.rate, item.bandwidth, _eqChannel, item.anten, _Id)) {
                        if (count < 3) {
                            _fi.logSystem += string.Format("RETRY = {0}\n", count);
                            goto REP;
                        }
                        else {
                            _fi.logSystem += string.Format("Phán định = {0}", "FAIL\n");
                            result = false;
                        }

                    }
                    else {
                        _fi.logSystem += string.Format("Phán định = {0}\r\n", "PASS");
                    }
                }
            }
            catch {
                result = false;
            }

            END:
            st.Stop();
            _fi.logSystem += string.Format("Thời gian đo : {0} ms\r\n", st.ElapsedMilliseconds);
            _fi.logSystem += "\r\n";
            return result;
        }

        private bool Verify_Signal(MasterDataBinding _fi, OntEconet<MasterDataBinding> ModemTelnet, IInstrument instrument, string Mode, string MCS, string BW, string Channel_Freq, string Anten, int _Id) {
            try {
                string Result_Measure_temp = "";
                double Pwr_measure_temp = 0;
                string _wifi = "";
                string standard_2G_5G = int.Parse(Channel_Freq.Substring(0, 4)) < 3000 ? "2G" : "5G";
                bool ret = true;

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

                //Thiết lập tần số máy đo
                string _error = "";
                instrument.config_Instrument_Channel(Channel_Freq, ref _error);
                Thread.Sleep(delay_instrument);
                string value = "";

                //Gửi lệnh yêu cầu ONT phát WIFI TX
                string _message = "";
                ModemTelnet.Verify_Signal_SendCommand(standard_2G_5G, Mode, MCS, BW, Channel_Freq, Anten, ref _message);
                Thread.Sleep(delay_modem);

                int count = 0;
                List<double> list_in = new List<double>();
            RE:
                count++;
                //Đọc kết quả từ máy đo
                //Result_Measure_temp = instrument.config_Instrument_get_Power("RFB", _wifi);
                Result_Measure_temp = instrument.config_Instrument_get_Power("VID", _wifi);

                //Lấy dữ liệu Power
                Pwr_measure_temp = double.Parse(Result_Measure_temp);
                list_in.Add(Pwr_measure_temp);

                //try {
                //    Pwr_measure_temp = double.Parse(Result_Measure_temp);
                //    list_in.Add(Pwr_measure_temp);
                //}
                //catch {
                //    if (count < 3) goto RE;
                //    else {
                //        Pwr_measure_temp = double.MaxValue;
                //        list_in.Add(Pwr_measure_temp);
                //    }
                //}

                if (list_in.Count < 3) goto RE;

                List<double> list_out = null;
                bool r = FunctionSupport.rejectDifferenceValueFromList(list_in, out list_out);
                Pwr_measure_temp = list_out.Average();

                value = Math.Round(Pwr_measure_temp, 2).ToString();

                //Hiển thị kết quả đo lên giao diện phần mềm 
                _fi.logSystem += "Average Power = " + value.ToString() + " dBm\r\n";

                App.Current.Dispatcher.BeginInvoke(new Action(() => {
                    foreach (var item in myGlobal.collectionMaster) {
                        if (item.Antenna == Anten && item.Frequency == Channel_Freq.Substring(0, 4)) {
                            List<double> lst = null;
                            switch (_Id) {
                                case 0: { item.Power1 = value; break; }
                                case 1: {
                                        item.Power2 = value;
                                        lst = new List<double>() { double.Parse(item.Power1), double.Parse(item.Power2) };
                                        break;
                                    }
                                case 2: {
                                        item.Power3 = value;
                                        lst = new List<double>() { double.Parse(item.Power1), double.Parse(item.Power2), double.Parse(item.Power3) };
                                        break;
                                    }
                                case 3: {
                                        item.Power4 = value;
                                        lst = new List<double>() { double.Parse(item.Power1), double.Parse(item.Power2), double.Parse(item.Power3), double.Parse(item.Power4) };
                                        break;
                                    }
                                case 4: {
                                        item.Power5 = value;
                                        lst = new List<double>() { double.Parse(item.Power1), double.Parse(item.Power2), double.Parse(item.Power3), double.Parse(item.Power4), double.Parse(item.Power5) };
                                        break;
                                    }
                            }

                            if (_Id > 0) {
                                item.Max = lst.Max().ToString();
                                item.Min = lst.Min().ToString();
                                item.Diff = (Math.Round(lst.Max() - lst.Min(), 3)).ToString();

                                //check power here
                                bool r1 = (lst.Min() + double.Parse(item.Attenuator)) >= double.Parse(myGlobal.mySetting.lowerLimit) && (lst.Max() + double.Parse(item.Attenuator)) <= double.Parse(myGlobal.mySetting.upperLimit);
                                bool r2 = double.Parse(item.Diff) <= double.Parse(myGlobal.mySetting.Difference);

                                //
                                item.Result = r1 && r2 ? "Passed" : "Failed";
                                if (item.Result.Equals("Failed")) total_result = false;
                                ret = item.Result.Equals("Passed") ? true : false;
                            }

                            double _avr = Math.Round((double.Parse(value)), 3);
                            double _ret = _avr + double.Parse(item.Attenuator);
                            if (item.Master == "" || item.Master == null || item.Master.Length == 0) {
                                item.Master = _ret.ToString();
                            }
                            else {
                                if (_ret - double.Parse(item.Master) > double.Parse(myGlobal.mySetting.Difference)) { item.Master = _ret.ToString(); }
                                else if (_ret - double.Parse(item.Master) <= double.Parse(myGlobal.mySetting.Difference) && _ret - double.Parse(item.Master) >= -double.Parse(myGlobal.mySetting.Difference)) {
                                    item.Master = Math.Round((double.Parse(item.Master) + _ret) / 2, 3).ToString();
                                }
                            }


                            myGlobal.myMaster.currentIndex++;
                            break;
                        }
                    }
                }));
                return ret;
            }
            catch {
                return false;
            }
        }


        #endregion


    }
}
