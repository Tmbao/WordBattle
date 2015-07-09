using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace WordBattleServer
{
    class Room
    {
        List<Socket> sockets;
        List<string> skNames;

        public Room()
        {
            sockets = new List<Socket>();
            skNames = new List<string>();
        }

        public void Add(Socket socket, string skName)
        {
            sockets.Add(socket);
            skNames.Add(skName);
        }

        public void Clear()
        {
            sockets.Clear();
            skNames.Clear();
        }

        public Socket GetSocket(int index)
        {
            return sockets[index];
        }

        public int Count
        {
            get { return sockets.Count; }
        }

        public string GetName(int index)
        {
            return skNames[index];
        }
    }
}
