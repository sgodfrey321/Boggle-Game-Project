using BoggleClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        /// <summary>
        /// The Boggle Client that is tied to this game window
        /// </summary>
        BoggleClient player;
        private int score;
        private int opponentsScore;
        /// <summary>
        /// Used for all of the game information we need to pass to the gameclose window constructor
        /// </summary>
        private List<Tuple<int, List<string>>> gameInfoToPass;
        MainWindow window;
        public GameWindow(string gameCharacters, string time, BoggleClient client, string playerName, string opponentsName, MainWindow mainWindow)
        {
            player = client;
            gameInfoToPass = new List<Tuple<int, List<string>>>();
            window = mainWindow;

            InitializeComponent();
            // General GUI Setup
            GameInfoPanal.Dispatcher.Invoke(new Action(() => { GameInfoPanal.Content = "Welcome " + playerName + " you are playing " + opponentsName + " today"; }));

            PlayerName.Dispatcher.Invoke(new Action(() => { PlayerName.Content = playerName + " score: "; }));
            OpponentName.Dispatcher.Invoke(new Action(() => { OpponentName.Content = opponentsName + " score: "; }));
            MyScore.Dispatcher.Invoke(new Action(() => { MyScore.Content = "0"; }));
            OpponentsScore.Dispatcher.Invoke(new Action(() => { OpponentsScore.Content = "0"; }));
            this.Dispatcher.Invoke(new Action(() => { this.Title = "Play Window"; }));
            // Here we assing all of the actions in the client class to the appropriate GUI updates
            populateBoard(gameCharacters, time);
            player.changeTime += updateTime;
            player.updateWordList += updateList;
            player.updateScore += updateScore;
            player.updateOpponentsScore += updateOpponentsScore;
            player.closeGame += closeGame;
            player.abortGame += closeGame;
        }
        /// <summary>
        /// Populat the boggle board using the characters passed in by the server/boggle game. We check for Q's and make sure to amend them to QU to be displayed
        /// </summary>
        /// <param name="s">Characters to be displayed</param>
        /// <param name="t">The time of the game</param>
        private void populateBoard(string s, string t)
        {
           
            int time;
            int.TryParse(t, out time);

            char[] letterArray = s.ToCharArray();
            string[] stringArray = new string[16];
            for (int i = 0; i < letterArray.Length; i++)
            {
                if (letterArray[i] != 'Q')
                {
                    stringArray[i] = letterArray[i].ToString();
                }
                else
                {
                    stringArray[i] = "QU";
                }
            }
            /// Populates the labels in the grid
            letter1.Content = stringArray[0].ToString();
            letter2.Content = stringArray[1].ToString();
            letter3.Content = stringArray[2].ToString();
            letter4.Content = stringArray[3].ToString();

            letter5.Content = stringArray[4].ToString();
            letter6.Content = stringArray[5].ToString();
            letter7.Content = stringArray[6].ToString();
            letter8.Content = stringArray[7].ToString();

            letter9.Content = stringArray[8].ToString();
            letter10.Content = stringArray[9].ToString();
            letter11.Content = stringArray[10].ToString();
            letter12.Content = stringArray[11].ToString();

            letter13.Content = stringArray[12].ToString();
            letter14.Content = stringArray[13].ToString();
            letter15.Content = stringArray[14].ToString();
            letter16.Content = stringArray[15].ToString();
            /// Updates time counter
            Time.Dispatcher.Invoke(new Action(() => { Time.Content = time; }));
        }
        /// <summary>
        /// Method for updating the time, tied to boggle client
        /// </summary>
        /// <param name="time">Time to update too</param>
        private void updateTime(string time)
        {
            Time.Dispatcher.Invoke(new Action(() => { Time.Content = time; }));
        }
        /// <summary>
        /// Method for proccessing the play word button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScoreWord_Click(object sender, RoutedEventArgs e)
        {
            string wordToScore = EnteredWords.Text;
            player.ScoreWord(wordToScore);
        }
        /// <summary>
        /// Method for updating the list of player words played, tied to boggle client
        /// </summary>
        /// <param name="word">Word to update</param>
        private void updateList(string word)
        {
            MyWordsPlayed.Dispatcher.Invoke(new Action(() => { MyWordsPlayed.Content += word + "\n"; }));
        }
        /// <summary>
        /// Method for updating your current score, tied to boggle client
        /// </summary>
        /// <param name="currentScore">Current score</param>
        private void updateScore(string currentScore)
        {
            int x;
            int.TryParse(currentScore, out x);
            score = x;
            MyScore.Dispatcher.Invoke(new Action(() => { MyScore.Content = score; }));
        }
        /// <summary>
        /// Method for updating your opponents score, tied to boggle client
        /// </summary>
        /// <param name="currentScore">Opponents score</param>
        private void updateOpponentsScore(string currentScore)
        {
            int x;
            int.TryParse(currentScore, out x);
            opponentsScore = x;
            OpponentsScore.Dispatcher.Invoke(new Action(() => { OpponentsScore.Content = opponentsScore; }));

        }
        /// <summary>
        /// Method for closing the current game window and opening up the game over window
        /// </summary>
        /// <param name="gameInfo">All of the game information passed by the client</param>
        /// <param name="playerScore">Your score</param>
        /// <param name="opponentsScore1">Opponents score</param>
        private void closeGame(List<Tuple<int, List<string>>> gameInfo, int playerScore, int opponentsScore1)
        {
            score = playerScore;
            opponentsScore = opponentsScore1;
            gameInfoToPass = gameInfo;
            /// Starts a new thread to open up the new window, takin from a StackOverflow question on opening up a seperate window
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
            /// Hides the current window
            this.Dispatcher.Invoke(new Action(() => { this.Hide(); }));
        }
        /// <summary>
        /// Method the thread uses as an entry point
        /// </summary>
        private void ThreadStartingPoint()
        {
            GameOverWindow tempWindow = new GameOverWindow(gameInfoToPass, player, score, opponentsScore, window, this);
            tempWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
        }
        /// <summary>
        /// Method to handle the window closing on the exit button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            /// Call a method to inform the server that the client has disconnected
            player.earlyTermination();
            Close();
            window.Dispatcher.Invoke(new Action(() => { window.Close(); }));
        }
        /// <summary>
        /// Ties the quit game button to the Window_Closed method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitGame_Click(object sender, RoutedEventArgs e)
        {
            Window_Closed(sender, e);
        }
    }
}
