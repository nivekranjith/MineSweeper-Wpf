   M S B A M L   `   `   `  ����F  BMineSweeper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:MineSweeperMineSweeper  5   	       MineSweeper.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   X TXceed.Wpf.Toolkit, Version=2.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      %localclr-namespace:MineSweeper  5   	     � Title$  MineSweeper��5	   	   $	��500��6   $	��525��6*   .��5      �� -   5        $��spMain��5      .+�5   
   �� ��.  $��Visible=�60   $��Visible=�6V   ��6~   ��  �� ElementName$ spMain�� �� Path$ ActualHeight���6�   �� $ spMain��$ ActualWidth�.��5      �� -   5        $��sp��5      .+�5      o� -   5         o�Name$
 menu��5      $��20��6%   .@�5      m� $��
Difficulty��6   .@�5      m� -   5        $��menuEasy��5   1   $
��Easy��6#   6A   m� 5      -   5        $��menuMed��5   3   $��Medium��6#   6B   m� 5      -   5        $��menuHard��5   1   $
��Hard��6#   6A   m� 5      -   5        $��
menuCustom��5   6   $��	Custom...��6#   .@�5      �� -   5        $	��sp1��5   )    �� Orientation$ Vertical=�64   .+�5   "   �� $��Number of Rows��6(   �� 5   "   $ 
Horizontal=�6-   .+�5   &   �� -	   5        $��	SliderRow��5   -    �� Orientation$ 
Horizontal=�6>   $	��100��6W   $��20��6d    � Minimum$ 2Q�6p    � Maximum$ 24Q�6|    � Value$ 2Q�6�   	 �� IsSnapToTickEnabled	 . 
 �� TickFrequency$
 1Q�6�   �� 5   &   ��6,   �� $ 	SliderRow��$ Value�5   #   5      �� 5      -
   5        $	��sp2��5   )   $ Vertical=�64   .+�5   "   �� $��Number of Columns��6(   �� 5   "   $ 
Horizontal=�6-   .+�5    &   �� -   5        $��	SliderCol��5    -   $ 
Horizontal=�6d   $	��100��6}   $��20��6�   $ 2Q�6�   $ 30Q�6�   $ 2Q�6�   	 . $
 1Q�6�   �� 5!   &   ��6,   �� $ 	SliderCol��$ Value�5"   #   5$      �� 5%      -   5        $	��sp3��5%   )   $ Vertical=�64   .+�5&   "   �� $��Number of Mines��6(   �� 5'   "   $ 
Horizontal=�6-   .+�5(   &   �� -   5        $��SliderMines��5(   -   $ 
Horizontal=�6@   $	��100��6Y   $��20��6f   $ 1Q�6r   $	 667Q�6~   $ 2Q�6�   	 . $
 1Q�6�   �� 5)   &   ��6,   �� $ SliderMines��$ Value�5*   #   �� 5,   "   -   5        $��	btnCustom��5,   )   $��Create��6:   6K   5.      50      51      m� 52      $
��View��6   .@�53      m� $��Colour��6#   .@�54      #  Xceed.Wpf.Toolkit.ColorPicker  -   5        $��ClrPcker��54   /   6v   56      m� 57      $
��Size��6#   .@�58      m� -   5        $��Normal��58   '   67   m� 59      -   5        $
��Zoom��59   '   65   5;      5=      5?      �� 5@      $ 
Horizontal=�6   $��
40,0,30,40q�66   .+�5A      �� -   5        $��tbMines��5A       $��30,0,-0.6,0q�6/   $	��100��6D   
���   �
���  ��$��Oblique$�6t   $��	ExtraBold"�6�   $��40(�6�   �� 5B      -   5        $��button��5B      $��80��6,   $��Center=�67   $��30,0,-0.6,0q�6T   $��80��6~   .��5C      �� $��
Smiley.png��6    $��Uniform=�65   5D      �� 5E      -   5         ��Name$ tbTime��5E       $��30,0,-0.6,0q�60   $	��110��6E   $��Right=�6Q   
���   �
���  ��$��	ExtraBold"�6�   $��25(�6�   $��Oblique$�6�   5F      � 5G      -   5        $��oGrid��5G      $��Center=�6$   $
��Auto��6A   $��0,10,0,0q�6O   $	��Top=�6a   $
��Auto��6y   
������ 5I      5L      5M      5N      5V      