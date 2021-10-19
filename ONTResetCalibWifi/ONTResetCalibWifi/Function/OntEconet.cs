using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace ONTResetCalibWifi.Function {
    public class OntEconet {

        SerialPort ont = null;
        string port_name = "";
        bool isConnected = false;

        public OntEconet(string _port) {
            this.port_name = _port;
        }

        private bool open() {
            try {
                ont = new SerialPort();
                ont.PortName = port_name;
                ont.BaudRate = 115200;
                ont.DataBits = 8;
                ont.Parity = Parity.None;
                ont.StopBits = StopBits.One;
                ont.Open();

                isConnected = ont.IsOpen;
                return isConnected;
            } catch { return false; }
        }

        private bool Write(string cmd) {
            try {
                ont.Write(cmd);
                return true;
            } catch { return false; }
        }

        private bool WriteLine(string cmd) {
            try {
                ont.Write(cmd + "\n");
                return true;
            }
            catch { return false; }
        }

        private string Read() {
            try {
                string data = ont.ReadExisting();
                myGlobal.myTesting.LogUart += data;
                return data;
            }
            catch { return ""; }
        }

        private bool Query(string cmd, string pattern, int timeout_sec, bool isEndWidth) {
            int delay_time = 25;
            int max_count = (timeout_sec * 1000) / delay_time;
            int count = 0;
            string data = "";
            bool r = false;

            this.Read();
            this.WriteLine(cmd);
        RE:
            count++;
            Thread.Sleep(delay_time);
            data += this.Read();
            if (isEndWidth == false) r = data.ToLower().Contains(pattern.ToLower());
            else r = data.Trim().ToLower().EndsWith(pattern.ToLower());
            if (!r) {
                if (count < max_count) goto RE;
            }
            return r;
        }


        private bool Query(string cmd, string pattern, int timeout_sec, bool isEndWidth, out string data) {
            int delay_time = 25;
            int max_count = (timeout_sec * 1000) / delay_time;
            int count = 0;
            data = "";
            bool r = false;

            this.Read();
            this.WriteLine(cmd);
        RE:
            count++;
            Thread.Sleep(delay_time);
            data += this.Read();
            if (isEndWidth == false) r = data.ToLower().Contains(pattern.ToLower());
            else r = data.Trim().ToLower().EndsWith(pattern.ToLower());
            if (!r) {
                if (count < max_count) goto RE;
            }
            return r;
        }

        private bool Query(string cmd, string patern, out string data, int timeout_sec) {
            int delay_time = 25;
            int max_count = (timeout_sec * 1000) / delay_time;
            int count = 0;
            data = "";
            bool r = false;

            this.Read();
            this.WriteLine(cmd);
        RE:
            count++;
            Thread.Sleep(delay_time);
            data += this.Read();
            r = data.ToLower().Contains(patern.ToLower());
            if (!r) {
                if (count < max_count) goto RE;
            }
            return r;
        }

        public bool Dispose() {
            try {
                if (ont != null && ont.IsOpen) ont.Close();
                return true;
            } catch { return false; }
        }

        public bool Login(string Username, string Password, int LoginTimeOutMs) {
            if (isConnected == false) this.open();
            if (isConnected == false) return false;

            string s = "";
            try {
                bool _flag = false;
                int index = 0;
                int max = 20;
                while (!_flag) {
                    //Gửi lệnh Enter để ONT về trạng thái đăng nhập
                    this.WriteLine("\r\n");
                    Thread.Sleep(250);
                    s = this.Read();
                    if (s.Replace("\r", "").Replace("\n", "").Trim().Contains("#")) return true;
                    while (!s.Contains("tc login:")) {
                        s += this.Read();
                        Thread.Sleep(500);
                        if (index >= max) break;
                        else index++;
                    }
                    if (index >= max) break;

                    //Gửi thông tin User
                    this.Write(Username + "\n");

                    //Chờ ONT xác nhận User
                    while (!s.Contains("Password:")) {
                        s += this.Read();
                        Thread.Sleep(500);
                        if (index >= max) break;
                        else index++;
                    }
                    if (index >= max) break;

                    //Gửi thông tin Password
                    this.Write(Password + "\n");

                    //Chờ ONT xác nhận Password
                    while (!s.Contains("root login  on `console'")) {
                        s += this.Read();
                        Thread.Sleep(500);
                        if (index >= max) break;
                        else index++;
                    }
                    if (index >= max) break;
                    else _flag = true;
                }
                return _flag;
            }
            catch {
                return false;
            }
        }

        public string Get_Mac() {
            string data = "";
            this.Query("ifconfig br0", "#", 3, true, out data);
            string mac = data.Split(new string[] { "Link encap:Ethernet  HWaddr" }, StringSplitOptions.None)[1].Trim();
            mac = mac.Substring(0, 17).Replace(":", "");
            return mac;
        }

        public bool Write_Register(string Register, string value) {
            return this.Query("iwpriv rai0 e2p " + Register + "=" + value, "#", 1, true);
        }

        public string Read_Register(string Register) {
            try {
                int count = 0;
            RE:
                count++;
                this.Read();
                string data = "";
                this.Query("iwpriv rai0 e2p " + Register, "#", out data, 3);
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

        public bool Save_Flash() {
            try {
                bool r = false;
                r = this.Query("iwpriv rai0 set efuseBufferModeWriteBack=1", "#", 1, true);
                if (!r) return false;
                r = this.Query("tcapi set WLan11ac_Common WriteBinToFlash 1", "#", 1, true);
                if (!r) return false;
                r = this.Query("tcapi commit WLan11ac", "#", 30, true);
                if (!r) return false;
                r = this.Query("ifconfig ra0 down", "#", 3, true);
                if (!r) return false;
                r = this.Query("ifconfig rai0 down", "#", 3, true);
                if (!r) return false;
                r = this.Query("rmmod mt7615_ap", "#", 5, true);
                if (!r) return false;
                r = this.Query("insmod lib/modules/mt7615_ap.ko", "#", 3, true);
                if (!r) return false;
                r = this.Query("ifconfig rai0 up", "#", 30, true);
                if (!r) return false;
                r = this.Query("ifconfig ra0 up", "#", 3, true);
                return r;
            }
            catch { return false; }
        }


    }
}
