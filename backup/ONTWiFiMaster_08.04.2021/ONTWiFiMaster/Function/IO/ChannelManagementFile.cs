using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {

    public class ChannelManagementFile {
        string fileName = string.Format("{0}Config\\ChannelManagement.csv", System.AppDomain.CurrentDomain.BaseDirectory);

        public bool loadDataFromFile() {
            try {
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                myGlobal.listChannel = new List<Custom.ChannelInfo>();
                foreach (var line in lines) {
                    if (!line.Contains("Channel")) {
                        string[] buffer = line.Split(',');
                        ChannelInfo ci = new ChannelInfo() { WiFi = buffer[0].Trim(), Channel = buffer[1].Trim(), Frequency = buffer[2].Trim() };
                        myGlobal.listChannel.Add(ci);
                    }
                }
                return true;
            } catch { return false; }
        }

    }
}
