   M S B A M L   `   `   `  ����C  ?GW040Hv2, Version=2.0.0.1, Culture=neutral, PublicKeyToken=nullE'clr-namespace:GW040Hv2Project.SubWindowGW040Hv2Project.SubWindow  5   	   (    "GW040Hv2Project.SubWindow.RDWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:GW040Hv2Project.SubWindow     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ 	RD Window��5	   	   $	��180��6   $	��300��6(   .��5
      �� $��3q�6   .+�5   
   �� $��DemiBold"�6   $��20(�6+   
����  �$��Center=�6L   $��RD Mode��6i   6z   �� 5   
    �� Orientation$ 
Horizontal=�6   $��5q�6.   .+�5      �� $��Nhập PASS:  ��6   6.   G� 5      -   5         G�Name$ txtPass��5   4   $	��200��6E   $�� 0 0 0 1q�6Q   6o   5      �� 5   
   -   5        $��0 5 0 0q�5      
������
���**��$	��130��6O   $��35��6[    � 	Resources 5       �� 
TargetType �� Property �� Value �� Triggers	 �� Binding
 �� Path �� UpdateSourceTrigger �� Mode �� Value��%m  '��       '�� Z     �� 5      " ��6   .�5      ��  �@��CornerRadius$ 10��65   6B   5      �� 5      " ��6   .�5      ��  � ��$ 	RD Mode ?��60   6D    5      �� 5      	 6&   �� $
 RDMode�$ PropertyChanged=�$ TwoWay=� TRUE6y   .a�5      ��  � ��$ Production Mode ?��68   6T   5      5      5       5$      �� 5%   
   -   5         ��Name$ 	txtStatus��5%   (   
���**��$��Center=�6<   $��0 10 0 0q�6Y   $��*****************��6k   6�   5'      5(      