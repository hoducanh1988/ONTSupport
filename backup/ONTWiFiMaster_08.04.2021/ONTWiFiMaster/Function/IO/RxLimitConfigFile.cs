using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class RxLimitConfigFile {

        string fileName = string.Format("{0}Config\\RxLimit-Config.csv", System.AppDomain.CurrentDomain.BaseDirectory);


        public bool loadDataFromFile() {
            try {
                myGlobal.listLimitWifiRX = new List<RxLimitInfo>();
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                foreach (var line in lines) {
                    if (!line.Contains("RangeFrequency")) {
                        string[] buffer = line.Split(',');
                        RxLimitInfo lr = new RxLimitInfo() {
                            rangefreq = buffer[0],
                            wifi = buffer[1],
                            mcs = buffer[2],
                            power_Transmit = buffer[3],
                            PER = buffer[4]
                        };
                        myGlobal.listLimitWifiRX.Add(lr);
                    }
                }
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
