using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattle.InvisibleGameEntities
{
    public enum PHASE
    {
        MENU,
        PRE_GAME,
        IN_GAME,
        DRAWING,
        POST_GAME
    }

    public class Phase : InvisibleGameEntity
    {
        PHASE currentPhase;

        public PHASE CurrentPhase
        {
            get { return currentPhase; }
            set { currentPhase = value; }
        }
    }
}
