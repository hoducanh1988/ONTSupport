   M S B A M L   `   `   `  ����N  JVNPTRecalledProduct, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:VNPTRecalledProductVNPTRecalledProduct  5   	   $    VNPTRecalledProduct.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:VNPTRecalledProduct     5   	   -   5        $	��700��5	   	   $
��1000��6     � WindowStartupLocation$  CenterScreen=�6#    � Title 5   	   ��  �� Path$ appTitle� �� UpdateSourceTrigger$ PropertyChanged=� J� Opacity 5
   	   �� $ Opacity�$ PropertyChanged=�.��5      � $��5q�6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��10 �6   .+�5   
   � -   5        $��0��5      .+�5      �� $��/Asset/Image/back.png��6   �� 5      
����  �$��Back to login form...��6&   $��13(�6F   $��SemiBold"�6T   5      �� 5   
   $��1��6   .+�5      � $��	0,-10,0,0q�6   .+�5      �� $��22(�6   $
��Bold"�6&   $��Product name:��68   �� 5      $��22(�6   $
��Bold"�6&   ��68   �� $ productName�$ PropertyChanged=�5       � 5!      $��0,-7,0,0q�6   .+�5"      �� $��17(�6   $��SemiBold"�6&   $��Station name:��6<   �� 5#      $��17(�6   $��SemiBold"�6&   ��6<   �� $ Station�$ PropertyChanged=�5$      5%      �� 5(   
   $��2��6   ���#777777$��	0,0,0,0.5q�64   ._�5)      � $��0,5,0,10q�6    5*      �� 5+      $��* �6$   �� 5,      $��4 �6$   .+�5/      � $��0��6   .+�50      � $��0,-2,0,0q�6     � 	Resources 51       �� 
TargetType �� Property	 �� Value
 �� Triggers P� Property P� Value��%�  '��       �� 52      " ��6$   .�53   "   ��  � ��$
	 Left=�6H   �� 54   "    � ��$	 17(�6=   �� 55   "    � ��$		 100��6:   �� 56   "    � ��$	 Center=�6M   �� 57   "    � ��$	 Center=�6O   �� 58   "    � ��
	 �   �6?   -   5        
 5;   "   P� 5<   &    �@J�IsMouseOver .  . �5=   *   ��  � ��$	 Normal"�6G   5>   '   P� 5?   &    �@J�IsMouseOver . . �5@   *   ��  � ��$
	 Bold"�6G   5A   '   5C      5D      .+�5E      �� $��RUN ALL��6    �� 5F      $��SETTING��6    �� 5G      $	��LOG��6    �� 5H      $
��HELP��6    �� 5I      $��ABOUT��6    5J      5K      �� 5L      -   5         ��Name$ lblMinus��5L      $��1��6*   $��Stretch=�67   $	��100��6S   $
��Left=�6_   
��� � �6z   5M      5N      � 5Q   
   -   5         �Name$ 	grid_main��5Q      $��3��6   6   5S      5U      