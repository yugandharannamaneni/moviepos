   M S B A M L   `   `   `  ���� F  BBoxOfficeUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BoxOfficeUI.UtilBoxOfficeUI.Util  %    BoxOfficeUI.SeatLayout.HoldTray   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Z VFirstFloor.ModernUI, Version=1.0.9.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  *localclr-namespace:BoxOfficeUI.Util  .���   � 	Resources  ;   5BoxOfficeUI.Util.ColorToSolidColorBrushValueConverter )  %ColorToSolidColorBrush_ValueConverter��%   &	           � RowDefinitions �� $��0 ��� $��65 ��� $��* ��� $��41 �.+���  �� Orientation$ Vertical=�$��0��.+��� $��Center=�$��	HOLD TRAY��$��24(�$	��0,0q�� $��0,0,0,5q�$��Stretch=�$��1��.+���  J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     Color$ White��   ShadowDepth$	 1.5Q��� -    ��Name$
 Form��$ 
Horizontal=�$
��20,0q�$��Center=�$��Center=�   �� 
TargetType �� BasedOn	 �� Property
 �� Value��%�   '��       7��'�� )     7��'�� R     7���� " ��8   .��� 	 � ��$	
 150���� " ��8   .��� 	 � ��$	
 150���� " ��8   .��� 	 � ��$
 Bottom=�.+��� $ Vertical=�.+��� $�� ���� $ 
Horizontal=�$��5,0,0,0q�.+��� -    ��Name$ btnminus��$��-��$��0q�$��0��{�.  $��0,0,0,0q�(  "System.Windows.Controls.DatePicker  -     Name$ dprStartingDate��$	��120��$��5,0,0,0q�$��1���� -   $ btnplus��$��+��$��0q�$��2��$��5,0,10,0q��� $ Vertical=�.+��� $��Movie�� �� Target ��  �� ElementName$ cmbMovie���� -    ��Name$ cmbMovie��$��3���� $ Vertical=�.+��� $��	MovieTime�� �� $ cmbMovieTime���� -   $ cmbMovieTime��$	��100��$��4���� $ Vertical=�.+��� $��Screen�� �� $ 	cmbScreen���� -   $ 	cmbScreen��$��5���� $ Vertical=�.+��� $��No Of Seats�� �� $ 	cmbScreen���� -   $ cmbSeatsCount��$��6��$��50���� $ Vertical=�.+��� 
������ $��hi���� $ 
Horizontal=�.+��� -	   $ btnBlockSeats��$��	Hold Tray��
 � Tag$ 1��$��7��$
��Left=�{�.  �� -
   $ btnClearSeats��$��Clear��$��8��$
��Left=�$��0q�{�.  �� -    ��Name$ 	lblAmount��$
��10,0q�$��18(�$��
Amount : 0���� $
��Auto=�$
��Auto=�$��2��.��� $��Stretch=�.+��� -    ��Name$ itmSeats��$��0��$��0��$
��20,0q����� *Seats+���� .R��� $��2q����� *	SeatWidth+���� *
SeatHeight+ �� *SeatOrientation+.+�d�  d�Name$ tglSeat��-   $
��Bold"�$��12(�
������
������  � Cursor$
 Hand��
�������$��Center=�$��Center=����� *	IsEnabled+y��� *CheckboxVisibility+ J� IsHitTestVisible �� *	IsEnabled+����  �� Path$ DisplayText� �� Mode$ TwoWay=� d� 	IsChecked �� *	IsChecked+$ TwoWay=� �� ���� " d�.��� 	 �@�OverridesDefaultStyle
 . �� 	 � ��
 ��  �� 
TargetType" d�.R�� .+���  ��Name$ BorderCircle������  �� Color$ Transparent���� $ 
BodyCircle��$��0q����� $ Transparent���� $
 Mask��$��25������ *	SeatWidth+ J� OpacityMask !�  j� Stretch$
 Fill=���  ��Name$ content��$��Center=�$��Center=� �� Triggers P�   P� Property  �@d�	IsChecked! P� Value$
! TrueY�. ��� " �� 
TargetName$" 
BodyCircle��
 �� $ 	SeatColor�$ TwoWay=�# �� 	Converter## [  	 � ���� $" BorderCircle��	 � ��

 ������� $
" Mask��	 � ��

 �   �P�   �@J�IsMouseOver! . . ��� $" 
BodyCircle��
 �� $ 	SeatColor�$ TwoWay=�## [  	 � ���� $" BorderCircle��	 � ��

 ������� $
" Mask��	 � ��

 �   ����� .R�� �� $��3��$ 
Horizontal=�$��Center=�$��Center=�.+��� $��20��$��18������ $ #FF6347���� $��Reserved Seats��$��5,0,10,0q��� $��20��$��18������ $ #FFA500���� $��Blocked Seats��$��5,0,10,0q��� $��20��$��18������ $ #DCDCDC���� $��Selected Seats��$��5,0,10,0q�=  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  -   $  Name$$ pgrProgress��%   IsActive% . 
��� ���$��0��$��3��$��80��$��80��  CircleProgressRingStyle#��[ 