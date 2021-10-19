using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestRSSI {

        public class RSSIChartHelper {

            List<double> list_RSSID = new List<double>();
            int ScaleX = 12, ScaleY = 5, MaxTime = 60, MaxDBM = 70, divX = 5, divY = 5, Standard_Value = -35;
            Canvas cvs_main = null;
            string ssidName = "", antenna = "";
            public string totalResult = "";

            public RSSIChartHelper(Canvas canvas, string ssid_name, List<double> list_rssid, string max_time, string min_dbm, string standard_dbm, string antenna) {
                this.list_RSSID = list_rssid;
                this.cvs_main = canvas;
                this.ssidName = ssid_name;
                this.MaxTime = int.Parse(max_time);
                this.MaxDBM = Math.Abs(int.Parse(min_dbm));
                this.Standard_Value = int.Parse(standard_dbm);
                this.antenna = antenna;

                ScaleX = (60 * 12) / this.MaxTime;
                ScaleY = (70 * 5) / this.MaxDBM;

                if (this.list_RSSID.Count <= MaxTime) {
                    totalResult = "Waiting...";
                }
                else {
                    var items = list_RSSID.Where(x => x < Standard_Value).ToList();
                    if (items != null && items.Count > 0) {
                        totalResult = "Failed";
                    }
                    else totalResult = "Passed";
                }
            }

            public void drawingRSSIChart() {
                this.cvs_main.Children.Clear(); //xoá chart

                drawing_X_Coordinate_Axis(); //vẽ trục toạ độ X
                drawing_X_Sub_Axis(); //vẽ đường phân chia trên trục X
                drawing_Y_Coordinate_Axis(); //vẽ trục toạ độ Y
                drawing_Y_Sub_Axis(); //vẽ đường phân chia trên trục Y
                drawing_Legend();
                drawing_Result();
                drawing_Title();

                //hiển thị nội dung chart
                for (int i = 0; i < list_RSSID.Count; i++) {
                    if (i < list_RSSID.Count - 1) {

                        //vẽ đường RSSID
                        Line line1 = new Line();
                        line1.X1 = i * ScaleX;
                        line1.Y1 = Math.Abs(list_RSSID[i]) * ScaleY;
                        line1.X2 = (i + 1) * ScaleX;
                        line1.Y2 = Math.Abs(list_RSSID[i + 1]) * ScaleY;
                        line1.StrokeThickness = 2;
                        line1.Stroke = Brushes.Blue;

                        //vẽ điểm RSSID
                        Line point1 = new Line();
                        point1.X1 = (i + 1) * ScaleX - 2;
                        point1.Y1 = Math.Abs(list_RSSID[i + 1]) * ScaleY - 2;
                        point1.X2 = (i + 1) * ScaleX + 2;
                        point1.Y2 = Math.Abs(list_RSSID[i + 1]) * ScaleY + 2;
                        point1.Stroke = Brushes.Blue;
                        point1.StrokeThickness = 5;

                        //vẽ đường tiêu chuẩn
                        Line line2 = new Line();
                        line2.X1 = i * ScaleX;
                        line2.Y1 = Math.Abs(Standard_Value) * ScaleY;
                        line2.X2 = (i + 1) * ScaleX;
                        line2.Y2 = Math.Abs(Standard_Value) * ScaleY;
                        line2.StrokeThickness = 2;
                        line2.Stroke = Brushes.Red;

                        //vẽ điểm tiêu chuẩn
                        Line point2 = new Line();
                        point2.X1 = (i + 1) * ScaleX - 2;
                        point2.Y1 = Math.Abs(Standard_Value) * ScaleY - 2;
                        point2.X2 = (i + 1) * ScaleX + 2;
                        point2.Y2 = Math.Abs(Standard_Value) * ScaleY + 2;
                        point2.Stroke = Brushes.Red;
                        point2.StrokeThickness = 5;

                        //vẽ giá trị RSSID
                        Label value = new Label();
                        value.Content = list_RSSID[i + 1].ToString();
                        value.FontWeight = FontWeights.Bold;
                        value.FontSize = 8.5;
                        value.Margin = Math.Abs(Standard_Value) > Math.Abs(list_RSSID[i + 1]) ? new Thickness(point1.X2 - 10, point1.Y2 - 25, 0, 0) : new Thickness(point1.X2 - 10, point1.Y2, 0, 0);

                        //add to canvas
                        this.cvs_main.Children.Add(line1);
                        this.cvs_main.Children.Add(line2);
                        this.cvs_main.Children.Add(point1);
                        this.cvs_main.Children.Add(point2);
                        this.cvs_main.Children.Add(value);
                    }
                }
            }

            private void drawing_X_Coordinate_Axis() {
                Line line = new Line();
                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 0;
                line.Y2 = MaxDBM * ScaleY + 40;
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;
                this.cvs_main.Children.Add(line);
            }

            private void drawing_X_Sub_Axis() {
                int maxx = MaxTime / divX;

                for (int i = 1; i <= maxx; i++) {

                    Line line = new Line();
                    line.X1 = i * divX * ScaleX;
                    line.Y1 = 0;
                    line.X2 = i * divX * ScaleX;
                    line.Y2 = MaxDBM * ScaleY;
                    line.Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#777777");
                    line.StrokeThickness = 0.5;

                    Label value = new Label();
                    value.Content = (i * divX).ToString();
                    value.FontWeight = FontWeights.Normal;
                    value.FontSize = 12;
                    value.Margin = new Thickness(i * divX * ScaleX - 10, -25, 0, 0);

                    Label legend = new Label();
                    legend.Content = "( giây )";
                    legend.FontWeight = FontWeights.Normal;
                    legend.FontSize = 12;
                    legend.Margin = new Thickness(MaxTime * ScaleX + 20, -25, 0, 0);

                    this.cvs_main.Children.Add(line);
                    this.cvs_main.Children.Add(value);
                    this.cvs_main.Children.Add(legend);
                }
            }

            private void drawing_Y_Coordinate_Axis() {
                Line line = new Line();
                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = MaxTime * ScaleX + 50;
                line.Y2 = 0;
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;
                this.cvs_main.Children.Add(line);
            }

            private void drawing_Y_Sub_Axis() {
                int maxy = MaxDBM / divY;

                for (int i = 1; i <= maxy; i++) {
                    Line line = new Line();
                    line.X1 = 0;
                    line.Y1 = i * divY * ScaleY;
                    line.X2 = MaxTime * ScaleX;
                    line.Y2 = i * divY * ScaleY;
                    line.Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#777777");
                    line.StrokeThickness = 0.5;

                    Label value = new Label();
                    value.Content = $"-{(i * divY).ToString()}";
                    value.FontWeight = FontWeights.Normal;
                    value.FontSize = 12;
                    value.Margin = new Thickness(-30, i * divY * ScaleY - 15, 0, 0);

                    Label legend = new Label();
                    legend.Content = "( dBm )";
                    legend.FontWeight = FontWeights.Normal;
                    legend.FontSize = 12;
                    legend.Margin = new Thickness(-30, MaxDBM * ScaleY + 40, 0, 0);

                    this.cvs_main.Children.Add(line);
                    this.cvs_main.Children.Add(value);
                    this.cvs_main.Children.Add(legend);
                }
            }

            private void drawing_Legend() {
                Line line1 = new Line();
                line1.X1 = 100;
                line1.Y1 = MaxDBM * ScaleY + 40;
                line1.X2 = 130;
                line1.Y2 = MaxDBM * ScaleY + 40;
                line1.StrokeThickness = 2;
                line1.Stroke = Brushes.Red;

                Label legend1 = new Label();
                legend1.Content = "Tiêu chuẩn";
                legend1.FontWeight = FontWeights.Normal;
                legend1.FontSize = 12;
                legend1.Margin = new Thickness(140, MaxDBM * ScaleY + 25, 0, 0);

                Line line2 = new Line();
                line2.X1 = 250;
                line2.Y1 = MaxDBM * ScaleY + 40;
                line2.X2 = 280;
                line2.Y2 = MaxDBM * ScaleY + 40;
                line2.StrokeThickness = 2;
                line2.Stroke = Brushes.Blue;

                Label legend2 = new Label();
                legend2.Content = "Thực tế";
                legend2.FontWeight = FontWeights.Normal;
                legend2.FontSize = 12;
                legend2.Margin = new Thickness(290, MaxDBM * ScaleY + 25, 0, 0);

                this.cvs_main.Children.Add(line1);
                this.cvs_main.Children.Add(legend1);
                this.cvs_main.Children.Add(line2);
                this.cvs_main.Children.Add(legend2);
            }

            private void drawing_Result() {

                foreach (var child in this.cvs_main.Children.OfType<System.Windows.Controls.Label>()) {
                    if (child.Name.Equals("label_result")) {
                        this.cvs_main.Children.Remove(child);
                        break;
                    }
                }

                Label result = new Label();
                result.Name = "label_result";
                result.Content = totalResult;
                result.FontWeight = FontWeights.SemiBold;
                result.FontSize = 40;

                switch (totalResult) {
                    case "Waiting...": {
                            result.Foreground = Brushes.Orange;
                            break;
                        }
                    case "Passed": {
                            result.Foreground = Brushes.Lime;
                            break;
                        }
                    case "Failed": {
                            result.Foreground = Brushes.Red;
                            break;
                        }
                }
                result.Margin = new Thickness(500, MaxDBM * ScaleY - 5, 0, 0);
                this.cvs_main.Children.Add(result);
            }

            private void drawing_Title() {
                Label title = new Label();
                title.Content = $"Biểu đồ đo giá trị rssi antenna {this.antenna} - ONT {this.ssidName}".ToUpper();
                title.FontWeight = FontWeights.SemiBold;
                title.FontSize = 25;
                title.Margin = new Thickness(0, -70, 0, 0);

                this.cvs_main.Children.Add(title);
            }

        }

    }
