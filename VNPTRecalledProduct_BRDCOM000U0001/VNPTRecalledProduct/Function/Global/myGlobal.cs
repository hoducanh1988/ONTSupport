﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPTRecalledProduct.Function.Custom;

namespace VNPTRecalledProduct.Function.Global {
    public class myGlobal {

        public static TestingInformation myTesting = new TestingInformation();
        public static ObservableCollection<SettingItemInfo> mySetting = new ObservableCollection<SettingItemInfo>();
        public static ObservableCollection<ResultInfo> datagridResult = new ObservableCollection<ResultInfo>();
        public static List<ItemInputBarcode> listBarcode = new List<ItemInputBarcode>();
        public static AuthorizationInfo myAuthorization = new AuthorizationInfo();

    }
}
