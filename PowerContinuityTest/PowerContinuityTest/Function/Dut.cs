using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerContinuityTest.Function {

    public class Dut {

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
            string s = serial.ReadExisting();
            myGlobal.myTesting.logDut += s;
            myGlobal.myTesting.logBinding += s;
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
