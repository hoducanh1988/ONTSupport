   M S B A M L   `   `   `  ����O  KBosaManagementSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:BosaManagementSystem.IControlBosaManagementSystem.IControl  5      0    *BosaManagementSystem.IControl.ucImportBosa   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =local+clr-namespace:BosaManagementSystem.IControl     5      
�������  � 	Resources  5
      >   8BosaManagementSystem.IControl.BosaPathToBooleanConverter   BosaPathToBooleanConverter �� 
TargetType  style_button��%�   &	        &	      'm� N       5   
   6+   �� 5   
   " ��6   .�5      -   5        5      �� 5   
   " m�6   .�5      -   5        5      5      .��5      � $��5,0,5,0q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��55 �6   .+�5   
   �� $��1��6   .+�5      �� $L��FChọn folder chứa các file bosa report cần import vào database:��6   � 5      $��25��6   .+�5      �� 
�������$��	105,0,0,0q�6-   $��0,0,0,1q�6@   $��SemiBold"�6Z   ��5      ��  �� Path$ bosaFolderPath� �� UpdateSourceTrigger$ PropertyChanged=�.��6d   ��5       �� 5!      " ��6    .�5"      ��  �� Property �@��
IsReadOnly �� Value .  6;    �� Triggers 5#      �� 5$   "    �� Binding 6.   �� $ totalResult�	 �� Value	 
Waiting...6S   .a�5%   &   ��  �@��
IsReadOnly . 6C   5&   #   5)      .��5*   %   5+      �� 5,      

 � Tag$
 browser��6   $
��Left=�6(   $	��100��6C   $��
Browser...��6O   ��5-      �� 5.      " ��6     �� BasedOn# [ 64   .�5/      ��  � {� . 6:    50      �� 51   "    6.   �� $ totalResult�	 
Waiting...6S   .a�52   &   ��  � {� .  6B   53   #   55      57      58      59      �� 5<   
   $��0,5,0,0q�6   $��0��6&    �� TabStripPlacement$
 Left=�63     5=       �� 
TargetType �� CornerRadius ��Name �� Triggers P� Property P� Value �� 
TargetName��%(  '��       '�� L     �� 5>      " ��6   .�5?      ��  � ��$ Center=�6:   5@      �� 5B      " ��6   .�5C      ��  � �� 5D      �� 5E      " ��6.   .R�5F   "   � .+�5G   &   �� $��Border��6;   $	��250��6-   $��0,0,0,0q�6I   $��1,1,1,1q�6Z   $ 3��6t   ._�5H   *   �� $ ContentSite��6;   $��Center=�6P   $��Center=�6k   $��Header��6�   $��	12,2,12,2q�6�   ��. 6�   5I   '   5J   #    5K   "   P� 5L   &    �@��
IsSelected . . �5M   *   ��  �@K�ZIndex$	 100��6I   �� 5N   *   $ Border��61    � �� �#d9d9d96[   �� 5O   *   $ Border��61    � ��$ 1,1,1,0q�6`   5P   '   P� 5Q   &    �@��
IsSelected .  . �5R   *   �� $ Border��61    � ��
 �����6[   5S   '   5U      5W      5X      5Z      .@�5\      �� ��5]      �� 5^      $��Excel Files��6   .��5a      &   System.Windows.Controls.DataGrid  -   5          Name$ 
dgBosaFile��5a      
�������   AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .     CanUserReorderColumns .  "   VerticalScrollBarVisibility$
 Auto=�5d   �   $   HorizontalScrollBarVisibility$
 Auto=�6�      GridLinesVisibility$
 None=�5e   /      HeadersVisibility$ Column=�5f   /   $��0,0,0,0q�5g   /       SelectionUnit$  FullRow=�5h   /   !   SelectionMode$! Extended=�6G   ��5c   /   �� " � ContextMenu" 5k      �� 5l      .@�5m      m� $
 import��6'   $��Import to database��64   �� 5n      m� 5o      $
 refresh��6'   $��Refresh��65   5p      #   	CellStyle# 5s      �� 5t      *  $System.Windows.Controls.DataGridCell"  6    .�5u      ��  � ��$ 0,0,1,1q�6@   �� 5v       � �� �	#FFB3B3B36<   �� 5w       � ��$ 13(�69   �� 5x       � ��$ Normal"�6;   �� 5y       � ��$ 25��67   �� 5z       � ��$ Center=�6I   5{      $   RowStyle$ 5~      �� 5      )  #System.Windows.Controls.DataGridRow"  6    .�5�      ��  � ��
 �����6;    5�      �� 5�   "    6.   �� $ Result�	 IGnored6N   .a�5�   &   ��  � �� �#d9d9d96C   5�   #   5�      %   ColumnHeaderStyle% 5�      �� 5�      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�5�      ��  � ��$ 13(�69   �� 5�       � ��$ 40��67   �� 5�       � ��$ SemiBold"�6;   �� 5�       � �� �	#FFB3B3B36<   �� 5�       � ��$ 0,0,0,1q�6@   �� 5�       � ��$ 3,0,0,0q�68   5�      &   Columns& 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn'   Header$' Index��6-   (   Width5  /System.Windows.Controls.DataGridLengthConverter$( * 6<   1	  +System.Windows.Controls.DataGridBoundColumn) 	  Binding) 6F   �� *Index+  5�      $' Bosa File Name��6-   $	( 10* 6E   ) 6Q   �� *
FileName+  5�      $' Bosa File MD5��6-   $( 6* 6D   ) 6O   �� *
InputMD5+$ PropertyChanged=�* �� Mode$* TwoWay=�  5�      $' Counted��6-   $( 2* 6>   ) 6I   �� *BosaCounted+  5�      $' Status��6-   $( 2* 6=   ) 6H   �� *Status+  5�      $' Result��6-   $( 2* 6=   ) 6H   �� *Result++   	CellStyle+ 5�      �� 5�   "   "  6(   .�5�   &   ��  � ��
 �   �6C   �� 5�   &    � ��$ 0,0,1,1q�6H   �� 5�   &    � �� �	#FFB3B3B36D   �� 5�   &    � ��$ 15(�6A   �� 5�   &    � ��$ SemiBold"�6C   �� 5�   &    � ��$ 25��6?   �� 5�   &    � ��$ Center=�6Q    5�   &   �� 5�   *    66   �� $ Result�	 
Waiting...6V   .a�5�   .   ��  � ��
 � ���6K   5�   +   �� 5�   *    66   �� $ Result�
	 Passed6V   .a�5�   .   ��  � ��
 � � �6K   5�   +   �� 5�   *    66   �� $ Result�
	 Failed6V   .a�5�   .   ��  � ��
 �  ��6K   5�   +   5�   #   5�      5�      5�      �� 5�      ��5�      �� 5�      $��
Log Import��6   .��5�      �� -   5        , ��Name$, scroll_viewer��5�      .��5�      �� 
������ $��0q�6<   - �� 
IsReadOnly- . . �� 
LineHeight$. 1��6b   $��0,5,0,0q�6w   . �5�      /� / /� 
PageHeight$	/ 180��6'   .T�5�      J� .*�6(   .*�5�   "   �� ���#050928$��Normal"�6;   �6O   �� $ 	logImport�$ PropertyChanged=�$* TwoWay=�5�      5�      5�      5�      5�      5�      5�      5�      