﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using BoggleGameClass;

namespace DatabaseUpdater
{
    public class DatabaseTool
    {
        private const string connectionPath = "server=atr.eng.utah.edu;database=cs3500_samuelg;uid=cs3500_samuelg;password=508146563";
        private string player1 { get; set; }
        private int player1Score { get; set; }
        private int player2Score { get; set; }
        private string player2 { get; set; }
        private BoggleGame game { get; set; }
        private string timeOfGame { get; set; }
        private List<string> player1WordsPlayed { get; set; }
        private IEnumerable<string> player1AllWords { get; set; }
        private List<string> player2WordsPlayed { get; set; }
        private IEnumerable<string> player2AllWords { get; set; }
        private List<string> player1IllegalWordsPlayed { get; set; }
        private List<string> player2IllegalWordsPlayed { get; set; }
        private List<string> wordsInCommon { get; set; }
        private string gameBoard { get; set; }
        private DateTime now { get; set; }
        private string timeString { get; set; }
        private int winner { get; set; }
        public DatabaseTool()
        {

        }
        public DatabaseTool(BoggleGame currentGame, string time)
        {
            now = DateTime.Now;
            timeString = "'" + now.ToString() + "'";

            game = currentGame;
            gameBoard = game.displayBoard();
            gameBoard = "'" + gameBoard + "'";
            player1 = currentGame.player1.playerName;
            player1 = "'" + player1 + "'";
            player1Score = currentGame.player1.playerScore;
            player2 = currentGame.player2.playerName;
            player2 = "'" + player2 + "'";
            player2Score = currentGame.player2.playerScore;
            timeOfGame = "'" + time + "'";
            player1WordsPlayed = currentGame.player1.wordsPlayed.ToList();
            player1IllegalWordsPlayed = currentGame.player1.illegalWordsPlayed.ToList();
            player2WordsPlayed = currentGame.player2.wordsPlayed.ToList();
            player2IllegalWordsPlayed = currentGame.player2.illegalWordsPlayed.ToList();
            wordsInCommon = currentGame.wordsInCommon.ToList();

            player1AllWords = player1WordsPlayed.Union(player1IllegalWordsPlayed);
            player2AllWords = player2WordsPlayed.Union(player2IllegalWordsPlayed);

            addGameInfo();
        }
        private void addGameInfo()
        {
            string wordToAdd;
            HashSet<string> playersInDB = new HashSet<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionPath))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "select * from PlayerList";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string change = "'" + reader["playerName"].ToString() + "'";
                            playersInDB.Add(change);
                        }
                    }
                    // Player one id
                    int id = addPlayer(player1, playersInDB, conn);
                    // Player two id
                    int id2 = addPlayer(player2, playersInDB, conn);

                    if (player1Score < player2Score)
                    {
                        winner = id2;
                    }
                    if (player1Score > player2Score)
                    {
                        winner = id;
                    }
                    else if (player1Score == player2Score)
                    {
                        winner = 0;
                    }

                    command.CommandText = "INSERT INTO GameInformation " +
                       "(gameBoard, player1, player1Score, player2, player2Score, timeOfGame, timeGameEnded, winner)"
                       + "VALUES (" + gameBoard + ", " + id + ", " + player1Score + ", " + id2 + ", " + player2Score + ", " + timeOfGame + ", "
                       + timeString + ", " + winner + ")";
                    using (MySqlDataReader reader = command.ExecuteReader()) { }

                    int currentGameID = getGameID(timeString, conn);

                    foreach (var item in player1AllWords)
                    {
                        if (!player1IllegalWordsPlayed.Contains(item))
                        {
                            wordToAdd = "'" + item + "'";
                            command.CommandText = "insert into WordsTable " +
                                           "(idFromGame, wordsPlayed, playersWhoPlayedWord, isIllegal)"
                                           + "values (" + currentGameID + ", " + wordToAdd + ", " + player1 + ", " + "'false')";
                            using (MySqlDataReader reader = command.ExecuteReader()) { }

                        }
                        else
                        {
                            wordToAdd = "'" + item + "'";
                            command.CommandText = "insert into WordsTable " +
                           "(idFromGame, wordsPlayed, playersWhoPlayedWord, isIllegal)"
                           + "values (" + currentGameID + ", " + wordToAdd + ", " + player1 + ", " + "'true')";
                            using (MySqlDataReader reader = command.ExecuteReader()) { }
                        }
                    }

                    foreach (var item in player2AllWords)
                    {
                        if (!player2IllegalWordsPlayed.Contains(item))
                        {
                            wordToAdd = "'" + item + "'";
                            command.CommandText = "insert into WordsTable " +
                                           "(idFromGame, wordsPlayed, playersWhoPlayedWord, isIllegal)"
                                           + "values (" + currentGameID + ", " + wordToAdd + ", " + player2 + ", " + "'false')";
                            using (MySqlDataReader reader = command.ExecuteReader()) { }
                        }
                        else
                        {
                            wordToAdd = "'" + item + "'";
                            command.CommandText = "insert into WordsTable " +
                           "(idFromGame, wordsPlayed, playersWhoPlayedWord, isIllegal)"
                           + "values (" + currentGameID + ", " + wordToAdd + ", " + player2 + ", " + "'true')";
                            using (MySqlDataReader reader = command.ExecuteReader()) { }
                        }
                    }
                    foreach (var item in wordsInCommon)
                    {
                        wordToAdd = "'" + item + "'";
                        command.CommandText = "insert into WordsTable " +
                                           "(idFromGame, wordsPlayed, playersWhoPlayedWord, isIllegal)"
                                           + "values (" + currentGameID + ", " + wordToAdd + ", " + "'Both'" + ", " + "'false')";
                        using (MySqlDataReader reader = command.ExecuteReader()) { }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public int addPlayer(string playerName, HashSet<string> playerList, MySqlConnection conn)
        {
            int id = 0;
            MySqlCommand command = conn.CreateCommand();
            if (!playerList.Contains(playerName))
            {
                command.CommandText = "insert into PlayerList (playerName) values (" + playerName + ")";
                using (MySqlDataReader reader = command.ExecuteReader()) { }
            }
            command.CommandText = "select idPlayerList from PlayerList where playerName = " + playerName;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int.TryParse(reader["idPlayerList"].ToString(), out id);
                }
            }
            return id;
        }
        private int getGameID(string timeString, MySqlConnection conn)
        {
            int id = 0;
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = "select * from GameInformation where timeGameEnded = " + timeString;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int.TryParse(reader["gameID"].ToString(), out id);
                }
            }
            return id;

        }
        public Dictionary<string, GameWonLossTie> returnPlayers()
        {
            Dictionary<string, GameWonLossTie> players = new Dictionary<string, GameWonLossTie>();
            Dictionary<string, int> listOfPlayers = new Dictionary<string, int>();
            int uniqueID = 0;
            int winnerID = 0;
            string nameToAdd = string.Empty;
            using (MySqlConnection conn = new MySqlConnection(connectionPath))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "select * from PlayerList";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nameToAdd = reader["playerName"].ToString();
                            int.TryParse(reader["idPlayerList"].ToString(), out uniqueID);
                            listOfPlayers.Add(nameToAdd, uniqueID);
                            players.Add(nameToAdd, new GameWonLossTie());
                        }
                    }
                    foreach (var item in listOfPlayers)
                    {
                        command.CommandText = "select * from GameInformation where player1 = " + item.Value + " or player2 = " + item.Value;
                        using (MySqlDataReader reader1 = command.ExecuteReader())
                        {
                            while (reader1.Read())
                            {
                                int.TryParse(reader1["winner"].ToString(), out winnerID);
                                if (item.Value == winnerID)
                                {
                                    players[item.Key].gamesWon = players[item.Key].gamesWon + 1;
                                }
                                else if (item.Value != winnerID && winnerID != 0)
                                {
                                    players[item.Key].gamesLost = players[item.Key].gamesLost + 1;
                                }
                                else if (winnerID == 0)
                                {
                                    players[item.Key].gamesTied = players[item.Key].gamesTied + 1;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return players;
        }
        public Dictionary<int, GameInformation> returnGamesOfPlayer(string playerName)
        {
            Dictionary<int, GameInformation> gamesByPlayer = new Dictionary<int, GameInformation>();
            int uniqueID = 0;
            int winner = 0;
            string didYouWin = string.Empty;
            string player = "'" + playerName + "'";
            using (MySqlConnection conn = new MySqlConnection(connectionPath))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText = "select idPlayerList from PlayerList where playerName = " + player;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int.TryParse(reader["idPlayerList"].ToString(), out uniqueID);
                        }
                    }

                    command.CommandText = "select * from GameInformation where player1 = " + uniqueID + " or player2 = " + uniqueID;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        int gameID = 0;
                        while (reader.Read())
                        {
                            int.TryParse(reader["gameID"].ToString(), out gameID);

                            int.TryParse(reader["winner"].ToString(), out winner);

                            if (winner == uniqueID)
                            {
                                didYouWin = "Win";
                            }
                            else if (winner != uniqueID && winner != 0)
                            {
                                didYouWin = "Loss";
                            }
                            else if (winner == 0)
                            {
                                didYouWin = "Tie";
                            }
                            gamesByPlayer.Add(gameID, new GameInformation());
                            gamesByPlayer[gameID].didYouWin = didYouWin;
                            gamesByPlayer[gameID].dateOfGame = reader["timeGameEnded"].ToString();
                            if (uniqueID == (int)reader["player1"])
                            {
                                gamesByPlayer[gameID].opponentsID = (int)reader["player2"];
                                gamesByPlayer[gameID].playerScore = (int)reader["player1Score"];
                                gamesByPlayer[gameID].opponentScore = (int)reader["player2Score"];
                            }
                            else
                            {
                                gamesByPlayer[gameID].opponentsID = (int)reader["player1"];
                                gamesByPlayer[gameID].playerScore = (int)reader["player2Score"];
                                gamesByPlayer[gameID].opponentScore = (int)reader["player1Score"];
                            }
                        }
                    }
                    foreach (var item in gamesByPlayer)
                    {
                        command.CommandText = "select playerName from PlayerList where idPlayerList = " + item.Value.opponentsID;
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                gamesByPlayer[item.Key].nameOfOpponent = reader["playerName"].ToString();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return gamesByPlayer;
        }
        public TotalGameInfo returnGameInfo(string gameID)
        {
            int gameNumber = 0;
            int p1ID = 0;
            int scoreP1 = 0;
            int p2ID = 0;
            int scoreP2 = 0;

            int.TryParse(gameID, out gameNumber);
            TotalGameInfo gameInfo = new TotalGameInfo();
            using (MySqlConnection conn = new MySqlConnection(connectionPath))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "select * from GameInformation where gameID = " + gameNumber;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gameInfo.boogleBoard = reader["gameBoard"].ToString();
                            int.TryParse(reader["player1"].ToString(), out p1ID);
                            int.TryParse(reader["player1Score"].ToString(), out scoreP1);
                            int.TryParse(reader["player2"].ToString(), out p2ID);
                            int.TryParse(reader["player2Score"].ToString(), out scoreP2);
                            gameInfo.dateTime = reader["timeGameEnded"].ToString();
                        }
                    }
                    command.CommandText = "select * from PlayerList where idPlayerList = " + p1ID + " or idPlayerList = " + p2ID;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if ((int)reader["idPlayerList"] == p1ID)
                            {
                                gameInfo.player1.Add(reader["playerName"].ToString(), scoreP1);
                            }
                            else
                            {
                                gameInfo.player2.Add(reader["playerName"].ToString(), scoreP2);
                            }
                        }
                    }
                    command.CommandText = "select * from WordsTable where idFromGame = " + gameNumber;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["playersWhoPlayedWord"].ToString() == gameInfo.player1.ElementAt(0).Key.ToString())
                            {
                                if (reader["isIllegal"].ToString() != "false")
                                {
                                    gameInfo.p1IllegalWords.Add(reader["wordsPlayed"].ToString());
                                }
                                else
                                {
                                    gameInfo.wordsPlayedByP1.Add(reader["wordsPlayed"].ToString());
                                }
                            }
                            else if (reader["playersWhoPlayedWord"].ToString() == gameInfo.player2.ElementAt(0).Key.ToString())
                            {
                                if (reader["isIllegal"].ToString() != "false")
                                {
                                    gameInfo.p2IllegalWords.Add(reader["wordsPlayed"].ToString());
                                }
                                else
                                {
                                    gameInfo.wordsPlayedByP2.Add(reader["wordsPlayed"].ToString());
                                }
                            }
                            else
                            {
                                gameInfo.wordsInCommon.Add(reader["wordsPlayed"].ToString());
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
            return gameInfo;
        }
    }
}
public class GameWonLossTie
{
    public int gamesWon { get; set; }
    public int gamesLost { get; set; }
    public int gamesTied { get; set; }
    public GameWonLossTie()
    {

    }
}
public class GameInformation
{
    public string nameOfOpponent { get; set; }
    public int opponentsID { get; set; }
    public int playerScore { get; set; }
    public int opponentScore { get; set; }
    public string dateOfGame { get; set; }
    public string didYouWin { get; set; }
    public GameInformation()
    {

    }
}
public class TotalGameInfo
{
    public List<string> wordsPlayedByP1 { get; set; }
    public List<string> wordsPlayedByP2 { get; set; }
    public List<string> p1IllegalWords { get; set; }
    public List<string> p2IllegalWords { get; set; }
    public List<string> wordsInCommon { get; set; }
    public string timeLimit { get; set; }
    public Dictionary<string, int> player1 { get; set; }
    public Dictionary<string, int> player2 { get; set; }
    public string dateTime { get; set; }
    public string boogleBoard { get; set; }
    public TotalGameInfo()
    {
        player1 = new Dictionary<string, int>();
        player2 = new Dictionary<string, int>();
        wordsPlayedByP1 = new List<string>();
        wordsPlayedByP2 = new List<string>();
        p1IllegalWords = new List<string>();
        p2IllegalWords = new List<string>();
        wordsInCommon = new List<string>();
    }

}