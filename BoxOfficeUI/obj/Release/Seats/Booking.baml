   M S B A M L   `   `   `  ���� F  BBoxOfficeUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BoxOfficeUI.UtilBoxOfficeUI.Util  $    BoxOfficeUI.SeatLayout.Booking   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Z VFirstFloor.ModernUI, Version=1.0.9.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  *localclr-namespace:BoxOfficeUI.Util  .���   � 	Resources  ;   5BoxOfficeUI.Util.ColorToSolidColorBrushValueConverter )  %ColorToSolidColorBrush_ValueConverter �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value��%�   &	        '��        �� " ��.���  �@�Cursor$
 Hand�� P�  �@��
IsSelected . . ���  � �� �#31708EP�  �@J�	IsFocused . . ���  � ��
 ����� � RowDefinitions �� $��0 ��� $��* ��� $��40 � � ColumnDefinitions �� $	��220 ��� $��* �.+��� 	 �� Orientation$	 Vertical=�$��0��$y�	Collapsed=�.+��� $��Center=�$��BOOCKING��$��24(�$	��0,0q�� $��Stretch=�$��1�� �� $��40 ��� $��* ��� $��* �.+��� $��0,0,1,1q�$��0��$��0,5,0,0q�$��40��._��� $	 
Horizontal=�$��5,0,0,10q�.+��� -   
 ��Name$
 btnminus��$��-��$��0q�$��0��{�.  $��0,0,0,0q�$	��Top=�(  "System.Windows.Controls.DatePicker  -     Name$ dprStartingDate��$	��120��$��5,0,0,0q�$��28��$��1���� -   $
 btnplus��$��+��$��0q�$��2��$��5,0,0,0q�$	��Top=��� $��0,1,1,1q�$��1��$��0,0,0,0q�
�������._��� -    ��Name$ lstMovie�� �� SelectedIndex$ 0�� �� SelectedValuePath$ Id��$��3��$��1������ .R��� $
��Wrap=�$��14(�$
��Bold"����� *	MovieName+���� " ��.���  � ��$ 5q���  � ��$ 5,5,0,0q���  � ��$	 180����  � ��$
 Left=���  � ��
 ������� $��0,0,1,1q�$��2��
�������._��� -   $ lstTimes��$ 0��$ Id��$��4��$��2������ .R��  � Orientation$ 
Horizontal=�$	��180��$
��Left=����� .R��� $
��Wrap=�$��14(�$
��Bold"����� *S_Date+���� " ��.���  � ��$ 5q���  � ��$ 5,5,0,0q���  � ��$ 80����  � ��$
 Left=���  � ��
 ������� $��0,0,1,0q�$��3��
�������$y�	Collapsed=�._��� -   $ 	lstScreen��$ 0��$ Id��$��5��$��3������ .R��� $
��Wrap=�$��15(����� *
ScreenName+���� " ��.���  � ��$ 5q���  � ��$ 5,5,0,0q���  � ��$	 180����  � ��$
 Left=���  � ��
 ������� $��0,0,0,1q�$��1��$��1��$��0,0,0,0q�
�������._�� $��0,0,5,0q�$��Stretch=�$��1��$��1�� �� $��41 ��� $��* ��� $��41 �.+��� $��0,0,0,1q�$��0��
�������._�� $��Stretch=�$��0,0,0,0q� �� $	��250 ��� $��2* �.+��� $	 
Horizontal=�$��0��.+��� -   $ lstSeats��$
��Auto=�$��6��$
��Left=�$��1��$��16(�
�������$
��Bold"����� .R��  K� IsItemsHost . ���� " ��.���  � ��$	 0,5q���  � ��$ 5,5,0,0q���  � ��$ 15(���  � ��$ 20����  � ��$ Center=��� -    ��Name$ 
stkBooking��$	 
Horizontal=�$��1��$��Right=�$��Center=�$y�	Collapsed=�.+��� -	    ��Name$ txbTotalAmount��$��18(�$��5q�
��������� -
   $ 	txbAmount��$��18(�$
��10,5q�
��������� -   $
 btnBlockSeats��$��
Book Seats��$
��Left=�$��0,0,10,0q��� -   $
 btnClearSeats��$��Clear��$
��Left=�$��0q��� $
��Auto=�$
��Auto=�$��1��.��� $��Stretch=�.+��� -    ��Name$ itmSeats��$��0��$��0��$
��20,0q����� *Seats+���� .R��� $��2q����� *	SeatWidth+���� *
SeatHeight+	 �� *SeatOrientation+.+�d�  d�Name$ tglSeat��-   $
��Bold"�$��12(�
������
������  � Cursor$
 Hand��
�������$��Center=�$��Center=�y��� *CheckboxVisibility+ J� IsHitTestVisible �� *	IsEnabled+���� *	IsEnabled+����  �� Path$ DisplayText� �� Mode$ TwoWay=� d� 	IsChecked �� *	IsChecked+$ TwoWay=�
 � Tag �� ���� " d�.���  �@�OverridesDefaultStyle . ��  � �� ��  �� 
TargetType" d�.R�� .+���  ��Name$ BorderCircle������  �� Color$ Transparent���� $ 
BodyCircle��$��0q����� $ Transparent���� $
 Mask��$��25������ *	SeatWidth+ J� OpacityMask !�  j� Stretch$
 Fill=���   ��Name$  content��$��Center=�$��Center=�! �� Triggers! P�  �@d�	IsChecked$
 TrueY�. ��� " �� 
TargetName$" 
BodyCircle�� �� $ 	SeatColor�$ TwoWay=�# �� 	Converter## [   � ���� $" BorderCircle�� � ��
 ������� $
" Mask�� � ��
 �   �P�  �@J�IsMouseOver . . ��� $" 
BodyCircle�� �� $ 	SeatColor�$ TwoWay=�## [   � ���� $" BorderCircle�� � ��
 ������� $
" Mask�� � ��
 �   ����� .R�� �� $��2��$	 
Horizontal=�$��Center=�$��Center=�.+��� $��20��$��18������ $ #FF6347���� $��Reserved Seats��$��5,0,10,0q��� $��20��$��18������ $ #FFA500���� $��Blocked Seats��$��5,0,10,0q��� $��20��$��18������ $ #DCDCDC���� $��Selected Seats��$��5,0,10,0q��� 
���2͚�$��0��$��2��$��2��$ �� Padding$
$ 10,0q�._��� $	 
Horizontal=�$��Center=�$��35��.+��� -   $ 	txbScreen��$��20(�$��5q�
�������$��Center=�$��Center=��� -   $ 	brdClass1��$��|��$��5q�$��24(�
�������$��Center=��� -   $ 	txbClass1��$��18(�$��5q�
�������$
��Left=�$��Center=�$
��Wrap=��� -   $ 	brdClass2��$��|��$��5q�$��24(�$y�	Collapsed=�
�������$��Center=��� -   $ 	txbClass2��$��18(�$��5q�
�������$
��Left=�$��Center=�$
��Wrap=��� -   $ 	brdClass3��$��|��$��24(�$y�	Collapsed=�
�������$��Center=��� -   $ 	txbClass3��$��18(�$��5q�
�������$
��Left=�$��Center=�$
��Wrap=�=  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing  -   %  Name$% pgrProgress��&   IsActive& . $��3��
��� ���$��3��$��80��$��80��  CircleProgressRingStyle#��[ 