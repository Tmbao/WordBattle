using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WordBattle.Utilities;
using Microsoft.Xna.Framework.Input;
using WordBattleCore.GridEntities;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using WordBattle.VisibleGameEntities;
using WordBattlePlayer;

namespace WordBattle.ControllerGameEntities
{
    public enum ControllerOwner
    {
        ME,
        OPPONENT
    }

    public class PlayerGameControllerOnline : GameController
    {
        private static PlayerGameControllerOnline playerGameControllerMe, playGameControllerOpponent;

        public static PlayerGameControllerOnline GetInstance(ControllerOwner owner) {
            if (owner == ControllerOwner.ME)
            {
                if (playerGameControllerMe == null)
                    playerGameControllerMe = new PlayerGameControllerOnline();
                playerGameControllerMe.owner = ControllerOwner.ME;
                
                return playerGameControllerMe;
            }
            else
            {
                if (playGameControllerOpponent == null)
                    playGameControllerOpponent = new PlayerGameControllerOnline();
                playGameControllerOpponent.owner = ControllerOwner.OPPONENT;
                return playGameControllerOpponent;
            }
        }

        KeyboardController keyboardController;
        MouseController mouseController;

        // 
        public class Message
        {
            string roomId;

            public string RoomId
            {
                get { return roomId; }
                set { roomId = value; }
            }

            int col, row;

            public int Row
            {
                get { return row; }
                set { row = value; }
            }

            public int Col
            {
                get { return col; }
                set { col = value; }
            }

            char value;

            public char Value
            {
                get { return this.value; }
                set { this.value = value; }
            }

            int turn;

            public int Turn
            {
                get { return turn; }
                set { turn = value; }
            }

            public Message(string jsonString)
            {
                var jToken = JObject.Parse(jsonString);
                roomId = (string)jToken.SelectToken("RoomID");
                col = int.Parse((string)jToken.SelectToken("Column"));
                row = int.Parse((string)jToken.SelectToken("Row"));
                value = char.Parse((string)jToken.SelectToken("Value"));
                turn = int.Parse((string)jToken.SelectToken("Turn"));
            }

            public Message()
            {
                RoomId = "";
            }

            public string ToJSON()
            {
                var jObject = new JObject();
                jObject.Add("RoomID", roomId);
                jObject.Add("Column", col);
                jObject.Add("Row", row);
                jObject.Add("Value", value);
                jObject.Add("Turn", turn);
                return jObject.ToString();
            }
        }

        ControllerOwner owner;
        static Socket senderSock;
        static string roomId;

        public static string RoomId
        {
            get { return PlayerGameControllerOnline.roomId; }
            set { PlayerGameControllerOnline.roomId = value; }
        }

        static int turn;

        public static int Turn
        {
            get { return turn; }
            private set { turn = value; }
        }

        private PlayerGameControllerOnline()
        {
            keyboardController = KeyboardController.GetInstance();
            mouseController = MouseController.GetInstance();
        }

        public static void Connect()
        {
            if (senderSock != null && senderSock.Connected)
                Disconnect();

            GameNotification.GetInstance().PushMessage("Connecting");

            try
            {
                // Create one SocketPermission for socket access restrictions 
                var permission = new SocketPermission(
                    NetworkAccess.Connect,    // Connection permission 
                    TransportType.Tcp,        // Defines transport types 
                    "",                       // Gets the IP addresses 
                    SocketPermission.AllPorts // All ports 
                    );

                // Ensures the code to have permission to access a Socket 
                permission.Demand();

                // Resolves a host name to an IPHostEntry instance            
                var ipHost = Dns.GetHostEntry(Consts.SERVER_IP);

                // Gets first IP address associated with a localhost 
                var ipAddr = ipHost.AddressList[2];

                // Creates a network endpoint 
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, Consts.PORT);

                // Create one Socket object to setup Tcp connection 
                senderSock = new Socket(
                    ipAddr.AddressFamily,// Specifies the addressing scheme 
                    SocketType.Stream,   // The type of socket  
                    ProtocolType.Tcp     // Specifies the protocols  
                    );

                // Establishes a connection to a remote host 
                for (int attemp = 0; attemp < Consts.MAX_CONNECTION_ATTEMPTS && !senderSock.Connected; attemp++)
                {
                    try
                    {
                        senderSock.Connect(ipEndPoint);
                        break;
                    }
                    catch
                    {
                    }
                }

                if (senderSock.Connected)
                {
                    GameNotification.GetInstance().PushMessage("Connected");
                    // Update player turn
                    SendMessage(new Message { RoomId = roomId, Turn = -1 });
                    var message = ReceiveMessage();

                    if (message.Turn >= 0)
                        turn = message.Turn;
                    else
                    {
                        Disconnect();
                    }
                }
                else
                    GameNotification.GetInstance().PushMessage("Cant connect");
            }
            catch (Exception e)
            {
                GameNotification.GetInstance().PushMessage("Cant connect");
            }
        }

