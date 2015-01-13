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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BoggleClientModel;
using System.Threading;

namespace BoggleClientView
{
    /// <Assignment> PS9 </Assignment>
    /// <Authors> Sam Godfrey and Fahad Alothaimeen </Authors>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Boggle client to handle playing the game
        /// </summary>
        public BoggleClient client;
        public GameWindow clientGame;
        /// <summary>
        /// All of the below are information about the game
        /// </summary>
        private string ipAddress;
        private string playerName;
        private string gameChars;
        private string opponentsName;
        private bool connected;
        private string timeToPlay;
        /// <summary>
        /// Creates a new main window and a new BoggleClient
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Dispatcher.Invoke(new Action(() => { this.Title = "Welcome To Boggle!"; }));
            client = new BoggleClient();
            client.GameStart += startGameWindow;
        }
        /// <summary>
        /// Connect To Server Button Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectToServer_Click(object sender, RoutedEventArgs e)
        {
            ipAddress = EnteredIPAddress.Text;
            playerName = EnteredName.Text;
            connected = true;
            try
            {
                /// Tries to connect to the server using the given info
                client.connect(ipAddress, playerName, client);
                ConnectToServer.Content = "Connecting To A Game";
            }
            catch(Exception ex)
            {
                displayConnectionError(ex);
            }
        }
        /// <summary>
        /// Starts a new game window
        /// </summary>
        /// <param name="characters">Characters in game board</param>
        /// <param name="opponents">Name of opponent</param>
        /// <param name="time">Length of game</param>
        private void startGameWindow(string characters, string opponents, string time)
        {
            gameChars = characters;
            opponentsName = opponents;
            timeToPlay = time;
            /// Spins off a new thread to handle the creation of the game window, borrowed from StackOverflow answer
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
            Dispatcher.Invoke(new Action(() => { Hide(); }));
        }
        private void ThreadStartingPoint()
        {
            GameWindow tempWindow = new GameWindow(gameChars, timeToPlay, client, playerName, opponentsName, this);
            tempWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
        }
        /// <summary>
        /// Method for handling socket error connecting to server
        /// </summary>
        /// <param name="e"></param>
        private void displayConnectionError(Exception e)
        {
            MessageBox.Show("Unable To Connect To The Host");
            ConnectToServer.Content = "Connect To Server";
        }
        /// <summary>
        /// Close the game if you hit the cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            /// If we are already connected we need to disconnect the socket and then close the windows
            if (connected == true)
            {
                client.disconnect();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
    }
}
