using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattle.ControllerGameEntities
{
    public abstract class GameController : GameEntity
    {
        public abstract string PressedCharacters();

        public abstract Tuple<int, int> SelectedIndex();
    }
}
