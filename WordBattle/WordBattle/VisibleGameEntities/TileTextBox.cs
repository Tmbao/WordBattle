using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.Utilities;
using Microsoft.Xna.Framework;
using WordBattle.ControllerGameEntities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WordBattle.InvisibleGameEntities;

namespace WordBattle.VisibleGameEntities
{
    class TileTextBox : VisibleGameEntity
    {
        string title;

        string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        float left, top;

        bool isFocused;

        Phase entityPhase;

        public Phase EntityPhase
        {
            get { return entityPhase; }
            set { entityPhase = value; }
        }

        float intensity;

        public TileTextBox(float left, float top, string title)
        {
            this.left = left;
            this.top = top;
            this.title = title;
            this.text = "";
            
            intensity = Consts.INTENSITY_MENU_MAX;
            entityPhase = Phase.MENU;
        }

        public override void Update(GameTime gameTime)
        {
            switch (entityPhase) {
                case Phase.MENU:
                    UpdateMouse();
                    UpdateKeyboard();
                    break;
                case Phase.MENU_SELECTED_ANIMATING:
                    intensity -= Consts.INTENSITY_MENU_DELTA;
                    if (intensity < 0)
                        intensity = 0;
                    break;
            }
            
            base.Update(gameTime);
        }

        private void UpdateKeyboard()
        {
            var keyboard = KeyboardController.GetInstance();
            var key = keyboard.PressedKey;
            if (keyboard.IsKeyPressed(key) && isFocused)
            {
                // Remove the last character
                if (key == Keys.Back && text.Length > 0)
                    text = text.Remove(text.Length - 1);
                else if ((Utils.IsLetter(key.ToString()) || Utils.IsDigit(key.ToString()) || key == Keys.Space) &&
                    text.Length < Consts.MAX_NAME_LENGTH)
                    text += key.ToString()[key.ToString().Length - 1];
            }
        }

        private void UpdateMouse()
        {
            var mouse = MouseController.GetInstance();
            if (mouse.IsLeftButtonPressed())
            {
                var position = Vector2.Transform(mouse.GetCurrentMousePosition(), Global.MainCamera.InvertWVP);
                if (left <= position.X && position.X <= left + Utils.GetTextboxWidth() &&
                    top <= position.Y && position.Y <= top + Consts.TEXTBOX_TEXT_HEIGHT)
                    isFocused = true;
                else
                    isFocused = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = TileSpriteContainer.GetInstance();
            // Draw title
            tiles.DrawText(gameTime, spriteBatch, title, left, top, Consts.TEXTBOX_TEXT_WIDTH, intensity);

            // Draw text
            string drawingText = text;
            while (drawingText.Length < Consts.MAX_NAME_LENGTH)
                drawingText += Consts.BLANK;
            tiles.DrawText(gameTime, spriteBatch, drawingText, left + Consts.TEXTBOX_TITLE_LENGTH * Consts.TEXTBOX_TEXT_WIDTH + (Consts.TEXTBOX_TITLE_LENGTH - 1) * Consts.TEXT_SPACING + 2 * Consts.COMPONENT_SPACING, top, Consts.TEXTBOX_TEXT_WIDTH, intensity);

            if (isFocused)
            {
                drawingText = "";
                while (drawingText.Length < Consts.MAX_NAME_LENGTH)
                    drawingText += Consts.LIGHT;

                tiles.DrawText(gameTime, spriteBatch, drawingText, left + Consts.TEXTBOX_TITLE_LENGTH * Consts.TEXTBOX_TEXT_WIDTH + (Consts.TEXTBOX_TITLE_LENGTH - 1) * Consts.TEXT_SPACING + 2 * Consts.COMPONENT_SPACING, top, Consts.TEXTBOX_TEXT_WIDTH, intensity);
            }

            if (intensity == 0)
                entityPhase = Phase.MENU_SELECTED_ANIMATING_FINISHED;
        }
    }
}
