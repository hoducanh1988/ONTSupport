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

namespace GW040Hv2Project {
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window {
        public About() {
            InitializeComponent();
            listHist.Add(new history() { ID = "1", VERSION = "GW040H000U0001", CONTENT = "- Phát hành lần đầu", DATE = "01/12/2020", CHANGETYPE = "Tạo mới", PERSON = "Hồ Đức Anh" });
            this.GridAbout.ItemsSource = listHist;
        }
        List<history> listHist = new List<history>();
        private class history {
            public string ID { get; set; }
            public string VERSION { get; set; }
            public string CONTENT { get; set; }
            public string DATE { get; set; }
            public string CHANGETYPE { get; set; }
            public string PERSON { get; set; }
        }

    }
}
