   M S B A M L   `   `   `  ���� F  BBoxOfficeUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BoxOfficeUI.UtilBoxOfficeUI.Util  )    #BoxOfficeUI.SeatLayout.LayoutDesign   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Z VFirstFloor.ModernUI, Version=1.0.9.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  .	Converterclr-namespace:BoxOfficeUI.Util    � 	Resources  2   ,BoxOfficeUI.Util.StringToVisibilityConverter   StringToVisibilityConverter��%   &	          .���  � RowDefinitions �� $��0 ��� $��65 ��� $��* ��� $
��Auto �.+���  �� Orientation$ Vertical=�$��0��.+��� $��Center=�$��LAYOUT��$��24(�$	��0,0q�� $��0,0,0,5q�$��Stretch=�$��1��.+���  J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     Color$ White��   ShadowDepth$	 1.5Q��� -    ��Name$
 Form��$ 
Horizontal=�$��1��$��Center=�$��Center=�   �� 
TargetType �� BasedOn	 �� Property
 �� Value��%�   '��       7��'�� )     7��'�� Q     7���� " ��8   .��� 	 � ��$	
 150���� " ��8   .��� 	 � ��$
 50���� " ��8   .��� 	 � ��$
 Bottom=�.+��� $ Vertical=�.+��� $��Screen�� �� Target ��  �� ElementName$ 	cmbScreen���� -    ��Name$ 	cmbScreen���� $ Vertical=�.+��� $��Class�� �� $ cmbClass���� -   $ cmbClass���� $ Vertical=�.+��� $��	Direction�� �� $ cmbSeatingDirection���� -   $ cmbSeatingDirection���� $ Vertical=�.+��� -    ��Name$ lblRows��$
��Rows�� �� $ txtRows���� -    ��Name$ txtRows�� �� 	MaxLength$ 2���� $ Vertical=�.+��� -   $ 
lblColumns��$��Columns�� �� $ 
txtColumns���� -   $ 
txtColumns��$ 2���� -	    ��Name$ btnGenerate��$��Generate���� -
   $ btnSave��$
��Save���� -   $ 	btnDelete��$��Delete���� $
��Auto=�$
��Auto=�$��2��.��� $��Stretch=�.+��� -    ��Name$ itmSeats��$��5q�����  �� Path$ Seats� �� Mode$ TwoWay=����� .R��� ���� *	SeatWidth+���� *
SeatHeight+ �� *SeatOrientation+.+��� -   $��25��y��� *CheckboxVisibility+ J� IsHitTestVisible �� *	IsEnabled+ d� 	IsChecked �� *	IsChecked+$ TwoWay=�
 � Tag ��    �� 
TargetType �� 
Background �� Bullet ��Name �� CornerRadius F�Name J� SnapsToDevicePixels �� Color  �� Triggers! P� Property" P� Value# �� 
TargetName��%�  '��       �� " ��.��� 	 �@J�SnapsToDevicePixels
 . �� 	 �@�OverridesDefaultStyle
 . �� 	 � ��$
 2,2,0,0q��� 	 � ��
 �� " ��.R��� 
 ����  �� $ Border��$��15��$��15��$ 0��
������ $��0q����#32CD32._�F� $ 	CheckMark�� .  $��15��$��1q�+����        ���j ��j �    ��j ���j     	����  �� *	SeatColor+._��� $��4,0,0,0q�$��Center=�$
��Left=���.   P� ! �@d�	IsChecked$" falseY�. ��� $# 	CheckMark��	 � y�$
 	Collapsed=�P� " Z� ! �@d�	IsChecked. ��� $# 	CheckMark��	 � ��+
 ��        ���j ��j �    ��j ���j     	P� ! �@J�IsMouseOver" . . ��� $# Border��	 � ��$
 1q��� $# Border��	 � ��
 �#32CD32P� ! � ��" . . ��� $# Border��	 � ��
 �#E0E0E0�� $# Border��	 � ��
 �#606060P� ! � {�" .  . ��� $# Border��	 � ��
 �#EEEEEE�� $# Border��	 � ��
 �#AAAAAA�� 	 � ��
 �#888888�� 
�������y��� *CheckboxVisibility+���� $ DisplayText�$ TwoWay=����� .R�� �� $��Center=�$��Center=�.+��� -   $ ��Name$$ txbLayoutMessage��$��24(�
�������\ZLayout already generated for this screen class, Please delete if you want to change layout�� -   $$ 
txbMessage��
���  ��$��3��$��Center=�$��20(�$��0��=  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  -   %  Name$% pgrProgress��&   IsActive& . $��0��
��� ���$��3��$��80��$��80��  CircleProgressRingStyle#��[ 