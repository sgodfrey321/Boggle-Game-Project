Hello and welcome the boggle server created by Sam and Fahad.

PS8:
When doing this project we felt that the first transmission of data should always be the name, this allowed us to build the data structures pretty early. We also decided that the next stuff
to come along would be dealing with the game itself, ie word plays. This way we could handle any invalid commands or closing of the socket in one method.
We build a Boggle Game Class to handle the scoring and the game itself, this was nice because it let us not be connected to the server. The timer was handled on the server side, we just
decided to spin off a new thread for each new game that was started. We felt this was a little better then putting in the Boggle Game class as it seemed to meet the specs a little bit better,
however putting the timer in the boggle game class would have been much easier.

PS9:
WOrking on project 9 was super fun, using WPF was a interesting and complex process. For this project we decided that we wanted to get everything working first before we started
working on the GUI; we decided that a correct implementation was much more important that making are GUI look pretty.

The way we set this project up was to have a BoggleClient class that handled all of the string socket stuff and parsing the game information. We have 5 or 6 actions that communicate
to the main GUI and these actions are all based upon messages that the BoggleClient has recieved. 

The main GUI consists of three seperate windows; one for the connection proccess, one for the gameplay and one for the gameover window.
The connection window asks for a IPAdress or HostName, and will pop up a message box if the connection is not established telling you that the destenation was incorrect. The
other label is for the name of the player.

The game window takes care of the game play, we set up and display the boggle board and have a text box for inputing words. We also have labels that display the time, your
opponents score, your score and the words you have played.

The game over window pops up when either the time has run out or your opponent has quit the game, either on purpose or not. This game window displays the words both you
and your opponent have played (both legal and illegal) and the words you have in commmon. We also have the option to close the game or reconnect which will take you back
to the connection window.

We decided not to to any text handlers, ie pressing enter in a text box to play a word because to be honest we just ran out of time (it was the least of our worries).

PS10:
This project set was pretty easy and fun after getting the hand of mySql down, which was a bit challenging. Neither Fahad or I have any experiance in HTML or CSS programming so this was a fun chance
to learn something new, and boy did HTML turn out to be pretty easy. Let me start by describing the database:

Database Description:
Our database is split up into three tables 1) A players table which has all of the players that have ever played a game in it. It has a unique ID which is autoincremeted and stored as the primary key.
2) The next table is a game information table which holds all of the high level information about a game: the game ID (the primary key, also autoincremted),time game was played, player1 and player2, each players score, the time the game was played for
and I decided to throw in who won the game by the player unique ID. 3) The third table had all of the deeper information about the game(no primary key in this table): the gameboard that was used, the list of each players words, the names of the players
, illegal words, words in common and if the word is legal. The way that we set up the data base required no updates, since you could never replay the same game (even if you choose to it would still count as a new game in the
eyes of the database) all we had to do was to add all the relevent information about the game to the respective tables. For example the insert into game information table looked like:

command.CommandText = "INSERT INTO GameInformation " +
                       "(gameBoard, player1, player1Score, player2, player2Score, timeOfGame, timeGameEnded, winner)"
                       + "VALUES (" + gameBoard + ", " + id + ", " + player1Score + ", " + id2 + ", " + player2Score + ", " + timeOfGame + ", "
                       + timeString + ", " + winner + ")";

Inserting a new player was simple enough as well, before we added a game we got all of the current players in the playerNameList table and put them in a list. If the player to add to the table was in this list
we just did not add them to the data base, however if they are not in the list we then added them to the table with an instruction like this:

MySqlCommand command = conn.CreateCommand();
            if (!playerList.Contains(playerName))
            {
                command.CommandText = "insert into PlayerList (playerName) values (" + playerName + ")";
                using (MySqlDataReader reader = command.ExecuteReader()) { }
            }

Adding the game information to the last table, the WordsPlayed table required a bit more logic but was simple enough. We just combined all of the words into a three lists, all of player1 words, all of player2 words
and all of the words in common. We then looped through the lists and added the words to the database but we kept a list of all the illegal words each player played and checked each item going into the database
to make sure it was legal, if not we just added true to the isIllegal column of the table. The code looked like this for player1:

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

Getting the data out of the database was a little more tricky and required the proper set up of some objects to do. Returning a players unique ID was simple enough:

command.CommandText = "select idPlayerList from PlayerList where playerName = " + playerName;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int.TryParse(reader["idPlayerList"].ToString(), out id);
                }
            }
Returning game information though was a bit more tricky and required us to make a couple classes to hold all of the information and make it easy to pass. To request all of the 
data about the players we just first found each players uniqueID and added the name of the player to a dictionary:

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
We then looped through all of the players in the playerNameList and found each of the games that the player was involved in and found out if they won, lost or tied the game and incremented the correct value:

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
This query returned all of the players and their game stats. To return the game information of a single player was simple enough:

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



Returning the actual game info was a bit more challenging but still just involed a class that held all of the data so we could easly pass it over. The main reason that this was a touch more challenging
was the fact that we first had to get the uniqueGameID, then find the players who played in that game and then get all of the words played in that game and sandwich them all together but like I said
we created a sepereate class which made it super easy to do:

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
After getting the basics of database queries and how to properly use what a database returns this assignment was super easy. Like I said Fahad and I do not know CSS and hardly and HTML so the website is a bit
boring and drab but I am happy with the way that it turned out.