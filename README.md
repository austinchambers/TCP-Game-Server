# TCP-Game-Server
A simple network server for a game using asynchronous IO. 
The server reads messages from individual clients and logs those messages. 
	
Solution Requirements:
   * Microsoft .NET 4.5 
   * Build tested on Windows 7 64-bit machine without any version of Visual 
     Studio installed. Application was created in Visual Studio 2013. 
   * If there are any troubles building without VS, please install the .NET 4.5
     SDK from the Windows Software Development Kit (SDK) for Windows 8 at
	 http://msdn.microsoft.com/en-us/library/windows/desktop/hh852363.aspx
	 
Building the Applications:
	* Run .\Scripts\makeServer.bat to build the server
	* Run .\Scripts\makeClient.bat to build the client
	* The compiled applications are also available in CompiledApplications.zip,
	  if it helps. 
	
Running the Applications:
	* Run .\Scripts\runServer.bat to run the server on port 30000
	  -or- 
	  Use command line: .\CompiledGameServer\GameServer <ServerPort>
	  
	* Run .\Scripts\runClient.bat to run the a simple test client that
	  generates a signle message to the local server on port 30000. 
	  -or-
	  Use command line: .\CompiledGameClient\GameClient <ServerHostName> <ServerPort>
	  
Design Decisions/Features:
	* Rather than simply spawning a new thread for each socket connection (which 
      would not even come close to the scale required), I used an approach using 
      asynchronous IO. It utilizes callbacks, which precludes the need to manage 
	  threads directly, and also scales much better.  
    * The messages are easily serializable. During serialization, the data is 
	  compressed. Likewise, the data is decompressed during deserialization. 
	  This should greatly help with performance, especially considering that 
	  the payload is in ASCII.  
	* Instead of using an Int16 for the MessageType value, I chose to use an 
      enumeration, which should be easier to deal with in the long run. I made 
	  the underlying type 16 bits to meet project requirements. 
	* Basic logging is supported for both the client and the server application. 
	  See debug.log. Note this logging is significantly reduced in production
	  environments, where log level is not DEBUG. This it to ensure that 
	  excessive logging won't interfere with performance. 
	
Future Fixes/Improvements:
    * Replace the "while (true) { }" in GameNetworkServer.cs with something
	  better. Maybe using events or possibly converting the server into a service.
	* The client test is very rudimentary. Next step would be to bombard the 
	  server with a massive amount of socket connections to see how it holds up. 
	* Right now the client is using the hostname to set up an endpoint to the 
	  server. This should support more options and be made more robust. 
	* The client and server endpoints are both acquired by always getting the first
  	  IP address associated with the machine. We need to be able to configure this. 
    * Implement the server in C++ (maybe with the Boost socket libraries). It be 
	  nice to not be confined to Windows. Plus, despite what I've read, I expect 
	  to get better performance in C++. It'd be an interesting experiment to 
	  build both using the same architecture and perform some benchmarking... 