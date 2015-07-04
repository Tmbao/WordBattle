using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WordBattle.VisibleGameEntities
{
    public abstract class VisibleGameEntity : GameEntity
    {
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, object param)
        {
        }
    }
}
