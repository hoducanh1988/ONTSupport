   M S B A M L   `   `   `  ����K  GEconetMasterWiFi, Version=2.0.0.1, Culture=neutral, PublicKeyToken=nullE'clr-namespace:GW040Hv2Project.SubWindowGW040Hv2Project.SubWindow  5   	   7    1GW040Hv2Project.SubWindow.wifiTestingConfigWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:GW040Hv2Project.SubWindow     5   	     � WindowStyle$
  None=�5   	    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�6A    � Title$+ %Thiết Lập Tiêu Chuẩn Test Wifi��5	   	   $	��400��60   $	��600��6=   .��5
      �� -   5        ���#BFCDDB$��2q�5
   #   ._�5   
   �  � RowDefinitions 5      �� 5      $��30 �6    6.   �� 5      $��5 �6    6-   �� 5      $��* �6    6-   �� 5      $��40 �6    6.   .+�5      � $��0��6   ���#BFCDDB.+�5      �� $��DemiBold"�6   $��Center=�6.   $
��Left=�6I   
���   �$��12(�6w   $+��%Thiết Lập Tiêu Chuẩn Test Wifi��6�   �� 5      -   5        $��50��5   5   $��Right=�6@   $��Center=�6c   $��Right=�6~   
���   �$��17(�6�   $��0,-5,5,0q�6�   $��X��6�   5      � 5      $��1��6   ���#BFCDDB67   &   System.Windows.Controls.DataGrid  5      -   5          Name$ dgtxTestWifi��5   0   $��2��6   $��5q�6%   
�������   AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .  	   CanUserResizeColumns	 .  
   CanUserResizeRows
 .     CanUserSortColumns .     CanUserReorderColumns .  "   VerticalScrollBarVisibility$
 Auto=�5   �   $   HorizontalScrollBarVisibility$
 Auto=�6�      GridLinesVisibility$
 None=�5   /      HeadersVisibility$ Column=�5   /   $��1q�5   /      SelectionUnit$
 Cell=�5    /      SelectionMode$ Single=�6E   ��5   /   �� *LstWifiTesting+   	CellStyle 5"      �� 5#       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6   .�5$      ��  �� Property � �� �� Value$ 0,0,1,1q�6<   �� 5%       � �� �	#FFB3B3B368   �� 5&       � ��$ 15(�65   �� 5'       � ��$
 Bold"�67   �� 5(       � ��$ 25��63   �� 5)       � ��$ Center=�6E   5*         ColumnHeaderStyle 5,      �� 5-      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5.      ��  � ��$ 13(�65   �� 5/       � ��$ 40��63   �� 50       � ��$ Normal"�67   �� 51       � �� �	#FFB3B3B368   �� 52       � ��$ 0,0,0,1q�6<   �� 53       � ��$ 3,0,0,0q�64   54         Columns 56      0  *System.Windows.Controls.DataGridTextColumn  57      ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly . -   5        txtFrequency��   Header$ Frequency(Mhz)��57   Q      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6i   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6s   �� *	Frequency+  58       . -   5        	txtAntena��$ Antena��58   O   $ * 6_    6i   �� *Antena+  59         
Foreground
 ��  � .  -   5        txtAttenuator��$ Attenuator(dBm)��59   i   $ * 6�    6�   �� *
Attenuator+  5:       . -   5        txtUpperLimit��$ UpperLimit(dBm)��5:   U   $ * 6n    6x   �� *
UpperLimit+  5;       . -   5        txtLowerLimit��$ LowerLimit(dBm)��5;   S   $ * 6l    6v   �� *
LowerLimit+5=      � 5>      $��3��6   ���#BFCDDB.+�5?      � $��Center=�6   $��3q�69   .+�5@      �� -	   5         ��Name$ btnOk��5@      $��Center=�6,   $��30��6\   $��90��6h   $��OK��6s   $��5 2 5 2q�6�   6�   �� 5A      -
   5        $ 	btnCancel��5A      $��Center=�61   $��30��6e   $��90��6q   $��Cancel��6|   $��5 2 5 2 q�6�   6�   5B      5C      5D      5E      5F      