        public static void SendMessage(Message message)
        {
            try
            {
                // Sending message 
                byte[] byteMessage = Encoding.Unicode.GetBytes(message.ToJSON());

                // Sends data to a connected Socket. 
                int bytesSend = senderSock.Send(byteMessage);
            }
            catch (Exception e)
            {
            }
        }

        public static void Disconnect()
        {
            try
            {
                // Disables sends and receives on a Socket. 
                senderSock.Shutdown(SocketShutdown.Both);

                //Closes the Socket connection and releases all resources 
                senderSock.Close();

                GameNotification.GetInstance().PushMessage("Disconnected");
            }
            catch (Exception e)
            {
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (owner == ControllerOwner.ME)
            {
                // Update from hardware

                keyboardController.Update(gameTime);
                mouseController.Update(gameTime);

                UpdatePressedCharacters();
                UpdateSelectedIndex();

                // Check and send to server
                if (selectedIndex != null && pressedCharacter != null)
                {
                    var message = new Message
                    {
                        RoomId = roomId,
                        Row = selectedIndex.Item1,
                        Col = selectedIndex.Item2,
                        Value = pressedCharacter[0],
                        Turn = turn
                    };
                    SendMessage(message);
                }
            }
            else if (senderSock.Connected)
            {
                selectedIndex = null;
                pressedCharacter = null;
                // Update from server
                try
                {
                    Message message = ReceiveMessage();

                    if (message.Turn > 0)
                    {
                        selectedIndex = new Tuple<int, int>(message.Row, message.Col);
                        pressedCharacter = message.Value.ToString().ToUpper();
                    }
                    else
                    {
                        // Quit ??
                        Disconnect();
                    }
                }
                catch (Exception e) 
                {
                }
            }
        }

        private static Message ReceiveMessage()
        {
            var bytes = new byte[1024];

            // Receives data from a bound Socket. 
            int bytesRec = senderSock.Receive(bytes);

            // Converts byte array to string 
            string receivedMessage = Encoding.Unicode.GetString(bytes, 0, bytesRec);

            // Continues to read the data till data isn't available 
            if (senderSock.Available > 0)
            {
                bytesRec = senderSock.Receive(bytes);
                receivedMessage += Encoding.Unicode.GetString(bytes, 0, bytesRec);
            }

            Message message = new Message(receivedMessage);
            return message;
        }

        private void UpdateSelectedIndex()
        {
            var currentPosition = Vector2.Transform(mouseController.GetCurrentMousePosition(), Global.MainCamera.InvertWVP);
            if (mouseController.IsLeftButtonPressed())
                selectedIndex = Utils.GetIndexOfMouse(currentPosition);
        }

        private void UpdatePressedCharacters()
        {
            var pressedKey = keyboardController.PressedKey;
            if (pressedKey != Keys.None)
            {
                pressedCharacter = pressedKey.ToString();
                if (pressedCharacter != null && pressedCharacter.Length == 1 && Utils.IsLetter(pressedCharacter[0])) ;
                else
                    pressedCharacter = null;
            }
            else
                pressedCharacter = null;
        }

        string pressedCharacter;

        public override string PressedCharacter()
        {
            return pressedCharacter;
        }

        Tuple<int, int> selectedIndex;

        public override Tuple<int, int> SelectedIndex()
        {
            if (WordGrid.GetInstance().CanFill(selectedIndex))
                return selectedIndex;
            else 
                return null;
        }
    }
}
