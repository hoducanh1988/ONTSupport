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

                return serial.IsOpen;
            }
            catch {
                return false;
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            // Show all the incoming data in the port's buffer
            myGlobal.myTesting.logController += serial.ReadExisting();
        }

        public bool IsConnected() {
            if (serial == null) return false;
            try {
                return serial.IsOpen;
            }
            catch {
                return false;
            }
        }

        public string Query(string cmd) {
            this.WriteLine(cmd);
            Thread.Sleep(100);
            return this.Read();
        }

        public string Read() {
            if (serial == null) return null;
            return serial.ReadExisting();
        }

        public bool Write(string cmd) {
            if (serial == null) return false;

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

        public bool Start() {
            serial.WriteLine("OP:1000");
            Thread.Sleep(10);
            return true;
        }

        public bool Stop() {
            serial.WriteLine("OP:0000");
            Thread.Sleep(10);
            return true;
        }

        public bool Close() {
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
