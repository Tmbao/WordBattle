using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.ControllerGameEntities;
using Microsoft.Xna.Framework.Content;
using WordBattle.InvisibleGameEntities;
using Microsoft.Xna.Framework;
using WordBattle.VisibleGameEntities;

namespace WordBattle
{
    public static class Global
    {
        public static Camera MainCamera = new Camera();
        public static Phase CurrentPhase;
        public static ContentManager Content;

        internal static void UpdateAll(GameTime gameTime)
        {
            MainCamera.Update(gameTime);
        }
    }
}
