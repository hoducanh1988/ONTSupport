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
using System.Windows.Shapes;
using System.IO;
using ONTWiFiMaster.Function.Global;
using System.Windows.Threading;

namespace ONTWiFiMaster {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {

        DispatcherTimer timer = null;

        public LoginWindow() {
            InitializeComponent();
            this.Title = myGlobal.myMainWindowDataBinding.appInfo;
            loadProduct();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += (s, e) => {
                bool isEnabled = false;
                foreach (var rb in this.sp_main.Children) {
                    if (rb is RadioButton) {
                        if ((rb as RadioButton).IsChecked == true) {
                            isEnabled = true;
                            break;
                        }
                    }
                }
                this.btn_next.IsEnabled = isEnabled;
            };
            timer.Start();
        }

        private void loadProduct() {
            var d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "BIN");
            var fs = d.GetFiles("*.csv");

            foreach (var f in fs) {
                RadioButton rb = new RadioButton();
                rb.Content = f.Name.Replace(".CSV", "").Replace(".csv", "");
                this.sp_main.Children.Add(rb);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string bTag = b.Tag as string;

            switch (bTag.ToLower()) {
                case "next": {
                        foreach (var rb in this.sp_main.Children) {
                            if (rb is RadioButton) {
                                if ((rb as RadioButton).IsChecked == true) {
                                    myGlobal.myMainWindowDataBinding.productName = (rb as RadioButton).Content.ToString();
                                    MainWindow mw = new MainWindow();
                                    timer.Stop();
                                    mw.Show();
                                    this.Close();
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }



        }
    }
}
