using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class AttenuatorConfigFile {

        public string fileName = string.Format("{0}Config\\Attenuator-Config.csv", System.AppDomain.CurrentDomain.BaseDirectory);
        public double date_diff = double.MaxValue;

        public AttenuatorConfigFile() {
            if (File.Exists(fileName) == false) return;
            FileInfo fi = new FileInfo(fileName);
            date_diff = (DateTime.Now - fi.LastWriteTime).TotalDays;
        }

        public bool loadFileForMeasure() {
            try {
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                myGlobal.collectionAttenuator.Clear();
                foreach (var line in lines) {
                    if (!line.Contains("ChannelNumber")) {
                        string[] buffer = line.Split(',');
                        AttenuatorInfo ai = new AttenuatorInfo() { Channel = buffer[0].Trim(), Frequency = buffer[1].Trim(), Antenna1 = "", Antenna2 = "", Result = "" };
                        myGlobal.collectionAttenuator.Add(ai);
                    }
                }
                return true;
            } catch { return false; }
        }

        public bool loadFileForShow() {
            try {
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                myGlobal.collectionAttenuator.Clear();
                foreach (var line in lines) {
                    if (!line.Contains("ChannelNumber")) {
                        string[] buffer = line.Split(',');
                        AttenuatorInfo ai = new AttenuatorInfo() { Channel = buffer[0].Trim(), Frequency = buffer[1].Trim(), Antenna1 = buffer[2].Trim(), Antenna2 = buffer[3].Trim(), Result = "Passed" };
                        myGlobal.collectionAttenuator.Add(ai);
                    }
                }
                return true;
            }
            catch { return false; }
        }


        public bool saveDataToFile(string antenna) {
            try {
                if (File.Exists(fileName) == false) return false;
                List<AttenuatorInfo> listtmp = new List<AttenuatorInfo>();
                var lines = File.ReadLines(fileName);
                foreach (var line in lines) {
                    if (!line.Contains("ChannelNumber")) {
                        string[] buffer = line.Split(',');
                        AttenuatorInfo ai = new AttenuatorInfo() { Channel = buffer[0].Trim(), Frequency = buffer[1].Trim(), Antenna1 = buffer[2].Trim(), Antenna2 = buffer[3].Trim() };
                        listtmp.Add(ai);
                    }
                }

                switch (antenna) {
                    case "1": {
                            foreach (var ai in listtmp) {
                                ai.Antenna1 = myGlobal.collectionAttenuator.Where(x => x.Frequency == ai.Frequency).FirstOrDefault().Antenna1;
                            }
                            break;
                        }
                    case "2": {
                            foreach (var ai in listtmp) {
                                ai.Antenna2 = myGlobal.collectionAttenuator.Where(x => x.Frequency == ai.Frequency).FirstOrDefault().Antenna2;
                            }
                            break;
                        }
                }

                //save file
                StreamWriter st = new StreamWriter(fileName);
                string _title = "ChannelNumber,ChannelFreq (Hz),Anten1-Attenuator (dBm),Anten2-Attenuator (dBm)";
                st.WriteLine(_title);
                foreach (var ai in listtmp) {
                    st.WriteLine(ai.ToString());
                }
                st.Dispose();
                return true;
            } catch { return false; }
        }

        public bool loadDataFromFile() {
            try {
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                myGlobal.listAttenuator = new List<AttenuatorInfo>();
                foreach (var line in lines) {
                    if (!line.Contains("ChannelNumber")) {
                        string[] buffer = line.Split(',');
                        AttenuatorInfo ai = new AttenuatorInfo() { Channel = buffer[0].Trim(), Frequency = buffer[1].Trim(), Antenna1 = buffer[2].Trim(), Antenna2 = buffer[3].Trim(), Result = "" };
                        myGlobal.listAttenuator.Add(ai);
                    }
                }
                return true;
            } catch { return false; }
        }

    }
}
