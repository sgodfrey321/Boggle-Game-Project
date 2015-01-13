using CustomNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoggleClientModel
{
    /// <Assignment> PS9 </Assignment>
    /// <Authors> Sam Godfrey and Fahad Alothaimeen </Authors>
    public class BoggleClient
    {
        public bool comandRecieved1 = false; // for testing popuses only!
        public bool comandRecieved2 = false; // for testing popuses only!
        public bool exceptionThrown = false; // for testing popuses only!
        public event Action<string, string, string> GameStart;
        public event Action<string> changeTime;
        public event Action<string> updateWordList;
        public event Action<string> updateScore;
        public event Action<string> updateOpponentsScore;
        public event Action<List<Tuple<int, List<string>>>, int, int> abortGame;

        public event Action<List<Tuple<int, List<string>>>, int, int> closeGame;

        public List<Tuple<int, List<string>>> listGameInfo;
        public StringSocket clientSS;

        private object myLock = new object();
        public BoggleClient()
        {
            clientSS = null;
            listGameInfo = new List<Tuple<int, List<string>>>();
        }

        /// <summary>
        /// Connects the clinet on the model side
        /// </summary>
        /// <param name="ipAddress"> the ipAddress where the client will connect to </param>
        /// <param name="playerName"> the name of the player connecting to server </param>
        /// <param name="currentClient"> This clinet </param>
        public void connect(string ipAddress, string playerName, BoggleClient currentClient)
        {
            if (clientSS == null)
            {
                try
                {
                    TcpClient client = new TcpClient(ipAddress, 2000);
                    clientSS = new StringSocket(client.Client, new UTF8Encoding());
                    clientSS.BeginSend("PLAY " + playerName + "\n", (e, o) => { }, clientSS);
                    clientSS.BeginReceive(message, clientSS);
                }
                catch (ArgumentException)
                {
                    exceptionThrown = true;
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// disconnects the client
        /// </summary>
        public void disconnect()
        {
            clientSS.Close();
        }

        /// <summary>
        /// A callbacke that is invoked by the server
        /// </summary>
        /// <param name="s"> The string/command that is recoeved from the server </param>
        /// <param name="e"> exception "if occured" that is sent by the server </param>
        /// <param name="payload"> this clinet socket </param>
        public void message(string s, Exception e, object payload)
        {
            /// The following if statements handle all the different commands that could be sent by the server
            /// Start the game
            if (Regex.Match(s, @"START ").Success)
            {
                List<string> words = s.Split(' ').ToList();
                string characters = words[1];
                string time = words[2];
                string opponentsName = words[3];
                GameStart(characters, opponentsName, time);

                ((StringSocket)payload).BeginReceive(message, payload);
            }
            /// Update the time
            else if (Regex.Match(s, @"TIME ").Success)
            {
                List<string> words = s.Split(' ').ToList();

                changeTime(words[1]);
                ((StringSocket)payload).BeginReceive(message, payload);
            }
                /// Update the score board
            else if (Regex.Match(s, @"SCORE ").Success)
            {
                List<string> score = s.Split(' ').ToList();
                updateScore(score[1]);
                updateOpponentsScore(score[2]);
                ((StringSocket)payload).BeginReceive(message, payload);
            }
                /// Handle generic game over message
            else if (Regex.Match(s, @"GAME OVER ").Success)
            {
                comandRecieved1 = true;
                ((StringSocket)payload).BeginReceive(message, payload);
            }
                /// Receives all the information that the server passes when it send the STOP command
            else if (Regex.Match(s, @"STOP ").Success)
            {
                string stop = s.Remove(0, 4);
                List<string> gameInfo = stop.Split('!').ToList();

                int numberOfWordsPlayed;
                int.TryParse(gameInfo[0], out numberOfWordsPlayed);
                List<string> playerWordsPlayed = new List<string>(gameInfo[1].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(numberOfWordsPlayed, playerWordsPlayed));

                int opponentsNumberOfWords;
                int.TryParse(gameInfo[2], out opponentsNumberOfWords);
                List<string> opponentsWordsPlayed = new List<string>(gameInfo[3].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(opponentsNumberOfWords, opponentsWordsPlayed));

                int wordsInCommon;
                int.TryParse(gameInfo[4], out wordsInCommon);
                List<string> listOfWordsInCommon = new List<string>(gameInfo[5].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(wordsInCommon, listOfWordsInCommon));

                int playerIllegalWords;
                int.TryParse(gameInfo[6], out playerIllegalWords);
                List<string> playerIllegalWordsPlayed = new List<string>(gameInfo[7].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(playerIllegalWords, playerIllegalWordsPlayed));

                int opponentsIllegalWords;
                int.TryParse(gameInfo[8], out opponentsIllegalWords);
                List<string> opponentsIllegalWordsPlayed = new List<string>(gameInfo[9].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(opponentsIllegalWords, opponentsIllegalWordsPlayed));

                int playerScore;
                int opponentsScore;
                int.TryParse(gameInfo[10], out playerScore);
                int.TryParse(gameInfo[11], out opponentsScore);
                /// Call the close game action event
                closeGame(listGameInfo, playerScore, opponentsScore);
            }
                /// Handles if the game was terminated early
            else if (Regex.Match(s, @"TERMINATE ").Success)
            {
                string stop = s.Remove(0, 9);
                List<string> gameInfo = stop.Split('!').ToList();

                int numberOfWordsPlayed;
                int.TryParse(gameInfo[0], out numberOfWordsPlayed);
                List<string> playerWordsPlayed = new List<string>(gameInfo[1].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(numberOfWordsPlayed, playerWordsPlayed));

                int opponentsNumberOfWords;
                int.TryParse(gameInfo[2], out opponentsNumberOfWords);
                List<string> opponentsWordsPlayed = new List<string>(gameInfo[3].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(opponentsNumberOfWords, opponentsWordsPlayed));

                int wordsInCommon;
                int.TryParse(gameInfo[4], out wordsInCommon);
                List<string> listOfWordsInCommon = new List<string>(gameInfo[5].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(wordsInCommon, listOfWordsInCommon));

                int playerIllegalWords;
                int.TryParse(gameInfo[6], out playerIllegalWords);
                List<string> playerIllegalWordsPlayed = new List<string>(gameInfo[7].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(playerIllegalWords, playerIllegalWordsPlayed));

                int opponentsIllegalWords;
                int.TryParse(gameInfo[8], out opponentsIllegalWords);
                List<string> opponentsIllegalWordsPlayed = new List<string>(gameInfo[9].Split(' ').ToList());
                listGameInfo.Add(new Tuple<int, List<string>>(opponentsIllegalWords, opponentsIllegalWordsPlayed));

                int playerScore;
                int opponentsScore;
                int.TryParse(gameInfo[10], out playerScore);
                int.TryParse(gameInfo[11], out opponentsScore);

                abortGame(listGameInfo, playerScore, opponentsScore);
            }
        }

        /// <summary>
        /// Gets invoked when a game terminates early
        /// </summary>
        public void earlyTermination()
        {
            comandRecieved2 = true;
            clientSS.BeginSend(null, (e, o) => { }, clientSS);
        }

        /// <summary>
        /// Closes the client
        /// </summary>
        public void CloseSocket()
        {
            this.disconnect();
        }

        /// <summary>
        /// Sends a word to the view (GUI)
        /// </summary>
        /// <param name="word"> word to be sent </param>
        public void ScoreWord(string word)
        {
            clientSS.BeginSend("WORD " + word + "\n", (e, o) => { }, clientSS);
            updateWordList(word);
        }
    }
}
