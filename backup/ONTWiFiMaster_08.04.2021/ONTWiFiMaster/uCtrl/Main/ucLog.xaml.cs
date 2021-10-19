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
using System.Diagnostics;
using ONTWiFiMaster.Function.Global;

namespace ONTWiFiMaster.uCtrl {
    /// <summary>
    /// Interaction logic for ucLog.xaml
    /// </summary>
    public partial class ucLog : UserControl {
        public ucLog() {
            InitializeComponent();
            this.cbb_datatype.ItemsSource = new List<string>() { "LOG", "CONFIG", "BIN", "TESTCASE", "MASTER" };
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            try {
                if (cbb_datatype.SelectedValue == null) return;
                string data_type = cbb_datatype.SelectedValue.ToString();
                switch (data_type.ToUpper()) {
                    case "LOG": { Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}Log\\{myGlobal.myMainWindowDataBinding.productName}\\{DateTime.Now.ToString("yyyy-MM-dd")}"); break; }
                    case "CONFIG": { Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}Config"); break; }
                    case "BIN": { Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}BIN"); break; }
                    case "TESTCASE": { Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}TestCase"); break; }
                    case "MASTER": { Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}tmpMaster"); break; }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
