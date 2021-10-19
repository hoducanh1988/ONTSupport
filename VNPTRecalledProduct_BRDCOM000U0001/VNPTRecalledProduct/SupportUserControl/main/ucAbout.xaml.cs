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
using VNPTRecalledProduct.Function.Global;
using VNPTRecalledProduct.Function.IO;
using System.IO;

namespace VNPTRecalledProduct.SupportUserControl {
    /// <summary>
    /// Interaction logic for ucAbout.xaml
    /// </summary>
    public partial class ucAbout : UserControl {

        private class history {
            public string ID { get; set; }
            public string VERSION { get; set; }
            public string CONTENT { get; set; }
            public string DATE { get; set; }
            public string CHANGETYPE { get; set; }
            public string PERSON { get; set; }
        }

        List<history> listHist = new List<history>();

        public ucAbout() {
            InitializeComponent();
            string code = File.ReadAllText(myGlobal.myTesting.fileProduct);
            if (code.Contains("using") == false) {
                Encryption encryption = new Encryption(myGlobal.myTesting.fileProduct);
                code = encryption.readSource();
            } 

            string[] buffer = code.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            //load about
            int start_idx = buffer.ToList().FindIndex(x => x.ToLower().Contains("about={") == true);
            int idx = 0;
            for (int k = start_idx + 1; k < buffer.Length; k++) {
                if (buffer[k].ToLower().Contains("version=")) {
                    idx++;
                    var his = new history();
                    string[] bff = buffer[k].Split(';');
                    his.ID = idx.ToString();
                    his.VERSION = bff[0].Split('=')[1];
                    his.CONTENT = bff[1].Split('=')[1];
                    his.DATE = bff[2].Split('=')[1];
                    his.CHANGETYPE = bff[3].Split('=')[1];
                    his.PERSON = bff[4].Split('=')[1];

                    listHist.Add(his);
                }
                else break;
            }

            this.GridAbout.ItemsSource = listHist;
        }

    }
}
