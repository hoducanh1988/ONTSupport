   M S B A M L   `   `   `  ����O  KBosaManagementSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:BosaManagementSystemBosaManagementSystem  5   	   &     BosaManagementSystem.LoginWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:BosaManagementSystem     5   	     � Title$>  8Optical Bosa Management System - Version: ONT000000U0001��5   	   $	��420��6J   $	��500��6W    � WindowStartupLocation$ CenterScreen=�6c    � 
ResizeMode$ NoResize=�6�   
�������.��5      �� $��
10,0,10,10q�6   .+�5   
   �� $��17(�6   $��SemiBold"�6   $-��'Đăng nhập vào cơ sở dữ liệu��64   �� 5   
   $"��Loại cơ sở dữ liệu:��6   $��13(�6.   �� 5   
   -   5         ��Name$ cbb_servertype��5      $��10,0,0,0q�6+   $��13(�6>   
����  �$��SemiBold"�6^   $��25��6t    �� 
IsEditable .  $	��300��6�   $
��Left=�6�   $��0,0,0,1q�6�   $��Center=�5       �� Text 5      ��  �� Path$ dbServerType� �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   $��Địa chỉ IP server:��6   $��13(�6-   �� 5   
   $��10,0,0,0q�6   $��13(�6%   
����  �$��SemiBold"�6E   $��25��6[   $	��300��6g   $
��Left=�6s   $��0,0,0,1q�6�   ��5      �� $ 
dbServerIP�$ PropertyChanged=��� 5   
   $��Tên đăng nhập:��6   $��13(�6)   �� 5   
   $��10,0,0,0q�6   $��13(�6%   
����  �$��SemiBold"�6E   $��25��6[   $	��300��6g   $
��Left=�6s   $��0,0,0,1q�6�   {�.  ��5      �� $ dbServerUser�$ PropertyChanged=��� 5   
   $��Password đăng nhập:��6   $��13(�6.   �� 5   
   $��10,0,0,0q�6   $��13(�6%   
����  �$��SemiBold"�6E   $��25��6[   $	��300��6g   $
��Left=�6s   $��0,0,0,1q�6�   ��5      �� $ dbServerPass�$ PropertyChanged=��� 5    
   $��Tên database:��6   $��13(�6(   �� 5!   
   $��10,0,0,0q�6   $��13(�6%   
����  �$��SemiBold"�6E   $��25��6[   $	��300��6g   $
��Left=�6s   $��0,0,0,1q�6�   {�.  ��5#      �� $ dbName�$ PropertyChanged=��� 5%   
   $��0,10,0,0q�6`   $C��=Tự động tạo database và tables nếu server chưa có��6r    d� 	IsChecked 6   �� $ autoCreateDB�$ PropertyChanged=�6r   � 5'   
   .+�5(      �� -   5        	 J� Opacity$		 0.5Q�5(   *   $��30��68   $��0,10,0,0q�6D   $	��120��6V   $
��Left=�6b   $
��Exit��6}   �� 5)      -   5        $��30��5)   *   $��	20,10,0,0q�66   $	��120��6I   $
��Left=�6U   $��OK��6p   5*      5-      5/      