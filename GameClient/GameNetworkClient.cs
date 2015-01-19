// GameNetworkClient -- Responsible for taking game messages and sending them to the network server.  
// Copyright 2015 Austin Chambers 
using Game.Messaging;
using log4net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Game.Networking
{
    class GameNetworkClient
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private short _serverPort;
        private IPAddress _serverIP;
        private static Socket _socket;

        // Constructor for the network client. 
        public GameNetworkClient(string hostName, short serverPort)
        {
             _serverPort = serverPort;
            // Get the server IP using DNS 
            IPHostEntry remoteMachineInfo = Dns.GetHostEntry(hostName);

            // Use the address from the first interface. This should be fixed later. 
            _serverIP = remoteMachineInfo.AddressList[0];
            log.Debug(String.Format("Resolving {0} to server IP: {1}", hostName, _serverIP.ToString()));
        }

        // Connect to the game network server. 
        public void Connect()
        {
            try
            {
                if (null != _socket)
                {
                    _socket.Close();
                }

                string localHostName = Dns.GetHostName();
                IPHostEntry localMachineInfo = Dns.GetHostEntry(localHostName);
                IPEndPoint serverEndpoint = new IPEndPoint(_serverIP, _serverPort);
                
                // Use the address from the first interface. This should be fixed later. 
                IPAddress localIP = localMachineInfo.AddressList[0];
                IPEndPoint myEndpoint = new IPEndPoint(localIP, 0);
                log.Debug(String.Format("Resolving {0} to local IP: {1}", localHostName, localIP.ToString()));

                _socket = new Socket(myEndpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(serverEndpoint);
                log.Debug("Server connection established");
            }
            catch (Exception e)
            {
                log.Error(e);
                throw e;
            }
        }

        // Serialize, compress, and send the game message to the network server.  
		public void SendMessage(Message gameMessage)
		{
			if (null != _socket)
			{
                _socket.Send(Message.SerializeToBytes(gameMessage));
			}
		}

        // Shutdown the socket and close the connection. 
		public void Close()
		{
			if (null != _socket)
			{
                _socket.Shutdown(SocketShutdown.Both);
				_socket.Close();
				_socket = null;
			}
		}
    }
}
