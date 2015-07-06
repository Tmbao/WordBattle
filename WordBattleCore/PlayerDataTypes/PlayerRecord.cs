using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattleCore.PlayerDataTypes
{
    public class PlayerRecord
    {
        string playerName;

        public string PlayerName
        {
            get { return playerName; }
            set { 
                playerName = value.ToUpper();
                if (playerName.Length > Consts.MAX_NAME_LENGTH)
                    playerName = playerName.Substring(0, Consts.MAX_NAME_LENGTH);
            }
        }

        int playerScore;

        public int PlayerScore
        {
            get { return playerScore; }
            set { playerScore = value; }
        }
    }
}
