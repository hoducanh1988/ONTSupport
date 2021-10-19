
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {
    public class Calibration {

        ModemTelnet _modem = null;
        Instrument _instrument = null;

        public Calibration(ModemTelnet _mt, Instrument _it) {
            this._modem = _mt;
            this._instrument = _it;
        }

        public bool Excute() {

            bool _flag = true;
            if (GlobalData.initSetting.ENCALIBPW2G || GlobalData.initSetting.ENCALIBPW5G) {
                GlobalData.logRegister = new logregister();
            }

            //1. Calib tần số
            if (GlobalData.initSetting.ENCALIBFREQ == true) {
                GlobalData.testingData.CALIBFREQRESULT = InitParameters.Statuses.Wait;
                int count = 0;
                string freError = "";
            REP:
                count++;
                bool ret = Calibrate_Freq(GlobalData.testingData, _modem, _instrument, count, out freError);
                if (ret == false && count < 3) goto REP;
                GlobalData.logManager.calibFreqResult = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.CALIBFREQRESULT = GlobalData.logManager.calibFreqResult;
                if (ret == false) {
                    string fre = freError != "" ? $"Frequency Error: {freError}" : "";
                    GlobalData.testingData.LOGERROR += string.Format($"[FAIL] Lỗi khi Calib tần số {fre} \n");
                    _flag = false;
                    //goto Finished;
                }
            }

            //2. Calib công suất 2G
            if (GlobalData.initSetting.ENCALIBPW2G == true) {
                GlobalData.testingData.CALIBPW2GRESULT = InitParameters.Statuses.Wait;
                bool ret = Calibrate_Pwr_2G_Total(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.calibPower2GResult = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.CALIBPW2GRESULT = GlobalData.logManager.calibPower2GResult;
                if (ret == false) {
                    _flag = false;
                    GlobalData.testingData.LOGERROR += string.Format($"[FAIL] Lỗi khi Calib công suất 2G \n");
                    //goto Finished;
                }
            }

            //3. Calib công suất 5G 
            if (GlobalData.initSetting.ENCALIBPW5G == true) {
                GlobalData.testingData.CALIBPW5GRESULT = InitParameters.Statuses.Wait;
                bool ret = Calibrate_Pwr_5G_Total(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.calibPower5GResult = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.CALIBPW5GRESULT = GlobalData.logManager.calibPower5GResult;
                if (ret == false) {
                    _flag = false;
                    GlobalData.testingData.LOGERROR += string.Format($"[FAIL] Lỗi khi Calib công suất 5G \n");

                    //goto Finished;
                }
            }

            //4. Lưu dữ liệu vào flash
            if (GlobalData.initSetting.ENCALIBFREQ == true || GlobalData.initSetting.ENCALIBPW2G == true || GlobalData.initSetting.ENCALIBPW5G == true) {
                bool ret = Save_Flash(GlobalData.testingData, _modem);
                if (!ret) {
                    _flag = false;
                    goto Finished;
                }
            }

            //Kiem tra gia tri thanh ghi sau khi write flash
            bool r = verifyRegister(GlobalData.testingData, _modem);
            if (r == false) {
                _flag = false;
                goto Finished;
            }

            //5. Test độ nhạy thu 2G
            if (GlobalData.initSetting.ENTESTRX2G == true) {
                GlobalData.testingData.TESTRX2GRESULT = InitParameters.Statuses.Wait;
                bool ret = Test_Sensitivity_2G(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.testSens2GResult = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.TESTRX2GRESULT = GlobalData.logManager.testSens2GResult;
                if (ret == false) {
                    _flag = false;
                    //goto Finished;
                }
            }

            //6. Test độ nhạy thu 5G
            if (GlobalData.initSetting.ENTESTRX5G == true) {
                GlobalData.testingData.TESTRX5GRESULT = InitParameters.Statuses.Wait;
                bool ret = Test_Sensitivity_5G(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.testSens5GResult = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.TESTRX5GRESULT = GlobalData.logManager.testSens5GResult;
                if (!ret) {
                    _flag = false;
                    //goto Finished;
                }
            }

            //7. Verify công suất phát 2G
            if (GlobalData.initSetting.ENTESTTX2G == true) {
                GlobalData.testingData.TESTTX2GRESULT = InitParameters.Statuses.Wait;
                bool ret = Verify_2G(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.verify2GResult = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.TESTTX2GRESULT = GlobalData.logManager.verify2GResult;
                if (!ret) {
                    _flag = false;
                    //goto Finished;
                }
            }

            //8. Verify công suất phát 5G
            if (GlobalData.initSetting.ENTESTTX5G == true) {
                GlobalData.testingData.TESTTX5GRESULT = InitParameters.Statuses.Wait;
                bool ret = Verify_5G(GlobalData.testingData, _modem, _instrument);
                GlobalData.logManager.verify5GResult = ret == true ? "PASS" : "FAIL";
                GlobalData.testingData.TESTTX5GRESULT = GlobalData.logManager.verify5GResult;
                if (!ret) {
                    _flag = false;
                    goto Finished;
                }
            }

        //9. Hiển thị kết quả
        Finished:
            return _flag;
        }

        #region SubFunction

        string Name_measurement = GlobalData.initSetting.INSTRUMENT;
        string RF_Port1 = GlobalData.initSetting.RFPORT1;
        string RF_Port2 = GlobalData.initSetting.RFPORT2;
        double Target_Pwr_2G = double.Parse(GlobalData.initSetting.TARGETPOWER2G);
        double Target_Pwr_5G = double.Parse(GlobalData.initSetting.TARGETPOWER5G);

        double Power_Measure;
        double Power_diferent;

        /// <summary>
        /// GHI DỮ LIỆU VÀO FLASH ONT GW040H
        /// </summary>
        /// <param name="_ti"></param>
        /// <param name="ModemTelnet"></param>
        /// <returns></returns>
        bool Save_Flash(testinginfo _ti, ModemTelnet ModemTelnet) {
            try {
                Stopwatch st = new Stopwatch();
                st.Start();
                //write defaut bin
                if (GlobalData.initSetting.ENWRITEBIN == true) {
                    GlobalData.testingData.SAVEBIN = InitParameters.Statuses.Wait;
                    try {
                        _ti.LOGSYSTEM += "------------------------------------------------------------\r\n";
                        _ti.LOGSYSTEM += "Thực hiện write giá trị file BIN.\r\n";
                        if (GlobalData.ListBinRegister.Count > 0) {
                            foreach (var item in GlobalData.ListBinRegister) {
                                ModemTelnet.Write_Register(item.Address, item.newValue);
                                GlobalData.testingData.LOGREGISTER += string.Format("{0}={1}\n", item.Address, item.newValue);
                                Thread.Sleep(100);
                                _ti.LOGSYSTEM += string.Format("Write Register: {0} = {1}\r\n", item.Address, item.newValue);
                            }
                        }
                        GlobalData.testingData.SAVEBIN = InitParameters.Statuses.Pass;
                    }
                    catch {
                        GlobalData.testingData.SAVEBIN = InitParameters.Statuses.Fail;
                    }
                }
                st.Stop();
                _ti.LOGSYSTEM += string.Format("Thời gian ghi file BIN : {0} ms\r\n", st.ElapsedMilliseconds);
                st.Restart();

                //save flash
                _ti.LOGSYSTEM += "------------------------------------------------------------\r\n";
                _ti.LOGSYSTEM += "Thực hiện lưu vào FLASH.\r\n";
                bool r = ModemTelnet.Write_into_Flash();
                st.Stop();
                _ti.LOGSYSTEM += string.Format("Thời gian lưu vào FLASH : {0} ms\r\n", st.ElapsedMilliseconds);
                return r;
            }
            catch {
                GlobalData.testingData.LOGERROR += string.Format($"[FAIL] Lỗi khi Save Flash \n");
                return false;
            }
        }


        public bool verifyRegister(testinginfo _ti, ModemTelnet modem_telnet) {
            bool r = false;
            bool ret = true;
            string[] buffer = GlobalData.testingData.LOGREGISTER.Split('\n');
            if (buffer == null || buffer.Length == 0) return true;

            _ti.LOGSYSTEM += "------------------------------------------------------------\r\n";
            _ti.LOGSYSTEM += "Thực hiện check lại giá trị thanh ghi sau khi save flash.\r\n";
            _ti.LOGSYSTEM += "Register".PadLeft(20) + "Before".PadLeft(20) + "After".PadLeft(20) + "Result".PadLeft(20) + "\n";

            foreach (var b in buffer) {
                if (string.IsNullOrEmpty(b) == false && string.IsNullOrWhiteSpace(b) == false) {
                    string std_register = b.Split('=')[0];
                    string std_value = b.Split('=')[1].Substring(2, 2);
                    string curr_value = modem_telnet.Read_Register(std_register).Substring(2, 2);
                    r = curr_value.ToLower().Replace("\n", "").Replace("\r", "").Equals(std_value.ToLower());

                    _ti.LOGSYSTEM += std_register.PadLeft(20) + std_value.PadLeft(20) + curr_value.PadLeft(20) + string.Format("{0}", r ? "Passed" : "Failed").PadLeft(20) + "\n";

                    if (r == false) ret = false;
                }
            }
            return ret;
        }

        /// <summary>
        /// CALIB TẦN SỐ *********************************************
        /// 1. Calibrate_Freq -------------//Calib tần số
        /// 2. Calculate_NewValue ---------//
        /// 
        /// </summary>
        #region CALIB FREQUENCY

        //OK
        private bool Calibrate_Freq(testinginfo _ti, ModemTelnet ModemTelnet, Instrument instrument, int retry, out string frequenError) {
            Stopwatch st = new Stopwatch();
            st.Start();
            _ti.LOGSYSTEM += "------------------------------------------------------------\r\n";
            string F4F6 = "";
            string F4 = "";
            string F4_6_0 = "";
            decimal F4_6_0_DEC;
            string F6 = "";
            string F6_5_0 = "";
            decimal F6_5_0_DEC;
            decimal FreOffset_new;
            string Current_Freq_Offset = "";
            string Freq_Err = "";
            string F7 = "";
            decimal F6_5_0_new_DEC;

            try {
                _ti.LOGSYSTEM += "Bắt đầu quá trình Calib tần số." + "\r\n";
                _ti.LOGSYSTEM += "Đang đọc thanh ghi F4,F6..." + "\r\n";
                ModemTelnet.WriteLine("iwpriv ra0 e2p F4");
                Thread.Sleep(50);
                ModemTelnet.WriteLine("iwpriv ra0 e2p F6");
                Thread.Sleep(50);
                F4F6 = ModemTelnet.Read();
                for (int i = 0; i < F4F6.Split('\n').Length; i++) {
                    if (F4F6.Split('\n')[i].Contains("[0x00F4]")) {
                        F4 = F4F6.Split('\n')[i].Split(':')[1].Trim();
                    }

                    if (F4F6.Split('\n')[i].Contains("[0x00F6]")) {
                        F6 = F4F6.Split('\n')[i].Split(':')[1].Trim();
                    }
                }

                _ti.LOGSYSTEM += "0x00F4 = " + F4 + "; " + "0x00F6 = " + F6 + "\r\n";
                F4_6_0 = FunctionSupport.HextoBin(F4.Substring(4, 2)).Substring(1, 7);
                //MessageBox.Show(F4_6_0);
                F4_6_0_DEC = Convert.ToInt64(F4_6_0, 2);
                F6_5_0 = FunctionSupport.HextoBin(F6.Substring(4, 2)).Substring(2, 6);
                F6_5_0_DEC = Convert.ToInt64(F6_5_0, 2);
                _ti.LOGSYSTEM += "F4[6:0] = " + F4_6_0 + "; " + "F6[5:0] = " + F6_5_0 + "\r\n";
                _ti.LOGSYSTEM += "F4[6:0]_DEC = " + F4_6_0_DEC + "; " + "F6[5:0]_DEC = " + F6_5_0_DEC + "\r\n";
                Current_Freq_Offset = FunctionSupport.Plus_F4_With_F6(F4, F6); //Tính giá trị FreqOffset ban đầu
                _ti.LOGSYSTEM += "Giá trị Frequency Offset hiện tại = F4[6,0] +/- F6[5,0] = " + Current_Freq_Offset + "\r\n";

                //Gửi lệnh phát tín hiệu kèm Frequency Offset
                _ti.LOGSYSTEM += "Gửi lệnh phát tín hiệu kèm Current_Freq_Offset: " + Current_Freq_Offset + "\r\n";
                _ti.LOGSYSTEM += string.Format("Đang cấu hình lần đầu cho máy đo {0}...", Name_measurement) + "\r\n";

                bool _configInstrIsOk = false;
                int _index = 0;
                while (_configInstrIsOk == false) {
                    string _error = "";
                    _configInstrIsOk = instrument.config_Instrument_Total(RF_Port1, "g", ref _error);
                    if (_error != "") _ti.LOGSYSTEM += string.Format("{0}\r\n", _error);
                    _error = "";
                    _configInstrIsOk = instrument.config_Instrument_Channel("2437000000", ref _error);
                    if (_error != "") _ti.LOGSYSTEM += string.Format("{0}\r\n", _error);
                    _index++;
                    if (_index > 20) break;
                }
                if (_configInstrIsOk == false) {
                    GlobalData.testingData.LOGERROR += string.Format("[FAIL] Lỗi cấu hình máy đo khi calib tần số \n");
                    frequenError = Freq_Err;
                    return false;
                }

                _ti.LOGSYSTEM += string.Format("Đang phát tín hiệu ở Anten 1 - Channel 6 - Máy đo {0} - Offset = {1}", Name_measurement, Current_Freq_Offset) + "\r\n";
                ModemTelnet.CalibFrequency_SendCommand("1", "7", "0", "6", "1", Current_Freq_Offset); //(mode,rate,bw,channel,anten,freqOffset)
                                                                                                      //Thread.Sleep(500);
                                                                                                      //for (int i = 0; i < 2; i++) {
                Freq_Err = instrument.config_Instrument_get_FreqErr("RFB", "g"); //Lệnh đọc giá trị về từ máy đo
                                                                                 //MessageBox.Show(Freq_Err);
                _ti.LOGSYSTEM += "..." + "\r\n";
                _ti.LOGSYSTEM += "Lấy kết quả đo lần thứ: " + retry.ToString() + "\r\n";
                if (!Freq_Err.Contains("999")) {
                    if (Convert.ToDouble(Freq_Err) > -2000 && Convert.ToDouble(Freq_Err) < 2000) {
                        _ti.LOGSYSTEM += "Frequency Err = " + Freq_Err + "\r\n";
                        _ti.LOGSYSTEM += "Frequency Error đã đạt Target." + "\r\n";
                        _ti.LOGSYSTEM += "---------------------------------" + "\r\n";
                        FreOffset_new = Decimal.Parse(Current_Freq_Offset);
                        //Result_FreqErr_Calib = "PASS";
                        //break;
                    }
                    else {
                        _ti.LOGSYSTEM += "Giá trị Frequency Error = " + Freq_Err + "\r\n";
                        if (Convert.ToDouble(Freq_Err) < 0)
                            _ti.LOGSYSTEM += "Frequency Error < 0 -> Cần giảm Frequency Offset" + "\r\n";
                        else
                            _ti.LOGSYSTEM += "Frequency Error > 0 -> Cần tăng Frequency Offset" + "\r\n";

                        _ti.LOGSYSTEM += "Mỗi 1500 Khz bị lệch tương ứng với 1 giá trị Decimal => Giá trị DEC mà Current_Freq_Offset và F6[5,0] cần thay đổi = Freq_Err / 1500 = " + Math.Round((Decimal.Parse(Freq_Err)) / 1500) + "\r\n"; //2350                  
                        FreOffset_new = Math.Round(Decimal.Parse(Current_Freq_Offset) + Math.Round(Decimal.Parse(Freq_Err) / 1500)); //2350
                        _ti.LOGSYSTEM += "Freq_Offset_new = Current_Freq_Offset + Freq_Err/1500 = " + FreOffset_new + "\r\n";
                        string F6_DEC = FunctionSupport.HextoDec(F6.Substring(4, 2));
                        string F6_toBin = "";
                        if (Int32.Parse(F6_DEC) == 0) {
                            F6_toBin = "00000000";
                        }
                        else {
                            F6_toBin = FunctionSupport.DECtoBin(Int32.Parse(F6_DEC));
                        }
                        string F6_5_0_old = F6_toBin.Substring(2);
                        Decimal F6_5_0_old_toDEC = Convert.ToInt32(F6_5_0_old, 2);
                        _ti.LOGSYSTEM += "F6[5,0] cũ ở dạng DEC = " + F6_5_0_old_toDEC + "\r\n";

                        //F6_5_0_new_DEC = FreOffset_new - F4_6_0_DEC;
                        if (FreOffset_new > F4_6_0_DEC) {
                            F6_5_0_new_DEC = FreOffset_new - F4_6_0_DEC;
                        }
                        else {
                            F6_5_0_new_DEC = F4_6_0_DEC - FreOffset_new;
                        }

                        //F6_5_0_new_DEC = Math.Round(F6_5_0_old_toDEC - (Decimal.Parse(Freq_Err) / 1500)); //2350
                        _ti.LOGSYSTEM += "F6[5,0] mới ở dạng DEC = " + F6_5_0_new_DEC + "\r\n";
                        string F6_full_new_BIN = "";
                        F7 = F6.Substring(2, 2);

                        //30/06/2020
                        if (FreOffset_new < F4_6_0_DEC) {
                            F6_full_new_BIN = FunctionSupport.KiemtraF6("Can Giam", Int32.Parse(F6_5_0_new_DEC.ToString()));
                            _ti.LOGSYSTEM += "F6_Full_Bin_New = " + F6_full_new_BIN + "\r\n";
                            _ti.LOGSYSTEM += "Giá trị F6 mới cần truyền: " + FunctionSupport.BintoHexOnly(F6_full_new_BIN) + "\r\n"; ;

                            _ti.LOGSYSTEM += "iwpriv ra0 e2p F6=" + F7 + FunctionSupport.BintoHexOnly(F6_full_new_BIN) + "\r\n";
                            ModemTelnet.WriteLine("iwpriv ra0 e2p F6=" + F7 + FunctionSupport.BintoHexOnly(F6_full_new_BIN));
                        }
                        else if (FreOffset_new > F4_6_0_DEC) {
                            F6_full_new_BIN = FunctionSupport.KiemtraF6("Can Tang", Int32.Parse(F6_5_0_new_DEC.ToString()));
                            _ti.LOGSYSTEM += "F6_Full_Bin_New = " + F6_full_new_BIN + "\r\n";
                            _ti.LOGSYSTEM += "Giá trị F6 mới cần truyền: " + FunctionSupport.BintoHexOnly(F6_full_new_BIN) + "\r\n";

                            _ti.LOGSYSTEM += "iwpriv ra0 e2p F6=" + F7 + FunctionSupport.BintoHexOnly(F6_full_new_BIN) + "\r\n";
                            ModemTelnet.WriteLine("iwpriv ra0 e2p F6=" + F7 + FunctionSupport.BintoHexOnly(F6_full_new_BIN));
                        }
                        else {
                            ModemTelnet.WriteLine("iwpriv ra0 e2p F6=" + F6);
                        }


                        _ti.LOGSYSTEM += "-------------------------------------------" + "\r\n";
                        //_ti.LOGSYSTEM += "Thực hiện Write to Flash.");
                        //Save_Flash();

                        _ti.LOGSYSTEM += "-------------------------------------------" + "\r\n";
                        _ti.LOGSYSTEM += "Bắt đầu thực hiện Verify Frequency Error." + "\r\n";
                        _ti.LOGSYSTEM += string.Format("Đang phát tín hiệu ở Anten 1 - Channel 6") + "\r\n";
                        ModemTelnet.CalibFrequency_SendCommand("1", "7", "0", "6", "1", FreOffset_new.ToString()); //(mode,rate,bw,channel,anten,freqOffset)
                        Freq_Err = instrument.config_Instrument_get_FreqErr("RFB", "g"); //Lệnh đọc giá trị về từ máy đo
                        _ti.LOGSYSTEM += "Frequency Error = " + Freq_Err + "\r\n";
                        //break;
                    }
                }
                else {
                    //continue;
                }
                //}
                st.Stop();
                _ti.LOGSYSTEM += string.Format("Thời gian calib Freq : {0} ms\r\n", st.ElapsedMilliseconds);

                frequenError = Freq_Err;

                return Math.Abs(Convert.ToDouble(Freq_Err)) < 2000;
            }
            catch {
                st.Stop();
                _ti.LOGSYSTEM += string.Format("Thời gian calib Freq : {0} ms\r\n", st.ElapsedMilliseconds);
                frequenError = Freq_Err;
                return false;
                //MessageBox.Show(Ex.ToString());
                //return "STOP";
            }
            //}
        }

        public bool Calculate_NewValue(double power_difference, string current_register_value, out string new_register_value) {
            new_register_value = "ERROR";
            try {
                string bk_curent_register = current_register_value;

                //check input data
                bool r = Regex.IsMatch(current_register_value, "^[0-9,A-F]{4}$");
                if (!r) return false;

                //convert current register hex value to binary value
                //Hex: 81 => Bin: 1000 0001
                current_register_value = current_register_value.Substring(2, 2);
                string bin_value = convert_Hex_To_Binary(current_register_value);
                if (bin_value == null) return false;

                //get dec value from binary value
                //Bin: 1000 1001 =>  [10][001001] => Dec: 9
                string sign_value = bin_value.Substring(1, 1);
                string dec_value = convert_Binary_To_Dec(bin_value.Substring(2, 6));
                if (dec_value == null) return false;

                //get new dec value
                //sign_value: 1 = plus, 0 = minus
                double new_dec_value = sign_value == "1" ? (double.Parse(dec_value) / 2) : (double.Parse(dec_value) / (-2));
                new_dec_value = new_dec_value - power_difference;
                if (Math.Abs(new_dec_value * 2) > 63) return false; //gia tri vuot nguong dieu chinh thanh ghi

                //convert new dec value to hex
                new_dec_value = new_dec_value * 2;
                string bin_new = convert_Dec_To_Binary((int)Math.Abs(new_dec_value));
                string sign_new = new_dec_value > 0 ? "11" : "10";
                bin_new = sign_new + bin_new;

                //convert new bin to new hex
                new_register_value = bk_curent_register.Substring(0, 2) + convert_Binary_To_Hex(bin_new);

                return true;
            }
            catch { return false; }
        }

        private int? convert_Hex_To_Dec(string hex_value) {
            try {
                int? decValue = int.Parse(hex_value, System.Globalization.NumberStyles.HexNumber);
                return decValue;
            }
            catch { return null; }
        }

        private string convert_Hex_To_Binary(string hex_value) {
            try {
                string bin_value = Convert.ToString(Convert.ToInt64(hex_value, 16), 2);
                return bin_value;
            }
            catch { return null; }
        }

        private string convert_Binary_To_Hex(string bin_value) {
            try {
                string strHex = Convert.ToInt32(bin_value, 2).ToString("X");
                return strHex;
            }
            catch { return null; }
        }

        private string convert_Binary_To_Dec(string bin_value) {
            try {
                string int_value = Convert.ToInt32(bin_value, 2).ToString();
                return int_value;
            }
            catch { return null; }
        }

        private string convert_Dec_To_Binary(int dec_value) {
            try {
                string binary = Convert.ToString(dec_value, 2);
                return binary.PadLeft(6, '0');
            }
            catch { return null; }
        }
        #endregion

        /// <summary>
        /// CALIB CÔNG SUẤT *******************************************
        /// 1. Calibrate_Pwr_Detail --------//Core
        /// 2. AutoCalibPower --------------//Hỗ trợ tự động test nhiều
        /// 3. Calibrate_Pwr_2G_Total ------//
        /// 4. Calibrate_Pwr_5G_Total ------//
        /// 
        /// </summary>
        #region CALIB POWER

        private bool Calibrate_Pwr_Detail(testinginfo _ti, ModemTelnet ModemTelnet, Instrument instrument, string Standard_2G_or_5G, string RFinput, string Anten, string Channel_Freq, string Register, double Attenuator, out string error) {
            string Register_Old_Value_Pwr = "", Register_New_Value_Pwr = ""; error = "";
            bool _flag = true;
            try {
                Register_Old_Value_Pwr = ModemTelnet.Read_Register(Register.Split('x')[1]);
                if (Register_Old_Value_Pwr.Contains(Register.Split('x')[1])) {
                    Register_Old_Value_Pwr = ModemTelnet.Read_Register(Register.Split('x')[1]);
                    return false;
                }
                else {
                    _ti.LOGSYSTEM += "Giá trị thanh ghi " + Register + " hiện tại: " + Register_Old_Value_Pwr + "\r\n";
                    ModemTelnet.CalibPower_SendCommand(Standard_2G_or_5G, Anten, Channel_Freq);

                    string _error = "";
                    instrument.config_Instrument_Channel(Channel_Freq, ref _error);
                    ModemTelnet.Read_Register(Register.Split('x')[1]);

                    int count = 0;
                    List<double> list_power = new List<double>();
                RETRY:
                    count++;
                    for (int i= 0; i < GlobalData.initSetting.CALIBTOTAL; i++) {
                        int c = 0;
                    RE:
                        c++;
                        Power_Measure = Convert.ToDouble(instrument.config_Instrument_get_Power(count == 0 ? (Standard_2G_or_5G == "2G" ? "RFB" : "VID") : (Standard_2G_or_5G == "2G" ? "VID" : "RFB"), "g")) + Attenuator;
                        if (Convert.ToDouble(Power_Measure) < 15) { if (c < 2) goto RE; }
                        list_power.Add(Power_Measure);
                        _ti.LOGSYSTEM += "Công suất đo được: " + Power_Measure + " (dBm)" + "\r\n";
                    }

                    List<double> list_out = null;
                    baseFunction.rejectDifferenceValueFromList(list_power, out list_out);

                    Power_Measure = FunctionSupport.RoundDecimal(list_out.Average());

                    if (Standard_2G_or_5G == "2G") Power_diferent = Power_Measure - Convert.ToDouble(Target_Pwr_2G);
                    else if (Standard_2G_or_5G == "5G") Power_diferent = Power_Measure - Convert.ToDouble(Target_Pwr_5G);
                    _ti.LOGSYSTEM += "Độ lệch công suất: " + Power_diferent + " (dBm)" + "\r\n";

                    if (Power_diferent == 0) goto END;
                    Calculate_NewValue(Power_diferent, Register_Old_Value_Pwr, out Register_New_Value_Pwr);

                    if (Register_New_Value_Pwr.Contains("ERROR")) {
                        _ti.LOGSYSTEM += "[FAIL] Bắt đầu thực hiện lại.\r\n";
                        _flag = false;
                        if (count < 3) goto RETRY;
                    }
                    else {
                        _ti.LOGSYSTEM += "Giá trị cần truyền: " + Register_New_Value_Pwr + "\r\n";
                        ModemTelnet.Write_Register(Register.Split('x')[1], Register_New_Value_Pwr);
                        GlobalData.testingData.LOGREGISTER += string.Format("{0}={1}\n", Register.Split('x')[1], Register_New_Value_Pwr);
                        _flag = true;
                    }
                }

            END:
                var propInfo = GlobalData.logRegister.GetType().GetProperty(string.Format("_{0}", Register));
                if (propInfo != null) {
                    propInfo.SetValue(GlobalData.logRegister, Register_New_Value_Pwr == "" ? Register_Old_Value_Pwr.Substring(2, 2) : Register_New_Value_Pwr.Substring(2, 2), null);
                }
                if (!_flag) { error = $"Lỗi khi calib TX band: {Standard_2G_or_5G} tần số: {Channel_Freq } thanh ghi: {Register} => độ lệch công suất: {Power_diferent}\n"; }
                return _flag;
            }
            catch {
                return false;
            }
        }

        private bool AutoCalibPower(testinginfo _ti, ModemTelnet ModemTelnet, Instrument instrument, string _CarrierFreq) {
            try {
                List<calibpower> list = null;
                _ti.LOGSYSTEM += "------------------------------------------------------------\r\n";
                _ti.LOGSYSTEM += string.Format("Bắt đầu thực hiện quá trình Calib Công Suất {0}.\r\n", _CarrierFreq);
                list = _CarrierFreq == "2G" ? GlobalData.listCalibPower2G : GlobalData.listCalibPower5G;
                string _error = "";
                bool _result = true;

                instrument.config_Instrument_Total(RF_Port1, "g", ref _error);
                Thread.Sleep(1000);
                string _antenna = "0";

                if (list.Count == 0) return true;
                foreach (var item in list) {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    string _eqChannel = string.Format("{0}000000", item.channelfreq);
                    double _attenuator = Attenuator.getAttenuator(item.channelfreq, item.anten);
                    string _channelNo = Attenuator.getChannelNumber(item.channelfreq);

                    //Switch RFIO Port
                    if ((item.anten != _antenna) && (RF_Port1 != RF_Port2)) {
                        _ti.LOGSYSTEM += string.Format("Switch From {0} to {1}\r\n", item.anten == "1" ? RF_Port2 : RF_Port1, item.anten == "1" ? RF_Port1 : RF_Port2);
                        int c = 0;
                    RE:
                        c++;
                        bool ret = instrument.config_Instrument_Port(item.anten == "1" ? RF_Port1 : RF_Port2, ref _error);
                        if (ret == false) if (c < 5) goto RE;
                        _antenna = item.anten;
                    }

                    //Calib Power
                    _ti.LOGSYSTEM += "*************************************************************************\r\n";
                    _ti.LOGSYSTEM += string.Format("{0} - {1} - 802.11g - MCS7 - BW20 - Anten {2} - Channel {3} - {4}  \r\n", _CarrierFreq, item.anten == "1" ? RF_Port1 : RF_Port2, item.anten, _channelNo, item.register);
                    _ti.LOGSYSTEM += string.Format("Attenuator: {0}(db)\n", _attenuator);
                    //check thanh ghi co can calib ko?
                    if (item.calibflag.Trim() == "1") { //Phai calib
                        int count = 0;
                    REP:
                        count++; string tmp1 = "";
                        if (!Calibrate_Pwr_Detail(_ti, ModemTelnet, instrument, _CarrierFreq, item.anten == "1" ? RF_Port1 : RF_Port2, item.anten, _eqChannel, item.register, _attenuator, out tmp1)) {
                            if (count < 3) goto REP;
                            GlobalData.testingData.LOGERROR += tmp1;
                            _result = false;
                        }
                    }
                    else { //khong can calib
                        string New_Value = "";
                        var propInfo = GlobalData.logRegister.GetType().GetProperty(string.Format("_{0}", item.refference));
                        if (propInfo != null) {
                            New_Value = propInfo.GetValue(GlobalData.logRegister).ToString();
                        }
                        _ti.LOGSYSTEM += string.Format("Thanh ghi reference {0}, Giá trị {1} \r\n", item.refference, New_Value);

                        //Ghi gia tri thanh ghi
                        _ti.LOGSYSTEM += "Giá trị cần truyền: " + New_Value + "\r\n";
                        ModemTelnet.Write_Register(item.register.Split('x')[1], New_Value);
                        GlobalData.testingData.LOGREGISTER += string.Format("{0}={1}\n", item.register.Split('x')[1], New_Value);
                    }

                    st.Stop();
                    _ti.LOGSYSTEM += string.Format("Thời gian calib : {0} ms\r\n", st.ElapsedMilliseconds);
                    _ti.LOGSYSTEM += "\r\n";
                }
                return _result;
            }
            catch {
                return false;
            }
        }

        private bool Calibrate_Pwr_2G_Total(testinginfo _ti, ModemTelnet _mt, Instrument _it) {

            return AutoCalibPower(_ti, _mt, _it, "2G");
        }

        private bool Calibrate_Pwr_5G_Total(testinginfo _ti, ModemTelnet _mt, Instrument _it) {

            return AutoCalibPower(_ti, _mt, _it, "5G");
        }

        #endregion

        /// <summary>
        /// XÁC NHẬN WIFI-TX *******************************************
        /// 1. Verify_Signal ----------------//Core
        /// 2. AutoVerifySignal -------------//Hỗ trợ tự động test nhiều
        /// 3. Verify_2G --------------------//
        /// 4. Verify_5G --------------------//
        /// </summary>
        #region VERIFY TX

        private bool Verify_Signal(testinginfo _ti, ModemTelnet ModemTelnet, Instrument instrument, string standard_2G_5G, string Mode, string MCS, string BW, string Channel_Freq, string Anten, double Attenuator, ref string _pw, ref string _evm, ref string _freqerr, ref string _pstd, ref string _evmmax, out string error) {
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
                limittx _limit = null;
                LimitTx.getData(standard_2G_5G, FunctionSupport.Get_WifiStandard_By_Mode(Mode, BW), MCS, out _limit);

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

                //Hiển thị kết quả đo lên giao diện phần mềm (RichTextBox)
                _pw = Pwr_measure_temp.ToString("0.##");
                _evm = EVM_measure_temp.ToString("0.##");
                _freqerr = FreqErr_measure_temp.ToString("0.##");
                _pstd = string.Format("{0}~{1}", _limit.power_MIN, _limit.power_MAX);
                _evmmax = string.Format("{0}", _limit.evm_MAX);

                _ti.LOGSYSTEM += "Average Power = " + _pw + " dBm ... ";
                _ti.LOGSYSTEM += string.Format("EVM All Carriers = {0} {1} ... ", _evm, _wifi == "b" ? " %" : " dB");
                _ti.LOGSYSTEM += "Center Frequency Error = " + _freqerr + " Hz\r\n";

                if (count < 1) goto RE;

                Pwr_measure_temp = list_Power.Average();
                EVM_measure_temp = list_Evm.Average();
                FreqErr_measure_temp = list_FreqErr.Average();

                //_ti.LOGSYSTEM += "...\r\n";
                //_ti.LOGSYSTEM += "Average Power = " + Math.Round(Pwr_measure_temp, 2) + " dBm - ";
                //_ti.LOGSYSTEM += string.Format("EVM All Carriers = {0} {1} - ", Math.Round(EVM_measure_temp, 2), _wifi == "b" ? " %" : " dB");
                //_ti.LOGSYSTEM += "Center Frequency Error = " + Math.Round(FreqErr_measure_temp, 2) + " Hz\r\n";

                //So sánh kết quả đo với giá trị tiêu chuẩn
                bool _result = false, _powerOK = false, _evmOK = false, _freqerrOK = true;
                _powerOK = FunctionSupport.Compare_TXMeasure_With_Standard(_limit.power_MAX, _limit.power_MIN, Pwr_measure_temp);
                _evmOK = FunctionSupport.Compare_TXMeasure_With_Standard(_limit.evm_MAX, _limit.evm_MIN, EVM_measure_temp);
                _freqerrOK = FunctionSupport.Compare_TXMeasure_With_Standard(_limit.freqError_MAX, _limit.freqError_MIN, FreqErr_measure_temp);

                if (_powerOK == false) {
                    _ti.LOGSYSTEM += "FAIL: Power\r\n";
                    error += string.Format($"[FAIL] Verify tín hiệu TX Power = {_pw} =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");
                }
                else if (_evmOK == false) {
                    error += string.Format($"[FAIL] Verify tín hiệu TX EVM = {_evm} =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");
                    _ti.LOGSYSTEM += "FAIL: EVM\r\n";
                }
                else if (_freqerrOK == false) {
                    _ti.LOGSYSTEM += "FAIL: Frequency Error\r\n";
                    error += string.Format($"[FAIL] Verify tín hiệu TX Frequency Error = {_freqerr} =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");
                }
                _result = _powerOK && _evmOK && _freqerrOK;

                return _result;
            }
            catch {
                error += string.Format($"[FAIL] Verify tín hiệu TX  =>  Antena: {Anten} band: {standard_2G_5G} chuẩn: {_wifi} MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");

                return false;
            }
        }

        private bool AutoVerifySignal(testinginfo _ti, ModemTelnet ModemTelnet, Instrument instrument, string _CarrierFreq) {
            List<verifysignal> list = null;
            _ti.LOGSYSTEM += "------------------------------------------------------------\r\n";
            _ti.LOGSYSTEM += string.Format("Bắt đầu thực hiện quá trình Verify tín hiệu {0}.\r\n", _CarrierFreq);
            list = _CarrierFreq == "2G" ? GlobalData.listVerifySignal2G : GlobalData.listVerifySignal5G;

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
                    _ti.LOGSYSTEM += string.Format("Switch From {0} to {1}\r\n", item.anten == "1" ? RF_Port2 : RF_Port1, item.anten == "1" ? RF_Port1 : RF_Port2);
                    int c = 0;
                RE:
                    c++;
                    bool ret = instrument.config_Instrument_Port(item.anten == "1" ? RF_Port1 : RF_Port2, ref _error);
                    if (ret == false) if (c < 5) goto RE;
                    _antenna = item.anten;
                }

                //Verify Signal
                _ti.LOGSYSTEM += "*************************************************************************\r\n";
                _ti.LOGSYSTEM += string.Format("{0} - {1} - {2} - MCS{3} - BW{4} - Anten {5} - Channel {6}\r\n", _CarrierFreq, item.anten == "1" ? RF_Port1 : RF_Port2, FunctionSupport.Get_WifiStandard_By_Mode(item.wifi, item.bandwidth), item.rate, 20 * Math.Pow(2, double.Parse(item.bandwidth)), item.anten, _channelNo);
                _ti.LOGSYSTEM += string.Format("Attenuator: {0}(dbm)\n", _attenuator);

                int count = 0;
                string _Power = "", _Evm = "", _FreqErr = "", _pStd = "", _eMax = "";
                bool _kq = true;
            REP:
                count++;
                string errorlog = "";
                if (!Verify_Signal(_ti, ModemTelnet, instrument, _CarrierFreq, item.wifi, item.rate, item.bandwidth, _eqChannel, item.anten, _attenuator, ref _Power, ref _Evm, ref _FreqErr, ref _pStd, ref _eMax, out errorlog)) {
                    if (count < 2) {
                        _ti.LOGSYSTEM += string.Format("RETRY = {0}\r\n", count);
                        _kq = false;
                        goto REP;
                    }
                    else {
                        _ti.LOGSYSTEM += string.Format("Phán định = {0}", "FAIL\r\n");
                        GlobalData.testingData.LOGERROR += errorlog;
                        _kq = false;
                        result = false;
                    }

                }
                else {
                    _ti.LOGSYSTEM += string.Format("Phán định = {0}\r\n", "PASS");
                    _kq = true;
                }
                App.Current.Dispatcher.Invoke(new Action(() => {
                    string _w = "", _bw = "";
                    switch (item.wifi) {
                        case "0": { _w = "802.11b"; break; }
                        case "1": { _w = "802.11g"; break; }
                        case "2": { _w = "802.11a"; break; }
                        case "3": { _w = "802.11n"; break; }
                        case "4": { _w = "802.11ac"; break; }
                    }
                    switch (item.bandwidth) {
                        case "0": { _bw = "20"; break; }
                        case "1": { _bw = "40"; break; }
                        case "2": { _bw = "80"; break; }
                        case "3": { _bw = "160"; break; }

                    }
                    GlobalData.datagridlogTX.Add(new logreviewtx() { rangeFreq = _CarrierFreq, Anten = item.anten, wifiStandard = _w, Rate = "MCS" + item.rate, Bandwidth = _bw, Channel = _channelNo, Result = _kq == true ? "PASS" : "FAIL", averagePower = _Power, centerFreqError = _FreqErr, Evm = _Evm, powerStd = _pStd, evmMAX = _eMax });
                }));
                st.Stop();
                _ti.LOGSYSTEM += string.Format("Thời gian verify : {0} ms\r\n", st.ElapsedMilliseconds);
                _ti.LOGSYSTEM += "\r\n";
            }
            return result;
        }

        private bool Verify_2G(testinginfo _ti, ModemTelnet _mt, Instrument _it) {
            return AutoVerifySignal(_ti, _mt, _it, "2G");
        }

        private bool Verify_5G(testinginfo _ti, ModemTelnet _mt, Instrument _it) {
            return AutoVerifySignal(_ti, _mt, _it, "5G");
        }

        #endregion

        /// <summary>
        /// XÁC NHẬN WIFI-RX *******************************************
        /// 1. Test_Sensivitity_Detail -------//Core
        /// 2. AutoTestSensivitity -----------//Hỗ trợ tự động test nhiều
        /// 3. Test_Sensitivity_2G -----------//
        /// 4. Test_Sensitivity_5G -----------//
        /// 
        /// </summary>
        #region VERIFY RX

        private bool Test_Sensivitity_Detail(testinginfo _ti, ModemTelnet ModemTelnet, Instrument instrument, string standard_2G_5G, string Mode, string MCS, string BW, string Channel_Freq, string Anten, int packet, out string pw_tran, out string received, out string per, out string error) {
            pw_tran = received = per = ""; error = "";
            try {

                string wave_form_name = "";
                string _channelNo = Attenuator.getChannelNumber(Channel_Freq);
                double _attenuator = Attenuator.getAttenuator(Channel_Freq, Anten);
                double power_transmit = -1000;
                double stPER = 0.0;
                double PER = 0.0;
                int RXCounter = 0;

                //Đọc giá trị PER, POWER TRANSMIT
                limitrx _limit = null;
                LimitRx.getData(standard_2G_5G, FunctionSupport.Get_WifiStandard_By_Mode(Mode, BW), MCS, out _limit);
                power_transmit = _limit.power_Transmit.Trim() == "-" ? -1000 : double.Parse(_limit.power_Transmit);
                pw_tran = power_transmit.ToString();
                stPER = _limit.PER.Trim() == "-" ? 0 : double.Parse(_limit.PER.Trim().Replace("%", ""));

                //Lấy tên file wave form
                WaveForm.getData(Name_measurement, Mode, MCS, BW, out wave_form_name);//001122334455_H2M7.wfm

                //Cấu hình ONT về chế độ WIFI RX
                _ti.LOGSYSTEM += "Cấu hình ONT...\r\n";
                string _message = "";
                ModemTelnet.TestSensitivity_SendCommand(standard_2G_5G, Mode, MCS, BW, _channelNo, Anten, ref _message);
                //Hien_Thi.Hienthi.SetText(rtbAll, _message);

                //Điều khiển máy đo phát gói tin
                _ti.LOGSYSTEM += string.Format("Cấu hình máy đo phát tín hiệu: Power={0} dBm, waveform={1}\r\n", power_transmit, wave_form_name);
                _ti.LOGSYSTEM += string.Format("Attenuator: {0}(db)\n", _attenuator);

                instrument.config_HT20_RxTest_MAC(Channel_Freq, (power_transmit + _attenuator).ToString(), packet.ToString(), wave_form_name, Anten == "1" ? RF_Port1 : RF_Port2);
                Thread.Sleep(1000);
                //Đọc số gói tin nhận được từ ONT
                if (standard_2G_5G == "2G") Thread.Sleep(2000);
                RXCounter = int.Parse(ModemTelnet.TestSensitivity_ReadPER_SendCommand(standard_2G_5G, ref _message));
                received = RXCounter.ToString();
                //Hien_Thi.Hienthi.SetText(rtbAll, _message);

                //Tính PER và hiển thị
                PER = Math.Round(((packet - RXCounter) * 100.0) / packet, 2);
                per = PER.ToString();
                _ti.LOGSYSTEM += string.Format("PER = {0}%, Sent={1}, Received={2}\r\n", PER, packet, RXCounter);

                //So sánh PER với tiêu chuẩn
                bool _result = false;
                _result = PER <= stPER;
                if (!_result) {
                    error += string.Format($"[FAIL]=> Test độ nhạy thu RX: PER= {per}  =>  Antena: {Anten} band: {standard_2G_5G}  MCS: {MCS} BW: {BW} tần số: {Channel_Freq}\n");
                }

                return _result;
            }
            catch {
                error += string.Format($"[FAIL]=> Lỗi Test độ nhạy thu RX  Antena: {Anten} band: {standard_2G_5G}  MCS: {MCS} BW: {BW} tần số: {Channel_Freq} \n");
                return false;
            }
        }
        private bool AutoTestSensivitity(testinginfo _ti, ModemTelnet ModemTelnet, Instrument instrument, string _CarrierFreq) {
            List<sensivitity> list = null;
            _ti.LOGSYSTEM += "------------------------------------------------------------\r\n";
            _ti.LOGSYSTEM += string.Format("Bắt đầu thực hiện Test Sensitivity {0}.\r\n", _CarrierFreq);
            list = _CarrierFreq == "2G" ? GlobalData.listSensivitity2G : GlobalData.listSensivitity5G;

            if (list.Count == 0) return false;
            bool result = true;
            string _error = "";
            string _antenna = "0";

            foreach (var item in list) {
                Stopwatch st = new Stopwatch();
                st.Start();

                string _channelNo = Attenuator.getChannelNumber(item.channelfreq);

                //Switch RFIO Port
                if ((item.anten != _antenna) && (RF_Port1 != RF_Port2)) {
                    _ti.LOGSYSTEM += string.Format("Switch From {0} to {1}\r\n", item.anten == "1" ? RF_Port2 : RF_Port1, item.anten == "1" ? RF_Port1 : RF_Port2);
                    int c = 0;
                RE:
                    c++;
                    bool ret = instrument.config_Instrument_OutputPort(item.anten == "1" ? RF_Port1 : RF_Port2, ref _error);
                    if (ret == false) if (c < 5) goto RE;
                    _antenna = item.anten;
                }

                _ti.LOGSYSTEM += "*************************************************************************\r\n";
                _ti.LOGSYSTEM += string.Format("{0} - {1} - {2} - MCS{3} - BW{4} - Anten {5} - Channel {6}\r\n", _CarrierFreq, item.anten == "1" ? RF_Port1 : RF_Port2, FunctionSupport.Get_WifiStandard_By_Mode(item.wifi, item.bandwidth), item.rate, 20 * Math.Pow(2, double.Parse(item.bandwidth)), item.anten, _channelNo);
                int count = 0;
                string pw_transmited = "";
                string rx_counted = "";
                string rx_per = "";
            REP:
                count++; string logError = "";
                if (!Test_Sensivitity_Detail(_ti, ModemTelnet, instrument, _CarrierFreq, item.wifi, item.rate, item.bandwidth, item.channelfreq, item.anten, item.packet, out pw_transmited, out rx_counted, out rx_per, out logError)) {
                    if (count < 9) {
                        _ti.LOGSYSTEM += string.Format("RETRY = {0}\r\n", count);
                        goto REP;
                    }
                    else {
                        GlobalData.testingData.LOGERROR += logError;
                        _ti.LOGSYSTEM += string.Format("Phán định = {0}", "FAIL\r\n");
                        result = false;
                    }
                    //result = false;
                }
                else _ti.LOGSYSTEM += string.Format("Phán định = {0}", "PASS\r\n");
                App.Current.Dispatcher.Invoke(new Action(() => {
                    string _w = "", _bw = "";
                    switch (item.wifi) {
                        case "0": { _w = "802.11b"; break; }
                        case "1": { _w = "802.11g"; break; }
                        case "2": { _w = "802.11a"; break; }
                        case "3": { _w = "802.11n"; break; }
                        case "4": { _w = "802.11ac"; break; }
                    }
                    switch (item.bandwidth) {
                        case "0": { _bw = "20"; break; }
                        case "1": { _bw = "40"; break; }
                        case "2": { _bw = "80"; break; }
                        case "3": { _bw = "160"; break; }

                    }
                    GlobalData.datagridlogRX.Add(new logreviewrx() { rangeFreq = _CarrierFreq, Anten = item.anten, wifiStandard = _w, Rate = "MCS" + item.rate, Bandwidth = _bw, Channel = _channelNo, transmitPower = pw_transmited, Sent = item.packet.ToString(), Received = rx_counted, Per = string.Format("{0}%", rx_per), Result = result == true ? "PASS" : "FAIL" });
                }));
                st.Stop();
                _ti.LOGSYSTEM += string.Format("Thời gian test độ nhạy thu : {0} ms\r\n", st.ElapsedMilliseconds);
                _ti.LOGSYSTEM += "\r\n";
            }
            return result;
        }

        private bool Test_Sensitivity_2G(testinginfo _ti, ModemTelnet _mt, Instrument _it) {
            return AutoTestSensivitity(_ti, _mt, _it, "2G");
        }

        private bool Test_Sensitivity_5G(testinginfo _ti, ModemTelnet _mt, Instrument _it) {
            return AutoTestSensivitity(_ti, _mt, _it, "5G");
        }

        #endregion

        #endregion

    }
}
