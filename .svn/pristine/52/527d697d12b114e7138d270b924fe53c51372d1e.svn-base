using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BB;
using System.Text.RegularExpressions;

namespace BoggleGameClass
{
    public class BoggleGame
    {
        /// <Assignment> PS8 </Assignment>
        /// <Authors> Sam Godfrey and Fahad Alothaimeen </Authors>
        public PlayerInfo player1 { get; private set; }
        public PlayerInfo player2 { get; private set; }
        private HashSet<string> legalWordsToPlay;
        private HashSet<string> wordsPlayed;
        public HashSet<string> wordsInCommon;
        public BoggleBoard boggle;
        /// <summary>
        /// Constructor with a random boggle board
        /// </summary>
        /// <param name="p1">First palyer</param>
        /// <param name="p2">Second Player</param>
        /// <param name="validWords">Hashset of valid words, comes from server</param>
        public BoggleGame(string p1, string p2, HashSet<string> validWords)
        {
            player1 = new PlayerInfo(p1);
            player2 = new PlayerInfo(p2);
            Tuple<PlayerInfo, PlayerInfo> players = new Tuple<PlayerInfo, PlayerInfo>(new PlayerInfo(p1), new PlayerInfo(p2));

            boggle = new BoggleBoard();

            legalWordsToPlay = validWords;
            wordsPlayed = new HashSet<string>();
            wordsInCommon = new HashSet<string>();

        }
        /// <summary>
        /// Constructor for a boggle board with predefined characters
        /// </summary>
        /// <param name="p1">First palyer</param>
        /// <param name="p2">Second Player</param>
        /// <param name="bChars">The 16 characters we want to see on our boggle board</param>
        /// <param name="validWords">Hashset of valid words, comes from server</param>
        public BoggleGame(string p1, string p2, string bChars, HashSet<string> validWords)
        {
            player1 = new PlayerInfo(p1);
            player2 = new PlayerInfo(p2);

            Tuple<PlayerInfo, PlayerInfo> players = new Tuple<PlayerInfo, PlayerInfo>(new PlayerInfo(p1), new PlayerInfo(p2));

            boggle = new BoggleBoard(bChars);
            legalWordsToPlay = validWords;
            wordsPlayed = new HashSet<string>();
            wordsInCommon = new HashSet<string>();
        }
        /// <summary>
        /// Returns the boggle board characters using the BoggleBoard class ToString override
        /// </summary>
        /// <returns></returns>
        public string displayBoard()
        {
            return boggle.ToString();
        }
        /// <summary>
        /// The method to score a word, checks that the word is valid and of proper lenght. Also checks what player played the word to give the appropriate person the score
        /// </summary>
        /// <param name="wordToCheck">Word that has been played</param>
        /// <param name="playerName">Who played the word</param>
        /// <returns>A tuple with the players score and the oppenents score</returns>
        public Tuple<int,int> wordScore(string wordToCheck, string playerName)
        {
            
            wordToCheck = wordToCheck.ToUpper();
            int score = 0;
            /// Player 1 area
            if (playerName == player1.playerName)
            {
                if (isValid(wordToCheck))
                {

                    if (wordToCheck.Count() < 3)
                    {
                        return new Tuple<int, int>(player2.playerScore, player1.playerScore);
                    }
                    if (wordsPlayed.Contains(wordToCheck) && wordsPlayed.Count != 0)
                    {
                        if (player2.wordsPlayed.Contains(wordToCheck))
                        {
                            wordsInCommon.Add(wordToCheck);
                            player2.wordsPlayed.Remove(wordToCheck);
                            score = scoreWord(wordToCheck);
                            score = score * -1;
                            player2.addScore(score);
                            return new Tuple<int, int>(player2.playerScore, player1.playerScore);
                        }
                        else
                        {
                            return new Tuple<int, int>(player2.playerScore, player1.playerScore);
                        }
                    }
                    if (!legalWordsToPlay.Contains(wordToCheck))
                    {
                        score--;
                        player1.addScore(score);
                        player1.illegalWordsPlayed.Add(wordToCheck);
                        return new Tuple<int, int>(player2.playerScore, player1.playerScore);
                    }
                    else
                    {
                        score = scoreWord(wordToCheck);
                        player1.addScore(score);
                        player1.wordsPlayed.Add(wordToCheck);
                        wordsPlayed.Add(wordToCheck);
                    }
                    return new Tuple<int, int>(player2.playerScore, player1.playerScore);
                }
                else
                {
                    score--;
                    player1.addScore(score);
                    player1.illegalWordsPlayed.Add(wordToCheck);
                    return new Tuple<int, int>(player2.playerScore, player1.playerScore);
                }

            }
            /// Player 2 area
            else
            {
                if (isValid(wordToCheck))
                {
                    if (wordToCheck.Count() < 3)
                    {
                        return new Tuple<int, int>(player1.playerScore, player2.playerScore);
                    }
                    if (wordsPlayed.Contains(wordToCheck) && wordsPlayed.Count != 0)
                    {
                        if (player1.wordsPlayed.Contains(wordToCheck))
                        {
                            wordsInCommon.Add(wordToCheck);
                            player1.wordsPlayed.Remove(wordToCheck);
                            score = scoreWord(wordToCheck);
                            score = score * -1;
                            player1.addScore(score);
                            return new Tuple<int, int>(player1.playerScore, player2.playerScore);
                        }
                        else
                        {
                            return new Tuple<int, int>(player1.playerScore, player2.playerScore);
                        }
                    }
                    if (!legalWordsToPlay.Contains(wordToCheck))
                    {
                        score--;
                        player2.addScore(score);
                        player2.illegalWordsPlayed.Add(wordToCheck);
                        return new Tuple<int, int>(player1.playerScore, player2.playerScore);
                    }
                    else
                    {
                        score = scoreWord(wordToCheck);
                        player2.addScore(score);
                        player2.wordsPlayed.Add(wordToCheck);
                        wordsPlayed.Add(wordToCheck);
                    }
                    return new Tuple<int, int>(player1.playerScore, player2.playerScore);
                }
                else
                {
                    score--;
                    player2.addScore(score);
                    player2.illegalWordsPlayed.Add(wordToCheck);
                    return new Tuple<int, int>(player1.playerScore, player2.playerScore);
                }
            }
        }
        public int scoreWord(string wordToScore)
        {
            int score = 0;
            if (wordToScore.Count() < 5)
            {
                score++;
            }
            if (wordToScore.Count() == 5)
            {
                score = score + 2;
            }
            if (wordToScore.Count() == 6)
            {
                score = score + 3;
            }
            if (wordToScore.Count() == 7)
            {
                score = score + 5;
            }
            if (wordToScore.Count() > 7)
            {
                score = score + 11;
            }
            return score;
        }
        /// <summary>
        /// Checks if any of the letters are contained in the boggle board
        /// </summary>
        /// <param name="str">Word to check</param>
        /// <returns>True or false</returns>
        public bool isValid(string str)
        {
            string removedQU = Regex.Replace(str, @"QU", "Q");
            char[] letter = removedQU.ToCharArray();
            foreach(var item in letter)
            {
                if (!boggle.ToString().Contains(item)) { return false; }
            }
            return true;
        }
        /// <summary>
        /// Game over method which bundles up all of the lists and returns them in a tuple
        /// </summary>
        /// <returns>A tuple with the form player1, player2 and all of their relevant info</returns>
        public Tuple<Tuple<IEnumerable<string>, IEnumerable<string>>, Tuple<IEnumerable<string>, IEnumerable<string>>> gameOver()
        {
            Tuple<IEnumerable<string>, IEnumerable<string>> player1Words = new Tuple<IEnumerable<string>, IEnumerable<string>>(player1.wordsPlayed.ToList(), player1.illegalWordsPlayed.ToList());
            Tuple<IEnumerable<string>, IEnumerable<string>> player2Words = new Tuple<IEnumerable<string>, IEnumerable<string>>(player2.wordsPlayed.ToList(), player2.illegalWordsPlayed.ToList());
            Tuple<Tuple<IEnumerable<string>, IEnumerable<string>>, Tuple<IEnumerable<string>, IEnumerable<string>>> gameOverInfo = new Tuple<Tuple<IEnumerable<string>, IEnumerable<string>>, Tuple<IEnumerable<string>, IEnumerable<string>>>(player1Words, player2Words);
            return gameOverInfo;
        }
    }
    /// <summary>
    /// A class to hold all of the player info, words played, score and such
    /// </summary>
    public class PlayerInfo
    {
        public string playerName { get; set; }
        public HashSet<string> wordsPlayed { get; set; }
        public HashSet<string> illegalWordsPlayed { get; set; }
        public int playerScore { get; set; }

        public PlayerInfo(string name)
        {
            playerScore = 0;
            playerName = name;
            wordsPlayed = new HashSet<string>();
            illegalWordsPlayed = new HashSet<string>();
        }
        public void addScore(int score)
        {
            playerScore += score;
        }
    }
}

