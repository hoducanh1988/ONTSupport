using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.Function.Custom;

namespace ONTWiFiMaster.Function.IO {
    public class CalibLogFile {

        string dir = $"{AppDomain.CurrentDomain.BaseDirectory}Log\\{myGlobal.myMainWindowDataBinding.productName}\\{DateTime.Now.ToString("yyyy-MM-dd")}";

        public CalibLogFile() {
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
        }

        public bool SaveToFile(CalibDataBinding cdb) {
            string file = $"{dir}\\{cdb.macAddress}_{DateTime.Now.ToString("HHmmss")}_logcalib.txt";
            StreamWriter sw = new StreamWriter(file, true, UnicodeEncoding.Unicode);
            sw.WriteLine(getCalibSetting());
            sw.WriteLine(getLogItem(cdb));
            sw.WriteLine(getLogTable());
            sw.WriteLine(getLogSystem(cdb));
            sw.WriteLine(getLogUart(cdb));
            sw.WriteLine(getLogInstrument(cdb));
            sw.Close();
            return true;
        }

        string getCalibSetting() {
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
            data += $"Target power 2G: {myGlobal.mySetting.target2G} dBm\n";
            data += $"Target power 5G: {myGlobal.mySetting.target5G} dBm\n";
            data += $"Lower limit: {myGlobal.mySetting.lowerLimit} dBm\n";
            data += $"Upper limit: {myGlobal.mySetting.upperLimit} dBm\n";
            data += $"Calib frequency: {myGlobal.mySetting.calibFreq}\n";
            data += $"Calib power 2G: {myGlobal.mySetting.calib2G}\n";
            data += $"Calib power 5G: {myGlobal.mySetting.calib5G}\n";
            data += $"Write BIN: {myGlobal.mySetting.writeBIN}\n";
            data += $"Save flash: {myGlobal.mySetting.saveFlash}\n";
            data += $"Check register: {myGlobal.mySetting.checkRegister}\n";

            return data;
        }


        string getLogItem(CalibDataBinding cdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG ITEM ***\n";
            data += "******************************************\n";
            data += $"Mac address: {cdb.macAddress}\n";
            data += "------------------------------------------\n";
            data += "Calib Frequency".PadLeft(20, ' ') +  $"{cdb.calibFrequencyResult}".PadLeft(10, ' ') + "\n";
            data += "Calib Power 2G".PadLeft(20, ' ') + $"{cdb.calib2GResult}".PadLeft(10, ' ') + "\n";
            data += "Calib Power 5G".PadLeft(20, ' ') + $"{cdb.calib5GResult}".PadLeft(10, ' ') + "\n";
            data += "Write BIN".PadLeft(20, ' ') + $"{cdb.writeBinResult}".PadLeft(10, ' ') + "\n";
            data += "Save Flash".PadLeft(20, ' ') + $"{cdb.saveFlashResult}".PadLeft(10, ' ') + "\n";
            data += "Check Register".PadLeft(20, ' ') + $"{cdb.checkRegisterResult}".PadLeft(10, ' ') + "\n";
            data += "------------------------------------------\n";
            data += $"Total time: {cdb.totalTime}\n";
            data += $"Total result: {cdb.totalResult}\n\n";
            return data;
        }

        string getLogTable() {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG TABLE ***\n";
            data += "******************************************\n";
            data += $"Kết quả calib tần số:\n";
            data += $"{"F4DEC".PadLeft(15,' ')}{"F6DEC_OLD".PadLeft(15, ' ')}{"OFFSET_OLD".PadLeft(15, ' ')}{"FREQERR_OLD".PadLeft(15, ' ')}{"OFFSET_NEW".PadLeft(15, ' ')}{"F6DEC_NEW".PadLeft(15, ' ')}{"FREQERR_NEW".PadLeft(15, ' ')}\n";
            foreach (var item in myGlobal.collectionCalibFreqResult) {
                data += item.ToString() + "\n";
            }
            data += $"\nKết quả calib công suất wifi band 2G:\n";
            data += $"{"RANGE".PadLeft(15, ' ')}{"ANTENNA".PadLeft(15, ' ')}{"CHANNEL".PadLeft(15, ' ')}{"ATTENUATOR".PadLeft(15, ' ')}{"ADDRESS".PadLeft(15, ' ')}{"CURRENT".PadLeft(15, ' ')}{"POWER1".PadLeft(15, ' ')}{"POWER2".PadLeft(15, ' ')}{"POWER3".PadLeft(15, ' ')}{"DIFFERENCE".PadLeft(15, ' ')}{"NEW".PadLeft(15, ' ')}{"RESULT".PadLeft(15, ' ')}\n";
            foreach (var item in myGlobal.collectionCalibResult2G) {
                data += item.ToString() + "\n";
            }
            data += $"\nKết quả calib công suất wifi band 5G:\n";
            data += $"{"RANGE".PadLeft(15, ' ')}{"ANTENNA".PadLeft(15, ' ')}{"CHANNEL".PadLeft(15, ' ')}{"ATTENUATOR".PadLeft(15, ' ')}{"ADDRESS".PadLeft(15, ' ')}{"CURRENT".PadLeft(15, ' ')}{"POWER1".PadLeft(15, ' ')}{"POWER2".PadLeft(15, ' ')}{"POWER3".PadLeft(15, ' ')}{"DIFFERENCE".PadLeft(15, ' ')}{"NEW".PadLeft(15, ' ')}{"RESULT".PadLeft(15, ' ')}\n";
            foreach (var item in myGlobal.collectionCalibResult5G) {
                data += item.ToString() + "\n";
            }

            data += "\n\n";
            return data;
        }

        string getLogSystem(CalibDataBinding cdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG SYSTEM ***\n";
            data += "******************************************\n";
            data += cdb.logSystem;
            data += "\n\n";
            return data;
        }

        string getLogUart(CalibDataBinding cdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG UART ***\n";
            data += "******************************************\n";
            data += cdb.logUart;
            data += "\n\n";
            return data;
        }

        string getLogInstrument(CalibDataBinding cdb) {
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
