   M S B A M L   `   `   `  ����H  DONTWiFiMaster, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:ONTWiFiMasterONTWiFiMaster  5   	       ONTWiFiMaster.LoginWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:ONTWiFiMaster     5   	     � Title$   ��5   	   $	��400��6   $	��700��6    � WindowStartupLocation$ CenterScreen=�6+    � 
ResizeMode$ NoResize=�6P    � 	Resources 5
       �� 
TargetType �� Property �� Value �� Triggers �� Binding	 �� RelativeSource
 �� Mode �� Path �� Value��%  '	�       �� 5   
   " 	�6   .�5      ��  � ��$ 20(�6)   �� 5       � ��$ 	20,0,10,5q�6'   �� 5       � ��$ Center=�62   �� 5       � ��$
 Left=�64   �� 5       � ��$ Center=�69   �� 5       � ��$ Center=�6;   �� 5       � ��$ Normal"�6+   �� 5       � ��
 �   �6+    5      �� 5       6   �� 	 �� $

 Self=�$ 	IsChecked� True6l   .a�5      ��  � ��$
 Bold"�63   �� 5       � ��
 �  ��63   5      5      5      .��5      �  � RowDefinitions 5    
   �� 5!      $��30 �6   �� 5"      $��* �6   �� 5#      $��60 �6   .+�5&   
   �� $��15(�6   $A��;Vui lòng chọn sản phẩm cần đo dữ liệu master:��6   �� 5'   
   $��5q�6   $��1��6   ���#777777$��0,1,0,1q�6?   ._�5(      �� .��5)      �� -   5         ��Name$ sp_main��5)   /   $��5,20,5,5q�6   6/   5*      5+      �� 5-   
   -   5         ��Name$ btn_next��5-      {�.  $��5,0,5,5q�65   $��22(�6F   
 � Tag$
 next��6T   $��2��6_   $
��Next��6l   6{   5/      50      