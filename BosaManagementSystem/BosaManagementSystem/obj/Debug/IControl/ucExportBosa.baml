   M S B A M L   `   `   `  ����O  KBosaManagementSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:BosaManagementSystem.IControlBosaManagementSystem.IControl  5      0    *BosaManagementSystem.IControl.ucExportBosa   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =local+clr-namespace:BosaManagementSystem.IControl     5      
�������  � 	Resources  5
      >   8BosaManagementSystem.IControl.BosaPathToBooleanConverter   BosaPathToBooleanConverter �� 
TargetType��%�   &	        '��      'm� N       5   
   6+   �� 5   
   " ��6   .�5      -   5        5      �� 5   
   " m�6   .�5      -   5        5      5      .��5      � $��5,0,5,0q�6     5   
    �� Property �� Value �� Triggers P� Property P� Value��%�  '��       '�� �     '�� �    �� 5      " ��6   .�5      ��  � ��$ 13(�6-   �� 5       � ��$ Center=�6=   �� 5       � ��$
 Left=�6?   �� 5       � ��$ 80��6*   �� 5       � ��$ 25��6+   5      �� 5!      " ��6   .�5"      ��  � ��$ 13(�6-   �� 5#       � ��$ SemiBold"�6/   �� 5$       � ��$ Center=�6=   �� 5%       � ��$
 Left=�6?   �� 5&       � ��
 �����6/   �� 5'       � ��$	 200��6*   �� 5(       � ��$ 25��6+   �� 5)       � ��$ 0,0,0,1q�64    5*      P� 5+       �@J�	IsFocused . . �5,      ��  � ��
 � ���67   5-      5/      �� 51      " ��6   .�52      ��  � ��$ 13(�6-   �� 53       � ��$ SemiBold"�6/   �� 54       � ��$ Center=�6=   �� 55       � ��$
 Left=�6?   �� 56       � ��
 �����6/   �� 57       � ��$	 200��6*   �� 58       � ��$ 25��6+   �� 59       � ��$ 0,0,0,1q�64   �� 5:       �@��
IsEditable . 6/    5;      P� 5<       �@J�	IsFocused . . �5=      ��  � ��
 � ���67   5>      5@      5B       � RowDefinitions 5D   
   �� 5E      $��* �6   �� 5F      $��90 �6   .+�5I   
   �� $��1��6   .+�5J      � .+�5K      � .+�5L      �� $��Bosa serial: ��6   �� 5M      ��6   ��  �� Path$ 
bosaSerial�	 �� UpdateSourceTrigger$	 PropertyChanged=�5N      � 5O      $��20,0,0,0q�6   .+�5P      �� $��
Ith (mA): ��6   �� 5Q      ��6   �� $	 Ith�$	 PropertyChanged=�5R      � 5S      $��20,0,0,0q�6   .+�5T      �� $��	Vbr (V): ��6   �� 5U      ��6   �� $	 Vbr�$	 PropertyChanged=�5V      5W      � 5X      $��0,5,0,0q�6   .+�5Y      �� $��Bosa file: ��6   �� 5Z      -   5        
 ��Name$
 cbb_bosafile��5Z      $	��800��61    �� Text 6=   �� $ bosaFile�$	 PropertyChanged=�5[      �� 5]      
 � Tag$ search��6   $
��Left=�6"   $��0,5,0,5q�6=   $��25��6N   $	��100��6Z   $��Search��6f   5^      &   System.Windows.Controls.DataGrid  5`   
   -   5          Name$ 
dgBosaInfo��5`       $��0��6   
�������   CanUserAddRows .     CanUserDeleteRows .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .     CanUserReorderColumns .  "   VerticalScrollBarVisibility$
 Auto=�5c   �   $   HorizontalScrollBarVisibility$
 Auto=�6�      GridLinesVisibility$
 None=�5d         HeadersVisibility$ Column=�5e      $��0,0,0,0q�5f         SelectionUnit$ FullRow=�5g         SelectionMode$ Extended=�6+    � ContextMenu 5i      �� 5j      .@�5k      m� $ export��6   $��Export to excel file��6,   �� 5l      m� 5m      $ refresh��6   $��Refresh��6-   5n         	CellStyle 5q      �� 5r      *  $System.Windows.Controls.DataGridCell"  6   .�5s      ��  � ��$ 0,0,1,1q�68   �� 5t       � �� �	#FFB3B3B364   �� 5u       � ��$ 12(�61   �� 5v       � ��$ Normal"�63   �� 5w       � ��$ 20��6/   �� 5x       � ��$ Center=�6A   5y         ColumnHeaderStyle 5|      �� 5}      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5~      ��  � ��$ 13(�61   �� 5       � ��$ 30��6/   �� 5�       � ��$ SemiBold"�63   �� 5�       � �� �	#FFB3B3B364   �� 5�       � ��$ 0,0,0,1q�68   �� 5�       � ��$ 3,0,0,0q�60   5�      5�      5�      5�      