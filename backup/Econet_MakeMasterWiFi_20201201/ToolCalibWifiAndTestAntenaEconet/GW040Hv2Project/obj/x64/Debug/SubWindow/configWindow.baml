   M S B A M L   `   `   `  ����C  ?GW040Hv2, Version=2.0.0.1, Culture=neutral, PublicKeyToken=null1clr-namespace:GW040Hv2ProjectGW040Hv2Project  5   	   "    GW040Hv2Project.configWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:GW040Hv2Project     5   	     � Title$  configWindow��5   	   $	��330��6   $	��450��6+    � WindowStartupLocation$ CenterScreen=�67      customWindow#��[  6\   .��5
      �� ���#BFCDDB �� CornerRadius$ 2��6"     colorBorder#��[ 63   $��1,1,1,1q�6^   ._�5   
   �  � RowDefinitions 5      �� 5      $��30 �6    �� 5      $��* �6    .+�5      � $��0��6   ���#FFFFFF.+�5      �� $��Center=�6   $
��Left=�63   
���   �$��12(�6a   $��Thiết lập cấu hình��6o   �� 5      -   5        $��50��5   4   $��Right=�6?   $��Center=�6b   $��Right=�6}   
���   �$��17(�6�   $��0,-5,5,0q�6�   $��X��6�   5      � 5      $��1��6   .+�5      �� $��5,10,0,0q�6   
���   �$	��0.5q�6A   $��Thiết lập ONT��6W   $	��225��6n   $	��185��6z   $	��Top=�6�   $
��Left=�6�   .��5      �� .+�5      � $��0,10,0,0q�6   .+�5      �� $
��Left=�6$   $��ONT Type��6?   �� 5      -   5         ��Name$ 
cbbOntType��5   9   {�.  $��SemiBold"�6M   $��Center=�6c   $��Right=�6�   $	��150��6�    �� Text 6�   ��  �� Path$ ONTTYPE� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�5      � 5      -   5        	 �Name$	 grd_ip��5      $��0,5,0,0q�6/   .+�5      �� $
��Left=�6$   $��
IP Address��6?   �� 5       $��SemiBold"�6&   $��Center=�6<   $��Right=�6^   $	��150��6z   ��6�   �� $ ONTIP�$ PropertyChanged=�$ TwoWay=�5!      � 5"      $��0,5,0,0q�6   .+�5#      �� $
��Left=�6$   $
��User��6?   �� 5$      $��SemiBold"�6&   $��Center=�6<   $��Right=�6^   $	��150��6z   ��6�   �� $ ONTUSER�$ PropertyChanged=�$ TwoWay=�5%      � 5&      $��0,5,0,0q�6   .+�5'      �� $
��Left=�6$   $��Password��6?   �� 5(      -   5        $ cbxPass��5(   9   
 �� 
IsEditable
 . $��SemiBold"�6J   $��Center=�6`   $��Right=�6�   $	��150��6�    6�   �� $ ONTPASS�$ PropertyChanged=�$ TwoWay=�5)      � 5*      -   5        $	 
grd_serial��5*      $��0,5,0,0q�63   .+�5+      �� $
��Left=�6$   $��
SerialPort��6?   �� 5,      -   5        $ cbbSerialPort��5,   '   $��SemiBold"�6>   $��Center=�6T   $��Right=�6v   $	��150��6�    6�   �� $ 
SERIALPORT�$ PropertyChanged=�$ TwoWay=�5-      5.      5/      �� 51      -   5         ��Name$ 	grbBefore��51      $��	5,200,0,0q�6.   
���   �$,��&Thiết lập tần suất đo suy hao��6U   $	��Top=�6|   $
��Left=�6�   $	��0.5q�6�   $	��225��6�   $��55��6�   .��52      � $��0,5,0,0q�6    .+�53      �� $��63��6    $
��Left=�6+   $��Tần suất��6F   �� 54      $��SemiBold"�6"   $��Center=�68   $��Right=�6Z   $��70��6v   ��6�   �� $ 	CALIBTIME�$ PropertyChanged=�$ TwoWay=��� 55      $��(lần đo)��6    56      57      � 5:      $	��230��6   $
��Left=�6(   $��	5,260,0,0q�6C   .+�5;      �� -   5        $��30��5;   2   $��80��6>   $��OK��6I   $��0,0,0,0q�6V   6i   �� 5<      -	   5        $��30��5<   2   $��60��6>   $��Default��6I   $��10,0,0,0q�6[   6o   �� 5=      -
   5        $��30��5=   2   $��60��6>   $��Cancel��6I   $��10,0,0,0q�6Z   5>      �� 5@      $��	5,10,10,0q�6   
���   �$	��0.5q�6B   $��Select Items��6X   $	��190��6n   $	��245��6z   $	��Top=�6�   $��Right=�6�   .��5A      �� .+�5B      �� -   5         ��Name$ spBefore��5B   %   .+�5C      �� $��0,10,0,0q�6�   $
��Left=�6�   $��Calibrate Tần Số��6�    d� 	IsChecked 6'   �� $ ENCALIBFREQ�$ PropertyChanged=�$ TwoWay=�6�   �� 5D      $��0,10,0,0q�6�   $
��Left=�6�   $��Calibrate Power 2G��6�    6'   �� $ ENCALIBPW2G�$ PropertyChanged=�$ TwoWay=�6�   �� 5E      $��0,10,0,0q�6�   $
��Left=�6�   $��Calibrate Power 5G��6�    6'   �� $ ENCALIBPW5G�$ PropertyChanged=�$ TwoWay=�6�   �� 5F      $��0,10,0,0q�6   $
��Left=�6�   $��Write BIN registers��6�    6'   �� $ 
ENWRITEBIN�$ PropertyChanged=�$ TwoWay=�6�   �� 5G      $��0,10,0,0q�6   $
��Left=�6�   $��Test Độ Nhạy Thu 2G��6�    6'   �� $ 
ENTESTRX2G�$ PropertyChanged=�$ TwoWay=�6�   �� 5H      $��0,10,0,0q�6   $
��Left=�6�   $��Test Độ Nhạy Thu 5G��6�    6'   �� $ 
ENTESTRX5G�$ PropertyChanged=�$ TwoWay=�6�   �� 5I      $��0,10,0,0q�6   $
��Left=�6�   $��Test Signal 2G��6�    6'   �� $ 
ENTESTTX2G�$ PropertyChanged=�$ TwoWay=�6�   �� 5J      $��0,10,0,0q�6   $
��Left=�6�   $��Test Signal 5G��6�    6'   �� $ 
ENTESTTX5G�$ PropertyChanged=�$ TwoWay=�6�   5K      �� 5L      -   5        $ spAfter��5L   %   .+�5M      �� $��0,10,0,0q�6�   $
��Left=�6�   $��Test Anten1��6�    6'   �� $ ENTESTANTEN1�$ PropertyChanged=�$ TwoWay=�6�   �� 5N      $��0,10,0,0q�6�   $
��Left=�6�   $��Test Anten2��6�    6'   �� $ ENTESTANTEN2�$ PropertyChanged=�$ TwoWay=�6�   5O      5Q      5S      � 5U      $��
0,0,112,20q�6   $��Bottom=�60   $��Right=�6K   .+�5V      �� -   5         ��Name$ ckb_all��5V      $��Chọn tất cả��60   6`   5W      5Y      5[      5\      5]      