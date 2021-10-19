using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GW040Hv2Project.Function
{
    public class TestAnten
    {
        ModemTelnet _modem = null;
        Instrument _instrument = null;

        public TestAnten(ModemTelnet _mt, Instrument _it)
        {
            this._modem = _mt;
            this._instrument = _it;
        }

        public bool Excute()
        {
            bool _flag = true;

            //1. Verify Anten1 - 2G
            if (GlobalData.initSetting.ENTESTANTEN1 == true)
            {
                GlobalData.testingData.TESTANTEN1RESULT2G = InitParameters.Statuses.Wait;
                bool ret = Verify_Anten1_2G(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.testAnten1Result2g = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.TESTANTEN1RESULT2G = GlobalData.logManager.testAnten1Result2g;
                if (!ret)
                {
                    _flag = false;
                    goto Finished;
                }
            }
            //1. Verify Anten1 - 5G
            if (GlobalData.initSetting.ENTESTANTEN1 == true)
            {
                GlobalData.testingData.TESTANTEN1RESULT5G = InitParameters.Statuses.Wait;
                bool ret = Verify_Anten1_5G(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.testAnten1Result5g = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.TESTANTEN1RESULT5G = GlobalData.logManager.testAnten1Result5g;
                if (!ret)
                {
                    _flag = false;
                    goto Finished;
                }
            }

            //3. Verify Anten2 - 2G
            if (GlobalData.initSetting.ENTESTANTEN2 == true)
            {
                GlobalData.testingData.TESTANTEN2RESULT2G = InitParameters.Statuses.Wait;
                bool ret = Verify_Anten2_2G(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.testAnten2Result2g = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.TESTANTEN2RESULT2G = GlobalData.logManager.testAnten2Result2g;
                if (!ret)
                {
                    _flag = false;
                    goto Finished;
                }
            }
            //4. Verify Anten2 - 5G
            if (GlobalData.initSetting.ENTESTANTEN2 == true)
            {
                GlobalData.testingData.TESTANTEN2RESULT5G = InitParameters.Statuses.Wait;
                bool ret = Verify_Anten2_5G(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.testAnten2Result5g = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.TESTANTEN2RESULT5G = GlobalData.logManager.testAnten2Result5g;
                if (!ret)
                {
                    _flag = false;
                    goto Finished;
                }
            }

        //3. Hiển thị kết quả
        Finished:
            return _flag;
        }

        #region SubFunction

        string Name_measurement = GlobalData.initSetting.INSTRUMENT;
        string RF_Port1 = GlobalData.initSetting.RFPORT1;
        string RF_Port2 = GlobalData.initSetting.RFPORT2;

        private bool Verify_Signal(testinginfo _ti, ModemTelnet ModemTelnet, Instrument instrument, string standard_2G_5G, string Mode, string MCS, string BW, string Channel_Freq, string Anten, double Attenuator, ref string _pw, ref string _evm, ref string _freqerr, ref string _pstd, ref string _evmmax, out string error)
        {
            error = "";
            standard_2G_5G = int.Parse(Channel_Freq.Substring(0, 4)) > 3000 ? "5G" : "2G";
            string Result_Measure_temp = "";
            decimal Pwr_measure_temp, EVM_measure_temp, FreqErr_measure_temp;
            string _wifi = "";
            try
            {
                //------------------------------------------------------------------------------

                switch (Mode)
                {
                    case "0": { _wifi = "b"; break; }
                    case "1": { _wifi = "g"; break; }
                    case "3": { _wifi = string.Format("n{0}", BW == "0" ? "20" : "40"); break; }
                    case "4":
                        {
                            switch (BW)
                            {
                                case "0": { _wifi = "ac20"; break; }
                                case "1": { _wifi = "ac40"; break; }
                                case "2": { _wifi = "ac80"; break; }
                                case "3": { _wifi = "ac160"; break; }
                            }
                            break;
                        }
                }
                //Đọc giá trị tiêu chuẩn
                limittx _limit = null;
                LimitTx.getData(standard_2G_5G, FunctionSupport.Get_WifiStandard_By_Mode(Mode, BW), MCS, out _limit);
                _limit.power_MAX = "25";
                _limit.power_MIN = "25";

                foreach (var tmp in GlobalData.lstWifiTestingInfor.LstWifiTesting)
                {
                    if (tmp.Frequency == Channel_Freq.Substring(0, 4) && tmp.Antena == Anten)
                    {
                        _limit.power_MAX = tmp.UpperLimit;
                        _limit.power_MIN = tmp.LowerLimit;
                        Attenuator = double.Parse(tmp.Attenuator);
                        break;
                    }
                }

                //Thiết lập tần số máy đo
                string _error = "";
                instrument.config_Instrument_Channel(Channel_Freq, ref _error);

                //Gửi lệnh yêu cầu ONT phát WIFI TX
                string _message = "";
                ModemTelnet.Verify_Signal_SendCommand(standard_2G_5G, Mode, MCS, BW, Channel_Freq, Anten, ref _message);

                //Đọc kết quả từ máy đo
                Result_Measure_temp = instrument.config_Instrument_get_TotalResult("RFB", _wifi);

                //Lấy dữ liệu Power
                Pwr_measure_temp = Decimal.Parse(Result_Measure_temp.Split(',')[19], System.Globalization.NumberStyles.Float) + Convert.ToDecimal(Attenuator);
                if (Pwr_measure_temp < 15)
                {
                    instrument.config_Instrument_get_TotalResult("VID", _wifi);
                    Result_Measure_temp = instrument.config_Instrument_get_TotalResult("VID", _wifi);
                    Pwr_measure_temp = Decimal.Parse(Result_Measure_temp.Split(',')[19], System.Globalization.NumberStyles.Float) + Convert.ToDecimal(Attenuator);
                }

                //Lấy dữ liệu EVM
                EVM_measure_temp = Decimal.Parse(Result_Measure_temp.Split(',')[1], System.Globalization.NumberStyles.Float);

                //Lấy dữ liệu Frequency Error
                FreqErr_measure_temp = Decimal.Parse(Result_Measure_temp.Split(',')[7], System.Globalization.NumberStyles.Float);

                //Hiển thị kết quả đo lên giao diện phần mềm (RichTextBox)
                _pw = Pwr_measure_temp.ToString("0.##");
                _evm = EVM_measure_temp.ToString("0.##");
                _freqerr = FreqErr_measure_temp.ToString("0.##");
                _pstd = string.Format("{0}~{1}", _limit.power_MIN, _limit.power_MAX);
                _evmmax = string.Format("{0}", _limit.evm_MAX);
                _ti.LOGSYSTEM += $"Attenuator: {Attenuator} (db)\r\n";

                _ti.LOGSYSTEM += "Power Limit = " + _pstd + " dBm, Average Power = " + _pw + " dBm\r\n";
                _ti.LOGSYSTEM += string.Format("EVM MAX = {0} {2}, EVM All Carriers = {1} {2}\r\n", _evmmax, _evm, _wifi == "b" ? " %" : " dB");
                _ti.LOGSYSTEM += "Center Frequency Error = " + _freqerr + " Hz\r\n";

                //So sánh kết quả đo với giá trị tiêu chuẩn
                bool _result = false, _powerOK = false;//, _evmOK = false, _freqerrOK = true;
                _powerOK = FunctionSupport.Compare_TXMeasure_With_Standard(_limit.power_MAX, _limit.power_MIN, Pwr_measure_temp);

                if (_powerOK == false)
                    _ti.LOGSYSTEM += "FAIL: Power\r\n";
                error += string.Format($"[FAIL] Test Antena sau đóng vỏ power = {_pw} =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");

                _result = _powerOK;
                return _result;
            }
            catch
            {
                error += string.Format($"[FAIL]  Test Antena sau đóng vỏ =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");
                return false;
            }
        }

        private bool AutoVerifySignal(testinginfo _ti, ModemTelnet ModemTelnet, Instrument instrument, int _anten, string band)
        {
            List<verifysignal> list = null;
            _ti.LOGSYSTEM += "------------------------------------------------------------\r\n";
            _ti.LOGSYSTEM += string.Format("Bắt đầu thực hiện quá trình kiểm tra anten {0} band {1}.\r\n", _anten, band);

            list = _anten == 1 ? GlobalData.listTestAnten1 : GlobalData.listTestAnten2;

            if (band == "2G")
            {
                list = list.Where((x) => int.Parse(x.channelfreq.Trim().Substring(0, 4)) < 3000).ToList();
            }
            else
            {
                list = list.Where((x) => int.Parse(x.channelfreq.Trim().Substring(0, 4)) > 3000).ToList();
            }
            if (list.Count == 0) return false;
            bool result = true;
            string _oldwifi = "";
            foreach (var item in list)
            {

                Stopwatch st = new Stopwatch();
                st.Start();

                string _eqChannel = string.Format("{0}000000", item.channelfreq);
                double _attenuator = 0;// Sẽ tính lại trong hàm Verify_Signal //Attenuator.getAttenuator(item.channelfreq, item.anten);
                string _channelNo = Attenuator.getChannelNumber(item.channelfreq);
                string _CarrierFreq = band;// int.Parse(item.channelfreq.ToString().Trim().Substring(0, 4)) > 3000 ? "5G" : "2G";
                string _wifi = "";
                switch (item.wifi)
                {
                    case "0": { _wifi = "b"; break; }
                    case "1": { _wifi = "g"; break; }
                    case "3": { _wifi = string.Format("n{0}", item.bandwidth == "0" ? "20" : "40"); break; }
                    case "4":
                        {
                            switch (item.bandwidth)
                            {
                                case "0": { _wifi = "ac20"; break; }
                                case "1": { _wifi = "ac40"; break; }
                                case "2": { _wifi = "ac80"; break; }
                                case "3": { _wifi = "ac160"; break; }
                            }
                            break;
                        }
                }
                if (_oldwifi != _wifi)
                {
                    string _error = "";
                    instrument.config_Instrument_Total(item.anten == "1" ? RF_Port1 : RF_Port2, _wifi, ref _error);
                    _oldwifi = _wifi;
                }

                _ti.LOGSYSTEM += "*************************************************************************\r\n";
                _ti.LOGSYSTEM += string.Format("{0} - {1} - {2} - MCS{3} - BW{4} - Anten {5} - Channel {6}\r\n", _CarrierFreq, item.anten == "1" ? RF_Port1 : RF_Port2, FunctionSupport.Get_WifiStandard_By_Mode(item.wifi, item.bandwidth), item.rate, 20 * Math.Pow(2, double.Parse(item.bandwidth)), item.anten, _channelNo);
                int count = 0;
                string _Power = "", _Evm = "", _FreqErr = "", _pStd = "", _eMax = "";
            REP:
                count++; string logError = "";
                if (!Verify_Signal(_ti, ModemTelnet, instrument, _CarrierFreq, item.wifi, item.rate, item.bandwidth, _eqChannel, item.anten, _attenuator, ref _Power, ref _Evm, ref _FreqErr, ref _pStd, ref _eMax, out logError))
                {
                    if (count < 2)
                    {
                        _ti.LOGSYSTEM += string.Format("RETRY = {0}\r\n", count);
                        goto REP;
                    }
                    else
                    {
                        GlobalData.testingData.LOGERROR += logError;
                        _ti.LOGSYSTEM += string.Format("Phán định = {0}", "FAIL\r\n");
                        result = false;
                    }

                }
                else _ti.LOGSYSTEM += string.Format("Phán định = {0}\r\n", "PASS");

                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    string _w = "", _bw = "";
                    switch (item.wifi)
                    {
                        case "0": { _w = "802.11b"; break; }
                        case "1": { _w = "802.11g"; break; }
                        case "2": { _w = "802.11a"; break; }
                        case "3": { _w = "802.11n"; break; }
                        case "4": { _w = "802.11ac"; break; }
                    }
                    switch (item.bandwidth)
                    {
                        case "0": { _bw = "20"; break; }
                        case "1": { _bw = "40"; break; }
                        case "2": { _bw = "80"; break; }
                        case "3": { _bw = "160"; break; }
                    }
                    //GlobalData.datagridlogTX.Add(new logreviewtx() { rangeFreq = _CarrierFreq, Anten = item.anten, wifiStandard = _w, Rate = "MCS" + item.rate, Bandwidth = _bw, Channel = _channelNo, Result = result == true ? "PASS" : "FAIL", averagePower = _Power, centerFreqError = _FreqErr, Evm = _Evm, powerStd = _pStd, evmMAX = _eMax });
                    GlobalData.datagridlogTX.Add(new logreviewtx() { rangeFreq = _CarrierFreq, Anten = item.anten, wifiStandard = _w, Rate = "MCS" + item.rate, Bandwidth = _bw, Channel = _channelNo, Result = result == true ? "PASS" : "FAIL", averagePower = _Power, centerFreqError = _FreqErr, Evm = _Evm, powerStd = _pStd, evmMAX = "-----" });
                }));

                st.Stop();
                _ti.LOGSYSTEM += string.Format("Thời gian verify : {0} ms\r\n", st.ElapsedMilliseconds);
                _ti.LOGSYSTEM += "\r\n";
            }
            return result;
        }
        //OK
        private bool Verify_Anten1_2G(testinginfo _ti, ModemTelnet _mt, Instrument _it)
        {
            return AutoVerifySignal(_ti, _mt, _it, 1, "2G");
        }
        //OK
        private bool Verify_Anten1_5G(testinginfo _ti, ModemTelnet _mt, Instrument _it)
        {
            return AutoVerifySignal(_ti, _mt, _it, 1, "5G");
        }
        //OK
        private bool Verify_Anten2_2G(testinginfo _ti, ModemTelnet _mt, Instrument _it)
        {
            return AutoVerifySignal(_ti, _mt, _it, 2, "2G");
        }
        //OK
        private bool Verify_Anten2_5G(testinginfo _ti, ModemTelnet _mt, Instrument _it)
        {
            return AutoVerifySignal(_ti, _mt, _it, 2, "5G");
        }
        #endregion

    }
}
