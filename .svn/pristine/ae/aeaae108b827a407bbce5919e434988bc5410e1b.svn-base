using BoggleClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BoggleClientView
{
    /// <Assignment> PS9 </Assignment>
    /// <Authors> Sam Godfrey and Fahad Alothaimeen </Authors>
    public partial class GameOverWindow : Window
    {
        /// <summary>
        ///  Player this window is tied to 
        /// </summary>
        BoggleClient player;
        /// <summary>
        /// MainWindow for the current client, need this so we can reopen the window if needed
        /// </summary>
        MainWindow mainWindow;
        /// <summary>
        /// GameWindow for the current client, need this so we can reopen the window if needed
        /// </summary>
        GameWindow gamePlayWindow;
        int score;
        int theirScore;
        /// <summary>
        /// Constructor for the game over window
        /// </summary>
        /// <param name="gameInfo">List of relevent game information</param>
        /// <param name="client">Client this window is tied too</param>
        /// <param name="playerScore">Your scire</param>
        /// <param name="opponentScore">Their score</param>
        /// <param name="window"></param>
        /// <param name="gameWindow"></param>
        public GameOverWindow(List<Tuple<int, List<string>>> gameInfo, BoggleClient client, int playerScore, int opponentScore, MainWindow window, GameWindow gameWindow)
        {
            mainWindow = window;
            gamePlayWindow = gameWindow;
            player = client;

            InitializeComponent();
            /// Call methods to display game info and then closes the socket
            displayWinnerLoser(playerScore, opponentScore);
            displayGameInfo(gameInfo);
            player.CloseSocket();
        }
        /// <summary>
        /// Display the winner or loser of the game based upon score
        /// </summary>
        /// <param name="player"></param>
        /// <param name="opponent"></param>
        private void displayWinnerLoser(int player, int opponent)
        {
            if (player > opponent)
            {
                WinnerLoserDisplay.Dispatcher.Invoke(new Action(() => { WinnerLoserDisplay.Content = "Winner!"; }));
            }
            else if (player < opponent)
            {
                WinnerLoserDisplay.Dispatcher.Invoke(new Action(() => { WinnerLoserDisplay.Content = "Loser!"; }));
            }
            else
            {
                WinnerLoserDisplay.Dispatcher.Invoke(new Action(() => { WinnerLoserDisplay.Content = "Tie Game!"; }));
            }
        }
        /// <summary>
        /// Runs through the gameInformation and updates all of the appropriate labels and tags
        /// </summary>
        /// <param name="gameInformation"></param>
        private void displayGameInfo(List<Tuple<int, List<string>>> gameInformation)
        {
            /// Displays the number of words you played and lists all of the legal words you have played
            NumberOfWordsDisplay.Dispatcher.Invoke(new Action(() => { NumberOfWordsDisplay.Content = gameInformation[0].Item1.ToString(); }));
            foreach (var item in gameInformation[0].Item2)
            {
                ListOfWordsPlayed.Dispatcher.Invoke(new Action(() => { ListOfWordsPlayed.Content += item + "\n"; }));
            }
            /// Displays the number of words your opponent played and lists all of the legal words your opponent have played
            OpponentsNumberWordsDisplay.Dispatcher.Invoke(new Action(() => { OpponentsNumberWordsDisplay.Content = gameInformation[1].Item1.ToString(); }));
            foreach (var item in gameInformation[1].Item2)
            {
                ListOfOpponentWordsPlayed.Dispatcher.Invoke(new Action(() => { ListOfOpponentWordsPlayed.Content += item + "\n"; }));
            }
            /// Displays the number of words in common between you and your opponent and then lists them all
            WordsInCommonCount.Dispatcher.Invoke(new Action(() => { WordsInCommonCount.Content = gameInformation[2].Item1.ToString(); }));
            foreach (var item in gameInformation[2].Item2)
            {
                WordsInCommonDisplay.Dispatcher.Invoke(new Action(() => { WordsInCommonDisplay.Content += item + "\n"; }));
            }
            /// Lists all of the illegal words that you have played
            foreach (var item in gameInformation[3].Item2)
            {
                IllegalWordsPlayed.Dispatcher.Invoke(new Action(() => { IllegalWordsPlayed.Content += item + "\n"; }));
            }
            /// Lists all of the illegal words that your opponent played
            foreach (var item in gameInformation[4].Item2)
            {
                OpponentsIllegalWordsPlayed.Dispatcher.Invoke(new Action(() => { OpponentsIllegalWordsPlayed.Content += item + "\n"; }));
            }
        }
        /// <summary>
        /// Handles the play again button click, we just close the current window and open up the main window again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Dispatcher.Invoke(new Action(() => { newWindow.Show(); }));
            Close();
        }
        /// <summary>
        /// Closes all the windows associated with this game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Dispatcher.Invoke(new Action(() => { mainWindow.Close(); }));
            gamePlayWindow.Dispatcher.Invoke(new Action(() => { gamePlayWindow.Close(); }));
            this.Dispatcher.Invoke(new Action(() => { this.Close(); }));
        }
    }
}
