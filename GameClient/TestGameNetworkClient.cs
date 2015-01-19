// TestGameNetworkClient -- Very simple test class for the network client.   
// Copyright 2015 Austin Chambers 
using Game.Messaging;
using System;

namespace Game.Networking
{
    class TestGameNetworkClient
    {
        // Generate a large payload message so that, even with compression, the input buffer 
        // on the server will overflow. 
        readonly static string largePayload = @"Large Message!
                5555555555555555555555555555555555
                6666666666666666666666666666666666
                777777777777777777777777777777777
                888888888888888888888888888888888
                99999999999999999999999999999999999
                00000000000000000000000000000000000
                5555555555555555555555555555555555
                6666666666666666666666666666666666
                777777777777777777777777777777777
                888888888888888888888888888888888
                99999999999999999999999999999999999
                00000000000000000000000000000000000
                5555555555555555555555555555555555
                6666666666666666666666666666666666
                777777777777777777777777777777777
                888888888888888888888888888888888
                99999999999999999999999999999999999
                00000000000000000000000000000000000
                End of Large Message!";

        // Simple command-line help. 
        public static void CommandHelp()
        {
            Console.WriteLine("GameClient serverHostName serverPort\n\n");
            Console.WriteLine("\tserverHostName    Specifies the host name of the game server");
            Console.WriteLine("\tserverPort        Specifies the port of the game server\n");
            Console.WriteLine("Example Usage: GameClient Asgard 30000");
        }

        static void Main(string[] args)
        {
            short serverPort;
            if ((args.Length < 2) || !Int16.TryParse(args[1], out serverPort))
            {
                CommandHelp();
                return;
            }

            // For convenience, get the server using the host name. 
            // This mechanism will be improved later.  
            string hostName = args[0];
            GameNetworkClient client = new GameNetworkClient(hostName, serverPort);
            client.Connect();

            // Simple client state message
            ClientStateUpdateMessage clientStateMessage = new ClientStateUpdateMessage()
            {
                Payload = largePayload,
                UserID = 123,
                Version = 1
            };
            Console.WriteLine(string.Format("Sending Message:\n{0}", clientStateMessage.ToString()));

            client.SendMessage(clientStateMessage);
            client.Close();
        }
    }
}
