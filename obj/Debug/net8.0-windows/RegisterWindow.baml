   M S B A M L   `   `   `  ����N  JInventoryManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:InventoryManagementInventoryManagement  5   	   (    "InventoryManagement.RegisterWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:InventoryManagement     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	   -   5        $��reigsterWindow��5   	      MaterialDesignWindow#��[  5
   	     � 
ResizeMode$  CanMinimize=�5   	    � Title$ Đăng kí tài khoản��5   	   $	��300��6#   $	��400��60   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5      6   .+�5   
   #  MaterialDesignThemes.Wpf.Card  $��1��6   $	��350��6+   $	��250��67   $��Center=�6D   $��Center=�6a   .��5      �  � 	Resources 5       �� 
TargetType �� Property �� Value��%^   '�       �� 5      " �6   .�5      ��  � ��$ 	15 5 15 5q�63   6G   5      5       5      �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64   �� 5      $
��auto �6$   64   .+�5!      � $��0��6   .+�5"      �� $��Center=�6;   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Họ và tên��6V    % !MaterialDesignFloatingHintTextBox#��[ 5#      ��5"      �� *DisplayName+5#      5$      � 5%      $��1��6   .+�5&      �� $��Center=�68   $ Tên đăng nhập��6S   #��[ 5'      ��5&      �� *
UserName+5'      5(      � 5)      $��2��6   .+�5*      G� -   5        $��Center=�5,         
Foreground
 �� ��$ Mật khẩu��5.      .  (MaterialDesignThemes.Wpf.TextFieldAssist	   UnderlineBrush
	 �� �� ) %MaterialDesignFloatingHintPasswordBox#��[ 50      52      � 53      $��3��6   .+�54      G� -   5        $��Center=�56      
 �� ��$ Xác nhận mật khẩu��58      
	 �� ��#��[ 5:      5<      � 5=      $��4��6   $��Center=�6$   .+�5?      �� $��0��6   $��5q�6-   $��Đăng kí tài khoản��5B        MaterialDesignRaisedButton#��[ 5C      
 � ToolTip$/
 )Resource name: MaterialDesignRaisedButton��5D      ��5@      �� *RegisterCommand+��5A      ��  �� ElementName$ reigsterWindow��5D      5F      5G      5H      5I      5J      