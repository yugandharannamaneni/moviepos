   M S B A M L   `   `   `  ���� F  BBoxOfficeUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:BoxOfficeUI.CalenderViewBoxOfficeUI.CalenderView  '    !BoxOfficeUI.SeatLayout.Scheduling   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6cal&clr-namespace:BoxOfficeUI.CalenderView     Z VFirstFloor.ModernUI, Version=1.0.9.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI    � 	Resources   �� 
TargetType-  'System.Windows.Input.KeyboardNavigation �� Property �� Value �� Triggers P� Property P� Value��%�   '��       '�� 7     �� " ��.���  �@ TabNavigation$ Continue=��� " ��.���  �@�Cursor$
 Hand�� P�  �@��
IsSelected . . ���  � �� �#31708EP�  �@J�	IsFocused . . ���  � ��
 �����.���  � ColumnDefinitions �� $	��150 ��� $��* �.+�� $��0��$��0,0,5,0q�.+��� -    ��Name$ 
lstScreens��
������ 	 �� SelectedIndex$	 0��
 �� SelectedValuePath$
 Id��$��0������ .R��� $
��Wrap=�$��14(�$
��Bold"����� *
ScreenName+���� " ��.���  � ��$ 5q���  � ��$ 5,5,0,0q���  � ��$	 130����  � ��$
 Left=���  � ��
 ������ -    �Name$ calanderGrid��$��1��$y�	Collapsed=�.+�(   "BoxOfficeUI.CalenderView.MonthView  -     Name$ SchedulCalendar��(   "BoxOfficeUI.CalenderView.MonthView   DisplayMonthChanged$) #SchedulCalendar_DisplayMonthChanged��   SchedulingDblClicked$* $SchedulCalendar_SchedulingDblClicked��   DayBoxDoubleClicked$) #SchedulCalendar_DayBoxDoubleClicked��$��Stretch=�$��Stretch=�$y�Visible=�� -   $ 	ctrlsGrid��$��1��$	��500��$y�	Collapsed=�.+�� $��20,50q�   �� BasedOn(  "System.Windows.Controls.DatePicker��%�   '��       7��'  (     7 '��      7���� " ��8   .���  � ��$ 42���� "  8   .���  � ��$	 180����  � ��$ 25����  � ��$
 Left=��� " ��8   .���  � ��$ 80�� �� $	��100 ��� $��* � � RowDefinitions �� $��0 ��� $��0 ��� $��40 ��� $��40 ��� $	��120 ��� $��40 ��� $��0 ��� $
��Auto ��� $
��Auto ��� $��40 �.+��� $��Movie��$��0��$��2�� �� Target ��  �� ElementName$ cmbMovie���� -    ��Name$ cmbMovie��$��1��$��2��$	��180��$��10���� $��Starting Date��$��0��$��3�� �� $ dprStartingDate��  -     Name$ dprStartingDate��$��1��$��3��$��11���� $
��Time��$��0��$��4�� �� $ 	cmbScreen���� $��1��$��4�� �� Orientation$ Vertical=�$��Center=�.+��� $ 
Horizontal=�.+��� $	��Hrs�� �� $ cmbHours���� -   $ cmbHours��$��1��$��2�� �� 
IsEditable .  w� TextPath$ Content��$��12���� $	��Mns�� �� $ 	cmbMinuts���� -   $ 	cmbMinuts��$��1��$��2�� . $ Content��$��13���� -	    ��Name$ 
btnAddTime��$��Add Time��$��14��$��0q�$��0,0,10,0q��� -
   $ btnTimeClear��$��Clear��$��0q�$��0,0,10,0q��� $ 
Horizontal=�$��0,10,0,0q��� -    ��Name$ txtShowTimes����.  �� 	MaxLength$ 50��$��0,10,0,0q�$	��300��$��50��{�.  �� $
��Days��$��0��$��5�� �� $ txtDaysCount���� $��1��$��5��$ 
Horizontal=�.+��� -   $ btnminus��$	�� - ��$��0q� J� IsHitTestVisible .  $��0,0,10,0q�$��15���� -   $ txtDaysCount��$��1��$��5��$��1��$��14����.  {�.  $ 2��$��0q�$��Center=��� -   $ btnplus��$	�� + ��$��0q�$��10,0,0,0q�$��16���� $��Internet Sales��$��0��$��6�� �� $ chkInternetSales���� -    ��Name$ chkInternetSales��$��1��$��6��$��17���� -    ��Name$ lblTemplate��$��Template��$��0��$��7�� �� $ listSeatTemplates���� -   $��listSeatTemplates��$��1��$��7��$��18��
������ 
�������$��0q����� .R��� $ 
Horizontal=�.+��� $��2q� d� 	IsChecked �� *
IsSelected+  �� Mode$  TwoWay=��� ���� *TEMPLATENAME+���� " ��.���  � ��$ 5,5,0,0q���  � ��$	 130����  � ��$
 Left=���  � ��
 �����&   System.Windows.Controls.DataGrid  -   $��	dgrdCosts��!   AutoGenerateColumns! .  
�������"   CanUserAddRows" .  $��0��$��8��$��2��$
��Left=�#   GridLinesVisibility$# 
Horizontal=�$   Columns$ 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn%   Header$% Class��&   
IsReadOnly& . '   
Foreground
' �����1  +System.Windows.Controls.DataGridBoundColumn(   Binding( �� *	ClassName+  $% TicketPrice��
' �����( �� *TicketPrice+  $
% CGST��
' �����( �� *CGST+  $
% SGST��
' �����( �� *SGST+  $% MC��
' �����( �� *MC+�� $��1��$��9��$ 
Horizontal=�.+��� -   $ btnSave��$
��Save��$��19���� -   $ 	btnCancel��$��Cancel��$��20��=	  7FirstFloor.ModernUI.Windows.Controls.ModernProgressRing	  -   ) 	 Name$) pgrProgress��* 	  IsActive* . $��0��
��� ���$��3��$��80��$��80��   CircleProgressRingStyle#��[  