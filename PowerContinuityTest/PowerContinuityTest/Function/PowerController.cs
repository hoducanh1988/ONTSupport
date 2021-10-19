using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace PowerContinuityTest.Function {

    public class PowerController {

        SerialPort serial;
        public bool isConnected = false;

        public bool Open(string portname, string baudrate, string databits, Parity parity, StopBits stopbits) {
            try {
                serial = new SerialPort();
                serial.PortName = portname;
                serial.BaudRate = int.Parse(baudrate);
                serial.DataBits = int.Parse(databits);
                serial.Parity = parity;
                serial.StopBits = stopbits;
                serial.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

                serial.Open();
                isConnected = serial.IsOpen;
                return isConnected;
            }
            catch {
                isConnected = false;
                return false;
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            // Show all the incoming data in the port's buffer
            myGlobal.myTesting.logController += serial.ReadExisting();
        }

        public string Query(string cmd) {
            if (!isConnected) return null;
            this.WriteLine(cmd);
            Thread.Sleep(100);
            return this.Read();
        }

        public string Read() {
            if (!isConnected) return null;
            return serial.ReadExisting();
        }

        public bool Write(string cmd) {
            if (!isConnected) return false;
            try {
                serial.Write(cmd);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool WriteLine(string cmd) {
            return this.Write(cmd + "\n");
        }

        public bool Start(int loop, double op, double cl) {
            if (!isConnected) return false;
            serial.WriteLine(string.Format("LP:{0}", loop));
            Thread.Sleep(300);
            if (cl < 1) { serial.WriteLine(string.Format("CL:{0}.0", cl * 10)); }
            else { serial.WriteLine(string.Format("CL:{0}", cl)); }
            Thread.Sleep(300);
            serial.WriteLine(string.Format("OP:{0}", op));
            Thread.Sleep(300);
            return true;
        }

        public bool SetOP(double op) {
            if (!isConnected) return false;
            serial.WriteLine(string.Format("OP:{0}", op));
            return true;
        }



        public bool Stop() {
            if (!isConnected) return false;
            serial.WriteLine("OP:0000");
            Thread.Sleep(300);
            return true;
        }

        public bool stopHighLevel() {
            if (!isConnected) return false;
            serial.WriteLine("OP:9999");
            Thread.Sleep(300);
            return true;
        }

        public bool Close() {
            if (!isConnected) return true;
            try {
                if (serial != null) serial.Close();
                return true;
            }
            catch {
                return false;
            }
        }



    }
}
