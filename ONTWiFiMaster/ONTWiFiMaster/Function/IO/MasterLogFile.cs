using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class MasterLogFile {

        string dir = $"{AppDomain.CurrentDomain.BaseDirectory}Log\\{myGlobal.myMainWindowDataBinding.productName}\\{DateTime.Now.ToString("yyyy-MM-dd")}";

        public MasterLogFile() {
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
        }

        public bool SaveToFile(MasterDataBinding mdb) {
            string file = $"{dir}\\{mdb.macAddress}_{DateTime.Now.ToString("HHmmss")}_logmaster.txt";
            StreamWriter sw = new StreamWriter(file, true, UnicodeEncoding.Unicode);
            sw.WriteLine(getMasterSetting());
            sw.WriteLine(getLogItem(mdb));
            sw.WriteLine(getLogTable());
            sw.WriteLine(getLogSystem(mdb));
            sw.WriteLine(getLogUart(mdb));
            sw.WriteLine(getLogInstrument(mdb));
            sw.Close();
            return true;
        }

        string getMasterSetting() {
            string data = "";
            data += $"{myGlobal.myMainWindowDataBinding.appInfo}\n";
            data += $"ONT type: {myGlobal.myMainWindowDataBinding.productName}\n";
            data += $"Serial port: {myGlobal.mySetting.serialPortName}\n";
            data += $"Login user: {myGlobal.mySetting.loginUser}\n";
            data += $"Login password: {myGlobal.mySetting.loginPassword}\n";
            data += $"Instrument type: {myGlobal.mySetting.instrumentType}\n";
            data += $"GPIB address: {myGlobal.mySetting.gpibAddress}\n";
            data += $"Antenna1: {myGlobal.mySetting.Port1}\n";
            data += $"Antenna2: {myGlobal.mySetting.Port2}\n";
            data += $"Số lần đo: {myGlobal.mySetting.measureTime}\n";
            data += $"Độ lệch giữa các lần đo: {myGlobal.mySetting.Difference}\n";
            return data;
        }


        string getLogItem(MasterDataBinding mdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG ITEM ***\n";
            data += "******************************************\n";
            data += $"Mac address: {mdb.macAddress}\n";
            data += "------------------------------------------\n";
            data += "------------------------------------------\n";
            data += $"Total time: {mdb.totalTime}\n";
            data += $"Total result: {mdb.totalResult}\n\n";
            return data;
        }

        string getLogTable() {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG TABLE ***\n";
            data += "******************************************\n";
            data += $"{"FREQ".PadLeft(15, ' ')}{"ANTENNA".PadLeft(15, ' ')}{"ATTENUATOR".PadLeft(15, ' ')}{"POWER1".PadLeft(15, ' ')}{"POWER2".PadLeft(15, ' ')}{"POWER3".PadLeft(15, ' ')}{"POWER4".PadLeft(15, ' ')}{"POWER5".PadLeft(15, ' ')}{"MASTER".PadLeft(15, ' ')}{"MAX".PadLeft(15, ' ')}{"MIN".PadLeft(15, ' ')}{"DIFFERENCE".PadLeft(15, ' ')}{"RESULT".PadLeft(15, ' ')}\n";
            foreach (var item in myGlobal.collectionMaster) {
                data += item.ToString() + "\n";
            }
            data += "\n\n";
            return data;
        }

        string getLogSystem(MasterDataBinding mdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG SYSTEM ***\n";
            data += "******************************************\n";
            data += mdb.logSystem;
            data += "\n\n";
            return data;
        }

        string getLogUart(MasterDataBinding mdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG UART ***\n";
            data += "******************************************\n";
            data += mdb.logUart;
            data += "\n\n";
            return data;
        }

        string getLogInstrument(MasterDataBinding mdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG INSTRUMENT ***\n";
            data += "******************************************\n";
            data += mdb.logInstrument;
            data += "\n\n";
            return data;
        }

    }
}
