using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CustomNetworking;
using BoggleGameClass;
using System.Threading;
using System.Text.RegularExpressions;
using BB;
using System.IO;
using HTMLwriter;
using DatabaseUpdater;

namespace BoggleServer
{
    /// <Assignment> PS8 </Assignment>
    /// <Authors> Sam Godfrey and Fahad Alothaimeen </Authors>
    public class BoggleServerClass
    {
        public int port = 2000;
        // Lock Object
        private static readonly object lockObject = new Object();
        // Server Object
        private TcpListener server;
        private TcpListener databaseServer;
        // A dictionary to hold all of the strings and their respective names
        public HashSet<Player> players;
        // A dictionary that holds a boggle game and the two players in that game
        public Dictionary<BoggleGame, Tuple<Player, Player>> gameDictionary;
        // A tuple to hold players
        public Tuple<Player, Player> tuple;
        // The queue to get into a game
        public Queue<Player> playGameQueue;
        // The time set by the user
        private string time;
        // File path to the game dictionary
        private string gameDicFilePath;
        // String of chars to be used in a custom game
        private string gameChars;
        // Another holder of the time
        private int pTime;
        // Tools for building the dictionary of acceptable words
        private string wordFromFile;
        public HashSet<string> words;


        public BoggleServerClass(string[] gameInfo)
        {
            // Gets the time from the first args and the game dictionary file path from the second args 
            time = gameInfo[0];
            int.TryParse(time, out pTime);
            gameDicFilePath = gameInfo[1];

            if (gameInfo.Length > 2)
            {
                gameChars = gameInfo[2];
            }
            else { gameChars = null; }
            // Starts a new thread to populate the dictionary with valid words
            Thread populateDictionary = new Thread(() => createValidWords(gameDicFilePath));
            populateDictionary.Start();

            // Initialize all of our data structures
            players = new HashSet<Player>();
            gameDictionary = new Dictionary<BoggleGame, Tuple<Player, Player>>();
            playGameQueue = new Queue<Player>();

            Console.WriteLine("server");
            // Start up the server and begin accepting sockets
            server = new TcpListener(IPAddress.Any, port);
            databaseServer = new TcpListener(IPAddress.Any, 2500);
            server.Start();
            databaseServer.Start();
            server.BeginAcceptSocket(new AsyncCallback(acceptSocket), new object());
            databaseServer.BeginAcceptSocket(new AsyncCallback(webpageRequest), new object());

        }
        /// <summary>
        /// Reads the file from the file path and creates a valid dictionary hashset
        /// </summary>
        /// <param name="pathToFile">Path to file of valid words</param>
        public void createValidWords(string pathToFile)
        {
            words = new HashSet<string>();
            try
            {
                using (StreamReader sr = new StreamReader(pathToFile))
                {
                    string line = sr.ReadToEnd();
                    StringBuilder(line, words);
                }
            }
            catch
            {
                Console.WriteLine("File Not Found");
            }
        }
        /// <summary>
        /// Splits up the words from the file based upon newline characters and then adds that word to the dictionary
        /// </summary>
        /// <param name="letter">List of words to check</param>
        /// <param name="word">HashSet to populate</param>
        /// <returns>Returns the word hashset</returns>
        private IEnumerable<string> StringBuilder(string letter, HashSet<string> word)
        {
            // Loops through each character in the list checking to see if it is a newline or not
            foreach (var item in letter)
            {
                if (item == '\n' || item == '\r')
                {
                    wordFromFile += "";
                    word.Add(wordFromFile);
                    wordFromFile = "";
                }
                if ((item != '\n' && item != '\r'))
                {
                    wordFromFile += item;
                }
            }

            HashSet<string> wordsToReturn = new HashSet<string>(word);
            wordsToReturn.RemoveWhere(isEmpty);
            return wordsToReturn;
        }
        /// <summary>
        /// Quick check to see if the string is a empty string or null string
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool isEmpty(string i)
        {
            return (i == "" || i == null);
        }
        /// <summary>
        /// Call back for an accepted socket
        /// </summary>
        /// <param name="ar">Info about the accepted socket</param>
        private void acceptSocket(IAsyncResult ar)
        {
            lock (lockObject)
            {
                Socket handler = server.EndAcceptSocket(ar);

                StringSocket ss = new StringSocket(handler, new UTF8Encoding());
                ss.BeginReceive(nameReceived, ss);
                server.BeginAcceptSocket(new AsyncCallback(acceptSocket), new object());
            }
        }
        private void webpageRequest(IAsyncResult ar)
        {
            Socket handler = databaseServer.EndAcceptSocket(ar);

            StringSocket ss = new StringSocket(handler, new UTF8Encoding());
            ss.BeginReceive(webpageRequestReceived, ss);
            databaseServer.BeginAcceptSocket(new AsyncCallback(webpageRequest), new object());
        }
        private void webpageRequestReceived(string s, Exception e, object payload)
        {
            HTMLCreator htmlMessages = new HTMLCreator();
            Console.WriteLine(s);
            if (!Regex.Match(s, @"(players)").Success && !Regex.Match(s, @"(games)").Success)
            {
                ((StringSocket)payload).BeginSend(htmlMessages.errorPageRequest(), (ex, o) => { }, payload);
            }
            else
            {
                if (Regex.Match(s, @"(/)").Success)
                {
                    ((StringSocket)payload).BeginSend(htmlMessages.webpageRequestResponse(), (ex, o) => { }, payload);
                    ((StringSocket)payload).BeginSend(htmlMessages.emptyLineSend(), (ex, o) => { }, payload);
                    ((StringSocket)payload).BeginSend(htmlMessages.sendHTML(), (ex, o) => { }, payload);
                }
                if (Regex.Match(s, @"(/players)").Success)
                {
                    ((StringSocket)payload).BeginSend(htmlMessages.pageRequested("Players"), (ex, o) => { }, payload);
                    ((StringSocket)payload).BeginSend(htmlMessages.sendPlayerList(), (ex, o) => { }, payload);
                }
                if (Regex.Match(s, @"(/games)").Success)
                {
                    if (Regex.Match(s, @"(players)").Success)
                    {
                        ((StringSocket)payload).BeginSend(htmlMessages.pageRequested("games"), (ex, o) => { }, payload);
                        string playerName = s.Substring(s.LastIndexOf('=') + 1);
                        playerName = playerName.Split(' ')[0];
                        Console.WriteLine(playerName);
                        ((StringSocket)payload).BeginSend(htmlMessages.gamesByPlayer(playerName), (ex, o) => { }, payload);
                    }
                    if (Regex.Match(s, @"(gameID)").Success)
                    {
                        string gameID = s.Substring(s.LastIndexOf('=') + 1);
                        gameID = gameID.Split(' ')[0];
                        List<string> htmlToSend = (htmlMessages.gameInformation(gameID));
                        foreach (var item in htmlToSend)
                        {
                            ((StringSocket)payload).BeginSend(item, (ex, o) => { }, payload);
                        }
                    }
                }

            }

            ((StringSocket)payload).Close();

        }
        /// <summary>
        /// First call back that we start, waits for the name to be input by the client. We are expected the message to be of the form PLAY ...
        /// </summary>
        /// <param name="stringReceived">String received from the clients begin send method</param>
        /// <param name="e">Exception, if there is one</param>
        /// <param name="payload">The socket that sent the message</param>
        private void nameReceived(string stringReceived, Exception e, object payload)
        {
            lock (lockObject)
            {
                StringSocket ss = (StringSocket)payload;

                if (Regex.Match(stringReceived, @"PLAY").Success)
                {
                    /* Single Player Version */
                    //string name = stringReceived.Substring(5);
                    //Console.WriteLine(name);
                    //Player singlePlayer = new Player(ss, name);
                    //players.Add(singlePlayer);
                    //Player computerPlayer = new Player(ss, "Computer");
                    //players.Add(computerPlayer);
                    //tuple = new Tuple<Player, Player>(singlePlayer, computerPlayer);
                    //BoggleGame game;
                    //if (gameChars == null)
                    //{
                    //    game = new BoggleGame(singlePlayer.playerName, computerPlayer.playerName, words);
                    //}
                    //else
                    //{
                    //    game = new BoggleGame(singlePlayer.playerName, computerPlayer.playerName, gameChars, words);
                    //}
                    //singlePlayer.setGame(game);
                    //computerPlayer.setGame(game);
                    //gameDictionary.Add(game, tuple);
                    //informToStartSinglePlayer(singlePlayer, game, time);

                    /* Multiplyer Version */
                    string name = stringReceived.Substring(5);
                    Console.WriteLine(name);
                    // Add the string socket that sent the message to the list of connected sockets
                    Player currentPlayer = new Player(ss, name);
                    players.Add(currentPlayer);
                    // Create a new player with the stringsocket and name
                    // Checks to see that we have no terminated sockets in the queue
                    if (playGameQueue.Count > 0 && playGameQueue.Peek().terminated)
                    {
                        playGameQueue.Dequeue();
                    }
                    // Enqueue the current socket into the game queue
                    playGameQueue.Enqueue(currentPlayer);
                    // If there are at least 2 players in the queue we dequeue them and pair them in a game
                    if (playGameQueue.Count % 2 == 0)
                    {
                        Player player1 = playGameQueue.Dequeue();
                        Player player2 = playGameQueue.Dequeue();
                        // Tuple to hold the two players
                        tuple = new Tuple<Player, Player>(player1, player2);

                        // Create a new boggle game
                        BoggleGame game;
                        if (gameChars == null)
                        {
                            game = new BoggleGame(player1.playerName, player2.playerName, words);
                        }
                        else
                        {
                            game = new BoggleGame(player1.playerName, player2.playerName, gameChars, words);
                        }
                        // Set the game variable of each player to the associated game
                        player1.setGame(game);
                        player2.setGame(game);
                        // Adds two players and their respective game to a dictionary
                        gameDictionary.Add(game, tuple);
                        // Inform the players to start the game
                        informToStart(player1, player2, game, time);
                    }
                    currentPlayer.playerSocket.BeginReceive(wordsReceived, currentPlayer);
                }
                // If we do not receive a message with PLAY ... we just start listing for that message again and do not do anything, save sending an IGNORE command
                else
                {
                    ss.BeginSend("IGNORING\n", (ex, o) => { }, ss);
                    ss.BeginReceive(nameReceived, ss);
                }
            }
        }
        private void informToStartSinglePlayer(Player P1, BoggleGame bg, string time)
        {
            string gameChars = bg.displayBoard();

            // Send the boggle board, the time and the opponent to each player
            string message = "START " + gameChars + " " + time + " " + bg.player2 + "\n";
            P1.playerSocket.BeginReceive(wordsReceived, P1);
            P1.playerSocket.BeginSend(message, (ex, o) => { }, P1.playerSocket);
            Thread timerThread = new Thread(() => createTimerClass(new Tuple<Player, Player>(P1, null), time));
            timerThread.Start();

        }        /// <summary>
        /// The call back that handles messages sent by the client with the WORD ... syntax
        /// </summary>
        /// <param name="stringReceived"></param>
        /// <param name="e"></param>
        /// <param name="payload"></param>
        private void wordsReceived(string stringReceived, Exception e, object payload)
        {
            // Get the current boggle game from the client
            BoggleGame boggleGame = ((Player)payload).currentGame;
            Tuple<Player, Player> players;
            // If we have received a null string, a socket has closed and we should terminate the game
            //if (Regex.Match(stringReceived, @"QUIT").Success)
            //{
            if (stringReceived == null)
            {
                lock (lockObject)
                {
                    // Make sure the boggle game is not null
                    if (boggleGame != null)
                    {

                        ((Player)payload).setTerminated(true);
                        bool truth = gameDictionary.TryGetValue(boggleGame, out players);
                        // If we can get a value from the gameDictionary
                        // Player 1
                        if (truth && ((Player)payload).playerName == players.Item1.playerName)
                        {
                            // If player 2 is not terminated, we need to tell player 2 to terminate
                            if (!players.Item2.terminated)
                            {
                                players.Item1.playerSocket.Close();
                                terminate(players.Item2);
                            }
                        }
                        // Player 2
                        if (truth)
                        {
                            // If player 1 is not terminated, we need to tell player 1 to terminate
                            if (!players.Item1.terminated)
                            {
                                players.Item2.playerSocket.Close();
                                terminate(players.Item1);
                            }
                        }
                    }
                }
                // Close both sides of the socket officially
                ((Player)payload).playerSocket.Close();
                // Set terminated to true
                ((Player)payload).setTerminated(true);
            }
            // If we receive a message with WORD at the beginning we proced to score the word in our boggle game
            else if (Regex.Match(stringReceived, @"WORD").Success)
            {
                if (((Player)payload).currentGame != null)
                {
                    // Remove WORD
                    string word = stringReceived.Substring(5);
                    // Returns both players score
                    Tuple<int, int> playedWordInfo = (boggleGame.wordScore(word, ((Player)payload).playerName));

                    Tuple<Player, Player> playersWithThisGame;
                    gameDictionary.TryGetValue(boggleGame, out playersWithThisGame);
                    // Send the score to the appropriate player
                    //// Player 1
                    playersWithThisGame.Item1.playerSocket.BeginSend("SCORE "
                            + boggleGame.player1.playerScore.ToString() + " " + boggleGame.player2.playerScore.ToString()
                            + "\n", (ex, o) => { }, playersWithThisGame.Item1.playerSocket);
                    playersWithThisGame.Item2.playerSocket.BeginSend("SCORE "
                        + boggleGame.player2.playerScore.ToString() + " " + boggleGame.player1.playerScore.ToString()
                        + "\n", (ex, o) => { }, playersWithThisGame.Item2.playerSocket);
                    //if (((Player)payload).playerName == playersWithThisGame.Item1.playerName)
                    //{
                    //    playersWithThisGame.Item2.playerSocket.BeginSend("WORDPLAYED " + word + "\n", (ex, o) => { }, payload);
                    //}
                    //else
                    //{
                    //    playersWithThisGame.Item1.playerSocket.BeginSend("WORDPLAYED " + word + "\n", (ex, o) => { }, payload);
                    //}
                }
                else
                {
                    ((Player)payload).playerSocket.BeginSend("GAME HAS NOT STARTED YET\n", (ex, o) => { }, payload);
                }
                // Call begin receive again
                ((Player)payload).playerSocket.BeginReceive(wordsReceived, payload);
            }
            // If we received an inappropriate command
            else
            {
                ((Player)payload).playerSocket.BeginSend("IGNORING\n", (ex, o) => { }, payload);
                ((Player)payload).playerSocket.BeginReceive(wordsReceived, payload);
            }
        }
        /// <summary>
        /// Informs both players to start the game
        /// </summary>
        /// <param name="P1">Player 1</param>
        /// <param name="P2">Player 2</param>
        /// <param name="bg">Boggle game for the two players</param>
        /// <param name="time">Preset time decided by the client</param>
        private void informToStart(Player P1, Player P2, BoggleGame bg, string time)
        {
            // The characters that will be on the boggle board
            string gameChars = bg.displayBoard();

            // Send the boggle board, the time and the opponent to each player
            string message = "START " + gameChars + " " + time + " " + bg.player2.playerName + "\n";
            P1.playerSocket.BeginReceive(wordsReceived, P1);
            P1.playerSocket.BeginSend(message, (ex, o) => { }, P1.playerSocket);

            message = "START " + gameChars + " " + time + " " + bg.player1.playerName + "\n";
            P2.playerSocket.BeginReceive(wordsReceived, P2);
            P2.playerSocket.BeginSend(message, (ex, o) => { }, P2.playerSocket);

            Tuple<Player, Player> playerTuple = new Tuple<Player, Player>(P1, P2);
            // Start a timer for the game on a new thread
            Thread timerThread = new Thread(() => createTimerClass(new Tuple<Player, Player>(P1, P2), time));
            timerThread.Start();
        }
        /// <summary>
        /// Starts a timer
        /// </summary>
        /// <param name="players">The players for which the timer is keeping track</param>
        /// <param name="time">Preset time</param>
        private void createTimerClass(Tuple<Player, Player> players, string time)
        {
            TimerClass currentGame = new TimerClass(players, time);
            players.Item1.setTimer(currentGame);
            players.Item2.setTimer(currentGame);

        }
        /// <summary>
        /// Tells the other client that a player has left the game and closes the socket
        /// </summary>
        /// <param name="playerToInform">Player to tell</param>
        private void terminate(Player playerToInform)
        {
            BoggleGame game = playerToInform.currentGame;
            Tuple<Player, Player> players;


            bool truth = gameDictionary.TryGetValue(game, out players);
            gameDictionary.Remove(game);
            playerToInform.setGame(null);

            string player1Wordsplayed = "";
            string player1IllegalWords = "";
            string player2Wordsplayed = "";
            string player2IllegalWords = "";
            string wordsInCommen = "";
            foreach (var item in game.player1.wordsPlayed)
            {
                player1Wordsplayed += item;
                player1Wordsplayed += " ";
            }
            foreach (var item in game.player1.illegalWordsPlayed)
            {
                player1IllegalWords += item;
                player1IllegalWords += " ";
            }
            foreach (var item in game.player2.wordsPlayed)
            {
                player2Wordsplayed += item;
                player2Wordsplayed += " ";
            }
            foreach (var item in game.player2.illegalWordsPlayed)
            {
                player2IllegalWords += item;
                player2IllegalWords += " ";
            }
            foreach (var item in game.wordsInCommon)
            {
                wordsInCommen += item;
                wordsInCommen += " ";
            }
            if (playerToInform.playerName == game.player1.playerName)
            {
                players.Item1.playerSocket.BeginSend(("TERMINATE " + game.player1.wordsPlayed.Count.ToString() + "!" + player1Wordsplayed + "!"
                    + game.player2.wordsPlayed.Count.ToString() + "!" + player2Wordsplayed + "!" +
                    game.wordsInCommon.Count.ToString() + "!" + wordsInCommen + "!" + game.player1.illegalWordsPlayed.Count.ToString() + "!"
                    + " " + player1IllegalWords + "!" + game.player2.illegalWordsPlayed.Count.ToString() + "!"
                    + player2IllegalWords + "!" + game.player1.playerScore.ToString() + "!" + game.player2.playerScore.ToString() + "\n")
                    , (e, o) => { }, players.Item1.playerSocket);
                players.Item1.playerSocket.Close();
            }
            else
            {
                players.Item2.playerSocket.BeginSend(("TERMINATE " + game.player2.wordsPlayed.Count.ToString() + "!" + player2Wordsplayed + "!"
                    + game.player1.wordsPlayed.Count.ToString() + "!" + player1Wordsplayed + "!" +
                    game.wordsInCommon.Count.ToString() + "!" + wordsInCommen + "!" + game.player2.illegalWordsPlayed.Count.ToString() + "!"
                    + player2IllegalWords + "!" + game.player1.illegalWordsPlayed.Count.ToString() + "!"
                    + player1IllegalWords + "!" + game.player2.playerScore.ToString() + "!" + game.player1.playerScore.ToString() + "\n")
                    , (e, o) => { }, players.Item2.playerSocket);
                players.Item2.playerSocket.Close();
            }
            playerToInform.currentTimer.stopTimer();
            DatabaseTool dbTool = new DatabaseTool(game, time);
        }
    }
    /// <summary>
    /// A class that holds all of the relavent information for a player
    /// </summary>
    public class Player
    {
        public StringSocket playerSocket { get; private set; }
        public string playerName { get; private set; }
        public BoggleGame currentGame { get; private set; }
        public TimerClass currentTimer { get; private set; }
        public bool terminated { get; set; }

