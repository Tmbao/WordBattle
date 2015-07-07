using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordBattle.VisibleGameEntities
{
    class TileSpriteContainer : VisibleGameEntity
    {
        private static TileSpriteContainer instance;

        public static TileSpriteContainer GetInstance()
        {
            if (instance == null)
                instance = new TileSpriteContainer();
            return instance;
        }

        private TileSpriteContainer()
        {
            allTiles = new Dictionary<char, Sprite2D>();
            allTiles[Consts.SPACE] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadTextures(Utilities.Utils.GetCharacterFileName(Consts.SPACE)));
            allTiles[Consts.BLANK] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadTextures(Utilities.Utils.GetCharacterFileName(Consts.BLANK)));
            allTiles[Consts.OBSTACLE] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadTextures(Utilities.Utils.GetCharacterFileName(Consts.OBSTACLE)));
            allTiles[Consts.LIGHT] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadTextures(Utilities.Utils.GetCharacterFileName(Consts.LIGHT)));
            allTiles[Consts.LIGHT_BLUE] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadTextures(Utilities.Utils.GetCharacterFileName(Consts.LIGHT_BLUE)));
            for (char c = 'A'; c <= 'Z'; c++)
                allTiles[c] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadTextures(Utilities.Utils.GetCharacterFileName(c)));
            for (char c = '0'; c <= '9'; c++)
                allTiles[c] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadTextures(Utilities.Utils.GetCharacterFileName(c)));
        }

        Dictionary<char, Sprite2D> allTiles;

        public Sprite2D GetTileSprite(char identity)
        {
            return allTiles[identity];
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entry in allTiles)
                entry.Value.Update(gameTime);

            base.Update(gameTime);
        }

        public void DrawText(GameTime gameTime, SpriteBatch spriteBatch, string text, float left, float top, int size, float intensity)
        {
            for (int index = 0; index < text.Length; index++)
            {
                allTiles[text[index]].Draw(gameTime, spriteBatch,
                    left + index * size + index * Consts.TEXT_SPACING,
                    top,
                    intensity,
                    (float)size / Consts.TILE_WIDTH);
            }
        }

        public void DrawText(GameTime gameTime, SpriteBatch spriteBatch, string text, float left, float top, int size, float[] intensity)
        {
            for (int index = 0; index < text.Length; index++)
            {
                allTiles[text[index]].Draw(gameTime, spriteBatch,
                    left + index * size + index * Consts.TEXT_SPACING,
                    top,
                    intensity[index],
                    (float)size / Consts.TILE_WIDTH);
            }
        }

        public void DrawText(GameTime gameTime, SpriteBatch spriteBatch, string text, float left, float top, int size)
        {
            DrawText(gameTime, spriteBatch, text, left, top, size, 1);
        }
    }
}
