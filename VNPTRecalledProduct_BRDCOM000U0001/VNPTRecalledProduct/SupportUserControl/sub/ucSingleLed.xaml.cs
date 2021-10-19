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
using VNPTRecalledProduct.Function.Custom;

namespace VNPTRecalledProduct.SupportUserControl {
    /// <summary>
    /// Interaction logic for ucSingleLed.xaml
    /// </summary>
    public partial class ucSingleLed : UserControl {

        public ItemLed item = null;

        public ucSingleLed(ItemLed _item) {
            InitializeComponent();
            this.item = _item;
            this.DataContext = this.item;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                this.item.Value = !this.item.Value;
            }
        }

    }
}
