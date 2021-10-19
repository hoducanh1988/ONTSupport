using BosaManagementSystem.IFunction.DataBase;
using BosaManagementSystem.IFunction.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace BosaManagementSystem {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            myGlobal.myExportInfo.splitValue = int.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "splitValue.ini"));
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            Label lb = sender as Label;
            string lb_tag = (string)lb.Tag;

            switch (lb_tag) {
                case "export": {
                        Thread t = new Thread(new ThreadStart(() => {
                            IDatabase db = null;
                            switch (myGlobal.mydbInfo.dbServerType) {
                                case "PostgreSQL": {
                                        db = new PostgreSQL();
                                        break;
                                    }
                            }
                            var dt = db.selectAllBosaFileFromTableBosaSerialNumber();
                            myParameter.listBosaFile = new List<string>();
                            if (dt != null && dt.Rows.Count > 0) {
                                for (int i = 0; i < dt.Rows.Count; i++) {
                                    string bsf = dt.Rows[i].ItemArray[0].ToString().Trim();
                                    myParameter.listBosaFile.Add(bsf);
                                }
                            }
                            myGlobal.isUpdateListBosaFile = true;
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
            }


        }
    }
}
