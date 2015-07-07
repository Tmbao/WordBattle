using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WordBattle.Utilities;

namespace WordBattle.VisibleGameEntities
{
    class LogoPanel : VisibleGameEntity
    {
        const int WORD_TEXT_SIZE = 44;
        const int BATTLE_TEXT_SIZE = 48;
        const int WITH_FRIENDS_TEXT_SIZE = 32;
        const int WITHFRIENDS_TEXT_SIZE = 36;
        const int BACKGROUND_SIZE = 128;

        private static LogoPanel instance;

        public static LogoPanel GetInstance()
        {
            if (instance == null)
                instance = new LogoPanel();
            return instance;
        }

        Sprite2D backgroundSprite;

        float left, top;

        private LogoPanel() {

            // Load background
            backgroundSprite = new Sprite2D(0, 0, Utils.LoadSprite(Utils.GetImageFileName("B")));

            // 
            left = Consts.LOGO_LEFT;
            top = Consts.LOGO_TOP;
        }

        public override void Update(GameTime gameTime)
        {
            backgroundSprite.Update(gameTime);
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawBackground(gameTime, spriteBatch);
            DrawText(gameTime, spriteBatch);
        }

        private void DrawText(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = AllTileSprites.GetInstance();

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

        public void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            backgroundSprite.Draw(gameTime, spriteBatch, left, top);
        }
    }
}
