   M S B A M L   `   `   `  ����   Circles    Circles.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � WindowStartupLocation$  CenterScreen=�}�.  $��5q� � AllowsTransparency .  � Title$ Circles��$	��600��$	��800�� � 
ResizeMode$ NoResize=� � WindowStyle$
 none=�   ImageSource1 � Icon# �    � 	Resources  �� CreateOptions �� CacheOption	 �� 	UriSource��%a   &	        �� $ IgnoreImageCache=�$ OnLoad=�$-	 'pack://siteoforigin:,,,/circlesicon.ico?�.��� -   $��OuterContainer��
 J� Effect
 3  -System.Windows.Media.Effects.DropShadowEffect     
BlurRadius$ 8Q�   	Direction$ 0Q�   RenderingBias$ Quality=�   ShadowDepth$ 0Q�   Color$ #888888��.+�� -   $��InnerContainer��}�. ���#ffffff.+��� -   $��image1��$	��600��$
��Left=�$��0,0,0,0q�$��Uniform=�$	��Top=�$	��800��� -   $��Circles��$��Stretch=�$��Stretch=�
������ }�. $��2��A� -   $��	Indicator�����#333333$
��Left=�$��68��$��545,129,0,0q�$��10��
���   �$	��Top=�$��68��A� -   $��
lIndicator�����#3333ff$
��Left=�$��68��$��
299,91,0,0q�$��10��
���   �$	��Top=�$��68��� -   $��SelectionPanel��$
��Left=�$	��550��$��0,40,0,0q�$	��Top=�$	��260�����#333333.+��� -	   $��SelectionName��$��Selected��$
��Left=�$	��Top=�$	��146��$��32(�$��4,4,0,0q�$��Segoe UI Light)����#ffffff�� -
   $��SelectionSize��$
��Left=�$��	76,63,0,0q�$	��Top=�$��20��$	��150���� -   $��SelectionAction��$y�Visible=�$��10,123,34,221q� �� MaxLines$ 10���� $
��Size��$
��Left=�$��	10,57,0,0q�$	��Top=�$��Segoe UI Light)����#ffffff$��15(��� $��Actions��$
��Left=�$��	10,88,0,0q�$	��Top=�$��Segoe UI Light)����#ffffff$��15(��� -   $��Test Actions��$
��Left=�$��151,520,0,0q�$	��Top=�$��99���� -   $��
Add Action��$
��Left=�$��
50,520,0,0q�$	��Top=�$��90��� -   $��TopBar��$��0,0,0,0q�$��40��$	��Top=�$��Stretch=����#dddddd � ColumnDefinitions �� $	��13* ��� $
��777* �.+��� -   $��
New Circle�����#555555
������ $
��Left=�$��683,52,0,-36q�$	��Top=�$��75��$��1��$��16(�$��	0,-12,0,0q�$��24���� -   $��rVelocityMonitor��$��	Right Vel��$��677,76,19,-64q�$��1���� $��Circles��$
��Left=�$��6,-7,0,0q�$	��Top=�$��30(�$��Segoe UI Light)�$��2���� -   $��x�����#555555$
��Left=�$��	749,2,0,0q�$	��Top=�$��17��$��0q�$��20(�$
��Bold"�$��Segoe UI)�$��0,-8,0,0q�$��32��$��1���� -   $��Toggletracking��$��Start��$
��Left=�$��87,7,0,0q�$	��Top=�$��39��$��0q�$��1��$
��Left=�$��14(����#555555�� -   $
��Save��$
��Save��$
��Left=�$��	133,7,0,0q�$	��Top=�$��38��$��0q�$��1��$
��Left=�$��14(����#555555�� -   $��SaveAs��$��Save As��$
��Left=�$��	177,7,0,0q�$	��Top=�$��58��$��0q�$��1��$
��Left=�$��14(����#555555�� -   $
��Open��$��	Open File��$
��Left=�$��	241,7,0,0q�$	��Top=�$��71��$��0q�$��1��$
��Left=�$��14(����#555555�� -    ��Name$ lVelocityMonitor��$��Left Vel��$��677,104,19,-92q�$��1��� -   $��action_selector��$
��Left=�$	��420��$��
250,90,0,0q�$	��Top=�$	��300�����#dddddd
   $ 6Q�$ 0Q�$ Quality=�$ 0Q�$ #333333��.+��� -   $��x�����#555555$
��Left=�$��
273,10,0,0q�$	��Top=�$��17��$��0q�$��20(�$
��Bold"�$��Segoe UI)�$��0,-8,0,0q�$��32��