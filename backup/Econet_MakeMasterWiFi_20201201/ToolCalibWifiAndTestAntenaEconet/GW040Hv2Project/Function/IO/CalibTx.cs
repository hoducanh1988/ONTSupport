using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {
    public class CalibTx {

        static string fileName = string.Format("{0}Config\\Calib-Config.csv", System.AppDomain.CurrentDomain.BaseDirectory);

        static CalibTx() {
            readFromFile();
        }

        //Load toàn bộ bài calib power từ file vào listCalibPowerWIFI
        public static bool readFromFile() {
            try {
                GlobalData.listCalibPower2G = new List<calibpower>();
                GlobalData.listCalibPower5G = new List<calibpower>();
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                foreach (var line in lines) {
                    if (!line.Contains("CarrierFreq")) {
                        string[] buffer = line.Split(',');
                        calibpower cp = new calibpower() {
                            carrierfreq = buffer[1],
                            anten = buffer[2],
                            channelfreq = buffer[3],
                            register = buffer[4],
                            calibflag = buffer[5],
                            refference = buffer[6]
                        };
                        if (cp.carrierfreq == "2G") GlobalData.listCalibPower2G.Add(cp);
                        else GlobalData.listCalibPower5G.Add(cp);
                    }
                }
                return true;
            }
            catch {
                return false;
            }
        }


        ////Search tên limit RX từ listLimitWifiRX
        //public static bool getData(string _rangefreq, string _wifi, string _mcs, out limitrx _limit) {
        //    _limit = new limitrx();
        //    try {
        //        if (GlobalData.listLimitWifiRX.Count == 0) return false;
        //        foreach (var item in GlobalData.listLimitWifiRX) {
        //            if (item.rangefreq == _rangefreq && item.wifi == _wifi && item.mcs == _mcs) {
        //                _limit = item;
        //                break;
        //            }
        //        }
        //        return true;
        //    }
        //    catch {
        //        return false;
        //    }
        //}



    }
}
