using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordBattlePlayer
{
    public abstract class GameController
    {
        public abstract string PressedCharacter();

        public abstract Tuple<int, int> SelectedIndex();

        public abstract void Update(GameTime gameTime);
    }
}
