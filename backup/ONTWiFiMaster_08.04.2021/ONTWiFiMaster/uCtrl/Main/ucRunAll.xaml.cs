using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.uCtrl.Sub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ONTWiFiMaster.uCtrl {
    /// <summary>
    /// Interaction logic for ucRunAll.xaml
    /// </summary>
    public partial class ucRunAll : UserControl {

        ucRF uc_RF = new ucRF();
        ucCalib uc_Calib = new ucCalib();
        ucVerify uc_Verify = new ucVerify();
        ucMaster uc_Master = new ucMaster();

        public ucRunAll() {
            InitializeComponent();
            this.DataContext = myGlobal.myTesting;
        }

        private void selectControl(UserControl uc) {
            this.gd_Main.Children.Clear();
            this.gd_Main.Children.Add(uc);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            Border bd = sender as Border;
            string bdTage = bd.Tag as string;

            switch (bdTage.ToLower()) {
                case "cable": { myGlobal.myTesting.showIndex = 1; selectControl(uc_RF); break; }
                case "calib": { myGlobal.myTesting.showIndex = 2; selectControl(uc_Calib); break; }
                case "verify": { myGlobal.myTesting.showIndex = 3; selectControl(uc_Verify); break; }
                case "master": { myGlobal.myTesting.showIndex = 4; selectControl(uc_Master); break; }
            }
        }

    }
}
