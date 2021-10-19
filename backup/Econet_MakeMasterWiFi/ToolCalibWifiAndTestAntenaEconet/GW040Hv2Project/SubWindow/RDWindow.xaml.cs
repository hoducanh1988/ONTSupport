using GW040Hv2Project.Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GW040Hv2Project.SubWindow
{
    /// <summary>
    /// Interaction logic for RDWindow.xaml
    /// </summary>
    public partial class RDWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public bool RDMode
        {
            get { return Properties.Settings.Default.RDMODE; }
            set
            {
                Properties.Settings.Default.RDMODE = value;
                Properties.Settings.Default.Save();
                GlobalData.testingData.RDContent = value ? "MODE Chỉ Dành Cho Nghiên Cứu Phát Triển" : "";
               OnPropertyChanged(nameof(RDMode));
            }
        }
        string buttonContent;
        public string ButtonContent
        {
            get { return buttonContent; }
            set
            {
                buttonContent = value;
                OnPropertyChanged(nameof(ButtonContent));
            }
        }
        public RDWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            txtStatus.Text = RDMode ? "***Hiện Tại: RD Mode" : "***Hiện Tại: Production Mode";
        }
        public static bool GetRDMode()
        {
            return new RDWindow().RDMode;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            HandleEvent();
        }
        void HandleEvent()
        {
            if (txtPass.Password == "TTCNRD2020")
            {
                RDMode = !RDMode;
                if (RDMode) txtStatus.Text = "Đã Chuyển Thành RD Mode";
                else { txtStatus.Text = "Đã Chuyển Thành Production Mode"; }
                this.Close();
            }
            else
            {
                txtStatus.Text = "Không Đúng PassWord";
            }
        }
        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleEvent();
            }
        }
    }
}
