   M S B A M L   `   `   `  ����N  JVNPTRecalledProduct, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:VNPTRecalledProduct.SupportUserControl&VNPTRecalledProduct.SupportUserControl  5      5    /VNPTRecalledProduct.SupportUserControl.ucRunAll   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Flocal4clr-namespace:VNPTRecalledProduct.SupportUserControl     5        � 	Resources  5
       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius ��Name �� Triggers P� Property	 P� Value
 �� 
TargetName �� Triggers �� Binding �� RelativeSource �� Mode �� Path �� Value��%�  '��       '�� 4    �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $��Border��6&   $��0,0,-4,0q�64   $��1,1,1,1q�6F   $ 2,12,0,0��6`   ._�5   "   �� $ ContentSite��63   $��Center=�6H   $��Center=�6c   $��Header��6�   $��	12,2,12,2q�6�   ��. 6�   5      5       5      P� 5       �@��
IsSelected	 . . �5   "   ��  �@K�ZIndex$	 100��6A   �� 5   "   $
 Border��6)    � �� �#43d8546S   �� 5   "   $
 Border��6)    � ��$ 1,1,1,0q�6X   5      P� 5       �@��
IsSelected	 .  . �5   "   �� $
 Border��6)    � �� �	#FFEEEEEE6S   5      P� 5       �@J�IsMouseOver	 . . �5   "   �� $
 Border��6)    � ��
 � ���6S   5       5"      5$      5%      �� 5'   
   " ��6   .�5(      ��  � ��$ 13(�6)   �� 5)       � ��$ Normal"�6+   �� 5*       � ��
 �   �6+    5+      �� 5,       6   ��  �� $
 Self=�$ 	IsChecked� True6l   .a�5-      ��  � ��$ SemiBold"�63   �� 5.       � ��
 ��  �63   5/      51      53      .��55      � $��10q�6    � ColumnDefinitions 56   
   �� 57      $	��320 �6   �� 58      $��5 �6   �� 59      $��* �6   .+�5<   
   �� $��0��6   .+�5>      �� -   5        $	��290��5>   *   $
