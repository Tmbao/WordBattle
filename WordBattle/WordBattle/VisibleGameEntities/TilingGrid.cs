using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WordBattle.InvisibleGameEntities;

namespace WordBattle.VisibleGameEntities
{
    public class TilingGrid : VisibleGameEntity
    {
        int gridCols, gridRows;

        public int NumberOfRows
        {
            get { return gridRows; }
            set { gridRows = value; }
        }

        public int NumberOfColumns
        {
            get { return gridCols; }
            set { gridCols = value; }
        }


        int tileWidth, tileHeight;

        public int TileHeight
        {
            get { return tileHeight; }
            private set { tileHeight = value; }
        }

        public int TileWidth
        {
            get { return tileWidth; }
            private set { tileWidth = value; }
        }

        int mapWidth, mapHeight;

        public int MapHeight
        {
            get { return mapHeight; }
            private set { mapHeight = value; }
        }

        public int MapWidth
        {
            get { return mapWidth; }
            private set { mapWidth = value; }
        }

        float left, top;

        public float Left
        {
            get { return left; }
            private set { left = value; }
        }

        public float Top
        {
            get { return top; }
            private set { top = value; }
        }

        Dictionary<char, Sprite2D> tiles;

        internal Dictionary<char, Sprite2D> Tiles
        {
            get { return tiles; }
            private set { tiles = value; }
        }

        char[,] map;
        float[,] intensity;

        public TilingGrid(float left, float top, int gridCols, int gridRows, int tileWidth, int tileHeight, char[,] map)
        {
            // Copy parameters
            this.left = left;
            this.top = top;
            this.gridCols = gridCols;
            this.gridRows = gridRows;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.mapWidth = tileWidth * gridCols;
            this.mapHeight = this.TileHeight * gridRows;
            this.map = map;

            intensity = new float[gridRows, gridCols];

            // Load textures
            LoadAllTiles();
        }

        private void LoadAllTiles()
        {
            tiles = new Dictionary<char, Sprite2D>();
            tiles[Consts.BLANK] = new Sprite2D(tileWidth, tileHeight, LoadTexture(Utilities.Utils.GetCharacterFileName(Consts.BLANK)));
            tiles[Consts.OBSTACLE] = new Sprite2D(tileWidth, tileHeight, LoadTexture(Utilities.Utils.GetCharacterFileName(Consts.OBSTACLE)));
            tiles[Consts.LIGHT] = new Sprite2D(tileWidth, tileHeight, LoadTexture(Utilities.Utils.GetCharacterFileName(Consts.LIGHT))); 
            for (char c = 'A'; c <= 'Z'; c++)
                tiles[c] = new Sprite2D(tileWidth, tileHeight, LoadTexture(Utilities.Utils.GetCharacterFileName(c)));
        }

        private List<Texture2D> LoadTexture(string path)
        {
            var result = new List<Texture2D>();
            result.Add(Global.Content.Load<Texture2D>(path));
            return result;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (Global.GamePhase.CurrentPhase)
            {
                case PHASE.IN_GAME:
                    UpdateGame(gameTime);
                    break;
                case PHASE.DRAWING:
                    UpdateDrawing(gameTime);
                    break;
            }

            foreach (var entry in tiles) 
                entry.Value.Update(gameTime);
        }

        private void UpdateDrawing(GameTime gameTime)
        {
            return;
            throw new NotImplementedException();
        }

        private void UpdateGame(GameTime gameTime)
        {
            // Update light effect 
            Tuple<int, int> index = GridIndexOf(Global.MouseHelper.GetCurrentMousePosition());
            UpdateIntensity(gameTime);

            if (index != null)
                intensity[index.Item1, index.Item2] = Consts.INTENSITY_MAX;
        }

        private void UpdateIntensity(GameTime gameTime)
        {
            for (int row = 0; row < gridRows; row++)
                for (int col = 0; col < gridCols; col++) {
                    intensity[row, col] -= Consts.INTENSITY_DELTA;
                    if (intensity[row, col] < 0)
                        intensity[row, col] = 0;
                }
        }

        private Tuple<int, int> GridIndexOf(Vector2 vector2)
        {
            if (vector2.X < left || vector2.Y < top)
                return null;
            else
            {
                int col = (int)(vector2.X - left) / tileWidth;
                int row = (int)(vector2.Y - top) / tileHeight;

                if (col >= gridCols || row >= gridRows)
                    return null;
                else
                    return new Tuple<int, int>(row, col);
            }
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime, object spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridCols; col++)
                {
                    DrawTile(gameTime, (SpriteBatch) spriteBatch, col, row);
                }
            }
        }

        private void DrawTile(GameTime gameTime, SpriteBatch param, int col, int row)
        {
            // Position of tile
            float top = this.top + row * tileHeight;
            float left = this.left + col * tileWidth;

            // Draw corresponding tile
            var tile = tiles[map[row, col]];
            tile.Draw(gameTime, param, left, top, 1);

            // Draw light effect
            tile = tiles[Consts.LIGHT];
            tile.Draw(gameTime, param, left, top, intensity[row, col]);
        }
    }
}
