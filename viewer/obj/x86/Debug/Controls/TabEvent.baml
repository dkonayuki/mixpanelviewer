   M S B A M L   `   `   `  ����U  QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35[<clr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkitMicrosoft.Windows.Controls  5      I EMixPanelGetter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:MixPanelViewer.ControlsMixPanelViewer.Controls 5      &    MixPanelViewer.Controls.TabEvent   5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      Hy<clr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit    5      8Controls%clr-namespace:MixPanelViewer.Controls    5        � 	Resources  5
          BooleanToVisibilityConverter��%   &	        �� 5   
   6'   5      .��5      � -   5         �Name$ grid_Tab��5       � ColumnDefinitions 5   
   �� 5      $	��200 �6   6-   �� 5      6!   .+�5   
   �� -   5        $��0��5      $��1��6'   
������� �� ResizeDirection$ Columns=�6O   $
��Auto��6i   $��4��5      $��Stretch=�6#   $��0q�6?   6J   �� 5   
   $��2��6r   $��Stretch=�5      $��Stretch=�62    K� ZIndex$ 1��6N   y�5      ��  �� Path$ 	IsLoading� �� 	Converter# [  5   N   .  (MixPanelViewer.Controls.LoadingAnimation  5   
   $��2��6�   $��Center=�5   $   $��Center=�6A   $ 2��6\   y�5   $   �� $ 	IsLoading�# [  5   \   )   #Microsoft.Windows.Controls.DataGrid  5   
   -   5          Name$ list_Reports��5      $��1��6+   $   HorizontalScrollBarVisibility$
 Auto=�6;   "	   VerticalScrollBarVisibility$
	 Auto=�5      
   CanUserResizeRows
 .    RowHeaderWidth$ 25Q�6Q      CanUserDeleteRows .    AutoGenerateColumns .     CanUserAddRows .  ��5      �� 5      ]� 5   
   $��0��6   $��5q�6$   .+�5      �� $	��Top=�6   .+�5      �� $��MixPanelViewer��6   $��Comic Sans MS)�61   $��22(�6L   $��Center=�5       $��Center=�67   $��70��6T   6b   �� 5!      $��
Event Type��6   6/   � 5"      -   5        $ event_Panel��5"       5#      �� 5$      $��* �6+   67   �� 5%      $
��Auto �6+   6:   .+�5'      �� -   5         ��Name$ 	box_Event��5'   /   $��0��6   $��23��6B   $��5q�6N   �� 5(      -   5         ��Name$ btn_Refresh��5(   -   $��1��6   $��5q�6B   $��25��6M   $y�	Collapsed=�5)      
�������.��5*      �� $��../Assets/refresh.png��6     J� SnapsToDevicePixels . 6\   5+      5,      �� 5-      $
��From��6   $��28��6'   +   %Microsoft.Windows.Controls.DatePicker  5.      -   5          Name$ 	date_From��5.      $	��Top=�62   $��5q�6J   �� 5/      $��To��6   $��28��6%     50      -   5        $ date_To��50      $	��Top=�60   $��5q�6H   �� 51      
���   �$��0,10,0,5q�6-    �� Padding$ 2q�6?   $��1q�6K   ._�52      9� -	   5         9�Name$ panel_Filter��52      $��Filter��65   .��53      �� .+�54      �� -
   5         ��Name$ list_Filter��54   &   $��5q�6�   $	��100��6�   6�   � 55       56   "   �� 57   &   69   �� 58   &   69   .+�5:   "   �� -   5        $ btn_Add��5:   )   $��0��6:   $��60��6J   $��29��6U    . .��5;   &   ��  �� Orientation$ 
Horizontal=�61   .+�5<   *   �� $��../Assets/add.png��60   6M   �� 5=   *   $	��Add��64   $��Center=�6?   5>   '   5?   #   �� 5@   "   -   5        $ 
btn_Remove��5@   )   $��1��6=   $��78��6M   $��29��6X    . {�6   �� $ 	CanRemove�.��5A   &   �� $ 
Horizontal=�61   .+�5B   *   �� $��../Assets/delete.png��60   $��3q�6N   6[   �� 5C   *   $��Remove��64   $��Center=�6B   5D   '   5E   #   5F      5G      5H      5I      �� 5J      -   5        $��btn_View��5J   4   $
��View��6   $��30��6(   $��10q�6X   $��75��6d   $��16(�6o   $��Tahoma)�6}   5K      �� 5L      $��3q�6   $��80��6)   
�������$��Bottom=�6M   ._�5M      �� $ 
Horizontal=�6   $��Center=�66   .+�5N      �� $��Total��6   $��5q�6,   $��Center=�67   $��24(�6R   6b   �� 5O      -   5         ��Name$ 	txt_Total��5O       $��5q�63    �� TextAlignment$ Center=�6>   $��Center=�6U   $��24(�6p   6�   5P      5Q      5R      5S      5T      