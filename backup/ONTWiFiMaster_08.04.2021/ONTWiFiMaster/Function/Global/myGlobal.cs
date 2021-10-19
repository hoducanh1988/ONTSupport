using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ONTWiFiMaster.Function.Custom;

namespace ONTWiFiMaster.Function.Global {

    public class myGlobal {
        public static string settingFileFullName = string.Format("{0}setting.xml", AppDomain.CurrentDomain.BaseDirectory);
        public static string helpFileFullName = string.Format("{0}help.xps", AppDomain.CurrentDomain.BaseDirectory);
        public static MainWindowDataBinding myMainWindowDataBinding = new MainWindowDataBinding();
        public static SettingDataBinding mySetting = null;
        public static TestingDataBinding myTesting = null;
        public static CableDataBinding myCable = null;
        public static CalibDataBinding myCalib = null;
        public static VerifyDataBinding myVerify = null;
        public static MasterDataBinding myMaster = null;

        public static ObservableCollection<AttenuatorInfo> collectionAttenuator = new ObservableCollection<AttenuatorInfo>();
        public static ObservableCollection<CalibPowerResultInfo> collectionCalibResult2G = new ObservableCollection<CalibPowerResultInfo>();
        public static ObservableCollection<CalibPowerResultInfo> collectionCalibResult5G = new ObservableCollection<CalibPowerResultInfo>();
        public static ObservableCollection<CalibFrequencyResultInfo> collectionCalibFreqResult = new ObservableCollection<CalibFrequencyResultInfo>();
        public static ObservableCollection<PerResultInfo> collectionPerResult2G = new ObservableCollection<PerResultInfo>();
        public static ObservableCollection<PerResultInfo> collectionPerResult5G = new ObservableCollection<PerResultInfo>();
        public static ObservableCollection<SignalResultInfo> collectionSignalResult2G = new ObservableCollection<SignalResultInfo>();
        public static ObservableCollection<SignalResultInfo> collectionSignalResult5G = new ObservableCollection<SignalResultInfo>();
        public static ObservableCollection<MasterResultInfo> collectionMaster = new ObservableCollection<MasterResultInfo>();

        public static List<AttenuatorInfo> listAttenuator = null;
        public static List<ChannelInfo> listChannel = null;
        public static List<WaveFormInfo> listWaveForm = null;
        public static List<RxLimitInfo> listLimitWifiRX = null;
        public static List<TxLimitInfo> listLimitWifiTX = null;

        public static List<BinRegisterInfo> listBinRegister = null;
        public static List<CalibPowerConfigInfo> listCalibPower2G = null;
        public static List<CalibPowerConfigInfo> listCalibPower5G = null;
        public static List<SensitivityConfigInfo> listSensivitity2G = null;
        public static List<SensitivityConfigInfo> listSensivitity5G = null;
        public static List<SignalConfigInfo> listVerifySignal2G = null;
        public static List<SignalConfigInfo> listVerifySignal5G = null;
        public static List<SignalConfigInfo> listCalMaster = null;

        public static List<SignalConfigInfo> tmplisttxWifi2G = null;
        public static List<SignalConfigInfo> tmplisttxWifi5G = null;
        public static List<SensitivityConfigInfo> tmplistrxWifi2G = null;
        public static List<SensitivityConfigInfo> tmplistrxWifi5G = null;

        public static RegisterInfo logRegister = null;

        public static List<ItemMasterResult> listItemMaster = new List<ItemMasterResult>();

    }
}
