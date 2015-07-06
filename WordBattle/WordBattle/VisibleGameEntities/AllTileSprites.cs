using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.Utilities;
using Microsoft.Xna.Framework;

namespace WordBattle.VisibleGameEntities
{
    class AllTileSprites : VisibleGameEntity
    {
        private static AllTileSprites allTileSprites;

        public static AllTileSprites GetInstance()
        {
            if (allTileSprites == null)
                allTileSprites = new AllTileSprites();
            return allTileSprites;
        }

        private AllTileSprites()
        {
            allTiles = new Dictionary<char, Sprite2D>();
            allTiles[Consts.SPACE] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadSprite(Utilities.Utils.GetCharacterFileName(Consts.SPACE)));
            allTiles[Consts.BLANK] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadSprite(Utilities.Utils.GetCharacterFileName(Consts.BLANK)));
            allTiles[Consts.OBSTACLE] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadSprite(Utilities.Utils.GetCharacterFileName(Consts.OBSTACLE)));
            allTiles[Consts.LIGHT] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadSprite(Utilities.Utils.GetCharacterFileName(Consts.LIGHT)));
            allTiles[Consts.LIGHT_BLUE] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadSprite(Utilities.Utils.GetCharacterFileName(Consts.LIGHT_BLUE)));
            for (char c = 'A'; c <= 'Z'; c++)
                allTiles[c] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadSprite(Utilities.Utils.GetCharacterFileName(c)));
            for (char c = '0'; c <= '9'; c++)
                allTiles[c] = new Sprite2D(Consts.TILE_WIDTH, Consts.TILE_HEIGHT, Utils.LoadSprite(Utilities.Utils.GetCharacterFileName(c)));
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
    }
}
