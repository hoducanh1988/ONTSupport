   M S B A M L   `   `   `  ����K  GEconetMasterWiFi, Version=2.0.0.1, Culture=neutral, PublicKeyToken=null1clr-namespace:GW040Hv2ProjectGW040Hv2Project  5   	   "    GW040Hv2Project.masterWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:GW040Hv2Project     5   	     � Title$  masterWindow��5   	   $	��700��6   $
��1000��6+    � WindowStartupLocation$ CenterScreen=�68      customWindow#��[  6]   .��5
      �� ���#BFCDDB �� CornerRadius$ 2��6"     colorBorder#��[ 63   $��1,1,1,1q�6^   ._�5   
   �  � RowDefinitions 5      �� 5      $��30 �6    �� 5      $��* �6    .+�5      � $��0��6   ���#FFFFFF.+�5      �� $��Center=�6   $
��Left=�63   
���   �$��12(�6a   $!��Thiết lập mạch master��6o   �� 5      -   5        $��50��5   4   $��Right=�6?   $��Center=�6b   $��Right=�6}   
���   �$��17(�6�   $��0,-5,5,0q�6�   $��X��6�   5      � 5      $��1��6    5      �� 5      $��70 �6$   �� 5      $��90 �6$   �� 5      $��* �6$   �� 5      $��3 �6$   �� 5      $	��100 �6$   .+�5      �� $��0��6   $��5q�6*   .+�5       �� $��13(�6   $��SemiBold"�6*   $3��-Vui lòng chọn file suy hao của dây đo:��6@   � 5!      .+�5"      �� -   5        $��
Browser...��5"   !   $��80��6K   $��30��6V   $��0,0,10,0q�6b   �� 5#      $��Center=�6"   $��30��6D   $��13(�6P   $	��898��6^    �� 
IsReadOnly . ��6j   ��  �� Path$ FILENAME� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�6�   5$      5%      �� 5'      -   5        ���#F4A460$��1��5'   .   $��5,0,5,15q�6;   $��Calculate Master��6b   $��35(�6}   $��SemiBold"�6�   &   System.Windows.Controls.DataGrid  5)      -   5          Name$ dgCalMaster��5)   9   $��2��6   $��5,0,5,5q�6(   
�������	   AutoGenerateColumns	 .  
   CanUserAddRows
 .     CanUserDeleteRows .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .     CanUserReorderColumns .  "   VerticalScrollBarVisibility$
 Auto=�5,   �   $   HorizontalScrollBarVisibility$
 Auto=�6�      GridLinesVisibility$
 None=�5-   /      HeadersVisibility$ Column=�5.   /   $��0,0,0,0q�5/   /      SelectionUnit$ FullRow=�50   /      SelectionMode$ Extended=�6G   ��5+   /   ��    	CellStyle 53      �� 54       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    .�55      ��  �� Property � �� �� Value$ 0,0,1,1q�6@   �� 56       � �� �	#FFB3B3B36<   �� 57       � ��$ 15(�69   �� 58       � ��$ Normal"�6;   �� 59       � ��$ 25��67   �� 5:       � ��$ Center=�6I   5;         ColumnHeaderStyle 5>      �� 5?      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�5@      ��  � ��$ 13(�69   �� 5A       � ��$ 40��67   �� 5B       � ��$ Normal"�6;   �� 5C       � �� �	#FFB3B3B36<   �� 5D       � ��$ 0,0,0,1q�6@   �� 5E       � ��$ 3,0,0,0q�68   5F         RowStyle 5I      �� 5J      )  #System.Windows.Controls.DataGridRow"  6    .�5K      ��  � ��
 �����6;    �� Triggers 5L      �� 5M   "    �� Binding 6.   �� $ Result� �� Value
 Failed6N   .a�5N   &   ��  � ��
 � ���6C   5O   #   5Q         Columns 5T      0  *System.Windows.Controls.DataGridTextColumn  5U      ,  &System.Windows.Controls.DataGridColumn    Header$  Channel��6-   !   Width5  /System.Windows.Controls.DataGridLengthConverter$! * 6>   1  +System.Windows.Controls.DataGridBoundColumn"   Binding" 6H   �� *	Channel+  5V      $  	Frequency��6-   $! * 6@   " 6J   �� *	Frequency+  5W      $  Anten��6-   $! * 6<   " 6F   �� *Anten+  5X      $  	WirePower��6-   $! * 6@   " 6J   �� *	wirePower+  5Y      $  Value1��6-   $
! 0.7* 6=   " 6J   �� *Value1+  5Z      $  Value2��6-   $
! 0.7* 6=   " 6J   �� *Value2+  5[      $  Value3��6-   $
! 0.7* 6=   " 6J   �� *Value3+  5\      $  Value4��6-   $
! 0.7* 6=   " 6J   �� *Value4+  5]      $  Value5��6-   $
! 0.7* 6=   " 6J   �� *Value5+  5^      #   
FontWeight$
# Bold"�6-   $  MasterPw(dBm)��6?   $
! 1.5* 6V   " 6c   �� *masterPower+  5_      $	  Max��6-   $
! 0.7* 6:   " 6G   �� *
maxValue+  5`      $	  Min��6-   $
! 0.7* 6:   " 6G   �� *
minValue+  5a      $
  Diff��6-   $
! 0.7* 6;   " 6H   �� *
Difference+  5b      $  Result��6-   $
! 0.7* 6=   " 6J   �� *Result+5f      �� 5h      $��3��6   $��Stretch=�6,   $��Stretch=�6H   �� 5i      -   5        $ ��Name$$ _scrollViewer��5i   =   $��4��6   $��5,0,5,5q�6,   .��5j      ��   colorBackground#��[ 6#    . $��1q�6c   % �� 
LineHeight$% 1��6w   $��0,5,0,0q�6�   . �5k      /� & /� 
PageHeight$	& 180��6'   .T�5l      J� .*�6(   .*�5m   "   �� ���#050928$��Normal"�6;   �6O   �� $ LOGDATA�$ PropertyChanged=�$ TwoWay=�5n      5o      5p      5q      5s      5u      5v      5x      