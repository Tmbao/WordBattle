using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordBattle.ControllerGameEntities;

namespace WordBattle.VisibleGameEntities
{
    class TileButton : VisibleGameEntity
    {
        float left, top;

        Sprite2D icon, iconHighlighter;

        bool isHover, isClicked;

        public bool IsClicked
        {
            get {
                var temp = isClicked;
                isClicked = false;
                return temp; 
            }
            private set { isClicked = value; }
        }

        public TileButton(float left, float top, string iconPath)
        {
            this.left = left;
            this.top = top;

            icon = new Sprite2D(0, 0, Utils.LoadTextures(iconPath));
            iconHighlighter = new Sprite2D(0, 0, Utils.LoadTextures(Utils.GetImageFileName(Consts.LIGHT)));
        }

        public override void Update(GameTime gameTime)
        {
            var mouse = MouseController.GetInstance();
            var position = Vector2.Transform(mouse.GetCurrentMousePosition(), Global.MainCamera.InvertWVP);

            if (left <= position.X && position.X <= left + Consts.BUTTON_WIDTH &&
                top <= position.Y && position.Y <= top + Consts.BUTTON_HEIGHT)
            {
                isHover = true;
                if (mouse.IsLeftButtonPressed())
                    isClicked = true;
                else
                    isClicked = false;
            }
            else
            {
                isHover = false;
                isClicked = false;
            }

            icon.Update(gameTime);
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            icon.Draw(gameTime, spriteBatch, left, top, 1, (float)Consts.BUTTON_WIDTH / icon.Width);

            if (isHover)
                iconHighlighter.Draw(gameTime, spriteBatch, left, top, 1, (float)Consts.BUTTON_WIDTH / iconHighlighter.Width);
        }
    }
}
