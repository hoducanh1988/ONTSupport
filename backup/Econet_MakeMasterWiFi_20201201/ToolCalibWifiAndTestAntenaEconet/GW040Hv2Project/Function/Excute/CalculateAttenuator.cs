using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {
    public class CalculateAttenuator {

        ModemTelnet _modem = null;
        Instrument _instrument = null;
        formattinfo _formi = null;
        string Name_measurement = GlobalData.initSetting.INSTRUMENT;
        string RF_Port1 = GlobalData.initSetting.RFPORT1;
        string RF_Port2 = GlobalData.initSetting.RFPORT2;

        public CalculateAttenuator(ModemTelnet _mt, Instrument _it, formattinfo _fi) {
            this._modem = _mt;
            this._instrument = _it;
            this._formi = _fi;
        }

        public bool Excute(out string error) {
            error = "";
            try {
                //Do suy hao
                if (!Verify_Attenuator(_formi, _modem, _instrument)) return false;
                Attenuator.Save();
                return true;
            } catch (Exception ex) {
                error = ex.ToString();
                return false;
            }
        }

        private bool Verify_Signal(formattinfo _fi, ModemTelnet ModemTelnet, Instrument instrument, string Mode, string MCS, string BW, string Channel_Freq, string Anten) {
            try {
                autoattenuator _at = new autoattenuator() { Anten = Anten, Frequency = Channel_Freq.Substring(0,4), Channel = Master.getChannel(Channel_Freq.Substring(0,4)), PowerMaster = Master.getPower(Channel_Freq.Substring(0,4), Anten) };

                string Result_Measure_temp = "";
                decimal Pwr_measure_temp = 0;
                string _wifi = "";
                string standard_2G_5G = int.Parse(Channel_Freq.Substring(0,4)) < 3000 ? "2G" : "5G";

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
                //Gửi lệnh yêu cầu ONT phát WIFI TX
                string _message = "";
                ModemTelnet.Verify_Signal_SendCommand(standard_2G_5G, Mode, MCS, BW, Channel_Freq, Anten, ref _message);//2G/1/7/0/2412000000/1

                int count = 0;
                List<double> list_in = new List<double>();
            RE:
                count++;
                //Đọc kết quả từ máy đo
                Result_Measure_temp = instrument.config_Instrument_get_TotalResult("RFB", _wifi);

                //Lấy dữ liệu Power
                try {
                    Pwr_measure_temp = Decimal.Parse(Result_Measure_temp.Split(',')[19], System.Globalization.NumberStyles.Float);
                    list_in.Add((double)Pwr_measure_temp);
                    _fi.LOGDATA += "Average Power = " + Pwr_measure_temp.ToString("0.##") + " dBm\r\n";
                    if (list_in.Count < 3) goto RE;

                } catch {
                    if (count < 3) { goto RE; }
                    instrument.ClearCommand();
                }

                List<double> list_out = null;
                bool r = baseFunction.rejectDifferenceValueFromList(list_in, out list_out);
                Pwr_measure_temp =(decimal) list_out.Average();

                _at.measuredPower = Pwr_measure_temp.ToString();
                _at.Attenuator = (double.Parse(_at.PowerMaster) - double.Parse(_at.measuredPower)).ToString();

                //Hiển thị kết quả đo lên giao diện phần mềm (RichTextBox)
                //_fi.LOGDATA += "Average Power = " + Pwr_measure_temp.ToString("0.##") + " dBm\r\n";

                App.Current.Dispatcher.BeginInvoke(new Action(() => { GlobalData.autoAttenuator.Add(_at); }));
                return true;
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.ToString());
                return false;
            }
        }

        
        private bool AutoVerifySignal(formattinfo _fi, ModemTelnet ModemTelnet, Instrument instrument) {
            List<verifysignal> list = null;
            _fi.LOGDATA += "------------------------------------------------------------\r\n";
            _fi.LOGDATA += string.Format("Bắt đầu thực hiện quá trình đo suy hao.\r\n");
            list = GlobalData.listCalAttenuator;
           

            if (list.Count == 0) return false;
            bool result = true;
            string _oldwifi = "";
            string _antenna = "0";
            string _error = "";

            foreach (var item in list) {
                Stopwatch st = new Stopwatch();
                if(item.channelfreq.Contains("5560"))
                {
                }
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
                    instrument.config_Instrument_Total(item.anten == "1" ?  RF_Port1 : RF_Port2, _wifi, ref _error);
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
                _fi.LOGDATA += "*************************************************************************\r\n";
                _fi.LOGDATA += string.Format("{0} - {1} - MCS{2} - BW{3} - Anten {4} - Channel {5}\r\n", item.anten == "1" ? RF_Port1 : RF_Port2, FunctionSupport.Get_WifiStandard_By_Mode(item.wifi, item.bandwidth), item.rate, 20 * Math.Pow(2, double.Parse(item.bandwidth)), item.anten, _channelNo);
                int count = 0;
                REP:
                count++;
                if (!Verify_Signal(_fi, ModemTelnet, instrument, item.wifi, item.rate, item.bandwidth, _eqChannel, item.anten)) {
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
                _fi.LOGDATA += string.Format("Thời gian đo suy hao : {0} ms\r\n", st.ElapsedMilliseconds);
                _fi.LOGDATA += "\r\n";
                System.Threading.Thread.Sleep(1000);
            }
            return result;
        }

        
        private bool Verify_Attenuator(formattinfo _fi, ModemTelnet _mt, Instrument _it) {
            return AutoVerifySignal(_fi, _mt, _it);
        }


    }
}
