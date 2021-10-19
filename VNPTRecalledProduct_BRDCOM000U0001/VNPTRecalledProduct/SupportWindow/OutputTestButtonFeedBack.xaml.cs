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

namespace VNPTRecalledProduct.SupportWindow {
    /// <summary>
    /// Interaction logic for OutputTestButtonFeedBack.xaml
    /// </summary>
    public partial class OutputTestButtonFeedBack : Window {

        public bool isShowing { get; set; }
        DispatcherTimer timer = null;
        int count = 0;
        public string warningText = "";
        public string legendText = "";

        public OutputTestButtonFeedBack(int timeout) {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) => {
                lbl_warning.Content = this.warningText;
                lbl_legend.Content = this.legendText;
                count++;
                if (count % 2 == 0) {
                    sp_main.Background = Brushes.Yellow;
                    lbl_warning.Foreground = Brushes.Red;
                }
                else {
                    sp_main.Background = Brushes.Red;
                    lbl_warning.Foreground = Brushes.White;
                }

                lbl_time.Content = (timeout - (count / 2)).ToString();
            };
            timer.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            timer.Stop();
            isShowing = false;
        }
    }
}
