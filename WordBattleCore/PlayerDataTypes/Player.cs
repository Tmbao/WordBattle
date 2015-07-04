using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattleCore.PlayerDataTypes
{
    class Player
    {
        string playerName;

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        int playerScore;

        public int PlayerScore
        {
            get { return playerScore; }
            set { playerScore = value; }
        }
    }
}
