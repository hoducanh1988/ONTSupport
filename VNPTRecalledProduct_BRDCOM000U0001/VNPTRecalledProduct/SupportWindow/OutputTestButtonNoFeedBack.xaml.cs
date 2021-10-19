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
using VNPTRecalledProduct.Function.Custom;

namespace VNPTRecalledProduct.SupportWindow {
    /// <summary>
    /// Interaction logic for OutputTestButtonNoFeedBack.xaml
    /// </summary>
    public partial class OutputTestButtonNoFeedBack : Window {
        public ItemButton item = null;

        public OutputTestButtonNoFeedBack(ItemButton _item) {
            InitializeComponent();
            this.item = _item;
            this.DataContext = this.item;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_content = (b.Content as string).ToLower();

            switch (b_content) {
                case "pass": {
                        this.item.Value = true;
                        break;
                    }
                case "fail": {
                        this.item.Value = false;
                        break;
                    }
            }
            this.Close();
        }
    }
}
