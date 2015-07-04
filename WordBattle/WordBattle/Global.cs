using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.ControllerGameEntities;
using Microsoft.Xna.Framework.Content;
using WordBattle.InvisibleGameEntities;
using Microsoft.Xna.Framework;

namespace WordBattle
{
    class Global
    {
        public static KeyboardController KeyboardHelper = new KeyboardController();
        public static MouseController MouseHelper = new MouseController();
        public static ContentManager Content;
        public static Camera MainCamera = new Camera();
        public static Phase GamePhase = new Phase();

        internal static void UpdateAll(GameTime gameTime)
        {
            KeyboardHelper.Update(gameTime);
            MouseHelper.Update(gameTime);
            MainCamera.Update(gameTime);
        }
    }
}
