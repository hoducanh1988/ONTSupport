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

    public class CalibrationWiFi {

        public bool Excuted() {
            bool _flag = false;
            bool _flagconnection = true, _flagcalibfreq = true, _flagcalib2g = true, _flagcalib5g = true, _flagsaveflash = true, _flagverifyflash = true;
            OntEconet<CalibDataBinding> ont = null;
            IInstrument equipment = null;
            if (myGlobal.mySetting.calib2G || myGlobal.mySetting.calib5G) {
                myGlobal.logRegister = new RegisterInfo();
            }
            App.Current.Dispatcher.Invoke(new Action(() => {
                myGlobal.collectionCalibResult2G.Clear();
                myGlobal.collectionCalibResult5G.Clear();
                myGlobal.collectionCalibFreqResult.Clear();
            }));

            try {
                myGlobal.myCalib.totalResult = "Waiting...";

                //0. Kết nối telnet tới ONT và máy đo
                _flagconnection = Connect_ONT(ref ont, myGlobal.mySetting.loginUser, myGlobal.mySetting.loginPassword, myGlobal.mySetting.serialPortName);
                if (!_flagconnection) goto Finished;
                _flagconnection = Connect_Instrument(ref equipment);
                if (!_flagconnection) goto Finished;

                //1. Calib tần số
                if (myGlobal.mySetting.calibFreq == true) {
                    myGlobal.myCalib.calibFrequencyResult = "Waiting...";
                    int count = 0;
                    string freError = "";
                REP:
                    count++;
                    bool ret = Calibrate_Freq(myGlobal.myCalib, ont, equipment, count, out freError);
                    if (ret == false && count < 3) goto REP;
                    myGlobal.myCalib.calibFrequencyResult = ret ? "Passed" : "Failed";
                    if (ret == false) {
                        string fre = freError != "" ? $"Frequency Error: {freError}" : "";
                        myGlobal.myCalib.logError += string.Format($"[FAIL] Lỗi khi Calib tần số {fre} \n");
                        _flagcalibfreq = false;
                    }
                }

                //2. Calib công suất 2G
                if (myGlobal.mySetting.calib2G == true) {
                    myGlobal.myCalib.calib2GResult = "Waiting...";
                    bool ret = Calibrate_Pwr_2G_Total(myGlobal.myCalib, ont, equipment);
                    myGlobal.myCalib.calib2GResult = ret ? "Passed" : "Failed";
                    if (ret == false) {
                        _flagcalib2g = false;
                        myGlobal.myCalib.logError += string.Format($"[FAIL] Lỗi khi Calib công suất 2G \n");
                    }
                }

                //3. Calib công suất 5G 
                if (myGlobal.mySetting.calib5G == true) {
                    myGlobal.myCalib.calib5GResult = "Waiting...";
                    bool ret = Calibrate_Pwr_5G_Total(myGlobal.myCalib, ont, equipment);
                    myGlobal.myCalib.calib5GResult = ret ? "Passed" : "Failed";
                    if (ret == false) {
                        _flagcalib5g = false;
                        myGlobal.myCalib.logError += string.Format($"[FAIL] Lỗi khi Calib công suất 5G \n");
                    }
                }

                //4. write bin va lưu dữ liệu vào flash
                if (myGlobal.mySetting.saveFlash == true) {
                    bool ret = Save_Flash(myGlobal.myCalib, ont);
                    if (!ret) {
                        _flagsaveflash = false;
                        goto Finished;
                    }
                }

                //Kiem tra gia tri thanh ghi sau khi write flash
                if (myGlobal.mySetting.checkRegister == true) {
                    myGlobal.myCalib.checkRegisterResult = "Waiting...";
                    bool r = verifyRegister(myGlobal.myCalib, ont);
                    myGlobal.myCalib.checkRegisterResult = r ? "Passed" : "Failed";
                    if (r == false) {
                        _flagverifyflash = false;
                        goto Finished;
                    }
                }

                goto Finished;

            }
            catch {
                goto Finished;
            }

        //9. Hiển thị kết quả
        Finished:
            _flag = _flagconnection && _flagcalibfreq && _flagcalib2g && _flagcalib5g && _flagsaveflash && _flagverifyflash;
            Close_ONT(ref ont);
            Close_Instrument(ref equipment);
            myGlobal.myCalib.totalResult = _flag ? "Passed" : "Failed";
            return _flag;
        }

        #region Connection

        bool Connect_ONT(ref OntEconet<CalibDataBinding> ont, string user, string pass, string _serialportname) {
            try {
                int count = 0;
                bool r = false;

            RE:
                count++;
                ont = new OntEconet<CalibDataBinding>(myGlobal.myCalib, myGlobal.mySetting.serialPortName);
                r = ont.Login(user, pass, 400);
                if (!r) {
                    myGlobal.myCalib.logSystem += string.Format("[FAIL] Connect to ONT FAIL => Retry {0}\r\n", count);
                    ont.Close();
                    if (count < 20) goto RE;
                }
                myGlobal.myCalib.logSystem += "[OK] Connect to ONT Successful.\r\n";
                if (r) {
                    myGlobal.myCalib.macAddress = ont.getMAC();
                    myGlobal.myCalib.logSystem += string.Format("ONT MAC Address: {0}\r\n", myGlobal.myCalib.macAddress);
                }
                return r;
            }
            catch {
                return false;
            }
        }

        bool Close_ONT(ref OntEconet<CalibDataBinding> ont) {
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
                    equipment = new E6640A<CalibDataBinding>(myGlobal.myCalib, myGlobal.mySetting.gpibAddress, out ret);
                }
                else {
                    equipment = new MT8870A<CalibDataBinding>(myGlobal.myCalib, myGlobal.mySetting.gpibAddress, out ret);
                }
                goto END;
            }
            catch {
                ret = false;
                goto END;
            }

        END:
            myGlobal.myCalib.logSystem += ret ? "[OK] Kết nối tới máy đo thành công.\n" : "FAIL: Lỗi kết nối máy đo.\n";
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


        #region SubFunction

        string Name_measurement = myGlobal.mySetting.instrumentType;
        string RF_Port1 = myGlobal.mySetting.Port1;
        string RF_Port2 = myGlobal.mySetting.Port2;
        double Target_Pwr_2G = double.Parse(myGlobal.mySetting.target2G);
        double Target_Pwr_5G = double.Parse(myGlobal.mySetting.target5G);

        double Power_Measure;
        double Power_diferent;

        /// <summary>
        /// GHI DỮ LIỆU VÀO FLASH ONT GW040H
        /// </summary>
        /// <param name="_ti"></param>
        /// <param name="ModemTelnet"></param>
        /// <returns></returns>
        bool Save_Flash(CalibDataBinding _ti, OntEconet<CalibDataBinding> ModemTelnet) {
            try {
                Stopwatch st = new Stopwatch();
                st.Start();
                //write defaut bin
                if (myGlobal.mySetting.writeBIN == true) {
                    myGlobal.myCalib.writeBinResult = "Waiting...";
                    try {
                        _ti.logSystem += "------------------------------------------------------------\r\n";
                        _ti.logSystem += "Thực hiện write giá trị file BIN.\r\n";
                        if (myGlobal.listBinRegister.Count > 0) {
                            foreach (var item in myGlobal.listBinRegister) {
                                ModemTelnet.Write_Register(item.Address, item.newValue);
                                _ti.logRegister += string.Format("{0}={1}\n", item.Address, item.newValue);
                                Thread.Sleep(100);
                                _ti.logSystem += string.Format("Write Register: {0} = {1}\r\n", item.Address, item.newValue);
                            }
                        }
                        myGlobal.myCalib.writeBinResult = "Passed";
                    }
                    catch {
                        myGlobal.myCalib.writeBinResult = "Failed";
                    }
                }
                st.Stop();
                _ti.logSystem += string.Format("Thời gian ghi file BIN : {0} ms\r\n", st.ElapsedMilliseconds);
                st.Restart();

                //save flash
                _ti.logSystem += "------------------------------------------------------------\r\n";
                _ti.logSystem += "Thực hiện lưu vào FLASH.\r\n";
                myGlobal.myCalib.saveFlashResult = "Waiting...";
                bool r = ModemTelnet.Write_into_Flash();
                myGlobal.myCalib.saveFlashResult = r ? "Passed" : "Failed";
                st.Stop();
                _ti.logSystem += string.Format("Thời gian lưu vào FLASH : {0} ms\r\n", st.ElapsedMilliseconds);
                return r;
            }
            catch {
                myGlobal.myCalib.logError += string.Format($"[FAIL] Lỗi khi Save Flash \n");
                return false;
            }
        }


        public bool verifyRegister(CalibDataBinding _ti, OntEconet<CalibDataBinding> ModemTelnet) {
            try {
                bool r = false;
                bool ret = true;
                string[] buffer = _ti.logRegister.Split('\n');
                if (buffer == null || buffer.Length == 0) return true;

                _ti.logSystem += "------------------------------------------------------------\r\n";
                _ti.logSystem += "Thực hiện check lại giá trị thanh ghi sau khi save flash.\r\n";
                _ti.logSystem += "Register".PadLeft(20) + "Before".PadLeft(20) + "After".PadLeft(20) + "Result".PadLeft(20) + "\n";

                foreach (var b in buffer) {
                    if (string.IsNullOrEmpty(b) == false && string.IsNullOrWhiteSpace(b) == false) {
                        string std_register = b.Split('=')[0];
                        string std_value = b.Split('=')[1].Substring(2, 2);
                        string curr_value = ModemTelnet.Read_Register(std_register).Substring(2, 2);
                        r = curr_value.ToLower().Replace("\n", "").Replace("\r", "").Equals(std_value.ToLower());

                        _ti.logSystem += std_register.PadLeft(20) + std_value.PadLeft(20) + curr_value.PadLeft(20) + string.Format("{0}", r ? "Passed" : "Failed").PadLeft(20) + "\n";

                        if (r == false) ret = false;
                    }
                }
                return ret;
            } catch {
                return true;
            }
            
        }

        /// <summary>
        /// CALIB TẦN SỐ *********************************************
        /// 1. Calibrate_Freq -------------//Calib tần số
        /// 2. Calculate_NewValue ---------//
        /// 
        /// </summary>
        #region CALIB FREQUENCY

        //OK
        private bool Calibrate_Freq(CalibDataBinding _ti, OntEconet<CalibDataBinding> ModemTelnet, IInstrument instrument, int retry, out string frequenError) {
            Stopwatch st = new Stopwatch();
            CalibFrequencyResultInfo cfri = new CalibFrequencyResultInfo();
            st.Start();
            _ti.logSystem += "------------------------------------------------------------\r\n";
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
                _ti.logSystem += "Bắt đầu quá trình Calib tần số." + "\r\n";
                _ti.logSystem += "Đang đọc thanh ghi F4,F6..." + "\r\n";
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

                _ti.logSystem += "0x00F4 = " + F4 + "; " + "0x00F6 = " + F6 + "\r\n";
                F4_6_0 = FunctionSupport.HextoBin(F4.Substring(4, 2)).Substring(1, 7);
                F4_6_0_DEC = Convert.ToInt64(F4_6_0, 2);
                F6_5_0 = FunctionSupport.HextoBin(F6.Substring(4, 2)).Substring(2, 6);
                F6_5_0_DEC = Convert.ToInt64(F6_5_0, 2);
                _ti.logSystem += "F4[6:0] = " + F4_6_0 + "; " + "F6[5:0] = " + F6_5_0 + "\r\n";
                _ti.logSystem += "F4[6:0]_DEC = " + F4_6_0_DEC + "; " + "F6[5:0]_DEC = " + F6_5_0_DEC + "\r\n";
                cfri.F4Dec = F4_6_0_DEC.ToString();
                cfri.F6DecOld = F6_5_0_DEC.ToString();

                Current_Freq_Offset = FunctionSupport.Plus_F4_With_F6(F4, F6); //Tính giá trị FreqOffset ban đầu
                _ti.logSystem += "Giá trị Frequency Offset hiện tại = F4[6,0] +/- F6[5,0] = " + Current_Freq_Offset + "\r\n";
                cfri.offsetOld = Current_Freq_Offset;

                //Gửi lệnh phát tín hiệu kèm Frequency Offset
                _ti.logSystem += "Gửi lệnh phát tín hiệu kèm Current_Freq_Offset: " + Current_Freq_Offset + "\r\n";
                _ti.logSystem += string.Format("Đang cấu hình lần đầu cho máy đo {0}...", Name_measurement) + "\r\n";

                bool _configInstrIsOk = false;
                int _index = 0;
                while (_configInstrIsOk == false) {
                    string _error = "";
                    _configInstrIsOk = instrument.config_Instrument_Total(RF_Port1, "g", ref _error);
                    if (_error != "") _ti.logSystem += string.Format("{0}\r\n", _error);
                    _error = "";
                    _configInstrIsOk = instrument.config_Instrument_Channel("2437000000", ref _error);
                    if (_error != "") _ti.logSystem += string.Format("{0}\r\n", _error);
                    _index++;
                    if (_index > 20) break;
                }
                if (_configInstrIsOk == false) {
                    myGlobal.myCalib.logError += string.Format("[FAIL] Lỗi cấu hình máy đo khi calib tần số \n");
                    frequenError = Freq_Err;
                    return false;
                }

                _ti.logSystem += string.Format("Đang phát tín hiệu ở Anten 1 - Channel 6 - Máy đo {0} - Offset = {1}", Name_measurement, Current_Freq_Offset) + "\r\n";
                ModemTelnet.CalibFrequency_SendCommand("1", "7", "0", "6", "1", Current_Freq_Offset); //(mode,rate,bw,channel,anten,freqOffset)
                                                                                                      //Thread.Sleep(500);
                                                                                                      //for (int i = 0; i < 2; i++) {
                Freq_Err = instrument.config_Instrument_get_FreqErr("RFB", "g"); //Lệnh đọc giá trị về từ máy đo
                if (string.IsNullOrEmpty(Freq_Err)) Freq_Err = "999";

                _ti.logSystem += "..." + "\r\n";
                _ti.logSystem += "Lấy kết quả đo lần thứ: " + retry.ToString() + "\r\n";
                if (!Freq_Err.Contains("999")) {
                    cfri.freqErrorOld = Freq_Err;

                    if (Convert.ToDouble(Freq_Err) > -2000 && Convert.ToDouble(Freq_Err) < 2000) {
                        _ti.logSystem += "Frequency Err = " + Freq_Err + "\r\n";
                        _ti.logSystem += "Frequency Error đã đạt Target." + "\r\n";
                        _ti.logSystem += "---------------------------------" + "\r\n";
                        FreOffset_new = Decimal.Parse(Current_Freq_Offset);
                        //Result_FreqErr_Calib = "PASS";
                        //break;
                    }
                    else {
                        _ti.logSystem += "Giá trị Frequency Error = " + Freq_Err + "\r\n";
                        if (Convert.ToDouble(Freq_Err) < 0)
                            _ti.logSystem += "Frequency Error < 0 -> Cần giảm Frequency Offset" + "\r\n";
                        else
                            _ti.logSystem += "Frequency Error > 0 -> Cần tăng Frequency Offset" + "\r\n";

                        _ti.logSystem += "Mỗi 1500 Khz bị lệch tương ứng với 1 giá trị Decimal => Giá trị DEC mà Current_Freq_Offset và F6[5,0] cần thay đổi = Freq_Err / 1500 = " + Math.Round((Decimal.Parse(Freq_Err)) / 1500) + "\r\n"; //2350                  
                        FreOffset_new = Math.Round(Decimal.Parse(Current_Freq_Offset) + Math.Round(Decimal.Parse(Freq_Err) / 1500)); //2350
                        _ti.logSystem += "Freq_Offset_new = Current_Freq_Offset + Freq_Err/1500 = " + FreOffset_new + "\r\n";
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
                        _ti.logSystem += "F6[5,0] cũ ở dạng DEC = " + F6_5_0_old_toDEC + "\r\n";

                        //F6_5_0_new_DEC = FreOffset_new - F4_6_0_DEC;
                        if (FreOffset_new > F4_6_0_DEC) {
                            F6_5_0_new_DEC = FreOffset_new - F4_6_0_DEC;
                        }
                        else {
                            F6_5_0_new_DEC = F4_6_0_DEC - FreOffset_new;
                        }

                        //F6_5_0_new_DEC = Math.Round(F6_5_0_old_toDEC - (Decimal.Parse(Freq_Err) / 1500)); //2350
                        _ti.logSystem += "F6[5,0] mới ở dạng DEC = " + F6_5_0_new_DEC + "\r\n";
                        cfri.F6DecNew = F6_5_0_new_DEC.ToString();
                        string F6_full_new_BIN = "";
                        F7 = F6.Substring(2, 2);

                        //30/06/2020
                        if (FreOffset_new < F4_6_0_DEC) {
                            F6_full_new_BIN = FunctionSupport.KiemtraF6("Can Giam", Int32.Parse(F6_5_0_new_DEC.ToString()));
                            _ti.logSystem += "F6_Full_Bin_New = " + F6_full_new_BIN + "\r\n";
                            _ti.logSystem += "Giá trị F6 mới cần truyền: " + FunctionSupport.BintoHexOnly(F6_full_new_BIN) + "\r\n"; ;

                            _ti.logSystem += "iwpriv ra0 e2p F6=" + F7 + FunctionSupport.BintoHexOnly(F6_full_new_BIN) + "\r\n";
                            ModemTelnet.WriteLine("iwpriv ra0 e2p F6=" + F7 + FunctionSupport.BintoHexOnly(F6_full_new_BIN));
                        }
                        else if (FreOffset_new > F4_6_0_DEC) {
                            F6_full_new_BIN = FunctionSupport.KiemtraF6("Can Tang", Int32.Parse(F6_5_0_new_DEC.ToString()));
                            _ti.logSystem += "F6_Full_Bin_New = " + F6_full_new_BIN + "\r\n";
                            _ti.logSystem += "Giá trị F6 mới cần truyền: " + FunctionSupport.BintoHexOnly(F6_full_new_BIN) + "\r\n";

                            _ti.logSystem += "iwpriv ra0 e2p F6=" + F7 + FunctionSupport.BintoHexOnly(F6_full_new_BIN) + "\r\n";
                            ModemTelnet.WriteLine("iwpriv ra0 e2p F6=" + F7 + FunctionSupport.BintoHexOnly(F6_full_new_BIN));
                        }
                        else {
                            ModemTelnet.WriteLine("iwpriv ra0 e2p F6=" + F6);
                        }


                        _ti.logSystem += "-------------------------------------------" + "\r\n";
                        //_ti.LOGSYSTEM += "Thực hiện Write to Flash.");
                        //Save_Flash();

                        _ti.logSystem += "-------------------------------------------" + "\r\n";
                        _ti.logSystem += "Bắt đầu thực hiện Verify Frequency Error." + "\r\n";
                        _ti.logSystem += string.Format("Đang phát tín hiệu ở Anten 1 - Channel 6") + "\r\n";
                        ModemTelnet.CalibFrequency_SendCommand("1", "7", "0", "6", "1", FreOffset_new.ToString()); //(mode,rate,bw,channel,anten,freqOffset)
                        Freq_Err = instrument.config_Instrument_get_FreqErr("RFB", "g"); //Lệnh đọc giá trị về từ máy đo
                        _ti.logSystem += "Frequency Error = " + Freq_Err + "\r\n";
                        cfri.freqErrorNew = Freq_Err;



                    }
                }
                else {
                    //continue;
                }
                //}
                st.Stop();
                _ti.logSystem += string.Format("Thời gian calib Freq : {0} ms\r\n", st.ElapsedMilliseconds);

                frequenError = Freq_Err;
                App.Current.Dispatcher.Invoke(new Action(() => { myGlobal.collectionCalibFreqResult.Add(cfri); }));
                return Math.Abs(Convert.ToDouble(Freq_Err)) < 2000;
            }
            catch {
                st.Stop();
                _ti.logSystem += string.Format("Thời gian calib Freq : {0} ms\r\n", st.ElapsedMilliseconds);
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

        private bool Calibrate_Pwr_2G_Total(CalibDataBinding _ti, OntEconet<CalibDataBinding> _mt, IInstrument _it) {
            myGlobal.myCalib.maxCalib2G = myGlobal.listCalibPower2G.Count;
            return AutoCalibPower(_ti, _mt, _it, "2G");
        }

        private bool Calibrate_Pwr_5G_Total(CalibDataBinding _ti, OntEconet<CalibDataBinding> _mt, IInstrument _it) {
            myGlobal.myCalib.maxCalib5G = myGlobal.listCalibPower5G.Count;
            return AutoCalibPower(_ti, _mt, _it, "5G");
        }


        private bool AutoCalibPower(CalibDataBinding _ti, OntEconet<CalibDataBinding> ModemTelnet, IInstrument instrument, string _CarrierFreq) {
            try {
                List<CalibPowerConfigInfo> list = null;
                _ti.logSystem += "------------------------------------------------------------\r\n";
                _ti.logSystem += string.Format("Bắt đầu thực hiện quá trình Calib Công Suất {0}.\r\n", _CarrierFreq);
                list = _CarrierFreq == "2G" ? myGlobal.listCalibPower2G : myGlobal.listCalibPower5G;
                string _error = "";
                bool _result = true;

                instrument.config_Instrument_Total(RF_Port1, "g", ref _error);
                Thread.Sleep(1000);
                string _antenna = "0";

                if (list.Count == 0) return true;

                foreach (var item in list) {
                    switch (_CarrierFreq) {
                        case "2G": { myGlobal.myCalib.valueCalib2G++; break; }
                        case "5G": { myGlobal.myCalib.valueCalib5G++; break; }
                    }

                    Stopwatch st = new Stopwatch();
                    st.Start();
                    string _eqChannel = string.Format("{0}000000", item.channelfreq);
                    double _attenuator = 0;
                    if (item.anten == "1") _attenuator = double.Parse(myGlobal.listAttenuator.Where(x => x.Frequency.Equals(item.channelfreq)).FirstOrDefault().Antenna1);
                    if (item.anten == "2") _attenuator = double.Parse(myGlobal.listAttenuator.Where(x => x.Frequency.Equals(item.channelfreq)).FirstOrDefault().Antenna2);
                    string _channelNo = myGlobal.listAttenuator.Where(x => x.Frequency.Equals(item.channelfreq)).FirstOrDefault().Channel;

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

                    //Calib Power
                    _ti.logSystem += "*************************************************************************\r\n";
                    _ti.logSystem += string.Format("{0} - {1} - 802.11g - MCS7 - BW20 - Anten {2} - Channel {3} - {4}  \r\n", _CarrierFreq, item.anten == "1" ? RF_Port1 : RF_Port2, item.anten, _channelNo, item.register);
                    _ti.logSystem += string.Format("Attenuator: {0}(db)\n", _attenuator);
                    
                    //check thanh ghi co can calib ko?
                    CalibPowerResultInfo cpri = null;
                    if (item.calibflag.Trim() == "1") { //Phai calib
                        int count = 0;
                    REP:
                        count++;
                        string tmp1 = "";
                        if (!Calibrate_Pwr_Detail(_ti, ModemTelnet, instrument, _CarrierFreq, item.anten == "1" ? RF_Port1 : RF_Port2, item.anten, _eqChannel, item.register, _attenuator, out tmp1, out cpri)) {
                            if (count < 3) goto REP;
                            myGlobal.myCalib.logError += tmp1;
                            _result = false;
                        }
                    }
                    else { //khong can calib
                        string New_Value = "";
                        var propInfo = myGlobal.logRegister.GetType().GetProperty(string.Format("_{0}", item.refference));
                        if (propInfo != null) {
                            New_Value = propInfo.GetValue(myGlobal.logRegister).ToString();
                        }
                        _ti.logSystem += string.Format("Thanh ghi reference {0}, Giá trị {1} \r\n", item.refference, New_Value);

                        //Ghi gia tri thanh ghi
                        _ti.logSystem += "Giá trị cần truyền: " + New_Value + "\r\n";
                        ModemTelnet.Write_Register(item.register.Split('x')[1], New_Value);
                        _ti.logRegister += string.Format("{0}={1}\n", item.register.Split('x')[1], New_Value);
                    }

                    App.Current.Dispatcher.Invoke(new Action(() => {
                        if (_CarrierFreq == "5G") {
                            myGlobal.collectionCalibResult5G.Add(cpri);
                            if (_result) myGlobal.myCalib.passedCalib5G++;
                            else myGlobal.myCalib.failedCalib5G++;
                        }
                        else { 
                            myGlobal.collectionCalibResult2G.Add(cpri);
                            if (_result) myGlobal.myCalib.passedCalib2G++;
                            else myGlobal.myCalib.failedCalib2G++;
                        }
                    }));

                    st.Stop();
                    _ti.logSystem += string.Format("Thời gian calib : {0} ms\r\n", st.ElapsedMilliseconds);
                    _ti.logSystem += "\r\n";
                }
                return _result;
            }
            catch {
                return false;
            }
        }


        private bool Calibrate_Pwr_Detail(CalibDataBinding _ti, OntEconet<CalibDataBinding> ModemTelnet, IInstrument instrument, string Standard_2G_or_5G, string RFinput, string Anten, string Channel_Freq, string Register, double Attenuator, out string error, out CalibPowerResultInfo cri) {
            cri = new CalibPowerResultInfo() {
                Range = Standard_2G_or_5G,
                Channel = Channel_Freq.Replace("000000", ""),
                Antenna = Anten,
                Attenuator = Attenuator.ToString(),
                registerAddress = Register,
            };

            string Register_Old_Value_Pwr = "", Register_New_Value_Pwr = ""; error = "";
            bool _flag = true;
            try {
                Register_Old_Value_Pwr = ModemTelnet.Read_Register(Register.Split('x')[1]);
                if (Register_Old_Value_Pwr.Contains(Register.Split('x')[1])) {
                    Register_Old_Value_Pwr = ModemTelnet.Read_Register(Register.Split('x')[1]);
                    return false;
                }
                else {
                    _ti.logSystem += "Giá trị thanh ghi " + Register + " hiện tại: " + Register_Old_Value_Pwr + "\r\n";
                    cri.currentValue = Register_Old_Value_Pwr;

                    ModemTelnet.CalibPower_SendCommand(Standard_2G_or_5G, Anten, Channel_Freq);

                    string _error = "";
                    instrument.config_Instrument_Channel(Channel_Freq, ref _error);
                    ModemTelnet.Read_Register(Register.Split('x')[1]);

                    int count = 0;
                    List<double> list_power = new List<double>();
                RETRY:
                    count++;
                    for (int i = 0; i < 3; i++) {
                        int c = 0;
                    RE:
                        c++;
                        Power_Measure = Convert.ToDouble(instrument.config_Instrument_get_Power(count == 0 ? (Standard_2G_or_5G == "2G" ? "RFB" : "VID") : (Standard_2G_or_5G == "2G" ? "VID" : "RFB"), "g")) + Attenuator;
                        if (Convert.ToDouble(Power_Measure) < 15) { if (c < 2) goto RE; }
                        list_power.Add(Power_Measure);
                        switch (i) {
                            case 0: { cri.powerValue1 = Math.Round(Power_Measure, 2).ToString(); break; }
                            case 1: { cri.powerValue2 = Math.Round(Power_Measure, 2).ToString(); break; }
                            case 2: { cri.powerValue3 = Math.Round(Power_Measure, 2).ToString(); break; }
                        }
                        _ti.logSystem += "Công suất đo được: " + Power_Measure + " (dBm)" + "\r\n";
                    }

                    List<double> list_out = null;
                    FunctionSupport.rejectDifferenceValueFromList(list_power, out list_out);

                    Power_Measure = FunctionSupport.RoundDecimal(list_out.Average());

                    if (Power_Measure < double.Parse(myGlobal.mySetting.lowerLimit) || Power_Measure > double.Parse(myGlobal.mySetting.upperLimit)) {
                        _flag = false;
                        goto END;
                    }


                    if (Standard_2G_or_5G == "2G") Power_diferent = Power_Measure - Convert.ToDouble(Target_Pwr_2G);
                    else if (Standard_2G_or_5G == "5G") Power_diferent = Power_Measure - Convert.ToDouble(Target_Pwr_5G);
                    _ti.logSystem += "Độ lệch công suất: " + Power_diferent + " (dBm)" + "\r\n";
                    cri.differencePower = Power_diferent.ToString();

                    if (Power_diferent == 0) { Register_New_Value_Pwr = Register_Old_Value_Pwr; goto END; }
                    Calculate_NewValue(Power_diferent, Register_Old_Value_Pwr, out Register_New_Value_Pwr);

                    if (Register_New_Value_Pwr.Contains("ERROR")) {
                        _ti.logSystem += "[FAIL] Bắt đầu thực hiện lại.\r\n";
                        _flag = false;
                        if (count < 3) goto RETRY;
                    }
                    else {
                        _ti.logSystem += "Giá trị cần truyền: " + Register_New_Value_Pwr + "\r\n";
                        ModemTelnet.Write_Register(Register.Split('x')[1], Register_New_Value_Pwr);
                        _ti.logRegister += string.Format("{0}={1}\n", Register.Split('x')[1], Register_New_Value_Pwr);
                        _flag = true;
                    }
                }

            END:
                cri.newValue = Register_New_Value_Pwr;
                var propInfo = myGlobal.logRegister.GetType().GetProperty(string.Format("_{0}", Register));
                if (propInfo != null) {
                    propInfo.SetValue(myGlobal.logRegister, Register_New_Value_Pwr == "" ? Register_Old_Value_Pwr.Substring(2, 2) : Register_New_Value_Pwr.Substring(2, 2), null);
                }
            }
            catch {
                _flag = false;
            }

            cri.Result = _flag ? "Passed" : "Failed";
            return _flag;
        }


        #endregion



        #endregion

    }

}
