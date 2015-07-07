using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using WordBattle.ControllerGameEntities;
using WordBattle.Utilities;
using WordBattle.InvisibleGameEntities;
using Microsoft.Xna.Framework.Graphics;

namespace WordBattle.VisibleGameEntities
{
    class MenuOption : VisibleGameEntity
    {
        string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        bool isSelected, isHover;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        float left, top;

        Phase entityPhase;

        public Phase EntityPhase
        {
            get { return entityPhase; }
            set { entityPhase = value; }
        }

        Sprite2D iconSprite, iconSpriteHighlighter;

        float intensity;

        public MenuOption(float left, float top, string text, string iconPath)
        {
            this.left = left;
            this.top = top;

            this.text = text;
            entityPhase = Phase.MENU;

            this.iconSprite = new Sprite2D(0, 0, Utils.LoadTextures(iconPath));
            iconSpriteHighlighter = new Sprite2D(0, 0, Utils.LoadTextures(Utils.GetImageFileName(Consts.LIGHT)));


            intensity = 0;
        }

        public override void Update(GameTime gameTime)
        {
            switch (entityPhase)
            {
                case Phase.MENU_LOADING:
                    intensity += Consts.INTENSITY_MENU_DELTA;
                    if (intensity > Consts.INTENSITY_MENU_MAX)
                        intensity = Consts.INTENSITY_MENU_MAX;
                    break;
                case Phase.MENU:
                    UpdateWaiting(gameTime);
                    break;
                case Phase.MENU_SELECTED_ANIMATING:
                    intensity -= Consts.INTENSITY_MENU_DELTA;
                    if (intensity < 0)
                        intensity = 0;
                    break;
            }

            iconSprite.Update(gameTime);
            iconSpriteHighlighter.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdateWaiting(GameTime gameTime)
        {
            var mouse = MouseController.GetInstance();
            var position = Vector2.Transform(mouse.GetCurrentMousePosition(), Global.MainCamera.InvertWVP);

            if (top <= position.Y && position.Y <= top + Consts.MENU_FONT_SIZE)
            {
                isHover = true;
                if (mouse.IsLeftButtonPressed())
                    if (left <= position.X && position.X <= left + Consts.MENU_FONT_SIZE + Consts.COMPONENT_SPACING + Utils.GetTextWidth(text, Consts.MENU_FONT_SIZE, Consts.TEXT_SPACING))
                        isSelected = true;
                    else
                        isSelected = false;
            }
            else
                isHover = false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw icon
            iconSprite.Draw(gameTime, spriteBatch, left, top, intensity, (float)Consts.MENU_FONT_SIZE / iconSprite.Width);
            iconSprite.Draw(gameTime, spriteBatch, left + Consts.MENU_FONT_SIZE + 2 * Consts.COMPONENT_SPACING + Utils.GetTextWidth(text, Consts.MENU_FONT_SIZE, Consts.TEXT_SPACING), top, intensity, (float)Consts.MENU_FONT_SIZE / iconSprite.Width);
            if (isHover)
            {
                // Draw highlight
                iconSpriteHighlighter.Draw(gameTime, spriteBatch, left, top, intensity, (float)Consts.MENU_FONT_SIZE / iconSpriteHighlighter.Width);
                iconSpriteHighlighter.Draw(gameTime, spriteBatch, left + Consts.MENU_FONT_SIZE + 2 * Consts.COMPONENT_SPACING + Utils.GetTextWidth(text, Consts.MENU_FONT_SIZE, Consts.TEXT_SPACING), top, intensity, (float)Consts.MENU_FONT_SIZE / iconSprite.Width);
            }

            // Draw text
            var tiles = TileSpriteContainer.GetInstance();
            tiles.DrawText(gameTime, spriteBatch, text,
                left + Consts.MENU_FONT_SIZE + Consts.COMPONENT_SPACING,
                top,
                Consts.MENU_FONT_SIZE,
                intensity);

            // Disapeared
            switch (entityPhase)
            {
                case Phase.MENU_LOADING:
                    if (intensity == Consts.INTENSITY_MENU_MAX)
                        entityPhase = Phase.MENU_LOADING_FINISHED;
                    break;
                case Phase.MENU_SELECTED_ANIMATING:
                    if (intensity == 0)
                        entityPhase = Phase.MENU_SELECTED_ANIMATING_FINISHED;
                    break;
            }
            
        }
    }
}
