using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {
    public class Serial : IProtocol {

        SerialPort _serialport = null;
        string _PortName = "";

        ~Serial() {
            try {
                this._serialport.Close();
            } catch {
            }
        }

        public Serial(string _portname) {
            this._PortName = _portname;

            int count = 0;
            bool result = false;
            REP:
            count++;
            try {
                this._serialport = new SerialPort();
                this._serialport.PortName = _PortName;
                this._serialport.BaudRate = 115200;
                this._serialport.DataBits = 8;
                this._serialport.Parity = Parity.None;
                this._serialport.StopBits = StopBits.One;
                this._serialport.Open();
                result = _serialport.IsOpen;
            }
            catch {
                this._serialport.Close();
                result = false;
            }
            if (!result) { if (count < 3) { this._serialport.Close(); Thread.Sleep(100); goto REP; } }
        }

        public string Login(string Username, string Password, int LoginTimeOutMs) {
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
                    if (s.Replace("\r", "").Replace("\n", "").Trim().Contains("#")) return s;
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
                return s;
            }
            catch {
                return s;
            }
        }

        public bool IsConnected() {
            return _serialport.IsOpen;
        }

        public bool Close() {
            try {
                this._serialport.Close();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool Write(string _cmd) {
            try {
                this._serialport.Write(_cmd);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool WriteLine(string _cmd) {
            try {
                this._serialport.WriteLine(_cmd);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool Query(string cmd, string patern, int timeout_sec) {
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
            r = data.ToLower().Contains(patern.ToLower());
            if (!r) {
                if (count < max_count) goto RE;
            }
            return r;
        }

        public bool Query(string cmd, string patern, out string data, int timeout_sec) {
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


        public bool Query(string cmd, int timeout_sec, params string[] paterns) {
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
            foreach (var patern in paterns) {
                r = data.ToLower().Contains(patern.ToLower());
                if (!r) break;
            }
            
            if (!r) {
                if (count < max_count) goto RE;
            }
            return r;
        }


        public string Read() {
            try {
                string data = this._serialport.ReadExisting();
                GlobalData.testingData.LOGUART += data;
                return data;
            }
            catch {
                return "";
            }
        }

    }
}
