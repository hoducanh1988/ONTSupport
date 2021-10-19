using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class CalibConfigFile {
        string fileName = string.Format("{0}Config\\Calib-Config.csv", System.AppDomain.CurrentDomain.BaseDirectory);

        public bool loadDataFromFile() {
            try {
                myGlobal.listCalibPower2G = new List<CalibPowerConfigInfo>();
                myGlobal.listCalibPower5G = new List<CalibPowerConfigInfo>();
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                foreach (var line in lines) {
                    if (!line.Contains("CarrierFreq")) {
                        string[] buffer = line.Split(',');
                        CalibPowerConfigInfo cp = new CalibPowerConfigInfo() {
                            carrierfreq = buffer[1],
                            anten = buffer[2],
                            channelfreq = buffer[3],
                            register = buffer[4],
                            calibflag = buffer[5],
                            refference = buffer[6]
                        };
                        if (cp.carrierfreq == "2G") myGlobal.listCalibPower2G.Add(cp);
                        else myGlobal.listCalibPower5G.Add(cp);
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
