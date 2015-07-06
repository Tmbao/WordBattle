using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordBattle.InvisibleGameEntities;
using WordBattle.Utilities;
using WordBattleCore.GridDataTypes;
using Microsoft.Xna.Framework.Input;
using WordBattle.ControllerGameEntities;

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

        Phase entityPhase;

        public Phase EntityPhase
        {
            get { return entityPhase; }
            set { entityPhase = value; }
        }

        Tuple<int, int> selectedIndex;

        Queue<Queue<Tuple<int, int>>> achievingWords;

        public void Load(float left, float top, int tileWidth, int tileHeight)
        {
            this.wordGrid = WordGrid.GetInstance();

            this.left = left;
            this.top = top;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.mapWidth = tileWidth * wordGrid.NumberOfColumns;
            this.mapHeight = this.TileHeight * wordGrid.NumberOfRows;

            intensity = new float[wordGrid.NumberOfRows, wordGrid.NumberOfColumns];
        }

        public override void Update(GameTime gameTime)
        {
            switch (entityPhase)
            {
                case Phase.IN_GAME_MOVING:
                    UpdateGame(gameTime);
                    break;
                case Phase.IN_GAME_ACHIEVING:
                    UpdateAchieving(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        int elapsedUpdateTime;
        string drawedWord;

        private void UpdateAchieving(GameTime gameTime)
        {
            // Reset selectedIndex to avoid unexpected behaviours
            selectedIndex = null;

            // Update light effect 
            UpdateIntensity(gameTime, Consts.INTENSITY_ACHIEVED_DELTA);
            
            // No more words
            if (achievingWords.Count == 0)
                return;

            if (elapsedUpdateTime >= Consts.DRAWING_EFFECT_TIME)
            {
                elapsedUpdateTime = 0;

                // Retrieve index to update
                var word = achievingWords.ElementAt(0);
                var index = word.Dequeue();

                drawedWord += wordGrid.Grid[index.Item1, index.Item2];

                // Finish a word
                if (word.Count == 0)
                {
                    achievingWords.Dequeue();
                    drawedWord = "";
                }

                // Update
                intensity[index.Item1, index.Item2] = Consts.INTENSITY_MAX;
            }
            else
            {
                elapsedUpdateTime += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        private void UpdateGame(GameTime gameTime)
        {
            // Update light effect 
            UpdateIntensity(gameTime, Consts.INTENSITY_DELTA);

            // Update on mouse over
            UpdateMouse();

            // Update on keyboard pressed
            UpdateKeyboard();
        }

        private void UpdateKeyboard()
        {
            var pressedKeyCode = PlayerTurn.GetInstance().CurrentPlayer.PlayerController.PressedCharacters();

            // The pressed key is a character
            if (pressedKeyCode != null && pressedKeyCode.Length == 1 && Char.IsLetter(pressedKeyCode[0]))
            {
                // Set the character at selected index
                if (selectedIndex != null)
                    wordGrid.Grid[selectedIndex.Item1, selectedIndex.Item2] = Char.ToUpper(pressedKeyCode[0]);
            }
        }

        private void UpdateMouse()
        {
            Tuple<int, int> index = Utils.GetIndexOfMouse(MouseController.GetInstance().GetCurrentMousePosition());
            if (index != null)
                intensity[index.Item1, index.Item2] = Consts.INTENSITY_MAX;

            selectedIndex = PlayerTurn.GetInstance().CurrentPlayer.PlayerController.SelectedIndex();
        }

        private bool CanSelect(Tuple<int, int> selectedIndex)
        {
            return wordGrid.CanFill(selectedIndex);
        }

        private void UpdateIntensity(GameTime gameTime, float delta)
        {
            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                {
                    intensity[row, col] -= delta;
                    if (intensity[row, col] < 0)
                        intensity[row, col] = 0;
                }
        }

        // Return true if intensity array is all zeros, false otherwise
        private bool IntensityState()
        {
            bool state = true;
            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                    if (intensity[row, col] != 0)
                        state = false;
            return state;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                    DrawTileAndEffects(gameTime, spriteBatch, col, row);

            switch (entityPhase)
            {
                case Phase.IN_GAME_ACHIEVING:
                    if (IntensityState() == true)
                        entityPhase = Phase.IN_GAME_ACHIEVING_FINISHED;
                    break;
                case Phase.IN_GAME_MOVING:
                    // Player has ended turn
                    if (selectedIndex != null &&
                        PlayerTurn.GetInstance().CurrentPlayer.PlayerController.PressedCharacters() != null)
                        entityPhase = Phase.IN_GAME_MOVING_FINISHED;
                    break;
            }
        }

        private void DrawTileAndEffects(GameTime gameTime, SpriteBatch param, int col, int row)
        {
            // Position of tile
            float top = this.top + row * tileHeight;
            float left = this.left + col * tileWidth;

            var tiles = AllTileSprites.GetInstance();

            // Draw corresponding tile
            var tile = tiles.GetTileSprite(wordGrid.Grid[row, col]);
            tile.Draw(gameTime, param, left, top);

            // Draw light effects
            if (entityPhase == Phase.IN_GAME_MOVING)
            {
                tile = tiles.GetTileSprite(Consts.LIGHT);
                if (selectedIndex != null && selectedIndex.Item1 == row && selectedIndex.Item2 == col)
                    tile.Draw(gameTime, param, left, top, Consts.INTENSITY_SELECTED);
                else
                    tile.Draw(gameTime, param, left, top, intensity[row, col]);
            }
            else if (entityPhase == Phase.IN_GAME_ACHIEVING)
            {
                tile = tiles.GetTileSprite(Consts.LIGHT_BLUE);
                tile.Draw(gameTime, param, left, top, intensity[row, col]);
            }
        }

        public void AchieveWords(Queue<Queue<Tuple<int, int>>> words)
        {
            achievingWords = words;

            // Prepare for DRAWING PHASE

            // Reset intensity
            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                    intensity[row, col] = 0;

            // Update CurrentPhase
            entityPhase = Phase.IN_GAME_ACHIEVING;

            // Draw immediately
            elapsedUpdateTime = Consts.DRAWING_EFFECT_TIME;

            drawedWord = "";
        }
    }
}
