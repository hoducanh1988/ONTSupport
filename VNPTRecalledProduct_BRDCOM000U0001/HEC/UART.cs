using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HEC {

    public class UART : IProtocol {

        SerialPort serial_port = null;

        public bool Close() {
            try {
                if (serial_port != null) serial_port.Close();
                return true;
            }
            catch {
                return false;
            }
        }

        public bool Open(string port_name, string baud_rate, string data_bit) {
            try {
                serial_port = new SerialPort();
                serial_port.PortName = port_name;
                serial_port.BaudRate = int.Parse(baud_rate);
                serial_port.DataBits = int.Parse(data_bit);
                serial_port.Parity = Parity.None;
                serial_port.StopBits = StopBits.One;
                serial_port.Open();

                return serial_port.IsOpen;
            }
            catch {
                return false;
            }
        }

        public bool isConnected() {
            if (serial_port == null) return false;
            try {
                return serial_port.IsOpen;
            }
            catch {
                return false;
            }
        }

        public bool Open(string visa_address) {
            throw new NotImplementedException();
        }

        public bool Open(string ip_address, string port) {
            throw new NotImplementedException();
        }

        public bool Open(string ip_address, string port, string width, string height) {
            throw new NotImplementedException();
        }

        public string Query(string cmd) {
            this.WriteLine(cmd);
            Thread.Sleep(100);
            return this.Read();
        }

        public bool Query(string cmd, string timeout_sec, string end_swith) {
            if (!isConnected()) return false;
            this.WriteLine(cmd);
            int delay_ms = 25;
            int count = 0;
            int max_count = (int.Parse(timeout_sec) * 1000) / delay_ms;
            string data = "";

        RE:
            bool r = true;
            count++;
            data += this.Read();
            r = data.Replace("\n", "").Replace("\r", "").Trim().ToLower().EndsWith(end_swith.ToLower());
            if (!r) {
                if (count < max_count) {
                    Thread.Sleep(delay_ms);
                    goto RE;
                }
            }

            return r;
        }

        public string Read() {
            if (isConnected() == false) return null;
            return serial_port.ReadExisting();
        }

        public bool Write(string cmd) {
            if (isConnected() == false) return false;

            try {
                serial_port.Write(cmd);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool WriteBreak() {
            throw new NotImplementedException();
        }

        public bool WriteLine(string cmd) {
            return this.Write(cmd + "\n");
        }

    }


}
