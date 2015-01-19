// Entry point for GameNetworkServer
// Copyright 2015 Austin Chambers 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Networking
{
    class GameNetworkServer
    {
        // Simple command-line help. 
        public static void CommandHelp()
        {
            Console.WriteLine("GameServer serverPort\n\n");
            Console.WriteLine("\tserverPort\t\tSpecifies the port of the game " +
                              "network server (e.g., GameServer 30000)\n\n");
        }

        // Main entry point
        static void Main(string[] args)
        {
            short serverPort;
            if ((args.Length < 1) || !Int16.TryParse(args[0], out serverPort))
            {
                CommandHelp();
                return;
            }

            TCPServer server = new TCPServer(serverPort);
            Console.WriteLine("Starting server\n");
            server.Start();

            // TODO: Improve this. 
            Console.WriteLine("Awaiting messages...\n");
            while (true) { }
        }
    }
}
