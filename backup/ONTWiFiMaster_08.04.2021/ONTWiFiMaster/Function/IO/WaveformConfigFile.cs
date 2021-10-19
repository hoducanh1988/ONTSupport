using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class WaveformConfigFile {
        string fileName = string.Format("{0}Config\\Waveform-Config.csv", System.AppDomain.CurrentDomain.BaseDirectory);

        public bool loadDataFromFile() {
            try {
                myGlobal.listWaveForm = new List<WaveFormInfo>();
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                foreach (var line in lines) {
                    if (!line.Contains("Instrument")) {
                        string[] buffer = line.Split(',');
                        WaveFormInfo wf = new WaveFormInfo() { instrument = buffer[0].Trim(), wifi = buffer[1].Trim(), mcs = buffer[2].Trim(), bandwidth = buffer[3].Trim(), waveform = buffer[4].Trim() };
                        myGlobal.listWaveForm.Add(wf);
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
