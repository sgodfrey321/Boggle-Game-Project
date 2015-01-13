# Boggle-Game-Project
A simple program that will run a server and two clients and play a game of Boggle

This was a fun project that involved network programming and also involed a partner. We first created a wrapper for the C# socket
that allowed us to simplify socket usage. We then created our own server to run a boggle game; this also involed creating a custom
boggle game class that took the input from the server, evaluated the input and then gave the server relevent information. We then
created a Windows Presentation Form for a client which allowed the client to connect to the server and play a game against another
user. The last part of the project involved MySQL databases; we created a class that would take the game information after the 
game was over and would update a database which could be accesed by a user through a browser (we also created a HTML writer class
to give the information to the browser)
