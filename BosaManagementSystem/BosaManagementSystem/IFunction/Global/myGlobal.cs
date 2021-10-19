using BosaManagementSystem.IFunction.Custom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BosaManagementSystem.IFunction.Global {

    public class myGlobal {
        public static databaseInfo mydbInfo = new databaseInfo();
        public static string dbInfoFile = string.Format("{0}dbInfo.xml", AppDomain.CurrentDomain.BaseDirectory);
        public static string settingFile = string.Format("{0}Setting.xml", AppDomain.CurrentDomain.BaseDirectory);

        public static ObservableCollection<bosaFileInfo> datagridBosaFile = new ObservableCollection<bosaFileInfo>();
        public static importBosaInfo myImportInfo = new importBosaInfo();
        public static exportBosaInfo myExportInfo = new exportBosaInfo();
        public static bool isUpdateListBosaFile = false;
    }
}
