using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class RFLogFile {

        string dir = $"{AppDomain.CurrentDomain.BaseDirectory}Log\\{myGlobal.myMainWindowDataBinding.productName}\\RF";

        public RFLogFile() {
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
        }

        public bool SaveToFile(CableDataBinding cdb, string antenna) {
            string file = $"{dir}\\Antenna{antenna}_{DateTime.Now.ToString("yyyyMMdd")}_{DateTime.Now.ToString("HHmmss")}_logRF.txt";
            StreamWriter sw = new StreamWriter(file, true, UnicodeEncoding.Unicode);
            sw.WriteLine(getRFSetting());
            sw.WriteLine(getLogTable());
            sw.WriteLine(getLogSystem(cdb));
            sw.WriteLine(getLogInstrument(cdb));
            sw.Close();
            return true;
        }

        string getRFSetting() {
            string data = "";
            data += $"{myGlobal.myMainWindowDataBinding.appInfo}\n";
            data += $"Instrument type: {myGlobal.mySetting.instrumentType}\n";
            data += $"GPIB address: {myGlobal.mySetting.gpibAddress}\n";
            data += $"Antenna1\n";
            data += $"Port phát: {myGlobal.mySetting.portTransmitter1}\n";
            data += $"Port thu: {myGlobal.mySetting.portReceiver1}\n";
            data += $"Công suất phát: {myGlobal.mySetting.powerTransmit1} dBm\n";
            data += $"Suy hao connection: {myGlobal.mySetting.Connector1} dBm\n";
            data += $"Antenna2\n";
            data += $"Port phát: {myGlobal.mySetting.portTransmitter2}\n";
            data += $"Port thu: {myGlobal.mySetting.portReceiver2}\n";
            data += $"Công suất phát: {myGlobal.mySetting.powerTransmit2} dBm\n";
            data += $"Suy hao connection: {myGlobal.mySetting.Connector2} dBm\n";
            return data;
        }

        string getLogTable() {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG TABLE ***\n";
            data += "******************************************\n";
            data += $"{"CHANNEL".PadLeft(15, ' ')}{"FREQ".PadLeft(15, ' ')}{"ANTENNA1".PadLeft(15, ' ')}{"ANTENNA2".PadLeft(15, ' ')}{"RESULT".PadLeft(15, ' ')}\n";
            foreach (var item in myGlobal.collectionAttenuator) {
                data += item.getLogTable() + "\n";
            }
            data += "\n\n";
            return data;
        }

        string getLogSystem(CableDataBinding cdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG SYSTEM ***\n";
            data += "******************************************\n";
            data += cdb.logSystem;
            data += "\n\n";
            return data;
        }

        string getLogInstrument(CableDataBinding cdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG INSTRUMENT ***\n";
            data += "******************************************\n";
            data += cdb.logInstrument;
            data += "\n\n";
            return data;
        }
    }
}