��Left=�66   $��0,0,0,0q�6Q   $��70��6b   $��25(�6n   $��SemiBold"�6|   $��Start��6�   ��5?      �� 5@      " ��6   .�5A      ��  � {� . 66   �� 5B       � ��
 �   �67   �� 5C       � y�$ Visible=�67    5D      �� 5E       6*   ��  �� $
 Self=�$ Content� Stop6v   .a�5F   "   ��  � {� .  6>   5G      �� 5H       6*   �� $ showStartButton�	 False6S   .a�5I   "   ��  � y�$ 	Collapsed=�6?   5J      5L      5N      �� 5P      $��0,20,0,0q�6   $��2��6+   .+�5Q      �� $
��Left=�6   $��0,5,0,0q�63   $��13(�6D   $��Total result:��6R   �� 5R      $
��Left=�6   $��	0,-15,0,0q�63   $��50(�6F   $��SemiBold"�6T   ��6j   �� $ totalResult� �� UpdateSourceTrigger$ PropertyChanged=���5S      �� 5T      " ��6    .�5U      ��  � ��
 �   �6;    5V      �� 5W   "    6.   ��  �� $
 Self=�$ Content� 
Waiting...6z   .a�5X   &   ��  � ��
 � ���6C   5Y   #   �� 5Z   "    6.   ��  �� $
 Self=�$ Content�
 Passed6z   .a�5[   &   ��  � ��
 � � �6C   5\   #   �� 5]   "    6.   ��  �� $
 Self=�$ Content�
 Failed6z   .a�5^   &   ��  � ��
 �  ��6C   5_   #   5a      5c      �� 5d      $��0,-5,0,0q�6   $��17(�6.   $��SemiBold"�6<   $��Center=�6R   $
��Left=�6m   $
��Wrap=�6�   ��6�   �� $ inputBarcode�$ PropertyChanged=�� 5f      .+�5g      �� $��	Progress:��6   �� 5h      ��6   �� $ testedItemCount�$ PropertyChanged=��� 5i      $��/��6   �� 5j      ��6   �� $ totalTestItem�$ PropertyChanged=�5k      � 5m      $��10��6�    � Value 6   �� $ testedItemCount�$ PropertyChanged=� � Maximum 6j   �� $ totalTestItem�$ PropertyChanged=�6�   � 5n      .+�5o      �� $��Item Error Rate (%) :��6   �� 5p      ��6   �� $ 	errorRate�$ PropertyChanged=�5q      � 5s      .+�5t      �� $��Total time : ��6   �� 5u      ��6   �� $ 	totalTime�$ PropertyChanged=�5v      �� 5y      $��0,30,0,0q�6   $��Quantity Counter :��6*   � 5z      .+�5{      �� $'��!Total number of products tested: ��6   �� 5|      ��6   �� $ productTested�$ PropertyChanged=�5}      � 5~      .+�5      �� $��Passed (pcs): ��6   �� 5�      ��6   �� $ productPassed�$ PropertyChanged=�5�      � 5�      .+�5�      �� $��Failed (pcs): ��6   �� 5�      ��6   �� $ productFailed�$ PropertyChanged=�5�      � 5�      .+�5�      �� $��Product Error Rate (%): ��6   �� 5�      ��6   �� $ productErrorRate�$ PropertyChanged=�5�      5�      5�      �� 5�   
   $��1��6   
�������$��Stretch=�6:   $��Stretch=�6V   � 5�   
   $��2��6    � RowDefinitions 5�      �� 5�      $��50 �6    �� 5�      $��* �6    .+�5�      �� .+�5�      �� $y�	Collapsed=�6   $��0,0,0,0q�62   $`��ZTự động kiểm tra khi ping mạng được tới địa chỉ IP của sản phẩm.��6C    d� 	IsChecked 6�   �� $ 	autoStart�$ PropertyChanged=��� 5�      $��0,7,0,0q�6   $`��ZVẫn tiếp tục kiểm tra bài test tiếp theo khi bài test hiện tại bị lỗi.��6,    6}   �� $ continueCheck�$ PropertyChanged=�5�      �� 5�      $��1��6    �� TabStripPlacement$ Bottom=�6&     5�      ��%V   '��       �� 5�      " ��6   .�5�      ��  � ��$ Center=�6@   5�      5�      .@�5�      �� ��5�      �� 5�      $��Item Result��6    .��5�      &   System.Windows.Controls.DataGrid  -   5          Name$ 	dg_result��5�   0   $��0,0,0,0q�6      AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .     CanUserReorderColumns .  "    VerticalScrollBarVisibility$
  Auto=�5�   �   $!   HorizontalScrollBarVisibility$
! Auto=�6�   "   GridLinesVisibility$
" None=�5�   /   #   HeadersVisibility$# Column=�5�   /   $��1,1,1,1q�5�   /   $   SelectionUnit$$ FullRow=�5�   /   %   SelectionMode$% Extended=�6G   ��5�   /   �� & � ContextMenu& 5�      �� 5�      .@�5�   "   m� -   5        $��Refresh��5�   +   6<   5�      '   	CellStyle' 5�      �� 5�      *  $System.Windows.Controls.DataGridCell"  6$   .�5�   "   ��  � ��$ 0,0,1,1q�6D   �� 5�   "    � �� �	#FFB3B3B36@   �� 5�   "    � ��$ 13(�6=   �� 5�   "    � ��$ Normal"�6?   �� 5�   "    � ��$ 25��6;   �� 5�   "    � ��$ Center=�6M   5�      (   ColumnHeaderStyle( 5�      �� 5�      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6$   .�5�   "   ��  � ��$ 13(�6=   �� 5�   "    � ��$ 40��6;   �� 5�   "    � ��$
 Bold"�6?   �� 5�   "    � �� �	#FFB3B3B36@   �� 5�   "    � ��$ 0,0,0,1q�6D   �� 5�   "    � ��$ 3,0,0,0q�6<   5�      )   Columns) 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      ,  &System.Windows.Controls.DataGridColumn*   Header$* Index��61   +   Width5  /System.Windows.Controls.DataGridLengthConverter$
+ 0.2* 6@   1  +System.Windows.Controls.DataGridBoundColumn,   Binding, 6M   �� *Index+$ PropertyChanged=�- �� Mode$- TwoWay=�  5�      .   
IsReadOnly. . $* Test item name��6C   $+ 0.75* 6[   /   
FontWeight$/ SemiBold"�6�   , 6i   �� *
itemName+$ PropertyChanged=�$- TwoWay=�6�   4  .System.Windows.Controls.DataGridComboBoxColumn  5�      -   5        cbb_ischeck��$* Actived��5�   J   $
+ 0.3* 6[   0   SelectedItemBinding0 6h   �� *	isCheck+$ PropertyChanged=�$- TwoWay=�  5�      . . $* Setting value��6C   $+ * 6Z   , 6d   �� *	itemInput+$ PropertyChanged=�$- TwoWay=�  5�      . . $* Actual value��6C   $
+ 0.7* 6Y   , 6f   �� *
itemOutput+$ PropertyChanged=�$- TwoWay=�  5�      . . $* Result��6C   $
+ 0.3* 6S   , 6`   �� *
itemResult+$ PropertyChanged=�$- TwoWay=�1   	CellStyle1 5�   "   �� 5�   &   "  6,   .�5�   *   ��  � ��$ 0,0,1,1q�6L   �� 5�   *    � �� �	#FFB3B3B36H   �� 5�   *    � ��$ 15(�6E   �� 5�   *    � ��$ SemiBold"�6G   �� 5�   *    � ��$ 25��6C   �� 5�   *    � ��$ Center=�6U   �� 5�   *    � ��
 �   �6G    5�   *   �� 5�   .    6:   �� $ 
itemResult� 
Waiting...6^   .a�5�   2   ��  � ��
 � ���6O   5�   /   �� 5�   .    6:   �� $ 
itemResult�
 Passed6^   .a�5�   2   ��  � ��
 � � �6O   5�   /   �� 5�   .    6:   �� $ 
itemResult�
 Failed6^   .a�5�   2   ��  � ��
 �  ��6O   5�   /   5�   '   5�      2   RowStyle2 5�      �� 5�      )	  #System.Windows.Controls.DataGridRow" 	 6$   .�5�   "   ��  � ��
 �   �6?    5�   "   �� 5�   &    62   �� *
itemResult+
 Failed6Q   .a�5�   *   ��  � ��
 �  ��6G   5�   '   �� 5�   &    62   �� *	isCheck+ Yes6N   .a�5�   *   ��  � ��
 �����6G   5�   '   �� 5�   &    62   �� *	isCheck+ No6N   .a�5�   *   ��  � �� �#c7c7c76G   5�   '   5�      5      5     �� 5     $y�	Collapsed=�6   ��5     �� 5     $��
Log system��6    .��5	     �� -   5        3 ��Name$3 
scr_system��5	  #   .��5
     �� 4 �� 
IsReadOnly4 . $��0,0,1,2q�68   $��5,5,0,0q�6I   $��Stretch=�6[   $��Stretch=�6w   . �5     /� .T�5  "   J� .*�6,   .*�5  &   �� �6*   �� $ 	logSystem�$ PropertyChanged=�5  #   5     5     5     5     5     5     5     5     