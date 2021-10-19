using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using VNPTRecalledProduct.Function.Global;

namespace VNPTRecalledProduct.SupportUserControl {
    /// <summary>
    /// Interaction logic for ucLog.xaml
    /// </summary>
    public partial class ucLog : UserControl {

        public ucLog() {
            InitializeComponent();
            cbb_logtype.ItemsSource = new List<string>() { "Log Detail", "Log Total" };
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (cbb_logtype.SelectedValue != null) {
                string value = cbb_logtype.SelectedValue as string;

                switch (value.ToLower()) {
                    case "log detail": {
                            try {
                                string dir = $"{AppDomain.CurrentDomain.BaseDirectory}LogDetail\\{myGlobal.myTesting.productName.Replace(" ", "")}\\{myGlobal.myTesting.Station.Replace(" ", "").Trim() }\\{DateTime.Now.ToString("yyyy-MM-dd")}";
                                Process.Start(dir);
                            }
                            catch (Exception ex) {
                                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            break;
                        }
                    case "log total": {
                            try {
                                string dir = $"{AppDomain.CurrentDomain.BaseDirectory}LogTotal\\{myGlobal.myTesting.productName.Replace(" ", "")}\\{myGlobal.myTesting.Station.Replace(" ", "").Trim()}";
                                Process.Start(dir);
                            }
                            catch (Exception ex) {
                                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            break;
                        }
                }
            }
        }

    }
}
