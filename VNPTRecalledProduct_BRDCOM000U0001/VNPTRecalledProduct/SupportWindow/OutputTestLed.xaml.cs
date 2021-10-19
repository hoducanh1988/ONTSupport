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
using System.Windows.Threading;
using VNPTRecalledProduct.SupportUserControl;

namespace VNPTRecalledProduct.SupportWindow {
    /// <summary>
    /// Interaction logic for OutputTestLed.xaml
    /// </summary>
    public partial class OutputTestLed : Window {
        public bool isShowing { get; set; }
        public bool Result { get; set; }

        public OutputTestLed() {
            InitializeComponent();
            isShowing = true;
            Result = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            isShowing = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            bool r = true;
            foreach (UIElement child in mainViewer.Children) {
                if (child is WrapPanel) {
                    foreach (UIElement led in (child as WrapPanel).Children) {
                        if (led is ucSingleLed) {
                            if ((led as ucSingleLed).item.Value == false) {
                                r = false;
                                break;
                            }
                        }
                    }
                }
                if (r == false) break;
            }
            Result = r;
            this.Close();
        }
    }
}
