using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoggleClientModel;
using BoggleServer;
using BoggleClientView;
using System.Windows.Markup;
using System.Collections.Generic;

namespace ClientTesting
{
    [TestClass]
    public class BoggleClientTest
    {
        private string opponentsName;
        private string score;
        private string opponentScore;
        private string currentTime;
        private List<Tuple<int, List<string>>> gameInfo;
        private int playerScore;
        private int oppScore;
        private string updatedWord;
        private bool _exceptionThrown = false;


        BoggleServerClass server = new BoggleServerClass(new string[3] { "30", "../../../Resources/Dictionary/Words.txt", "ABCDEFGQIMNTHEIR" });
        MainWindow main = new MainWindow();

        [TestMethod]
        public void OpponentValidation()
        {
            main.client.GameStart += opponentValidation;
            main.client.updateScore += updateScore;
            main.client.updateOpponentsScore += updateOpponentsScore;
            main.client.changeTime += changeTime;
            main.client.closeGame += closeGame;
            main.client.abortGame += abortGame;
            main.client.updateWordList += updateWordList;

            // testing the behavior of the model when a null host name is passed
            try
            {
                main.client.connect(null, null, main.client);
            }
            catch (Exception)
            {
                _exceptionThrown = true;
            }
            Assert.AreEqual(_exceptionThrown, main.client.exceptionThrown);

            // testing the supposed behavior of the model toward the view when the game starts
            main.client.connect("localhost", "Sam", main.client);
            main.client.message("START ABCDEFGQIMNTHEIR 30 Fahad", null, main.client.clientSS);
            Assert.AreEqual("Fahad", opponentsName);

            // testing the supposed behavior of the model toward the view when the score changes
            main.client.message("SCORE 1 0", null, main.client.clientSS);
            Assert.AreEqual("1", score);

            // testing the supposed behavior of the model toward the view when time is sent
            main.client.message("TIME 29", null, main.client.clientSS);
            Assert.AreEqual("29", currentTime);

            // testing the supposed behavior of the model toward the view when the game ends
            main.client.message("GAME OVER ", null, main.client.clientSS);
            Assert.IsTrue(main.client.comandRecieved1);

            // testing the supposed behavior of the model toward the view when the game stops
            main.client.message("STOP 1!2!3!4!5!6!7!8!9!10!11!12", null, main.client.clientSS);
            Assert.AreEqual(11, playerScore);
            Assert.AreEqual(12, oppScore);

            // testing the supposed behavior of the model toward the view when the game terminates
            main.client.message("TERMINATE 1!2!3!4!5!6!7!8!9!10!12!11", null, main.client.clientSS);
            Assert.AreEqual(12, playerScore);
            Assert.AreEqual(11, oppScore);

            // testing the supposed behavior of the model toward the view when the list of words need to be updated
            main.client.ScoreWord("AMAZING");
            Assert.AreEqual("AMAZING", updatedWord);

            // testing the supposed behavior of the model toward the view when the game terminates early
            main.client.earlyTermination();
            Assert.IsTrue(main.client.comandRecieved2);

            // testing the supposed behavior of the model toward the view when the socket closes
            main.client.CloseSocket();
            main.client.clientSS.BeginSend("blah", null, main.client.clientSS);
            Assert.IsTrue(main.client.clientSS.comandRecieved);

        }
        private void opponentValidation(string chars, string opponent, string time)
        {
            opponentsName = opponent;
        }
        private void updateScore(string yourScore)
        {
            score = yourScore;
        }
        private void updateOpponentsScore(string theirScore)
        {
            opponentScore = theirScore;
        }
        private void changeTime(string _currentTime)
        {
            currentTime = _currentTime;
        }
        private void closeGame(List<Tuple<int, List<string>>> _gameInfo, int _playerScore, int _opponentsScore1)
        {
            gameInfo = _gameInfo;
            playerScore = _playerScore;
            oppScore = _opponentsScore1;
        }
        private void abortGame(List<Tuple<int, List<string>>> _gameInfo, int _playerScore, int _opponentsScore1)
        {
            gameInfo = _gameInfo;
            playerScore = _playerScore;
            oppScore = _opponentsScore1;
        }
        private void updateWordList(string word)
        {
            updatedWord = word;
        }
    }
}
