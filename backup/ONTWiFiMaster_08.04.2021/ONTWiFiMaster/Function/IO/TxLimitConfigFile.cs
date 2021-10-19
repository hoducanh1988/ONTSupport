using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class TxLimitConfigFile {

        string fileName = string.Format("{0}Config\\TxLimit-Config-NewBIN.csv", System.AppDomain.CurrentDomain.BaseDirectory);

        //Load toàn bộ giá trị tên limit TX từ file vào listLimitWifiTX
        public bool loadDataFromFile() {
            try {
                myGlobal.listLimitWifiTX = new List<TxLimitInfo>();
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                foreach (var line in lines) {
                    if (!line.Contains("RangeFrequency")) {
                        string[] buffer = line.Split(',');
                        TxLimitInfo lt = new TxLimitInfo() {
                            rangefreq = buffer[0],
                            wifi = buffer[1],
                            mcs = buffer[2],
                            power_MAX = buffer[3],
                            power_MIN = buffer[4],
                            evm_MAX = buffer[5],
                            evm_MIN = buffer[6],
                            freqError_MAX = buffer[7],
                            freqError_MIN = buffer[8],
                            symclock_MAX = buffer[9],
                            symclock_MIN = buffer[10]
                        };
                        myGlobal.listLimitWifiTX.Add(lt);
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
