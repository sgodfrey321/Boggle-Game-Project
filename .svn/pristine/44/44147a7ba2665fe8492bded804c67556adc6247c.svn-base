using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

namespace CustomNetworking
{
    /// <summary>
    /// A Wrapper class for the Socket class that uses strings instead of bytes
    /// </summary>
    /// <Assignment> PS8 </Assignment>
    /// <Authors> Sam Godfrey and Fahad Alothaimeen </Authors>
    public class StringSocket
    {
        public bool comandRecieved = false; // for testing PS9 popuses only!
        // These delegates describe the callbacks that are used for sending and receiving strings.
        /// <summary>
        /// DescribeS the callbacks that are used for sending strings.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="payload">Payload Object</param>
        public delegate void SendCallback(Exception e, object payload);
        /// <summary>
        /// Describe the callbacks that are used for receiving strings.
        /// </summary>
        /// <param name="s">Message Recieved</param>
        /// <param name="e">Exception</param>
        /// <param name="payload">Payload Object</param>
        public delegate void ReceiveCallback(String s, Exception e, object payload);

        private Socket socket;
        private static Encoding encoder;
        private readonly object sendSync = new object();
        private readonly object recieveSync = new object();
        private String outgoingMessage;
        private String incomingMessage;
        private String incomingString;
        private Queue<CallBacksInfo> recieveCallQueue;
        private Queue<CallBacksInfo> sendCallBackQueue;
        private Queue<string> penddingMessages;
        private bool sendIsOngoing;


        /// <summary>
        /// The constructor for our string socket class
        /// </summary>
        /// <param name="s">Connected socket to be used throughout the string socket to send and receive messages</param>
        /// <param name="e">The encoding style to be used when we are building strings from bytes</param>
        public StringSocket(Socket s, Encoding e)
        {
            socket = s;
            encoder = e;
            sendCallBackQueue = new Queue<CallBacksInfo>();
            recieveCallQueue = new Queue<CallBacksInfo>();
            penddingMessages = new Queue<string>();
            sendIsOngoing = false;
            outgoingMessage = "";
            incomingMessage = "";
        }

