   M S B A M L   `   `   `  ����F  BBoxOfficeUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BoxOfficeUI.UtilBoxOfficeUI.Util  5      ,    &BoxOfficeUI.SeatLayout.ManagerBlocking   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.9.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      *localclr-namespace:BoxOfficeUI.Util  5      .��5
      �   � 	Resources  5   
   ;   5BoxOfficeUI.Util.ColorToSolidColorBrushValueConverter )  %ColorToSolidColorBrush_ValueConverter��%   &	          5      6:   5       � RowDefinitions 5   
   �� 5      $��0 �6   6)   �� 5      $��65 �6   6*   �� 5      $��* �6   6)   .+�5   
   ��  �� Orientation$ Vertical=�6   $��0��6,   .+�5      �� $��Center=�6   $��MANAGER BLOCKING��65   $��24(�6M   $	��0,0q�6[   5      � 5   
   $��0,0,0,5q�6   $��Stretch=�6    $��1��6>   .+�5      ��  J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ White��6'      ShadowDepth$	 1.5Q�65   5      �� 5       -   5         ��Name$
 Form��5       $ 
Horizontal=�6'   $
��20,0q�6@   $��Center=�6O   $��Center=�6j     5!       �� 
TargetType �� BasedOn	 �� Property
 �� Value��%  '��       7��'�� N     7��'�� �     7���� 5"      " ��6   8   .�5#      �� 	 � ��$	
 150��62   5$      �� 5&      " ��6   8   .�5'      �� 	 � ��$	
 150��62   5(      �� 5*      " ��6   8   .�5+      �� 	 � ��$
 Bottom=�6>   5,      5-      .+�5.      �� $ Vertical=�6   .+�5/      �� $��Screen��6    �� Target 6-   ��  �� ElementName$ 	cmbScreen���� 50      -   5         ��Name$ 	cmbScreen��50      62   51      �� 53      $ Vertical=�6   .+�54      �� $��Vendor��6    6-   �� $ 	cmbVendor��6Y   �� 55      -   5        $ 	cmbVendor��55       �� SelectedIndex$ 0��62   ��56      5  -System.Windows.Controls.ItemContainerTemplate�  57      .R�58   "   �� $ 
Horizontal=�6-   .+�59   &   �� $��20��60   $��20��6;   $��0,0,5,0q�6G   ��6X   ��  �� Path$ Color� �� 	Converter# [  6�   �� 5:   &   �� 62   �66   �� *
VendorName+6U   5;   #   5<      5>      5?      �� 5A      $ Vertical=�6   .+�5B      �� $��Template��6    6/   �� $ txtTemplate��6\   �� 5C      -   5         ��Name$ txtTemplate��5C      66   5D      �� 5F      -   5         ��Name$ btnBlockSeats��5F   7   $��Block Seats��6   
 � Tag$ 1��6/   $
��Left=�6N   6i   �� 5H      -   5        $ btnUnBlockSeats��5H   9   $��UnBlock Seats��6   $ 0��61   $
��Left=�6R   6m   5J      5K      �� 5M   
   $
��Auto=�6   $
��Auto=�6:   $��2��6_   .��5N      � $��Stretch=�6   .+�5O      �� -   5         ��Name$ itmSeats��5O      $��0��6O   $��0��6\   $
��20,0q�6l   ��61   �� *Seats+��5P      �� 5Q      .R�5R      �� $��2q�6�   ��6)   �� *	SeatWidth+��6E   �� *
SeatHeight+ 6c   �� *SeatOrientation+.+�5S   "   d�  d�Name$ tglSeat��6/   -   5        $
��Bold"�5S   B   $��12(�6T   
������
������  � Cursor$
 Hand��5T   D   
�������$��Center=�6e   $��Center=�6�   y�5S   c   �� *CheckboxVisibility+ J� IsHitTestVisible 6�   �� *	IsEnabled+��5T   �   �� $ DisplayText� �� Mode$ TwoWay=� d� 	IsChecked 5U   /   �� *	IsChecked+$ TwoWay=� 6\   �� ��5V   &   �� 5W   *   " d�60   .�5X   .   �� 	 �@�OverridesDefaultStyle
 . 6V   �� 5Y   .   	 � ��
 5Z   2   �� 5[   6    �� 
TargetType" d�6F   .R�5\   :   � .+�5]   >   ��  ��Name$ BorderCircle��6H   ��5^   B   �� 5_   F    �� Color$ Transparent��6V   5a   ?   �� 5c   >   $ 
BodyCircle��6I   $��0q�6]   ��5d   B   �� 5e   F   $ Transparent��6V   5g   ?   �� 5h   >   $
 Mask��6H   $��25��6r   ��6V   �� *	SeatWidth+ J� OpacityMask 5i   B   !� 5j   F    j� Stretch$
 Fill=�6R   5l   ?   �� 5m   >    ��Name$ content��6O   $��Center=�6`   $��Center=�6}   5n   ;     �� Triggers  5o   :   P� 5p   >   ! P� Property! �@d�	IsChecked" P� Value$
" TrueY�6[   . �5q   B   �� # �� 
TargetName$# 
BodyCircle��6I   
 6q   �� $ 	SeatColor�$ TwoWay=�# [  	 � ���� 5r   B   $# BorderCircle��6I   	 � ��

 �����6s   �� 5s   B   $
# Mask��6I   	 � ��

 �   �6k   5t   ?   P� 5u   >   ! �@J�IsMouseOver" . . �5v   B   �� $# 
BodyCircle��6I   
 6q   �� $ 	SeatColor�$ TwoWay=�# [  	 � ���� 5w   B   $# BorderCircle��6I   	 � ��

 �����6s   �� 5x   B   $
# Mask��6I   	 � ��

 �   �6k   5y   ?   5{   7   5}   /   5~   +   5�   #   5�      5�      ��5�      �� 5�      .R�5�      � 5�      5�      5�      5�      =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5�   
   -	   5        $  Name$$ pgrProgress��5�   !   %   IsActive% . 
��� ���$��0��6[   $��3��6h   $��80��6y   $��80��6�     CircleProgressRingStyle#��[ 6�   5�      5�      