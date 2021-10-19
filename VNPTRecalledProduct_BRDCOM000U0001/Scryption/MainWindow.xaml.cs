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
using System.IO;

namespace Scryption {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_content = (string)b.Content;

            switch (b_content.ToLower()) {
                case "browser...": {
                        var dlg = new System.Windows.Forms.FolderBrowserDialog();
                        dlg.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                            this.txt_Path.Text = dlg.SelectedPath;
                        }
                        break;
                    }
                case "encryption": {
                        if (Directory.Exists(this.txt_Path.Text) == false) {
                            MessageBox.Show("Vui lòng chọn folder file trước.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var fs = Directory.GetFiles(this.txt_Path.Text, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".ini") || s.EndsWith(".txt")).ToArray();
                        if (fs.Length == 0) return;

                        foreach (var f in fs) {
                            string data = File.ReadAllText(f);
                            if (f.EndsWith(".ini") == false) {
                                if (data.Contains("using") == false) {
                                    MessageBox.Show("Success!", "Encryption", MessageBoxButton.OK, MessageBoxImage.Information);
                                    return;
                                }
                            }
                            
                            HEC.Encryption encryption = new HEC.Encryption(f);
                            encryption.saveSource(data);
                        }

                        MessageBox.Show("Success!" , "Encryption", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                        break;
                    }
                case "decryption": {
                        if (Directory.Exists(this.txt_Path.Text) == false) {
                            MessageBox.Show("Vui lòng chọn folder file trước.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        var fs = Directory.GetFiles(this.txt_Path.Text, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".ini") || s.EndsWith(".txt")).ToArray();
                        if (fs.Length == 0) return;

                        foreach (var f in fs) {
                            HEC.Encryption encryption = new HEC.Encryption(f);
                            string data = encryption.readSource();
                            File.WriteAllText(f, data);
                        }

                        MessageBox.Show("Success!", "Decryption", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
            }


        }
    }
}
