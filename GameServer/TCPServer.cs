// TCP server class for GameNetworkServer
// Copyright 2015 Austin Chambers 
using Game.Messaging;
using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Game.Networking
{
    class TCPServer
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Socket _serverSocket; 
        private short _port; 

        // Constructor.
        public TCPServer(short port)
        {
             _port = port;
        }

        // Used to keep track of socket state when making asynchronous calls. 
        private class SocketState 
        { 
            public const int BUFFER_SIZE = 255;
            public Socket Socket; 
            public byte[] Buffer; 
            public MemoryStream Data;
        } 

        // Start the socket server. 
        public void Start() 
        {
            try
            {
                // Resolving local machine information 
                string localHostName = Dns.GetHostName();
                IPHostEntry localMachineInfo = Dns.GetHostEntry(localHostName);

                // Use the address from the first interface. This should be fixed later. 
                IPAddress localIP = localMachineInfo.AddressList[0];

                IPEndPoint _endpoint = new IPEndPoint(localIP, _port);
                log.Debug(String.Format("Resolving {0} to local IP: {1}", localHostName, localIP.ToString()));

                // Create the socket, bind it, and start listening 
                log.Debug("Binding TCP Socket");
                _serverSocket = new Socket(_endpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(_endpoint);

                log.Debug("Listening for socket connections");
                // Sets up internal queues for the socket. Whenever a connection request is attempted, it's placed in a queue. 
                _serverSocket.Listen((int)SocketOptionName.MaxConnections);
                _serverSocket.BeginAccept(new AsyncCallback(SocketConnectionCallback), _serverSocket); 
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }
        }

        // Triggered when a new connection is initiated. 
        private void SocketConnectionCallback(IAsyncResult result) 
        { 
            SocketState state = new SocketState(); 
            try 
            { 
                // Initialize socket's state on new connection
                Socket newSocket = (Socket)result.AsyncState; 
                state.Socket = newSocket.EndAccept(result); 
                state.Buffer = new byte[SocketState.BUFFER_SIZE]; 
                state.Data = new MemoryStream();
                
                // Get ready to read data from the new connection
                state.Socket.BeginReceive(state.Buffer, 0, SocketState.BUFFER_SIZE, SocketFlags.None, new AsyncCallback(SocketDataCallback), state); 

                // Get ready to accept more connections on the server
                _serverSocket.BeginAccept(new AsyncCallback(SocketConnectionCallback), result.AsyncState); 
            }
            catch (ObjectDisposedException)
            {
                // The socket was already closed -- we don't know if all data has been sent.  
                CloseSocket(state);
            }
            catch (SocketException e)
            {
                // Handle "Connection reset by peer" (Socket error code WSAECONNRESET == 10054)
                // From: (http://msdn.microsoft.com/en-us/library/windows/desktop/ms740668(v=vs.85).aspx)
                if (e.ErrorCode == 10054)
                {
                    CloseSocket(state);
                }
                else
                {
                    // Log unexpected socket exceptions in debug mode. 
                    log.Debug(e);
                }
            }
        } 
        
        // Triggered when data is received on the socket connection. 
        private void SocketDataCallback(IAsyncResult result) 
        { 
            SocketState state = (SocketState)result.AsyncState;
            Socket socket = state.Socket;
            try 
            {
                int readBytes = socket.EndReceive(result);
                // If there was something read from the socket, process it. 
                // Otherwise, close the connection. 
                if (readBytes > 0) 
                {
                    // Append from the buffer to the data stream. 
                    state.Data.Write(state.Buffer, 0, readBytes);
                    state.Socket.BeginReceive(state.Buffer, 0, SocketState.BUFFER_SIZE, SocketFlags.None, new AsyncCallback(SocketDataCallback), state);
                } 
                else 
                {
                    if (state.Data.Length > 1)
                    {
                        ClientStateUpdateMessage clientStateMessage;
                        clientStateMessage = Message.DeserializeFromStream<ClientStateUpdateMessage>(state.Data);

                        // All the data has been read. 
                        Console.WriteLine(string.Format("Message from {0}:\n{1}", state.Socket.RemoteEndPoint, clientStateMessage.ToString()));
                    }
                    CloseSocket(state); 
                } 
            }
            catch (ObjectDisposedException)
            {
                // The socket was unexpectedly closed. 
                CloseSocket(state);
            }
            catch (SocketException e)
            {
                // Handle "Connection reset by peer" (Socket error code WSAECONNRESET == 10054)
                // From: (http://msdn.microsoft.com/en-us/library/windows/desktop/ms740668(v=vs.85).aspx)
                if (e.ErrorCode == 10054)
                {
                    CloseSocket(state);
                }
                else
                {
                    // Log unexpected socket exceptions in debug mode. 
                    log.Info(e);
                }
            }
        } 
        
        // Close the socket. 
        private void CloseSocket(SocketState ci) 
        { 
            ci.Socket.Close(); 
        }
    }
}
