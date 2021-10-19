using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ONTWiFiMaster.Function.IO {
    public class MasterFile {

        public bool SaveToFile(string mac_address, out string filename) {
            filename = "";
            try {
                if (myGlobal.collectionMaster == null || myGlobal.collectionMaster.Count == 0) return true;
                string dir = $"{AppDomain.CurrentDomain.BaseDirectory}tmpMaster";
                if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
                Thread.Sleep(100);
                filename = string.Format("{0}\\{3}_Master_{4}_{1}_{2}.csv", dir, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"), mac_address.Replace(":", "").Replace("-", "").Trim(), myGlobal.myMainWindowDataBinding.productName);
                StreamWriter st = new StreamWriter(filename);
                string _title = "Channel,Freq,Anten1,Anten2";
                st.WriteLine(_title);

                //1,2412,1,9.45
                //1.2412,2,8.54
                //tach list ban dau ra lam 2 list at1 va list at2
                List<AttenuatorInfo> _li1 = new List<AttenuatorInfo>();
                List<AttenuatorInfo> _li2 = new List<AttenuatorInfo>();
                foreach (var item in myGlobal.collectionMaster) {
                    if (item.Antenna == "1") {
                        AttenuatorInfo _mi = new AttenuatorInfo() { Channel = myGlobal.listAttenuator.Where(x => x.Frequency.Equals(item.Frequency)).FirstOrDefault().Channel, Frequency = item.Frequency, Antenna1 = item.Master };
                        _li1.Add(_mi);
                    }
                    else {
                        AttenuatorInfo _mi = new AttenuatorInfo() { Channel = myGlobal.listAttenuator.Where(x => x.Frequency.Equals(item.Frequency)).FirstOrDefault().Channel, Frequency = item.Frequency, Antenna2 = item.Master };
                        _li2.Add(_mi);
                    }
                }

                //add du lieu vao list attenuator
                List<AttenuatorInfo> _liMaster = new List<AttenuatorInfo>();
                for (int i = 0; i < _li1.Count; i++) {
                    AttenuatorInfo mf = new AttenuatorInfo() { Channel = _li1[i].Channel, Frequency = _li1[i].Frequency, Antenna1 = _li1[i].Antenna1, Antenna2 = _li2[i].Antenna2 };
                    _liMaster.Add(mf);
                }

                //ghi du lieu vao file
                foreach (var item in _liMaster) {
                    st.WriteLine(item.ToString());
                }

                st.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

    }
}
