   M S B A M L   `   `   `  ����K  GEconetMasterWiFi, Version=2.0.0.1, Culture=neutral, PublicKeyToken=nullE'clr-namespace:GW040Hv2Project.SubWindowGW040Hv2Project.SubWindow  5   	   3    -GW040Hv2Project.SubWindow.resetRegisterWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:GW040Hv2Project.SubWindow     5   	     � Title$(  "Thiết lập giá trị thanh ghi��5   	   $	��450��6-   $	��800��6:    � WindowStartupLocation$ CenterScreen=�6F   .��5
      �� $��5q�6   .+�5   
   �� $9��3Load file giá trị thanh ghi cần thiết lập:��6   �� 5   
   $��25��6    �� Orientation$ 
Horizontal=�6!   $��5,5,5,5q�6:   .+�5      ��  �� 
IsReadOnly . $��0,0,10,0q�6(   $	��680��6:   $��Center=�6G   $��25��6i   ��6u   ��  �� Path$ 
fileConfig� �� UpdateSourceTrigger$ PropertyChanged=��� 5      -   5        
 � Tag$ browser��5      $��Browser��68   $��75��6J   $
��Left=�6U   $��25��6p   5      �� 5   
   $ 
Horizontal=�6   .+�5      �� -   5        $ start��5      $��5,5,0,0q�66   $	��120��6G   $��40��6S   $��20(�6_   $
��Left=�6m   $��Start��6�   �� 5      $��Center=�6   $
��Left=�6/   $��25(�6J   $��50,0,0,0q�6X   ��6j   �� $ totalResult�$ PropertyChanged=�5      �� 5   
   -   5         ��Name$ _scrollViewer��5   6   $	��280��6   $��5,10,0,0q�6$   .��5      �� ���#FFFFFF$��1,1,1,1q�60    .  �� 
LineHeight$ 1��6\   $��0,5,0,0q�6q   . �5      /� 	 /� 
PageHeight$		 180��6   .T�5      J� .*�6    .*�5      �� 
���   �$��Normal"�61   �6E   �� $ 	logSystem�$ PropertyChanged=�
 �� Mode$
 TwoWay=�5      5      5      5      5       5"      