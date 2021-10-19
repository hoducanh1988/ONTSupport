using System;
using System.Collections.Generic;
using System.IO;
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
using VNPTRecalledProduct.Function.Global;

namespace VNPTRecalledProduct {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {

        public LoginWindow() {
            InitializeComponent();
            this.DataContext = myGlobal.myTesting;
            
            //add product to login form
            this.wp_product.Children.Clear();
            DirectoryInfo dr = new DirectoryInfo("Script");
            FileInfo[] fi = dr.GetFiles("*.txt");
            foreach (var f in fi) {
                RadioButton rb = new RadioButton();
                rb.Content = f.Name.ToUpper().Replace(".TXT", "").Trim();
                this.wp_product.Children.Add(rb);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            bool isChecked = false;
            string rb_content = "";

            foreach (RadioButton rb in FindVisualChildren<RadioButton>(this.wp_product)) {
                if (rb.IsChecked == true) {
                    isChecked = true;
                    rb_content = rb.Content as string;
                    break;
                }
            }
            if (isChecked == false) {
                MessageBox.Show("Bạn chưa chọn sản phẩm cần kiểm tra.", "Lỗi login", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            myGlobal.myTesting.fileProduct = $"Script\\{rb_content}.txt";
            myGlobal.myTesting.fileSetting = $"Setting\\{rb_content}.ini"; 
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject {
            if (depObj != null) {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T) {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child)) {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
