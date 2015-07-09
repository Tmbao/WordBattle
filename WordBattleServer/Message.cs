using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WordBattleServer
{
    class Message
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
}
