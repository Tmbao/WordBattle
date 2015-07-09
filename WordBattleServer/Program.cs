using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace WordBattleServer
{
    class Program
    {
        const int PORT = 4510;
        const int MAX_CONNECTION = 10;

        // Would cause error due to asynchronous tasks
        static ConcurrentDictionary<string, Room> rooms = new ConcurrentDictionary<string, Room>();

        static byte[] buffer = new byte[1024];

        static void Main(string[] args)
        {
            try
            {
                // Resolves a host name to an IPHostEntry instance 
                var ipHost = Dns.GetHostEntry("");

                // Gets first IP address associated with a localhost 
                IPAddress ipAddr = ipHost.AddressList[2];

                // Creates a network endpoint 
                var ipEndPoint = new IPEndPoint(ipAddr, PORT);

                // Creates one SocketPermission object for access restrictions
                var permission = new SocketPermission(
                    NetworkAccess.Accept, // Allowed to accept connections 
                    TransportType.Tcp, // Defines transport types 
                    "", // The IP addresses of local host 
                    SocketPermission.AllPorts // Specifies all ports 
                    );

                // Ensures the code to have permission to access a Socket 
                permission.Demand();

                // Create one Socket object to listen the incoming connection
                var listener = new Socket(
                    ipAddr.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp);

                // Associates a Socket with a local endpoint 
                listener.Bind(ipEndPoint);
                Console.WriteLine("The server has started.");

                // Places a Socket in a listening state and specifies the maximum 
                // Length of the pending connections queue 
                listener.Listen(MAX_CONNECTION);

                // Begins an asynchronous operation to accept an attempt 
                var callback = new AsyncCallback(AcceptCallback);
                listener.BeginAccept(callback, listener);
                Console.WriteLine("The server is listening on {0} port {1}", ipEndPoint.Address, ipEndPoint.Port.ToString());

                while (true)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = null;

            Socket handler = null;
            try
            {
                // Get Listening Socket object 
                listener = (Socket)ar.AsyncState;

                // Create a new socket 
                handler = listener.EndAccept(ar);

                // Begins to asynchronously receive data 
                handler.BeginReceive(
                    buffer, // An array of type Byt for received data 
                    0, // The zero-based position in the buffer  
                    buffer.Length, // The number of bytes to receive 
                    SocketFlags.None, // Specifies send and receive behaviors 
                    new AsyncCallback(ReceiveCallback), //An AsyncCallback delegate 
                    handler // Specifies infomation for receive operation 
                    );

                // Begins an asynchronous operation to accept an attempt 
                var callback = new AsyncCallback(ReceiveCallback);
                listener.BeginAccept(callback, listener);

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // A Socket to handle remote host communication. 
                var handler = (Socket)ar.AsyncState;                    

                // The number of bytes received. 
                var bytesRead = handler.EndReceive(ar);

                // Received message 
                var content = Encoding.Unicode.GetString(buffer, 0, bytesRead);

                if (bytesRead > 0)
                {
                    var message = new Message(content);

                    if (message.Turn == -1) // New game
                    {
                        Console.WriteLine("New player has connected into {0}", message.RoomId);

                        var room = rooms.GetOrAdd(message.RoomId, new Room());
                        
                        // Check if room is full or not
                        if (room.Count < 2)
                        {
                            room.Add(handler);

                            // Send to the client the player turn
                            if (room.Count == 2)
                            {
                                for (int index = 0; index < room.Count; index++)
                                    SendMessage(room[index], new Message { Turn = index });
                            }
                        }
                        else
                        {
                            // The room is full
                            message = new Message
                            {
                                Turn = -3
                            };
                            SendMessage(handler, message);
                        }

                        rooms.AddOrUpdate(message.RoomId, room, (key, existing) =>
                        {
                            return room;
                        });
                    }
                    else if (message.Turn == -2) // Quit game
                    {
                        var room = rooms.GetOrAdd(message.RoomId, new Room());
                        
                        if (room != null)
                        {
                            message = new Message
                            {
                                Turn = -2
                            };
                            SendMessage(handler, message);

                            for (int index = 0; index < room.Count; index++)
                                SendMessage(room[index], message);
                            room.Clear();
                        }

                        rooms.AddOrUpdate(message.RoomId, room, (key, existing) =>
                        {
                            return room;
                        });
                    }
                    else
                    {
                        var room = rooms.GetOrAdd(message.RoomId, new Room());
                        
                        if (room.Count == 2)
                        {
                            // Send the message to the other
                            SendMessage(room[message.Turn ^ 1], message);
                        }

                        rooms.AddOrUpdate(message.RoomId, room, (key, existing) =>
                        {
                            return room;
                        });
                    }
                }

                // Continues to asynchronously receive data
                handler.BeginReceive(
                    buffer,
                    0,
                    buffer.Length,
                    SocketFlags.None,
                    new AsyncCallback(ReceiveCallback),
                    handler
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message); 
            }
        }

        public static void SendMessage(Socket handler, Message message)
        {
            try
            {
                // Prepare the reply message 
                var byteData = Encoding.Unicode.GetBytes(message.ToJSON());

                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
        }

        public static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // A Socket which has sent the data to remote host 
                var handler = (Socket)ar.AsyncState;

                // The number of bytes sent to the Socket 
                int bytesSend = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to Client", bytesSend);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
        }
    }
}
