﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordBattle.InvisibleGameEntities;
using WordBattleCore.GridDataTypes;
using Microsoft.Xna.Framework.Input;

namespace WordBattle.VisibleGameEntities
{
    public class TilingGrid : VisibleGameEntity
    {
        private static TilingGrid tilingGrid;

        public static TilingGrid GetInstance()
        {
            if (tilingGrid == null)
                tilingGrid = new TilingGrid();
            return tilingGrid;
        }

        private TilingGrid()
        {
        }

        // Events

        public abstract class NewCharacterAppearedEventArgs
        {
            public virtual void OnNewCharacterAppeared(int col, int row)
            {
            }
        }

        //

        NewCharacterAppearedEventArgs newCharacterAppeared;

        public NewCharacterAppearedEventArgs NewCharacterAppeared
        {
            get { return newCharacterAppeared; }
            set { newCharacterAppeared = value; }
        }

        WordGrid wordGrid;
        float[,] intensity;

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

        Tuple<int, int> selectedIndex;

        public void Load(float left, float top, int tileWidth, int tileHeight, String mapPath)
        {
            this.wordGrid = WordGrid.GetInstance();
            this.wordGrid.Load(Global.Content.Load<GridData>(mapPath));

            this.left = left;
            this.top = top;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.mapWidth = tileWidth * wordGrid.NumberOfColumns;
            this.mapHeight = this.TileHeight * wordGrid.NumberOfRows;

            intensity = new float[wordGrid.NumberOfRows, wordGrid.NumberOfColumns];

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

            // Update light effect 
            UpdateIntensity(gameTime);

            switch (Global.CurrentPhase)
            {
                case Phase.IN_GAME:
                    UpdateGame(gameTime);
                    break;
                case Phase.DRAWING:
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
            // Update on mouse over
            Tuple<int, int> index = GridIndexOf(Global.MouseHelper.GetCurrentMousePosition());
            if (index != null)
            {
                intensity[index.Item1, index.Item2] = Consts.INTENSITY_MAX;

                if (Global.MouseHelper.IsLeftButtonPressed())
                    selectedIndex = index;

                if (!CanSelect(selectedIndex))
                    selectedIndex = null;
            }

            // Update on keyboard pressed
            var pressedKey = Global.KeyboardHelper.PressedKey;
            if (pressedKey != Keys.None)
            {
                var pressedKeyCode = pressedKey.ToString();

                // The pressed key is a character
                if (pressedKeyCode.Length == 1 && Char.IsLetter(pressedKeyCode[0]))
                {
                    // Set the character at selected index
                    if (selectedIndex != null)
                    {
                        wordGrid.Grid[selectedIndex.Item1, selectedIndex.Item2] = Char.ToUpper(pressedKeyCode[0]);

                        // Event
                        if (newCharacterAppeared != null)
                        {
                            newCharacterAppeared.OnNewCharacterAppeared(selectedIndex.Item1, selectedIndex.Item2);
                        }
                    }
                }
            }
        }

        private bool CanSelect(Tuple<int, int> selectedIndex)
        {
            // Not selected yet
            if (selectedIndex == null)
                return false;
            // The current index has been already filled
            else if (wordGrid.Grid[selectedIndex.Item1, selectedIndex.Item2] != Consts.BLANK)
                return false;
            else
                return true;
        }

        private void UpdateIntensity(GameTime gameTime)
        {
            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                {
                    intensity[row, col] -= Consts.INTENSITY_DELTA;
                    if (intensity[row, col] < 0)
                        intensity[row, col] = 0;
                }
        }

        private Tuple<int, int> GridIndexOf(Vector2 vector2)
        {
            // Outside of grid
            if (vector2.X < left || vector2.Y < top)
                return null;
            else
            {
                int col = (int)(vector2.X - left) / tileWidth;
                int row = (int)(vector2.Y - top) / tileHeight;

                // Outside of grid
                if (col >= wordGrid.NumberOfColumns || row >= wordGrid.NumberOfRows)
                    return null;
                else
                    return new Tuple<int, int>(row, col);
            }
        }

        public override void Draw(GameTime gameTime, object spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                {
                    DrawTileAndEffects(gameTime, (SpriteBatch)spriteBatch, col, row);
                }
        }

        private void DrawTileAndEffects(GameTime gameTime, SpriteBatch param, int col, int row)
        {
            // Position of tile
            float top = this.top + row * tileHeight;
            float left = this.left + col * tileWidth;

            // Draw corresponding tile
            var tile = tiles[wordGrid.Grid[row, col]];
            tile.Draw(gameTime, param, left, top, 1);

            // Draw light effects
            if (Global.CurrentPhase== Phase.IN_GAME)
            {
                tile = tiles[Consts.LIGHT];
                if (selectedIndex != null && selectedIndex.Item1 == row && selectedIndex.Item2 == col)
                    tile.Draw(gameTime, param, left, top, Consts.INTENSITY_SELECTED);
                else
                    tile.Draw(gameTime, param, left, top, intensity[row, col]);
            }
        }
    }
}
