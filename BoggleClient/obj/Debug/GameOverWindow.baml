   M S B A M L   `   `   `  ����  BoggleClient%    BoggleClientView.GameOverWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  GameOverWindow��5   	   $	��299��6    $
��1027��6-   .��5      � .+�5   
   �� -   5         ��Name$ GameOverTitle��5      $��The Game Has Ended��6'   $
��Left=�6D   $��
393,16,0,0q�6_   $	��Top=�6s   $	��160��6�   $��15(�6�   $
��Bold"�6�   �� 5   
   -   5        $ WinnerOrLoserTitle��5      $��You are the:��6,   $
��Left=�6C   $��
393,46,0,0q�6^   $	��Top=�6r   $��79��6�   �� 5   
   -   5        $ WinnerLoserDisplay��5      $�� ��6,   $
��Left=�67   $��
477,46,0,0q�6R   $	��Top=�6f   $��86��6~   $
��Bold"�6�   $��14(�6�   �� 5	   
   -   5        $ WordsPlayedTitle��5	      $,��&Number Of Legal Words You Have Played:��6*   $
��Left=�6[   $��0,77,0,0q�6v   $	��Top=�6�   $	��249��6�   $
��Bold"�6�   �� 5
   
   -   5        $ NumberOfWordsDisplay��5
      $�� ��6.   $
��Left=�69   $��
249,77,0,0q�6T   $	��Top=�6h   $��52��6�   �� 5   
   -   5        $ WordsOpponentPlayed��5      $1��+Number Of Legal Words Your Opponent Played:��6-   $
��Left=�6c   $��
634,77,0,0q�6~   $	��Top=�6�   $	��287��6�   $
��Bold"�6�   �� 5   
   -   5        $! OpponentsNumberWordsDisplay��5      $�� ��65   $
��Left=�6@   $��
904,77,0,0q�6[   $	��Top=�6o    J� RenderTransformOrigin$ -0.342,0.654&�6�   $��45��6�   �� 5   
   -   5        $ WordsPlayedTitle1��5      $ ��The words you have played:��6+   $
��Left=�6P   $��	0,103,0,0q�6k   $	��Top=�6~   $	��160��6�   �� 5   
   -	   5        $ ListOfWordsPlayed��5      $�� ��6+   $
��Left=�66   $��
10,134,0,0q�6Q   $	��Top=�6e   $	��140��6}   $	��135��6�   �� 5   
   -
   5        $ OpponentWordsPlayed��5      $%��The words your opponent played:��6-   $
��Left=�6W   $��634,103,0,0q�6r   $	��Top=�6�   $	��198��6�   �� 5   
   -   5        $ ListOfOpponentWordsPlayed��5      $�� ��63   $
��Left=�6>   $��634,134,0,0q�6Y   $	��Top=�6n   $	��140��6�   $	��135��6�   �� 5   
   -   5        $ WordsInCommon��5      $��Words Played In Common��6'   $
��Left=�6H   $��
404,77,0,0q�6c   $	��Top=�6w   $	��159��6�   $
��Bold"�6�   �� 5   
   -   5        $ WordsInCommonDisplay��5      $�� ��6.   $
��Left=�69   $��404,108,0,0q�6T   $	��Top=�6i   $	��149��6�   $	��161��6�   �� 5   
   -   5        $ WordsInCommonCount��5      $�� ��6,   $
��Left=�67   $��
553,77,0,0q�6R   $	��Top=�6f   $��46��6~   �� 5   
   -   5        $ IllegalWordsDIsplay��5      $��Illegal Words Played:��6-   $
��Left=�6M   $��175,103,0,0q�6h   $	��Top=�6}   $	��121��6�   �� 5   
   -   5        $ IllegalWordsPlayed��5      $�� ��6,   $
��Left=�67   $��175,134,0,0q�6R   $	��Top=�6g   $	��140��6   $	��135��6�   �� 5   
   -   5        $ IllegalWordsDIsplay_Copy��5      $��Illegal Words Played:��62   $
��Left=�6R   $��876,103,0,0q�6m   $	��Top=�6�   $	��121��6�   �� 5   
   -   5        $! OpponentsIllegalWordsPlayed��5      $�� ��65   $
��Left=�6@   $��876,134,0,0q�6[   $	��Top=�6p   $	��140��6�   $	��135��6�   �� 5   
   -   5         ��Name$ 	PlayAgain��5      $��
Play Again��6$   $
��Left=�69   $��	71,16,0,0q�6T   $	��Top=�6g   $��89��6   $��56��6�   $
��Bold"�6�   $��14(�6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      5      �� 5   
   -   5        $ CloseButton��5      $��Close��6&   $
��Left=�66   $��
841,16,0,0q�6Q   $	��Top=�6e   $��80��6}   $��58��6�   $��14(�6�   $
��Bold"�6�    5        5      5!      5"      5#      