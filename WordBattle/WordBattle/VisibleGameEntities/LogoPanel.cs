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

        private static LogoPanel logoPanel;

        public static LogoPanel GetInstance()
        {
            if (logoPanel == null)
                logoPanel = new LogoPanel();
            return logoPanel;
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
            float shift;

            // Draw 'WORD'
            string word = "WORD";
            shift = BACKGROUND_SIZE + Consts.PANEL_COMPONENT_SPACING;
            for (int index = 0; index < word.Length; index++)
            {
                tiles.GetTileSprite(word[index]).Draw(gameTime, spriteBatch,
                    left + shift + index * WORD_TEXT_SIZE + index * Consts.PLAYER_TEXT_SPACING,
                    top + 3 * Consts.PANEL_COMPONENT_SPACING,
                    1,
                    (float) WORD_TEXT_SIZE / Consts.TILE_WIDTH);
            }

            // Draw 'BATTLE'
            string battle = "ATTLE";
            shift = BACKGROUND_SIZE + Consts.PANEL_COMPONENT_SPACING;
            for (int index = 0; index < battle.Length; index++)
            {
                tiles.GetTileSprite(battle[index]).Draw(gameTime, spriteBatch,
                    left + shift + index * BATTLE_TEXT_SIZE + index * Consts.PLAYER_TEXT_SPACING,
                    top + 3 * Consts.PANEL_COMPONENT_SPACING + WORD_TEXT_SIZE + Consts.PANEL_COMPONENT_SPACING,
                    1,
                    (float)BATTLE_TEXT_SIZE / Consts.TILE_WIDTH);
            }

            // Draw 'WITH FRIENDS'
            string withFriends = "WITH FRIENDS";
            for (int index = 0; index < withFriends.Length; index++)
            {
                tiles.GetTileSprite(withFriends[index]).Draw(gameTime, spriteBatch,
                    left + index * WITH_FRIENDS_TEXT_SIZE + index * Consts.PLAYER_TEXT_SPACING,
                    top + BACKGROUND_SIZE + Consts.PANEL_COMPONENT_SPACING,
                    1,
                    (float)WITH_FRIENDS_TEXT_SIZE / Consts.TILE_WIDTH);
            }
        }

        public void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            backgroundSprite.Draw(gameTime, spriteBatch, left, top);
        }
    }
}