        public Player(StringSocket ss, string Name)
        {
            playerSocket = ss;
            playerName = Name;
            terminated = false;

        }
        public void setGame(BoggleGame game)
        {
            currentGame = game;
        }
        public void setTerminated(bool t)
        {
            terminated = t;
        }
        public void setTimer(TimerClass timer)
        {
            currentTimer = timer;
        }
    }
    /// <summary>
    /// A class that handles the timer
    /// </summary>
    public class TimerClass
    {
        /// <summary>
        /// Players that the timer is working for
        /// </summary>
        public Tuple<Player, Player> playerTuple { get; private set; }
        public int pTime { get; private set; }
        private int timE;
        private object lockObject = new object();
        private System.Threading.Timer TimerReference;
        public TimerClass(object playersCon, string time)
        {
            playerTuple = (Tuple<Player, Player>)playersCon;
            int.TryParse(time, out timE);
            pTime = timE;
            startTimer();
        }
        public void startTimer()
        {
            TimerReference = new Timer(sendTime, playerTuple, 1000, 1000);
        }
        public void stopTimer()
        {
            this.TimerReference.Change(Timeout.Infinite, Timeout.Infinite);
        }
        /// <summary>
        /// Every second we call this method to send the time to the client
        /// </summary>
        /// <param name="playersTuple">Players to send time too</param>
        public void sendTime(object playersTuple)
        {
            if (pTime > 0)
            {
                pTime--;
                Console.WriteLine(pTime);
                Tuple<Player, Player> players = (Tuple<Player, Player>)playersTuple;
                players.Item1.playerSocket.BeginSend("TIME " + pTime.ToString() + "\n", (e, o) => { }, players.Item1.playerSocket);
                players.Item2.playerSocket.BeginSend("TIME " + pTime.ToString() + "\n", (e, o) => { }, players.Item2.playerSocket);
            }
            else
            {
                // If either player is terminated we stop the timer
                this.TimerReference.Change(Timeout.Infinite, Timeout.Infinite);

                Tuple<Player, Player> players = (Tuple<Player, Player>)playersTuple;
                TimerReference.Change(Timeout.Infinite, Timeout.Infinite);

                players.Item1.playerSocket.BeginSend("GAME OVER " + "\n", (e, o) => { }, players.Item1.playerSocket);
                players.Item2.playerSocket.BeginSend("GAME OVER " + "\n", (e, o) => { }, players.Item2.playerSocket);
                gameOver(players);

            }
        }
        /// <summary>
        /// The method that is called when the timer runs out, that is the game is over
        /// </summary>
        /// <param name="players">Players to send info to</param>
        public void gameOver(Tuple<Player, Player> players)
        {
            BoggleGame game = players.Item1.currentGame;
            // Item1 is player 1, Item1 of Item1 is legal words, Item2 of Item1 is illegal words
            // Item 2 is player 2, Item1 of Item2 is legal words, Item2 of Item2 is illegal words
            Tuple<Tuple<IEnumerable<string>, IEnumerable<string>>, Tuple<IEnumerable<string>, IEnumerable<string>>> gameOverInfo = game.gameOver();
            /// These 5 for loops create a string that has all the words, illegal words and words in common for the players
            string player1Wordsplayed = "";
            string player1IllegalWords = "";
            string player2Wordsplayed = "";
            string player2IllegalWords = "";
            string wordsInCommen = "";

            foreach (var item in gameOverInfo.Item1.Item1)
            {
                player1Wordsplayed += item;
                player1Wordsplayed += " ";
            }
            foreach (var item in gameOverInfo.Item1.Item2)
            {
                player1IllegalWords += item;
                player1IllegalWords += " ";
            }
            foreach (var item in gameOverInfo.Item2.Item1)
            {
                player2Wordsplayed += item;
                player2Wordsplayed += " ";
            }
            foreach (var item in gameOverInfo.Item2.Item2)
            {
                player2IllegalWords += item;
                player2IllegalWords += " ";
            }
            foreach (var item in game.wordsInCommon)
            {
                wordsInCommen += item;
                wordsInCommen += " ";
            }
            players.Item1.playerSocket.BeginSend(("STOP " + game.player1.wordsPlayed.Count.ToString() + "!" + player1Wordsplayed + "!"
                + game.player2.wordsPlayed.Count.ToString() + "!" + player2Wordsplayed + "!" +
                game.wordsInCommon.Count.ToString() + "!" + wordsInCommen + "!" + game.player1.illegalWordsPlayed.Count.ToString() + "!"
                + " " + player1IllegalWords + "!" + game.player2.illegalWordsPlayed.Count.ToString() + "!"
                + player2IllegalWords + "!" + game.player1.playerScore.ToString() + "!" + game.player2.playerScore.ToString() + "\n")
                , (e, o) => { }, players.Item1.playerSocket);
            players.Item2.playerSocket.BeginSend(("STOP " + game.player2.wordsPlayed.Count.ToString() + "!" + player2Wordsplayed + "!"
                + game.player1.wordsPlayed.Count.ToString() + "!" + player1Wordsplayed + "!" +
                game.wordsInCommon.Count.ToString() + "!" + wordsInCommen + "!" + game.player2.illegalWordsPlayed.Count.ToString() + "!"
                + player2IllegalWords + "!" + game.player1.illegalWordsPlayed.Count.ToString() + "!"
                + player1IllegalWords + "!" + game.player2.playerScore.ToString() + "!" + game.player1.playerScore.ToString() + "\n")
                , (e, o) => { }, players.Item2.playerSocket);
            players.Item1.playerSocket.Close();
            players.Item2.playerSocket.Close();
            DatabaseTool dbTool = new DatabaseTool(game, timE.ToString());
        }
    }
}

