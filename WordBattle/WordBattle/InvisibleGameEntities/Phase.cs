using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattle.InvisibleGameEntities
{
    public enum Phase
    {
        NONE,
        MENU_LOADING,
        MENU_LOADING_FINISHED,
        MENU,
        MENU_SELECTED_ANIMATING,
        MENU_SELECTED_ANIMATING_FINISHED,
        IN_GAME_LOADING,
        IN_GAME_LOADING_FINISHED,
        IN_GAME_MOVING,
        IN_GAME_MOVING_FINISHED,
        IN_GAME_ACHIEVING,
        IN_GAME_ACHIEVING_FINISHED,
        IN_GAME_END_TURN,
        END_GAME,
        END_GAME_ANIMATING,
        END_GAME_ANIMATING_FINISHED,
    }
}
