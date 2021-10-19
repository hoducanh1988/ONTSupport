﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace GW040Hv2Project.Function {
    public class CalculateMaster {

        ModemTelnet _modem = null;
        Instrument _instrument = null;
        formattinfo _formi = null;
        string Name_measurement = GlobalData.initSetting.INSTRUMENT;
        string RF_Port1 = GlobalData.initSetting.RFPORT1;
        string RF_Port2 = GlobalData.initSetting.RFPORT2;
        public bool total_result = true;
        int delay_instrument = 0;
        int delay_modem = 100;


        public CalculateMaster(ModemTelnet _mt, Instrument _it, formattinfo _fi) {
            this._modem = _mt;
            this._instrument = _it;
            this._formi = _fi;
            total_result = false;

            string[] lines = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}sleep.ini");
            delay_instrument = int.Parse(lines[0].Split('=')[1]);
            delay_modem = int.Parse(lines[1].Split('=')[1]);

        }
        public bool Excute(out string error) {
            error = "";
            try {
                //Do suy hao
                GlobalData.mtIndex = 0;
                GlobalData.mtIsOk = true;
                total_result = false;
                if (!Verify_Master(_formi, _modem, _instrument)) return false;
                //Attenuator.Save();
                return true;
            }
            catch (Exception ex) {
                error = ex.ToString();
                return false;
            }
        }

        private bool Verify_Signal(formattinfo _fi, ModemTelnet ModemTelnet, Instrument instrument, string Mode, string MCS, string BW, string Channel_Freq, string Anten, int _Id) {
            try {
                string Result_Measure_temp = "";
                double Pwr_measure_temp = 0;
                string _wifi = "";
                string standard_2G_5G = int.Parse(Channel_Freq.Substring(0, 4)) < 3000 ? "2G" : "5G";

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


                RE:
                //Gửi lệnh yêu cầu ONT phát WIFI TX
                string _message = "";
                ModemTelnet.Verify_Signal_SendCommand(standard_2G_5G, Mode, MCS, BW, Channel_Freq, Anten, ref _message);
                Thread.Sleep(delay_modem);

                //Đọc kết quả từ máy đo
                //Result_Measure_temp = instrument.config_Instrument_get_Power("RFB", _wifi);
                Result_Measure_temp = instrument.config_Instrument_get_Power("VID", _wifi);

                //Lấy dữ liệu Power
                try {
                    Pwr_measure_temp = double.Parse(Result_Measure_temp);
                }
                catch {
                    goto RE;
                }


                value = Pwr_measure_temp.ToString();

                //Hiển thị kết quả đo lên giao diện phần mềm (RichTextBox)
                _fi.LOGDATA += "Average Power = " + Pwr_measure_temp.ToString("0.##") + " dBm\r\n";

                App.Current.Dispatcher.BeginInvoke(new Action(() => {
                    foreach (var item in GlobalData.autoCalculateMaster) {
                        if (item.Anten == Anten && item.Frequency == Channel_Freq.Substring(0, 4)) {
                            switch (_Id) {
                                case 0: { item.Value1 = value; break; }
                                case 1: { item.Value2 = value; break; }
                                case 2: { item.Value3 = value; break; }
                                case 3: { item.Value4 = value; break; }
                                case 4: {
                                        item.Value5 = value;
                                        List<double> lst = new List<double>() {
                                            double.Parse(item.Value1),
                                            double.Parse(item.Value2),
                                            double.Parse(item.Value3),
                                            double.Parse(item.Value4),
                                            double.Parse(item.Value5)
                                        };

                                        item.maxValue = lst.Max().ToString();
                                        item.minValue = lst.Min().ToString();
                                        item.Difference = (Math.Round(lst.Max() - lst.Min(), 3)).ToString();
                                        item.Result = double.Parse(item.Difference) <= 0.5 ? "Passed" : "Failed";
                                        if (item.Result.Equals("Failed")) total_result = false;
                                        break;
                                    }
                            }

                            double _avr = Math.Round((double.Parse(value)), 3);
                            double _ret = _avr + double.Parse(item.wirePower);
                            if (item.masterPower == "" || item.masterPower == null || item.masterPower.Length == 0) {
                                item.masterPower = _ret.ToString();
                            }
                            else {
                                if (_ret - double.Parse(item.masterPower) > 0.2) item.masterPower = _ret.ToString();
                                else if (_ret - double.Parse(item.masterPower) <= 0.2 && _ret - double.Parse(item.masterPower) >= -0.2) {
                                    item.masterPower = Math.Round((double.Parse(item.masterPower) + _ret) / 2, 3).ToString();
                                }
                            }
                           
                            GlobalData.mtIndex++;
                            break;
                        }
                    }
                }));
                return true;
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.ToString());
                return false;
            }
        }

        private bool AutoVerifySignal(formattinfo _fi, ModemTelnet ModemTelnet, Instrument instrument, int _Id) {
            try {
                List<verifysignal> list = null;
                _fi.LOGDATA += "------------------------------------------------------------\r\n";
                _fi.LOGDATA += string.Format("Bắt đầu thực hiện quá trình đo master power.\r\n");
                list = GlobalData.listCalMaster;

                if (list.Count == 0) return false;
                bool result = true;
                string _oldwifi = "";
                string _antenna = "0";
                string _error = "";

                foreach (var item in list) {
                    Stopwatch st = new Stopwatch();
                    st.Start();

                    string _eqChannel = string.Format("{0}000000", item.channelfreq);
                    double _attenuator = Attenuator.getAttenuator(item.channelfreq, item.anten);
                    string _channelNo = Attenuator.getChannelNumber(item.channelfreq);
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
                        _fi.LOGDATA += string.Format("Switch From {0} to {1}\r\n", item.anten == "1" ? RF_Port2 : RF_Port1, item.anten == "1" ? RF_Port1 : RF_Port2);
                        int c = 0;
                        RE:
                        c++;
                        bool ret = instrument.config_Instrument_Port(item.anten == "1" ? RF_Port1 : RF_Port2, ref _error);
                        if (ret == false) if (c < 5) goto RE;
                        _antenna = item.anten;
                    }

                    _fi.LOGDATA = "";
                    _fi.LOGDATA += "*************************************************************************\r\n";
                    _fi.LOGDATA += string.Format("{0} - {1} - MCS{2} - BW{3} - Anten {4} - Channel {5}\r\n", item.anten == "1" ? RF_Port1 : RF_Port2, FunctionSupport.Get_WifiStandard_By_Mode(item.wifi, item.bandwidth), item.rate, 20 * Math.Pow(2, double.Parse(item.bandwidth)), item.anten, _channelNo);
                    int count = 0;
                    REP:
                    count++;
                    if (!Verify_Signal(_fi, ModemTelnet, instrument, item.wifi, item.rate, item.bandwidth, _eqChannel, item.anten, _Id)) {
                        if (count < 2) {
                            _fi.LOGDATA += string.Format("RETRY = {0}\r\n", count);
                            goto REP;
                        }
                        else {
                            _fi.LOGDATA += string.Format("Phán định = {0}", "FAIL\r\n");
                            result = false;
                        }

                    }
                    else _fi.LOGDATA += string.Format("Phán định = {0}\r\n", "PASS");
                    st.Stop();
                    _fi.LOGDATA += string.Format("Thời gian đo : {0} ms\r\n", st.ElapsedMilliseconds);
                    _fi.LOGDATA += "\r\n";

                    if (GlobalData.mtIsOk == false) {
                        result = false;
                        break;
                    }
                }

                //Save master data
                //if (result == true) Master.Save();
                //else
                //    System.Windows.MessageBox.Show("Mạch không đủ tiêu chuẩn làm Master.", "Master", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return result;
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.ToString());
                return false;
            }
        }


        private bool Verify_Master(formattinfo _fi, ModemTelnet _mt, Instrument _it) {
            for (int i = 0; i < 5; i++) {
                GlobalData.mtIndex = 0;
                AutoVerifySignal(_fi, _mt, _it, i);
            }

            if (total_result == true) {
                Master.Save();
                Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}tmpMaster");
            }
            return true;
        }


    }
}
