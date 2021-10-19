using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class BinFile {

        string fileName = "";

        public BinFile(string name) {
            fileName = string.Format("{0}BIN\\{1}", System.AppDomain.CurrentDomain.BaseDirectory, name);
        }

        public bool loadDataFromFile() {
            try {
                myGlobal.listBinRegister = new List<Custom.BinRegisterInfo>();
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                foreach (var line in lines) {
                    if (!line.Contains("Register")) {
                        string[] buffer = line.Split(',');
                        BinRegisterInfo br = new BinRegisterInfo() {
                            Address = buffer[0],
                            oldValue = buffer[1],
                            newValue = buffer[2]
                        };
                        myGlobal.listBinRegister.Add(br);
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
