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

        public static void UpdateAll(GameTime gameTime)
        {
            MainCamera.Update(gameTime);
        }

        public static void UpdatePhase(Phase phase)
        {
            CurrentPhase = phase;
            GameNotification.GetInstance().EntityPhase = phase;
            PlayerTurn.GetInstance().UpdatePhase(phase);
            TilingGrid.GetInstance().EntityPhase = phase;
            MenuContainer.GetInstance().UpdatePhase(phase);
            LogoPanel.GetInstance().EntityPhase = phase;
        }
    }
}
