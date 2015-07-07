using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WordBattle.Utilities;
using WordBattle.InvisibleGameEntities;

namespace WordBattle.VisibleGameEntities
{
    class LogoPanel : VisibleGameEntity
    {
        const int WORD_TEXT_SIZE = 44;
        const int BATTLE_TEXT_SIZE = 48;
        const int WITH_FRIENDS_TEXT_SIZE = 32;
        const int BACKGROUND_SIZE = 128;

        private static LogoPanel instance;

        public static LogoPanel GetInstance()
        {
            if (instance == null)
                instance = new LogoPanel();
            return instance;
        }

        Phase entityPhase;

        public Phase EntityPhase
        {
            get { return entityPhase; }
            set { entityPhase = value; }
        }

        Sprite2D backgroundSprite;

        float left, top, center_left, center_top, dx, dy;

        private LogoPanel() {

            // Load background
            backgroundSprite = new Sprite2D(0, 0, Utils.LoadTextures(Utils.GetImageFileName("B")));

            // 
            center_left = (Consts.SCREEN_WIDTH - GetWidth()) / 2f;
            center_top = 2 * Consts.LOGO_TOP;

            dx = (center_left - Consts.LOGO_LEFT) / Consts.LOGO_TRANSLATION_TIME;
            dy = (center_top - Consts.LOGO_TOP) / Consts.LOGO_TRANSLATION_TIME;

            left = center_left;
            top = center_top;
        }

        public override void Update(GameTime gameTime)
        {
            backgroundSprite.Update(gameTime);

            switch (entityPhase)
            {
                case Phase.MENU_SELECTED_ANIMATING:
                    left -= dx;
                    top -= dy;

                    if (left < Consts.LOGO_LEFT)
                        left = Consts.LOGO_LEFT;
                    if (top < Consts.LOGO_TOP)
                        top = Consts.LOGO_TOP;
                    break;
            }

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawBackground(gameTime, spriteBatch);
            DrawText(gameTime, spriteBatch);

            switch (entityPhase)
            {
                case Phase.MENU_SELECTED_ANIMATING:
                    if (left == Consts.LOGO_LEFT && top == Consts.LOGO_TOP)
                        entityPhase = Phase.MENU_SELECTED_ANIMATING_FINISHED;
                    break;
            }
        }

        private void DrawText(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = TileSpriteContainer.GetInstance();

            // Draw 'WORD'
            tiles.DrawText(gameTime, spriteBatch, "WORD",
                left + BACKGROUND_SIZE + Consts.COMPONENT_SPACING,
                top + 3 * Consts.COMPONENT_SPACING,
                WORD_TEXT_SIZE);

            // Draw 'BATTLE'
            tiles.DrawText(gameTime, spriteBatch, "ATTLE",
                left + BACKGROUND_SIZE + Consts.COMPONENT_SPACING,
                top + 3 * Consts.COMPONENT_SPACING + WORD_TEXT_SIZE + Consts.COMPONENT_SPACING,
                BATTLE_TEXT_SIZE);

            // Draw 'WITH FRIENDS'
            tiles.DrawText(gameTime, spriteBatch, "WITH FRIENDS",
                left,
                top + BACKGROUND_SIZE + Consts.COMPONENT_SPACING,
                WITH_FRIENDS_TEXT_SIZE);
        }

        public int GetWidth()
        {
            return Utils.GetTextWidth("WITH FRIENDS", WITH_FRIENDS_TEXT_SIZE, Consts.TEXT_SPACING);
        }

        public void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            backgroundSprite.Draw(gameTime, spriteBatch, left, top);
        }
    }
}
