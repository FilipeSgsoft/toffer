   M S B A M L   `   `   `  ����G  CAssistenteTI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:AssistenteTI.PagesAssistenteTI.Pages  5      7 clr-namespace:AssistenteTI.ModalAssistenteTI.Modal  5
      $    AssistenteTI.Pages.SupportPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:AssistenteTI.Pages     5      V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      ,model clr-namespace:AssistenteTI.Modal  5
        N� Title$  SupportPage��5       � 	Resources 5       �� DataType1   +AssistenteTI.Modal.ErrorNotificationMessage � ColumnDefinitions'  !MaterialDesignThemes.Wpf.PackIcon   Kind    MaterialDesignTitleTextBlock)  #MaterialDesignThemes.Wpf.DialogHost   CloseDialogCommand0   *AssistenteTI.Modal.InfoNotificationMessage�� 5   
   %�  (��       5      *, +)7   (�� 8    5)      *, +)�� 5      "  6   .R�5      �� $��20q�6   .+�5      �  5      �� 5      $
��Auto �6/   6>   �� 5      $
��Auto �6/   6>   .+�5        $ Error=�62   $��48��5   2   $��48��5   2   $��20, 25, 20, 20q�5   2   $��0��5   2   �� 5      $��20q�6%   $��1��5   %   $	��300��5   %   .+�5      �� 8��  ��6(   �� *Title+5   `   �� 5       $��0,20,0,0q�5!   (   ��5    (   �� *	Message+5!   <   5"      5#      �� 5$      $��OK��6   #��Z 5%      ���#1818306v   5&      5'      �� 5)      "  6   .R�5+      �� $��20q�6   $��1��5,      $	��300��5-      .+�5/      �� ��6    �� *Title+6:   �� 50      $��	0,10,0,20q�51       ��50       �� *	Message+51   5   �� 52      $��Ok��6   #��Z 53      ���#1818306w   54      55      56      .��58      � .+�59   
   � ���#030c20 J� Opacity$	 0.3Q�6$   5;        5<   
   $y�Visible=�6%      
Identifier$ ModalAlertSupport��6:   .��5=      � -   5         �Name$ 	GridLogin��5=      .+�5?      � -   5        $ CardIdenticacaoProblema��5?   ,   $y�Visible=�6   $��Center=�6M   $��Center=�6j   .+�5B      �� $��Precisa de suporte?��6   $��22(�6:   $	��Top=�6H   $��Center=�6`   
�������$��
52,10,76,0q�6�   $	��205��6�   �� 5C      $%��Informe o que está acontecendo��6   $��12(�6E   $	��Top=�6S   $��Center=�6k   
�������$��
69,75,76,0q�6�   $	��188��6�   �� 5E      $��33��6    $��Bottom=�6,   
������ 
�������	 �� RadiusY$	 4.667��5F   C   
 �� RadiusX$
 4.667��6S   $��-44,0,-31,206q�6c   5H      �� 5J      -   5         ��Name$ cbxTipoTratativa��5J       $	��384��5K   -   $��16(�5L       $��Center=�5M   -   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Informe o problema��5N   -   
�������
�������$��-32,104,-19,206q�5Q   E    J� RenderTransformOrigin$ 0.5,0.5&�6^   $��33��6~   ��6-   �� ��5R      �� 5S      .R�5T   "   ��  �� Orientation$ 
Horizontal=�6-   .+�5U   &   �� $��0��6D   ��60   �� *id+6D   �� 5V   &   $��1��6E   ��60   �� *url+6E   �� 5W   &   $	��200��6F   ��60   �� *nome+6F   5X   #   5Y      5[      �� 5\      -   5        $��ResolverCaso��5\       � Cursor$
 Hand��6L   $��Resolver Caso��6Z     MaterialDesignRaisedButton#��[ 6r   $
��Auto��6�   ���#0cb754
�������
������ $��104,275,109,35q�6�   5]      � 5_      -   5        $ CardFeedbackProblema��5_   7   $	��314��6   $y�Hidden=�6#   $��Center=�6U   $��Center=�6r   .+�5b      �� $��O problema foi resolvido?��6   $��22(�6@   $	��Top=�6N   $��Center=�6f   
�������$��
25,26,10,0q�6�   $	��279��6�   �� 5c      $��47,98,0,205q�6#   .��5d      �� -   5         ��Name$ Feedback��5d   %   $	��119��67   $	��222��6D   .+�5e      	� 
�������$��Sim, muito obrigado!��6=   6^   	� 5f      
�������$#��Infelizmente não resolveu :(��6=   $��0,5,0,0q�6d   6w   5g      5h      �� 5i      -   5        $��ConfirmaFeedBack��5i      $
 Hand��6T   $��Enviar Feedback��6b   #��[ 6|   $
��Auto��6�   ���#0cb754
�������
������ $��92,278,87,44q�6  $ 
0.501,0.81&�6  5j      � 5k      -   5        $ GridBackground��5k      $y�Hidden=�6/   
���   �$	 0.6Q�6V   � 5l      -	   5        $ GridLoading��5l       K� ZIndex$ 9��6,   $��Center=�6=   $��Center=�6X   $y�Hidden=�6u   $	��450��6�   $��22,134,28,116q�6�   $	��100��6�   .+�5m      �� ���#fff
������ $��0q�6I    �� CornerRadius$ 10��6]   $��0,0,0,0q�6o   ._�5n      �� $��10q�6%   .+�5o        $ Loading=�66   $��Center=�6E   $��Center=�6b   $ .5,.5&�6}   ���#fff$��50��6�   $��50��6�   ��5p   "   �� 5q   &    �� 
TargetType"  6,   .�5r   *   ��  �� Property �@J�RenderTransformOrigin �� Value$ 0.5,0.5&�6R   �� 5s   *    � z� 5t   .   �� 5u   2   5w   +    �� Triggers 5x   *   P� 5y   .    P� Property �@J�	IsVisible P� Value .  N� EnterActions 5z   2   �� 5{   6   .��5|   :   ��  g� RepeatBehavior$ Forever��6E   .��5}   >   U�  �� TargetProperty$ RenderTransform.Angle�5~   %   .\�5�   B   n�  O� KeyTime$ 0:0:0.1��6Y    O� Value 306k   n� 5�   B   $ 0:0:0.2��6Y    606k   n� 5�   B   $ 0:0:0.3��6Y    906k   n� 5�   B   $ 0:0:0.4��6Y    1206k   n� 5�   B   $ 0:0:0.5��6Y    1506k   n� 5�   B   $ 0:0:0.6��6Y    1806k   n� 5�   B   $ 0:0:0.7��6Y    2106k   n� 5�   B   $ 0:0:0.8��6Y    2406k   n� 5�   B   $ 0:0:0.9��6Y    2706k   n� 5�   B   $ 0:0:1.0��6Y    3006k   n� 5�   B   $ 0:0:1.1��6Y    3306k   n� 5�   B   $ 0:0:1.2��6Y    3606k   5�   ?   5�   ;   5�   7   5�   /   5�   '   5�      �� 5�      -
   5         ��Name$ 
txtLoading��5�   (   $��
Carregando��6<   ���#fff$��Center=�6`   $��16(�6}   5�      5�      5�      5�      5�      5�      5�      