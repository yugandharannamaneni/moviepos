   M S B A M L   `   `   `  ����F  BBoxOfficeUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BoxOfficeUI.UtilBoxOfficeUI.Util  5      )    #BoxOfficeUI.SeatLayout.LayoutDesign   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Z VFirstFloor.ModernUI, Version=1.0.9.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      .	Converterclr-namespace:BoxOfficeUI.Util  5        � 	Resources  5	      2   ,BoxOfficeUI.Util.StringToVisibilityConverter   StringToVisibilityConverter��%   &	          5
   
   6V   5      .��5      �  � RowDefinitions 5   
   �� 5      $��0 �6   6)   �� 5      $��65 �6   6*   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   .+�5   
   ��  �� Orientation$ Vertical=�6   $��0��6,   .+�5      �� $��Center=�6   $��LAYOUT��65   $��24(�6C   $	��0,0q�6Q   5      � 5   
   $��0,0,0,5q�6   $��Stretch=�6    $��1��6>   .+�5      ��  J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ White��6'      ShadowDepth$	 1.5Q�65   5       �� 5"      -   5         ��Name$
 Form��5"      $ 
Horizontal=�6'   $��1��6@   $��Center=�6M   $��Center=�6h     5#       �� 
TargetType �� BasedOn	 �� Property
 �� Value��%  '��       7��'�� N     7��'�� �     7���� 5$      " ��6   8   .�5%      �� 	 � ��$	
 150��62   5&      �� 5(      " ��6   8   .�5)      �� 	 � ��$
 50��62   5*      �� 5,      " ��6   8   .�5-      �� 	 � ��$
 Bottom=�6>   5.      5/      .+�51      �� $ Vertical=�6   .+�52      �� $��Screen��6    �� Target 6-   ��  �� ElementName$ 	cmbScreen��6X   �� 53      -   5         ��Name$ 	cmbScreen��53      62   54      �� 56      $ Vertical=�6   .+�57      �� $��Class��6    6,   �� $ cmbClass��6V   �� 58      -   5        $ cmbClass��58      61   59      �� 5;      $ Vertical=�6   .+�5<      �� $��	Direction��6    60   �� $ cmbSeatingDirection��6e   �� 5=      -   5        $ cmbSeatingDirection��5=      5>      �� 5@      $ Vertical=�6   .+�5A      �� -   5         ��Name$ lblRows��5A      $
��Rows��6-    6<   �� $ txtRows���� 5B      -   5         ��Name$ txtRows��5B       �� 	MaxLength$ 2��6/   6k   5C      �� 5E      $ Vertical=�6   .+�5F      �� -   5        $ 
lblColumns��5F      $��Columns��60    6B   �� $ 
txtColumns��6n   �� 5G      -   5        $ 
txtColumns��5G      $ 2��62   6n   5H      �� 5J      -	   5         ��Name$ btnGenerate��5J   ,   $��Generate��6   6A   �� 5K      -
   5        $ btnSave��5K   (   $
��Save��6   69   �� 5M      -   5        $ 	btnDelete��5M   *   $��Delete��6   6=   5N      5O      �� 5Q   
   $
��Auto=�6   $
��Auto=�6:   $��2��6_   .��5R      � $��Stretch=�6   .+�5S      �� -   5         ��Name$ itmSeats��5S      $��5q�6a   ��61   ��  �� Path$ Seats� �� Mode$ TwoWay=���5T      �� 5U      .R�5V      �� ��6)   �� *	SeatWidth+��6E   �� *
SeatHeight+ 6c   �� *SeatOrientation+.+�5W   "   �� -   5        $��25��5W   �   y�6+   �� *CheckboxVisibility+ J� IsHitTestVisible 6U   �� *	IsEnabled+ d� 	IsChecked 6�   �� *	IsChecked+$ TwoWay=�
 � Tag 6�   ��   5X   &    �� 
TargetType �� 
Background �� Bullet ��Name �� CornerRadius F�Name J� SnapsToDevicePixels �� Color  �� Triggers! P� Property" P� Value# �� 
TargetName��%�  '��       �� 5Y   *   " ��6J   .�5Z   .   �� 	 �@J�SnapsToDevicePixels
 . 6T   �� 5[   .   	 �@�OverridesDefaultStyle
 . 6V   �� 5\   .   	 � ��$
 2,2,0,0q�6G   �� 5]   .   	 � ��
 5^   2   �� 5_   6   " ��6F   .R�5`   :   �� 
 ����  5a   >   �� 5b   B   $ Border��6I   $��15��6Y   $��15��6d   $ 0��6p   
������ $��0q�6�   ���#32CD32._�5c   F   F� $ 	CheckMark��6L    .  $��15��6{   $��1q�6�   +����        ���j ��j �    ��j ���j     	��5d   J   �� 5e   N    6^   �� *	SeatColor+5g   G   5h   C   ._�5j   >   �� $��4,0,0,0q�6O   $��Center=�6`   $
��Left=�6{   ��. 6�   5k   ;     5l   :   P� 5m   >   ! �@d�	IsChecked$" falseY�6[   . �5n   B   �� $# 	CheckMark��6I   	 � y�$
 	Collapsed=�6v   5o   ?   P� 5p   >   " 6[   Z� ! �@d�	IsChecked. �5q   B   �� $# 	CheckMark��6I   	 � ��+
 ��        ���j ��j �    ��j ���j     	6p   5r   ?   P� 5s   >   ! �@J�IsMouseOver" . . �5t   B   �� $# Border��6I   	 � ��$
 1q�6x   �� 5u   B   $# Border��6I   	 � ��
 �#32CD326t   5v   ?   P� 5w   >   ! � ��" . . �5x   B   �� $# Border��6I   	 � ��
 �#E0E0E06s   �� 5y   B   $# Border��6I   	 � ��
 �#6060606t   5z   ?   P� 5{   >   ! � {�" .  . �5|   B   �� $# Border��6I   	 � ��
 �#EEEEEE6s   �� 5}   B   $# Border��6I   	 � ��
 �#AAAAAA6t   �� 5~   B   	 � ��
 �#8888886_   5   ?   5�   7   5�   /   5�   +   5�   '   5�   #   �� 5�   "   
�������y�6,   �� *CheckboxVisibility+��6V   �� $ DisplayText�$ TwoWay=�6�   5�      5�      ��5�      �� 5�      .R�5�      � 5�      5�      �� 5�      $��Center=�6   $��Center=�6:   .+�5�      �� -   5        $ ��Name$$ txbLayoutMessage��5�       $��24(�6:   
�������\ZLayout already generated for this screen class, Please delete if you want to change layout6�   5�      5�      5�      �� 5�   
   -   5        $$ 
txbMessage��5�      
���  ��$��3��69   $��Center=�6F   $��20(�6c   $��0��6q   6~   =  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  5�   
   -   5        %  Name$% pgrProgress��5�   !   &   IsActive& . $��0��6F   
��� ���$��3��6g   $��80��6x   $��80��6�     CircleProgressRingStyle#��[ 6�   5�      5�      