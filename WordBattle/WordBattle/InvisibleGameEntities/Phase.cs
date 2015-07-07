using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattle.InvisibleGameEntities
{
    public enum Phase
    {
        NONE,
        NEW_GAME,
        IN_GAME_LOADING,
        IN_GAME_LOADING_FINISHED,
        IN_GAME_MOVING,
        IN_GAME_MOVING_FINISHED,
        IN_GAME_ACHIEVING,
        IN_GAME_ACHIEVING_FINISHED,
        IN_GAME_END_TURN
    }
}
