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
using ONTWiFiMaster.Function.Custom;

namespace ONTWiFiMaster.uCtrl.Sub {
    /// <summary>
    /// Interaction logic for ucItemMaster.xaml
    /// </summary>
    public partial class ucItemMaster : UserControl {

        ItemMasterResult imr = null;

        public ucItemMaster(ItemMasterResult _imr) {
            InitializeComponent();
            this.imr = _imr;
            this.DataContext = this.imr;
        }
    }
}
