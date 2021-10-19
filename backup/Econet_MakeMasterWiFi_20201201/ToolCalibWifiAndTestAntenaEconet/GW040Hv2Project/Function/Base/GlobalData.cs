using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {
    public static class GlobalData {

        static GlobalData() {
            LimitTx.readFromFile();
            LimitRx.readFromFile();
            Attenuator.readFromFile();
            WaveForm.readFromFile();
            ChannelManagement.readFromFile();
            BIN.readFromFile();
            TestCase.Load();
        }

        public static int mtIndex = 0;
        public static bool mtIsOk = true;

        public static ModemTelnet MODEM = null;
        public static Instrument INSTRUMENT = null;

        public static List<waveformInfo> listWaveForm = null;
        public static List<attenuatorInfo> listAttenuator = null;
        public static List<limittx> listLimitWifiTX = null;
        public static List<limitrx> listLimitWifiRX = null;
        public static List<channelmanagement> listChannel = null;
        public static List<masterinformation> listMasterData = null;
        public static List<binregister> ListBinRegister = new List<binregister>();

        public static logdata logManager = null;
        public static logregister logRegister = null;
        public static defaultSetting initSetting = new defaultSetting();
        public static testinginfo testingData = new testinginfo();
        public static resetRegisterBinding rrBinding = new resetRegisterBinding();

        public static ObservableCollection<logreviewregister> reviewRegister = new ObservableCollection<logreviewregister>();
        public static ObservableCollection<logreviewtx> reviewTX = new ObservableCollection<logreviewtx>();
        public static ObservableCollection<logreviewrx> reviewRX = new ObservableCollection<logreviewrx>();
        public static ObservableCollection<logreviewtx> datagridlogTX = new ObservableCollection<logreviewtx>();
        public static ObservableCollection<logreviewrx> datagridlogRX = new ObservableCollection<logreviewrx>();

        public static ObservableCollection<autoattenuator> autoAttenuator = new ObservableCollection<autoattenuator>();
        public static ObservableCollection<calmaster> autoCalculateMaster = new ObservableCollection<calmaster>();

        public static List<verifysignal> tmplisttxWifi2G = null;
        public static List<verifysignal> tmplisttxWifi5G = null;
        public static List<sensivitity> tmplistrxWifi2G = null;
        public static List<sensivitity> tmplistrxWifi5G = null;
        public static List<verifysignal> tmplisttestAnten1 = null;
        public static List<verifysignal> tmplisttestAnten2 = null;
        public static List<verifysignal> tmplistCalAttenuator = null;

        public static List<verifysignal> listTestAnten1 = null;
        public static List<verifysignal> listTestAnten2 = null;
        public static List<verifysignal> listVerifySignal2G = null;
        public static List<verifysignal> listVerifySignal5G = null;
        public static List<sensivitity> listSensivitity2G = null;
        public static List<sensivitity> listSensivitity5G = null;
        public static List<verifysignal> listCalAttenuator = null;
        public static List<verifysignal> listCalMaster = null;

        //Cau hinh bai test Calib Power TX
        public static List<calibpower> listCalibPower2G = null;
        public static List<calibpower> listCalibPower5G = null;
        // Bài Test Wifi 
        public static LstWifiTestingInfor lstWifiTestingInfor = new LstWifiTestingInfor();
        public static string version = "Version: GW040H000U0001 - Buid Date: 1.12.2020 ";

        public static Dictionary<string, string> dictLoginInfo = null;

    }

}
