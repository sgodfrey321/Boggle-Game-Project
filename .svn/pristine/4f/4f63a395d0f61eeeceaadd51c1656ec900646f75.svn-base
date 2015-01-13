using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using CustomNetworking;
using System.Text;
using BoggleServer;
using System.Threading;
using BoggleGameClass;
using System.IO;
using System.Collections.Generic;

namespace BoggleServerTests
{
    /// <Assignment> PS8 </Assignment>
    /// <Authors> Sam Godfrey and Fahad Alothaimeen </Authors>
    [TestClass]
    public class TestClass1
    {
        /* 
        HashSet<string> words;
        string wordFromFile;
        string message;

        [TestMethod]
        public void BoggleBoardTest()
        {
            createValidWords("../../../Resources/Dictionary/Words.txt");
            BoggleGame boggleGame = new BoggleGame("Sam", "Fahad", "hellolovearethat", words);
            Assert.AreEqual(true, boggleGame.isValid("TEAR"));
        }

        [TestMethod]
        public void BoggleBoardScoring()
        {
            createValidWords("../../../Resources/Dictionary/Words.txt");
            BoggleGame boggleGame = new BoggleGame("Sam", "Fahad", "dictionarythatis", words);
            boggleGame.wordScore("dictionary", "Sam");
            Assert.AreEqual(11, boggleGame.player1.playerScore);
        }

        [TestMethod]
        public void BoggleBoardScoringWithTwoPlayers()
        {
            createValidWords("../../../Resources/Dictionary/Words.txt");
            BoggleGame boggleGame = new BoggleGame("Sam", "Fahad", "dictionarythatis", words);
            boggleGame.wordScore("dictionary", "Sam");
            boggleGame.wordScore("that", "Sam");
            boggleGame.wordScore("randy", "Sam");

            boggleGame.wordScore("than", "Fahad");
            boggleGame.wordScore("this", "Fahad");
            boggleGame.wordScore("tardy", "Fahad");

            Assert.AreEqual(14, boggleGame.player1.playerScore);
            Assert.AreEqual(4, boggleGame.player2.playerScore);
        }
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
        public IEnumerable<string> StringBuilder(string letter, HashSet<string> word)
        {

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
        private bool isEmpty(string i)
        {
            return (i == "" || i == null);
        }

        [TestMethod]
        public void SimpleConnection()
        {
            
            BoggleServerClass boggleServer = new BoggleServerClass(new string[3] { "10", "../../../Resources/Dictionary/Words.txt", "hellolovearethat" });

            TcpClient client = new TcpClient("localhost", 2000);
            Socket clientSocket = client.Client;
            StringSocket clientSS = new StringSocket(clientSocket, new UTF8Encoding());
            clientSS.BeginReceive(messegeReceived, clientSS);
            string msg = "PLAY Sam\n";
            clientSS.BeginSend(msg, (e, o) => { }, clientSS);

            msg = "PLAY Fahad\n";
            TcpClient client1 = new TcpClient("localhost", 2000);
            Socket clientSocket1 = client1.Client;
            StringSocket clientSS1 = new StringSocket(clientSocket1, new UTF8Encoding());
            clientSS1.BeginReceive(messegeReceived, clientSS1);
            clientSS1.BeginSend(msg, (e, o) => { }, clientSS1);

            Thread.Sleep(100);

            Assert.AreEqual(1, boggleServer.gameDictionary.Count);

            clientSS.BeginSend("WORD hello\n", (e, o) => { }, clientSS);
            clientSS1.BeginSend("WORD that\n", (e, o) => { }, clientSS1);
            clientSS1.BeginSend("WORD fdss\n", (e, o) => { }, clientSS1);
            clientSS.BeginSend("WORD svfv\n", (e, o) => { }, clientSS);
            clientSS.BeginSend("WORD hello\n", (e, o) => { }, clientSS);

            Thread.Sleep(100);

            Assert.AreEqual(1, boggleServer.tuple.Item1.currentGame.player1.playerScore);

        }
        private void messegeReceived(string s, Exception e, object payload)
        {

            message = s;
            ((StringSocket)payload).BeginReceive(messegeReceived, (StringSocket)payload);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TimerCheck()
        {
            createValidWords("../../../Resources/Dictionary/Words.txt");
            Player p1 = new Player(null, "Sam");
            Player p2 = new Player(null, "Fahad");
            BoggleGame boggle = new BoggleGame("Sam", "Fahad", "dictionarythatis", words);
            p1.setGame(boggle);
            p2.setGame(boggle);
            
            Tuple<Player, Player> players = new Tuple<Player, Player>(p1, p2);
            TimerClass timerToCheck = new TimerClass(players, "60");
            timerToCheck.startTimer();

            boggle.wordScore("dictionary", "Sam");
            boggle.wordScore("that", "Sam");
            boggle.wordScore("randy", "Sam");

            boggle.wordScore("than", "Fahad");
            boggle.wordScore("this", "Fahad");
            boggle.wordScore("tardy", "Fahad");
            timerToCheck.gameOver(players);
        }
    }
         * */
    }
}
