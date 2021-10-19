using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace GW040Hv2Project.Function {

    public class ModemTelnet {
        /*----------------------------------------------------------*/
        IProtocol telnet2modem;
        public int TimeOutMs = 400;
        public int maxModem_DELAY = 200;
        public int avgModem_DELAY = 100;
        public int minModem_DELAY = 50;

        /*----------------------------------------------------------*/
        public ModemTelnet(string IPaddress, int port, string _serialPort) {
            try {
                switch (GlobalData.initSetting.STATION) {
                    case "Trước đóng vỏ": {
                            telnet2modem = new Serial(_serialPort);
                            break;
                        }
                    default: {
                            telnet2modem = new TelnetConnection(IPaddress, port);
                            break;
                        }
                }
            }
            catch {
            };
        }

        public bool IsConnected() {
            return telnet2modem.IsConnected();
        }

        /*----------------------------------------------------------*/
        public bool Login(string username, string pass, int timeout) {
            try {
                string str = telnet2modem.Login(username, pass, timeout);
                str = str.TrimEnd();
                str = str.Substring(str.Length - 1, 1);
                if ((str == "$") || (str == "#")) {
                    telnet2modem.WriteLine("iwpriv rai0 e2p f6");
                    Thread.Sleep(200);
                    string temp = telnet2modem.Read();
                    if (temp.Contains("[0x00F6]")) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else
                    return false;
            }
            catch {
                return false;
            };
        }
        /*----------------------------------------------------------*/
        public void Close() {
            telnet2modem.Close();
        }
        /*----------------------------------------------------------*/
        public void WriteLine(string cmd) {
            try {
                telnet2modem.WriteLine(cmd);
            }
            catch {

            };
        }
        /*----------------------------------------------------------*/
        public string Read() {
            string value = "NULL";
            try {
                value = telnet2modem.Read();
                return value;
            }
            catch {
                return value;
            };
        }


        public void CalibPower_SendCommand(string Standard_2G_or_5G, string Anten, string Channel) {
            string ChannelNumber = Attenuator.getChannelNumber(Channel.Replace("000000", ""));
            List<string> ATEcommands = new List<string>() { string.Format("iwpriv {0} set ATE=ATESTART\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0"),
                                                            string.Format("iwpriv {0} set ATECHANNEL={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0", ChannelNumber),
                                                            string.Format("iwpriv {0} set ATETXMODE=1\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0"),
                                                            string.Format("iwpriv {0} set ATETXMCS=7\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0"),
                                                            string.Format("iwpriv {0} set ATETXBW=0\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0"),
                                                            string.Format("iwpriv {0} set ATETXANT={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0",Anten),
                                                            string.Format("iwpriv {0} set ATETXCNT=0\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0"),
                                                            string.Format("iwpriv {0} set ATE=TXFRAME\r\n", Standard_2G_or_5G == "2G" ? "ra0":"rai0") };

            try {
                if (telnet2modem.IsConnected()) {
                    foreach (var item in ATEcommands) {
                        telnet2modem.Query(item, "#", 1);
                    }
                    if (Standard_2G_or_5G == "2G") {
                        Thread.Sleep(300);
                    }
                    else {
                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception Ex) {
                MessageBox.Show("ERROR CODE: " + Ex.ToString());
            }
        }

        /*-----------------------------------------------------------*/
        //Hàm gửi lệnh phát tín hiệu ở cả 2G và 5G
        public void Verify_Signal_SendCommand(string Standard_2G_or_5G, string Mode, string MCS, string BW, string Channel, string Anten, ref string message) {//2G/1/7/0/2412000000/1
            string ChannelNumber = Attenuator.getChannelNumber(Channel.Replace("000000", ""));
            List<string> ATEcommands = new List<string>() { string.Format("iwpriv {0} set ATE=ATESTART\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0"),
                                                            string.Format("iwpriv {0} set ATECHANNEL={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0", ChannelNumber),
                                                            string.Format("iwpriv {0} set ATETXMODE={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0", Mode),
                                                            string.Format("iwpriv {0} set ATETXMCS={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0", MCS),
                                                            string.Format("iwpriv {0} set ATETXBW={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0", BW),
                                                            string.Format("iwpriv {0} set ATETXANT={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0",Anten),
                                                            string.Format("iwpriv {0} set ATETXCNT=0\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0"),
                                                            string.Format("iwpriv {0} set ATE=TXFRAME\r\n", Standard_2G_or_5G == "2G" ? "ra0":"rai0") };

            try {
                if (telnet2modem.IsConnected()) {
                    foreach (var item in ATEcommands) {
                        telnet2modem.Query(item, "#", 1);
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception Ex) {
                MessageBox.Show("ERROR CODE: " + Ex.ToString());
            }
        }
        /*----------------------------------------------------------*/
        //Hàm gửi lệnh Calib tần số.
        public bool CalibFrequency_SendCommand(string mode, string rate, string bw, string channel, string annten, string FreOffset) {
            bool boolResult = false;
            string[] ATEcommand = { "iwpriv ra0 set ATE=ATESTART\r\n",
                                    "iwpriv ra0 set ATETXMODE=" + mode + "\r\n",       //mode=2 (HT_Mix)
                                    "iwpriv ra0 set ATETXMCS=" + rate + "\r\n",         //MCS
                                    "iwpriv ra0 set ATETXBW=" + bw + "\r\n",           // Bandwith
                                    "iwpriv ra0 set ATECHANNEL=" + channel + "\r\n",    // Channel
                                    "iwpriv ra0 set ATETXGI=0\r\n",
                                    "iwpriv ra0 set ATETXANT=" + annten + "\r\n",      // Annten
                                    "iwpriv ra0 set ATETXFREQOFFSET=" + FreOffset + "\r\n",
                                    "iwpriv ra0 set ATETXCNT=0\r\n",
                                    "iwpriv ra0 set ATE=TXFRAME\r\n"
                                  };
            try {
                if (telnet2modem.IsConnected()) {
                    for (int i = 0; i < 10; i++) {
                        telnet2modem.Query(ATEcommand[i], "#", 1);
                    }
                    Thread.Sleep(100);
                    boolResult = true;
                }
            }
            catch {
                MessageBox.Show("ERROR CODE: [Modem_ConfigLAN] \n Loi! cau hinh cho Modem qua cong LAN\n");
                boolResult = false;
            }
            return boolResult;
        }

        /*----------------------------------------------------------*/
        //Hàm gửi lệnh Test Sensitivity.
        public bool TestSensitivity_SendCommand(string Standard_2G_or_5G, string mode, string rate, string bw, string channel, string annten, ref string message) {
            bool boolResult = false;
            List<string> ATEcommands = new List<string>() { string.Format("iwpriv {0} set ATE=ATESTART\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0"),
                                                            string.Format("iwpriv {0} set ResetCounter=0\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0"),
                                                            string.Format("iwpriv {0} set ATETXMODE={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0",mode),
                                                            string.Format("iwpriv {0} set ATETXMCS={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0",rate),
                                                            string.Format("iwpriv {0} set ATETXBW={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0",bw),
                                                            string.Format("iwpriv {0} set ATECHANNEL={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0",channel),
                                                            string.Format("iwpriv {0} set ATERXANT={1}\r\n",Standard_2G_or_5G == "2G" ? "ra0":"rai0",annten),
                                                            string.Format("iwpriv {0} set ATE=RXFRAME\r\n", Standard_2G_or_5G == "2G" ? "ra0":"rai0") };

            try {
                if (telnet2modem.IsConnected()) {
                    foreach (var item in ATEcommands) {
                        telnet2modem.Query(item, "#", 1);
                    }
                    Thread.Sleep(100);
                    boolResult = true;
                }
            }
            catch {
                MessageBox.Show("ERROR CODE: [Modem_ConfigLAN] \n Loi! cau hinh cho Modem qua cong LAN\n");
                boolResult = false;
            }
            return boolResult;
        }

        /*----------------------------------------------------------*/
        //Hàm gửi lệnh đọc PER
        public string TestSensitivity_ReadPER_SendCommand(string Standard_2G_or_5G, ref string _message) {
            string Result = "";
            string commandLine = Standard_2G_or_5G == "2G" ? "iwpriv ra0 stat" : "iwpriv rai0 stat";
            try {
                if (telnet2modem.IsConnected()) {
                    telnet2modem.WriteLine(commandLine);
                    Thread.Sleep(200);
                    Result = telnet2modem.Read();
                    _message = Result;
                    string[] buffer = Result.Split('\n');
                    foreach (var item in buffer) {
                        if (item.Contains("Rx success")) {
                            Result = item.Split('=')[1].Replace("\r", "").Replace("\n", "").Trim();
                            break;
                        }
                    }
                }
            }
            catch {
                MessageBox.Show("ERROR CODE: [Modem_ConfigLAN] \n Loi! cau hinh cho Modem qua cong LAN\n");
                Result = "ERROR";
            }
            return Result;
        }

        //Hàm ghi offset vào thanh ghi DRAM
        public string Read_Register(string Register) {
            try {
                int count = 0;
            RE:
                count++;
                telnet2modem.Read();
                string data = "";
                telnet2modem.Query("iwpriv rai0 e2p " + Register, "#", out data, 3);
                string[] buffer = data.Split('x');
                data = buffer[buffer.Length - 1].Split('#')[0].Substring(0, 4);
                string pattern = "^[0-9,A-F]{4}$";
                bool r = Regex.IsMatch(data, pattern, RegexOptions.None);
                if (r == false) {
                    if (count < 3) goto RE;
                }
                return data;
            }
            catch {
                return "";
            }
        }

        //Hàm ghi offset vào thanh ghi DRAM
        public bool Write_Register(string Register, string value) {
            return telnet2modem.Query("iwpriv rai0 e2p " + Register + "=" + value, "#", 1);
        }

        //Hàm ghi dữ liệu calib lên Flash
        public bool Write_into_Flash() {
            bool r = false;
            r = telnet2modem.Query("iwpriv rai0 set efuseBufferModeWriteBack=1", "#", 10);
            telnet2modem.WriteLine("");
            Thread.Sleep(100);
            if (!r) return false;
            r = telnet2modem.Query("tcapi set WLan11ac_Common WriteBinToFlash 1", "#", 10);
            telnet2modem.WriteLine("");
            Thread.Sleep(100);
            if (!r) return false;
            r = telnet2modem.Query("tcapi commit WLan11ac", 10, "xyyou:write bin to flash", "#");
            telnet2modem.WriteLine("");
            Thread.Sleep(100);
            if (!r) return false;
            r = telnet2modem.Query("ifconfig ra0 down", "#", 10);
            telnet2modem.WriteLine("");
            Thread.Sleep(100);
            if (!r) return false;
            r = telnet2modem.Query("ifconfig rai0 down", 10, "(rai0) entering forwarding state", "#");
            telnet2modem.WriteLine("");
            Thread.Sleep(100);
            if (!r) return false;
            r = telnet2modem.Query("rmmod mt7615_ap", "#", 10);
            telnet2modem.WriteLine("");
            Thread.Sleep(100);
            if (!r) return false;
            r = telnet2modem.Query("insmod lib/modules/mt7615_ap.ko", "#", 10);
            telnet2modem.WriteLine("");
            Thread.Sleep(100);
            if (!r) return false;
            r = telnet2modem.Query("ifconfig rai0 up", "#", 20);
            telnet2modem.WriteLine("");
            Thread.Sleep(100);
            if (!r) return false;
            r = telnet2modem.Query("ifconfig ra0 up", "#", 20);
            telnet2modem.WriteLine("");
            Thread.Sleep(100);
            if (!r) return false;
            return true;
        }

        //Hàm kiểm tra dữ liệu được lưu thành công chưa.
        public void Wl_Down_Up() {
            telnet2modem.Query("ifconfig rai0 down", "(rai0) entering forwarding state", 10);
            telnet2modem.Query("ifconfig rai0 up", "(rai0) entering forwarding state", 10);
        }

        //Hàm đọc địa chỉ MAC
        public string getMAC() {
            try {
                int count = 0;
            RE:
                count++;
                telnet2modem.WriteLine("ifconfig br0");
                Thread.Sleep(300);
                string tmpStr = telnet2modem.Read().Replace("\n", "").Replace("\r", "").Trim();
                if (tmpStr.Contains("HWaddr") == false) { if (count < 3) goto RE; }
                string[] buffer = tmpStr.Split(new string[] { "HWaddr" }, StringSplitOptions.None);
                tmpStr = buffer[1].Trim();
                if (tmpStr.Length < 17) { if (count < 3) goto RE; }
                string mac = tmpStr.Substring(0, 17).Replace(":", "").Replace("-", "").ToUpper();
                string pattern = "^[0-9,A-F]{12}$";
                bool r = Regex.IsMatch(mac, pattern, RegexOptions.None);
                if (r == false) { if (count < 3) goto RE; }

                return mac;
            }
            catch {
                return "";
            }

        }

    }
}