        /// <summary>
        /// The begin send method that takes in a string, changes it to a byte array and then uses the underlying socket to send a message
        /// </summary>
        /// <param name="s">String to send</param>
        /// <param name="callback">Callback method to be called after the message has been sent</param>
        /// <param name="payload">Arbitrary object that can be used as anything, identifier or whatever</param>
        public void BeginSend(String s, SendCallback callback, object payload)
        {
            lock (sendSync)
            {
                if (s != null)
                {
                    // we do not need to lock main
                    // we need to send everything as they come!
                    CallBacksInfo callBackObject = new CallBacksInfo(payload, callback, s);
                    sendCallBackQueue.Enqueue(callBackObject);

                    // If there's not a send ongoing, start one.
                    if (!sendIsOngoing)
                    {
                        outgoingMessage += s;
                        sendIsOngoing = true;
                        SendBytes();
                    }
                }
                else
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// Sends the bytes to the endPoint destination
        /// </summary>
        private void SendBytes()
        {
            if (outgoingMessage == "")
            {
                CallBacksInfo currentSendCallBack = sendCallBackQueue.Dequeue();
                SendCallback callback = currentSendCallBack.ScallBack;
                if (callback != null)
                {
                    object payload = currentSendCallBack.payload;
                    string currentString = currentSendCallBack.sendString;
                    try
                    {
                        if (payload != null)
                        { new Thread(() => callback(null, payload)).Start(); }
                        else
                        { new Thread(() => callback(null, null)).Start(); }
                    }
                    catch (Exception e)
                    {
                        if (payload != null)
                        { new Thread(() => callback(e, payload)).Start(); }
                        else
                        { new Thread(() => callback(e, null)).Start(); }
                    }
                }

                if (sendCallBackQueue.Count != 0)
                {
                    outgoingMessage += sendCallBackQueue.Peek().sendString;
                    SendBytes();
                }
                else
                {
                    sendIsOngoing = false;
                }
            }
            else
            {
                byte[] outgoingBuffer = encoder.GetBytes(outgoingMessage);
                outgoingMessage = ""; // assume the message will be sent in full
                try
                {
                    socket.BeginSend(outgoingBuffer, 0, outgoingBuffer.Length,
                                     SocketFlags.None, MessageSent, outgoingBuffer);
                }

                catch
                {
                    comandRecieved = true; // for testing PS9 popuses only!
                }
            }
        }

        /// <summary>
        /// The callback that is called upon completion of a socket.beginsend method, this is not the callback that we pass into the stringsocket.beginsend method
        /// </summary>
        /// <param name="result">The result of the socket.beginsend method</param>
        private void MessageSent(IAsyncResult result)
        {
            // Find out how many bytes were actually sent
            int bytes = socket.EndSend(result);

            // Get exclusive access to send mechanism
            lock (sendSync)
            {
                // Get the bytes that we attempted to send
                byte[] outgoingBuffer = (byte[])result.AsyncState;

                // The socket has been closed
                if (bytes == 0)
                {
                    socket.Close();
                }

                // Prepend the unsent bytes and try sending again.
                else
                {
                    outgoingMessage += encoder.GetString(outgoingBuffer, bytes,
                                                  outgoingBuffer.Length - bytes);
                    SendBytes();
                }
            }
        }

        /// <summary>
        /// The method that calls BeginReceive with the underlying socket. We queue the parameters so we can use them later in the BeginRecieve callback
        /// </summary>
        /// <param name="callback">Callback method to be called during the BeginReceive callback</param>
        /// <param name="payload">Arbitrary object that can be used as anything, identifier or whatever</param>
        public void BeginReceive(ReceiveCallback callback, object payload)
        {
            lock (recieveSync)
            {
                // we cannot assume that the payload is always going to be integer, so we return it as is!
                // we do not need to lock main
                recieveCallQueue.Enqueue(new CallBacksInfo(payload, callback));
                byte[] recieveBuffer = new byte[1028];
                try
                {
                    socket.BeginReceive(recieveBuffer, 0, recieveBuffer.Length, SocketFlags.None, MessageRecieved, recieveBuffer);
                }
                catch
                {

                }
            }
        }


        /// <summary>
        /// The message received call back, this method checks to make sure that we have received all of the bytes that were sent, or that we received 0 bytes
        /// </summary>
        /// <param name="ar">The result of the socket.beginreceive method</param>
        private void MessageRecieved(IAsyncResult ar)
        {
            try
            {
                int bytesRecieved = socket.EndReceive(ar);

                lock (recieveSync)
                {
                    byte[] bufferRecieved = (byte[])(ar.AsyncState);

                    incomingMessage = encoder.GetString(bufferRecieved, 0, bytesRecieved);
                    if (incomingMessage.Count(x => x == '\n') == 0 && incomingMessage.Count(x => x == '\r') == 0)
                    {
                        // do not call callBack, keep asking for more!
                        incomingString += incomingMessage;
                    }
                    else
                    {
                        incomingMessage = incomingString + incomingMessage;
                        incomingString = "";


                        List<string> message = incomingMessage.Split(new Char[] { '\n', '\r' }).ToList();
                        message.RemoveAt(message.Count - 1);
                        foreach (string item in message)
                        {
                            penddingMessages.Enqueue(item);
                        }

                        // start excuting the pending callBacks on the pending Messages
                        // if no callBacks, store/keep the remaining messages
                        List<string> list = penddingMessages.ToList<string>();
                        foreach (string item in list)
                        {
                            if (recieveCallQueue.Count == 0)
                            {
                                break;
                            }

                            CallBacksInfo callBackObject = recieveCallQueue.Dequeue();
                            ReceiveCallback callback = callBackObject.Rcallback;
                            if (callback != null)
                            {
                                if (callBackObject.payload != null)
                                {
                                    Thread t = new Thread(() => callback(penddingMessages.Dequeue(), null, callBackObject.payload));
                                    t.SetApartmentState(ApartmentState.STA);
                                    t.Start();
                                }
                                else
                                { new Thread(() => callback(penddingMessages.Dequeue(), null, null)).Start(); }
                            }
                        }
                    }
                    // ask for more!
                    socket.BeginReceive(bufferRecieved, 0, bufferRecieved.Length, SocketFlags.None, MessageRecieved, bufferRecieved);
                }
            }
            catch (SocketException)
            {
                lock (recieveSync)
                {
                    penddingMessages.Enqueue(null);
                    if (recieveCallQueue.Count != 0)
                    {
                        CallBacksInfo callBackObject = recieveCallQueue.Dequeue();
                        ReceiveCallback callback = callBackObject.Rcallback;
                        new Thread(() => callback(penddingMessages.Dequeue(), null, callBackObject.payload)).Start();
                    }
                }
            }
            catch (ObjectDisposedException)
            {

            }
        }




        /// <summary>
        /// First shutsdown the underlying socket and then closes down the socket
        /// </summary>
        public void Close()
        {
            while (sendIsOngoing)
            {
                System.Threading.Thread.Sleep(1000);
            }

            recieveCallQueue.Clear();
            sendCallBackQueue.Clear();
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (ObjectDisposedException)
            {

            }
            socket.Close();
        }


        /// <summary>
        /// A structure that holds all of the info for the Message Receive call back, such as payload and the callback function
        /// </summary>
        private struct CallBacksInfo
        {
            public object payload { get; set; }
            public ReceiveCallback Rcallback { get; set; }
            public SendCallback ScallBack { get; set; }
            public string sendString { get; set; }



            public CallBacksInfo(object _payload, ReceiveCallback _callback)
                : this()
            {
                payload = _payload;
                Rcallback = _callback;
                ScallBack = (e, o) => { };
                sendString = "";
            }

            public CallBacksInfo(object _payload, SendCallback _callBack, string _sendString)
                : this()
            {
                payload = _payload;
                Rcallback = (s, e, o) => { };
                ScallBack = _callBack;
                sendString = _sendString;
            }
        }
    }
}