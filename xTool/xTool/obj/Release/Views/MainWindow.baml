   M S B A M L   `   `   `  ���� @  <xTool, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null)clr-namespace:xTool.ViewsxTool.Views      xTool.Views.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:xTool.Views     V RMaterialDesignThemes.Wpf, Version=3.1.0.1244, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  -   $	��620��$
��1024��  � WindowStartupLocation$  CenterScreen=� � Icon$ /Assets/facebook.png�� � Title$3 -Công cụ Quảng cáo Facebook Tự động��.��� -   ���	#FF44325F � RowDefinitions �� $	��100 ��� $��* ��� $��20 �.+�� $��0��$	��Top=�
�������.+��� $��/Assets/facebook.png��$
��Left=�$
��10 0q��� $3��-CÔNG CỤ QUẢNG CÁO FACEBOOK TỰ ĐỘNG��$��10q�$��14(����	#FF2A6088$��Center=��� $��Model: (trống)��$��10q�$��14(����	#FF2A6088$	��Top=�$��Right=���  �� Orientation$ 
Horizontal=�$��Center=�.+��� $��Bottom=�.+��� $��TỔNG��$
��10 0q�$��16(�$
��Bold"����	#FF2A6088�� -   $��tblTotal��$��20��$��Center=�$��5 0 5 5q�$��16(����	#FF2A6088�� $��Bottom=�.+��� $��ĐANG CHỜ��$
��10 0q�$��16(�$
��Bold"����#F57F17�� -   $��
tblPending��$��9��$��Center=�$��5 0 5 5q�$��16(����#F57F17�� $��Bottom=�.+��� $��LỖI��$
��10 0q�$��16(�$
��Bold"����#B71C1C�� -   $��tblError��$��0��$��Center=�$��5 0 5 5q�$��16(����#B71C1C�� $��Bottom=�.+��� $��HOÀN THÀNH��$
��10 0q�$��16(�$
��Bold"����#009688�� -    ��Name$ tblCompleted��$��11��$��Center=�$��5 0 5 5q�$��16(����#009688�� $ 
Horizontal=�$
��Auto��$��60��$��Bottom=�$��Right=����	#FFE4E4E4 J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     
BlurRadius$ 5Q�   ShadowDepth$ 1Q�.+��� -   $��btnModel��	 � ToolTip$	 Chọn tập tin Model�� *  &MaterialDesignFloatingActionMiniButton#��[  $��10 0 0 0q�.��'  !MaterialDesignThemes.Wpf.PackIcon  
   Kind$
 FolderCogOutline=�
�������$��32��$��32���� -   $��btnStart��$	 Bắt đầu��#��[  $��10 0 0 0q�.��  $

 Play=�
����  �$��32��$��32���� -	   $��btnStop��$	 Dừng��#��[  $��10 0 0 0q�.��  $

 Stop=�
���  ��$��32��$��32���� -
   $��btnDeleteAll��$
	 Xóa��#��[  $��25 0 0 0q�.��  $
 Delete=�
��� E��$��32��$��32���� -   $��btnPaste��$
	 Dán��#��[  $��10 0 0 0q�.��  $"
 ClipboardTextMultipleOutline=�
�������$��32��$��32���� -   $��btnImportExcel��$��	10 0 10 0q�$��60��$��40��$	 Nhập từ Excel�����	#FF30ACBF���	#FF1E9191.��  $
 MicrosoftExcel=�$��32��$��32��� $��1��.+�&   System.Windows.Controls.DataGrid  -     Name$ 	dgAccount��   CanUserResizeColumns .     CanUserResizeRows .     
IsReadOnly . $��0,0,0,0q�   ColumnHeaderStyle ��  �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  .���  �� Property � �� �� Value$ 30����  � ��$ Center=���  � ��
 �������  � ��$ 14(���  � ��$
 Bold"���  � ��$	 0.5q���  � �� �	#FFFFFFFF��  � �� �	#FF44325F   	CellStyle �� *  $System.Windows.Controls.DataGridCell"  .���  � ��$ 28����  � ��$ 12(���  � ��
 �������  � ��$	 0.5q���  � ��
 �   ���  � ��
 ����� �� Triggers ��  �� Binding ��  �� Path$ Color� �� Value Red.a���  � ��
 �  ����  �� $ Color�
 Yellow.a���  � ��
 � �����  �� $ Color�
 Orange.a���  � ��
 � �����  �� $ Color� Blue.a���  � ��
 ��  ���  �� $ Color�	 Green.a���  � ��
 � � �P�  P� Property �@ 
IsSelected P� Value . . ���  � �� �#1DE9B6��  � ��
 �������  � ��$ 0q�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 1* 1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding �� $ Id� 	  ElementStyle �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=�   Header �� $	��STT��  $ 3*  �� $ UserName� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $��Tài khoản��  $ 3*  �� $ Password� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $��Mật khẩu��  $ 6*  �� $ TwoFactorAuthentication� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $"��Mã xác thực 2 yếu tố��  $ 4*  �� $ 
CardNumber� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $��
Số thẻ��  $ 2*  �� $ Month� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $��Tháng��  $ 2*  �� $
 Year� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $
��Năm��  $ 2*  �� $ CardCode� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $��	Mã thẻ��  $ 5*  �� $ 
AdsAddress� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $��Địa chỉ��  $
 3.2*  �� $ AdsName� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $
��Tên��  $
 3.2*  �� $ Status� �� " ��.���  �@��TextAlignment$ Center=���  � ��$ Center=� �� $��Trạng thái��� $��2��.+��� -   $��cbHideBrowser��$	��150��$
��Left=�$��Center=�
�������$��10,0,0,0q�.��Ẩn trình duyệt�� $-��'Copyright vanthanhkdt@gmail.com © 2020��$
��10 0q�$��12(����	#FFFFFFFF$��Center=�$��Right=