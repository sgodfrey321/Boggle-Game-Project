using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseUpdater;

namespace HTMLwriter
{
    public class HTMLCreator
    {
        private DatabaseTool dbTool;
        public HTMLCreator()
        {
            dbTool = new DatabaseTool();
        }
        public string webpageRequestResponse()
        {
            string response = "HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n";
            return response;
        }
        public string emptyLineSend()
        {
            string emptyLine = "\r\n";
            return emptyLine;
        }
        public string sendHTML()
        {
            string htmlToSend = "<h1>Hello and Welcome the Boggle Server Created By Sam and Fahad!</h1>";
            return htmlToSend;
        }
        public string errorPageRequest()
        {
            string htmlToSend = "<p>The Page You Have Requested Is Not Avaliable</p>";
            return htmlToSend;
        }
        public string pageRequested(string pageRequested)
        {
            string htmlToSend = "<p>You are looking at a list of all " + pageRequested + " in the boggle database</h>";
            return htmlToSend;
        }
        public string sendPlayerList()
        {
            Dictionary<string, GameWonLossTie> players = dbTool.returnPlayers();
            string htmlToSend = "<table>";
            foreach (var item in players)
            {
                htmlToSend += "<tr>";
                htmlToSend += ("<td><a href=\"http://localhost:2500/games?players=" + item.Key + "\"> " + item.Key + "</a></td>" + "<td>Games Won : " + item.Value.gamesWon + "</td>"
                    + "<td>Games Lost : " + item.Value.gamesLost + "</td>" + "<td>Games Tied : " + item.Value.gamesTied + "</td>");
                htmlToSend += "</tr>";
            }
            htmlToSend += "</table>";
            return htmlToSend;
        }
        public string gamesByPlayer(string player)
        {
            Dictionary<int, GameInformation> gamesPlayedByPlayer = dbTool.returnGamesOfPlayer(player);
            string htmlToSend = "<table>";
            foreach (var item in gamesPlayedByPlayer)
            {
                htmlToSend += "<tr><td> " + "<a href=\"http://localhost:2500/games?gameID=" + item.Key + "\">" + item.Key + "</a></td></tr>" + "<tr><td>Outcome of Game: " + item.Value.didYouWin +"</td></tr>"+ "<tr><td>Your Score: " 
                    + item.Value.playerScore +"</td>"+ "<td>Name Of Opponent: " + item.Value.nameOfOpponent + "</rd>"+ "<td>Oponnents Score : "
                    + item.Value.opponentScore +"</td>"+ "<td>Date Game Was Played: " + item.Value.dateOfGame + "</td></tr>";
            }
            htmlToSend += "</table>";
            return htmlToSend;
        }
        public List<string> gameInformation(string gameID)
        {
            List<string> htmlToSend = new List<string>();
            TotalGameInfo gameInfo = new TotalGameInfo();
            gameInfo = dbTool.returnGameInfo(gameID);

            string htmlToSend1 = "<table>";
            htmlToSend1 += "<tr><td>Player 1: " + gameInfo.player1.ElementAt(0).Key.ToString() + "</td>" + "<td>      Player 1 Score: "
                + gameInfo.player1.ElementAt(0).Value + "<td></tr>" + "<tr><td>Player 2: " + gameInfo.player2.ElementAt(0).Key.ToString()
            + "</td>" + "<td>     Player 2 Score: " + gameInfo.player2.ElementAt(0).Value + "</td></tr>";
            htmlToSend1 += "</table>";
            htmlToSend.Add(htmlToSend1);
            htmlToSend.Add("<p></p>");
            htmlToSend.Add("The Boggle Board For This Game Looked Like:");
            string boggleBoard = gameInfo.boogleBoard;
            string htmlToSend2 = "<table><tr>";
            int columnCounter = 0;
            foreach (var item in boggleBoard)
            {
                if (columnCounter <= 3)
                {
                    if (item == 'Q')
                    {
                        htmlToSend2 += "<td>QU</td>";
                    }
                    else
                    {
                        htmlToSend2 += "<td>" + item.ToString() + "</td>";
                    }
                }
                else
                {
                    columnCounter = 0;
                    htmlToSend2 += "</tr><tr>" + "<td>" + item.ToString() + "</td>";
                }
                columnCounter++;
            }
            htmlToSend2 += "</tr></table><td>";
            htmlToSend.Add(htmlToSend2);

            htmlToSend.Add("<p>Words Played By " + gameInfo.player1.ElementAt(0).Key.ToString() + "</p>");
            string htmlToSend3 = "<table>";
            foreach (var item in gameInfo.wordsPlayedByP1)
            {
                htmlToSend3 += "<tr><td>" + item + "</td></tr>";
            }
            htmlToSend3 += "</table>";
            htmlToSend.Add(htmlToSend3);


            htmlToSend.Add("<p>Illegal Words Played By " + gameInfo.player1.ElementAt(0).Key.ToString() + "</p>");
            string illegalWords = "<table>";
            foreach (var item in gameInfo.p1IllegalWords)
            {
                illegalWords += "<tr><td>" + item + "</td></tr>";
            }
            illegalWords += "</table>";
            htmlToSend.Add(illegalWords);

            htmlToSend.Add("<p>Words Played By " + gameInfo.player2.ElementAt(0).Key.ToString() + "</p>");
            string htmlToSend4 = "<table>";
            foreach (var item in gameInfo.wordsPlayedByP2)
            {
                htmlToSend4 += "<tr><td>" + item + "</td></tr>";
            }
            htmlToSend4 += "</table>";
            htmlToSend.Add(htmlToSend4);

            htmlToSend.Add("<p>Illegal Words Played By " + gameInfo.player2.ElementAt(0).Key.ToString() + "</p>");

            illegalWords = "<table>";
            foreach (var item in gameInfo.p2IllegalWords)
            {
                illegalWords += "<tr><td>" + item + "</td></tr>";
            }
            illegalWords += "</table>";
            htmlToSend.Add(illegalWords);


            htmlToSend.Add("<p>Words Played In Common: </p>");

            string htmlToSend5 = "<table>";
            foreach (var item in gameInfo.wordsInCommon)
            {
                htmlToSend5 += "<tr><td>" + item + "</td></tr>";
            }
            htmlToSend5 += "</table>";
            htmlToSend.Add(htmlToSend5);


            return htmlToSend;
        }
    }
}
