   M S B A M L   `   `   `  ����   BoxOfficeUI    BoxOfficeUI.MainPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � WindowStartupLocation$  CenterScreen=� � WindowState$ 	Maximized=� � Title$ 
Box Office��.��� $��Stretch=�����  �� EndPoint$ 	0.504,1.5&� �� 
StartPoint$ 
0.504,0.03&�.���  � Color$ 	#FF31708E�� � Offset$ 0Q�� $ 	#FFB3D5E5��$
 0.75Q�.+�� -    �Name$ UserGrid��$y�	Collapsed=� � RowDefinitions �� $��42 ��� $��* �	 � ColumnDefinitions	 �� $��* ��� $
��Auto �.+��� 
 �� Orientation$
 
Horizontal=�$��1��$��0��$��Center=�.+��� -    ��Name$ lblUserDisplayName��$��Admin���� -    ��Name$ 	btnLogOut��$��0,0,10,0q�$��Log Out���� -    ��Name$ 	TabMaster��$��30��$��10q�$��0q�$��2�� � 	Resources  �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName��%  '��       �� " ��.���  �@�Cursor$
 Hand����  � �� �� " ��.R�� $��Panel��.+��� $ ContentSite��$��Center=�$��Center=�$��Header��$
��10,2q� P�  �@��
IsSelected . . ��� $ Panel�� � �� �#FFFFFF��  � �� �	#FF31708EP�  �@��
IsSelected .  . ��� $ Panel�� � ��
 ���� ��  � �� �#FFFFFF.@��� -    ��Name$ LAYOUTDESIGN��$��LAYOUT DESIGN��$y�	Collapsed=��� -   $ MANAGERBLOCKING��$��MANAGER BLOCKING��$y�	Collapsed=��� -   $ 
SCHEDULING��$��
SCHEDULING��$y�	Collapsed=��� -	   $ BOOKING��$��BOOKING��$y�	Collapsed=��� -
   $ REPORTS��$��REPORTS��$y�	Collapsed=��� -   $ HOLDTRAY��$��	HOLD TRAY��$y�	Collapsed=��� $��1��$��
10,0,10,10q�$��1q�
�������$��2��._�� -   $ DisplayBodyGrid��� -   $ 	LoginGrid��$��Center=�$��Center=�  �� BasedOn��%�  '��       7��'�� D     7��'G� �     7G��� " ��8   .���  � ��$ Segoe UI)���  � ��$ 18(��� " ��8   .���  � ��$ Segoe UI)���  � ��$ 16(���  � ��$	 300����  � ��$ 25����  � ��$ 0,0,0,2q��� " G�8   .���  � ��$ Segoe UI)���  � ��$ 16(���  � ��$	 300����  � ��$ 25����  � ��$ 0,0,0,2q� �� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��30 �	 �� $	��100 ��� $	��400 �.+��� -   $ lblTheatreName��$��0��$��0��$��Theatre��$��Right=��� -    ��Name$ cmbTheatres��$��1��$��0��$	��300��
������ $��	0,0,0,1.5q� �� SelectedIndex$ 0��
�������$��14(�{�.  �� -   $ lblUserName��$��0��$��1��$��	User Name��$��Right=��� -    ��Name$ txtUserName��$��1��$��1�� �� 	MaxLength$ 50��|�. 
������ 
��������� -   $ lblPassword��$��0��$��2��$��Password��$��Right=�G� -    G�Name$ txtPassword��$��1��$��2�� G� 	MaxLength$ 50��
������ 
�������$
��Left=��� -   $ btnLogin��$��1��$��3��$��Log In��$��30���� -   $ 
lblMessage��$��1��$��4��
���  ��