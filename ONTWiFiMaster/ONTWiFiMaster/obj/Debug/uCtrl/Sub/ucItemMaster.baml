   M S B A M L   `   `   `  ����H  DONTWiFiMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:ONTWiFiMaster.uCtrl.SubONTWiFiMaster.uCtrl.Sub  5      *    $ONTWiFiMaster.uCtrl.Sub.ucItemMaster   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:ONTWiFiMaster.uCtrl.Sub     5      
�������  � 	Resources  5	       �� 
TargetType   label_title �� Property �� Value  label_content �� Triggers �� Binding �� RelativeSource �� Mode �� Path	 �� Value  progressbar_content��%�  &	        &	 ^    &	 q    �� 5   
   " ��6   .�5      ��  � ��$ 15(�6)   �� 5       � ��$ Normal"�6+   �� 5       � �� �#7777776,   �� 5       � ��$ 1,0,0,1q�60   �� 5       � ��$	 150��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ Center=�69   �� 5       � �� �#dedede6+   5      �� 5   
   " ��6   .�5      ��  � ��$ 15(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � �� �#7777776,   �� 5       � ��$ 1,0,1,1q�60   �� 5       � ��$	 300��6&   �� 5       � ��$ 30��6'   �� 5       � ��
 �   �6+   �� 5       � ��$ 10,0,0,0q�6(   �� 5       � ��$ Center=�69    5       �� 5!       6   ��  �� $
 Self=�$ Content�	 
Waiting...6j   .a�5"      ��  � ��
 � ���63   5#      �� 5$       6   ��  �� $
 Self=�$ Content�
	 Passed6j   .a�5%      ��  � ��
 � � �63   5&      �� 5'       6   ��  �� $
 Self=�$ Content�
	 Failed6j   .a�5(      ��  � ��
 �  ��63   5)      5+      �� 5-   
   " �6   .�5.      ��  � ��
 � � �6+   5/      51      .��53      � .+�54   
   �� #��[  6   $��1,0,0,1q�65   ��6O   �� $ 
labelTitle�
 �� UpdateSourceTrigger$
 PropertyChanged=�� 55   
   .+�56      �� #��[ 6   $��1,0,1,1q�6;   ��6U   �� $ Result�$
 PropertyChanged=�� 57       � Value 6   �� $ valueProgress�$
 PropertyChanged=� � Maximum 6d   �� $ maxProgress�$
 PropertyChanged=���58      �� 59      " �6    �� BasedOn# [ 65   .�5:      ��  �@J�Opacity$	 0.5Q�64   �� 5;       � ��
 �����67   �� 5<       � ��$ 5q�63   �� 5=       � ��$ 0q�6<   5>      5@      � 5B      $��Right=�6   .+�5C      �� $��-5,0,0,0q�6   ��6*   �� $ valueProgress�$
 PropertyChanged=��� 5D      $��-5,0,0,0q�6   $��/��6*   �� 5E      $��-5,0,0,0q�6   ��6*   �� $ maxProgress�$
 PropertyChanged=�5F      5H      5I      5L      