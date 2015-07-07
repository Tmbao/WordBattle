using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.Utilities;
using Microsoft.Xna.Framework;
using WordBattle.ControllerGameEntities;
using Microsoft.Xna.Framework.Graphics;

namespace WordBattle.VisibleGameEntities
{
    class CustomCusor : VisibleGameEntity
    {
        private static CustomCusor instance;

        public static CustomCusor GetInstance()
        {
            if (instance == null)
                instance = new CustomCusor();
            return instance;
        }

        Sprite2D icon;

        private CustomCusor()
        {
            icon = new Sprite2D(0, 0, Utils.LoadTextures(@"Cursor\cursor_hand"));
        }

        Vector2 position;

        public override void Update(GameTime gameTime)
        {
            position = MouseController.GetInstance().GetCurrentMousePosition();
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            icon.Draw(gameTime, spriteBatch, position.X, position.Y);
        }
    }
}
