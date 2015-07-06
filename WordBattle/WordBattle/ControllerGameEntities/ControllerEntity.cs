using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordBattle.ControllerGameEntities
{
    public abstract class ControllerEntity<T> : GameEntity
    {
        protected T currentState;
        protected T previousState;
        protected int lastUpdate;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            previousState = currentState;
        }
    }
}
