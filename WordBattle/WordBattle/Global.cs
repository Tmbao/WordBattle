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
    public class Global
    {
        public static KeyboardController KeyboardHelper = KeyboardController.GetInstance();
        public static MouseController MouseHelper = MouseController.GetInstance();
        public static Camera MainCamera = new Camera();
        public static Phase CurrentPhase;
        public static ContentManager Content;

        internal static void UpdateAll(GameTime gameTime)
        {
            KeyboardHelper.Update(gameTime);
            MouseHelper.Update(gameTime);
            MainCamera.Update(gameTime);
        }
    }
}
