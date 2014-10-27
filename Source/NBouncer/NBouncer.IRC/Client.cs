using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NBouncer.Tools;
using NBouncer.Tools.IO;

namespace NBouncer.IRC
{
    public class StateObject
    {
        public const int BufferSize = 8 * 1024;
        
        public Socket Client { get; private set;}
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder StringBuilder { get; private set; }

        public StateObject(Socket client)
        {
            if (client == null) throw new ArgumentNullException("client");

            StringBuilder = new StringBuilder();
            Client = client;
        }
    }

    public class Client
    {
        private const string CRLF = "\r\n";

        public string Name { get; private set; }
        public IPEndPoint Server { get; private set; }
        public Encoding Encoder { get; set; }

        private readonly Logger log;


        #region Network bigyok

        private Socket client;


        #endregion


        public Client(string name, IPEndPoint server)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name");
            if (server == null) throw new ArgumentNullException("endPoint");

            Name = name;
            Server = server;
            log = new Logger("cl:" + name);
            this.Encoder = Encoding.UTF8;
        }



        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);


        public void Connect()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.
            client.BeginConnect(Server, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();

            // Receive the response from the remote device.
            Receive(client);
            //receiveDone.WaitOne();


            // Release the socket.
            //client.Shutdown(SocketShutdown.Both);
            //client.Close();
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);

                log.Write("Connected to: {0}", client.RemoteEndPoint.ToString());

                connectDone.Set();
            }
            catch (Exception ex)
            {
                log.Exception(ex);
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject(client);

                Receive(state);

            }
            catch (Exception e)
            {
                log.Exception(e);
            }
        }


        private void Receive(StateObject state)
        {
            state.Client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.Client;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    var str = Encoder.GetString(state.Buffer, 0, bytesRead);

                    var endsWithCRLF = str.EndsWith(CRLF);

                    if (str.Contains("\r\n"))
                    {
                        var lines = str.Split("\r\n");
                        var linesCount = lines.Length;

                        for (int i = 0; i < linesCount; i++)
                        {
                            var line = lines[i];

                            // Ha nem CRLF-el fejeződött be, akkor hozzáadjuk a stringbuilderhez
                            if (!endsWithCRLF && i == linesCount - 1)
                            {
                                state.StringBuilder.Append(line);
                            }
                            else
                            {
                                // Ha van még az első sor előtt a StringBuilderben, akkor azt is hozzávesszük
                                if (i == 0 && state.StringBuilder.Length > 0)
                                {
                                    line = line + state.StringBuilder.ToString();
                                    state.StringBuilder.Clear();
                                }

                                RaiseReceivedLine(line);
                            }
                        }
                    }
                    else
                    {
                        state.StringBuilder.Append(str);                    
                    }

                    Receive(state);
                }
                else
                {
                    log.Write("<<EOF>>");

                    
                    // Signal that all bytes have been received.
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                log.Exception(e);
            }
        }

        private void RaiseReceivedLine(string line)
        {
            log.Write(line);
        }

        
        public void Send(string message)
        {
            log.Developer(message);

            if (!message.EndsWith("\r\n"))
                message += "\r\n";

            var byteData = this.Encoder.GetBytes(message);


            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }


        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
                log.Write("Sent {0} bytes to server.", bytesSent);

                sendDone.Set();
            }
            catch (Exception e)
            {
                log.Exception(e);
            }
        }



    }
}
