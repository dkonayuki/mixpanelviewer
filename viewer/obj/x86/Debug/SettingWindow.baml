   M S B A M L   `   `   `  ����  MixPanelGetter"    MixPanelViewer.SettingWindow   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	     � Title$  Settings��5   	   $	��160��6   $	��400��6'    � WindowStartupLocation$ CenterScreen=�63    � Icon$9 3/MixPanelGetter;component/Assets/mixpanelviewer.ico��5   	    � 
ResizeMode$ NoResize=�5   	   .��5      �� .+�5   
   �  � RowDefinitions 5	      �� 5
      6"   �� 5      6"    � ColumnDefinitions 5      �� 5      $	��100 �6#   61   �� 5      6%   .+�5      �� $��API Key��6   $��0��6&   $��0��66   $��10,0,0,0q�6C   $
��Left=�6U   $��Center=�6p   6�   �� 5      -   5         ��Name$ txt_API_Key��5      $��1��6+   $��0��6;   $��10q�6H   6V   �� 5      $��
API Secret��6   $��0��6)   $��1��69   $��10,0,0,0q�6F   $
��Left=�6X   $��Center=�6s   6�   �� 5      -   5        $ txt_API_Secret��5      $��1��6.   $��1��6>   $��10q�6K   6Y   5      �� 5   
    �� Orientation$ 
Horizontal=�6   $��Center=�6.   .+�5      �� -   5         ��Name$ btn_OK��5      $��60��6%   $��5,5,20,5q�60   	 �� 	IsDefault	 . .��6e   OK6i   �� 5      -   5        $ 
btn_Cancel��5      $��60��6*   $��20,5,5,5q�65   
 �� IsCancel
 . .��6m   Cancel6u   5      5      5      