using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WordBattle.InvisibleGameEntities;
using WordBattle.Utilities;
using WordBattleCore.GridEntities;
using Microsoft.Xna.Framework.Input;
using WordBattle.ControllerGameEntities;
using Microsoft.Xna.Framework.Media;

namespace WordBattle.VisibleGameEntities
{
    public class TilingGrid : VisibleGameEntity
    {
        private static TilingGrid instance;

        public static TilingGrid GetInstance()
        {
            if (instance == null)
                instance = new TilingGrid();
            return instance;
        }

        private TilingGrid()
        {
            lastDrawnWord = "";
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

        public Tuple<int, int> SelectedIndex
        {
            get { return selectedIndex; }
            private set { selectedIndex = value; }
        }

        char pressedCharacter;

        Queue<Queue<Tuple<int, int>>> achievingWords;

        int elapsedUpdateTime;

        string drawnWord;

        string lastDrawnWord;

        public string LastDrawnWord
        {
            get { return lastDrawnWord; }
            set { lastDrawnWord = value; }
        }

        List<Tuple<int, int>> appearOrder;

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
            InitializeAnimating();
        }

        public void InitializeAnimating()
        {
            intensity = new float[wordGrid.NumberOfRows, wordGrid.NumberOfColumns];
            appearOrder = new List<Tuple<int, int>>();
            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                {
                    intensity[row, col] = Consts.INTENSITY_LOADING_MAX;
                    appearOrder.Add(new Tuple<int, int>(row, col));
                }

            // Shuffle the order
            Random rand = new Random();
            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                {
                    int i = rand.Next(appearOrder.Count);
                    int j = rand.Next(appearOrder.Count);

                    // Swap i and j
                    var temp = appearOrder[i];
                    appearOrder[i] = appearOrder[j];
                    appearOrder[j] = temp;
                }
        }

        public override void Update(GameTime gameTime)
        {
            switch (entityPhase)
            {
                case Phase.IN_GAME_LOADING:
                    UpdateLoading(gameTime);
                    break;
                case Phase.IN_GAME_MOVING:
                    UpdateMoving(gameTime);
                    break;
                case Phase.IN_GAME_ACHIEVING:
                    UpdateAchieving(gameTime);
                    break;
                case Phase.END_GAME_ANIMATING:
                    UpdateLoading(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateLoading(GameTime gameTime)
        {
            float delta = Consts.INTENSITY_LOADING_DELTA;
            for (int index = 0; index < appearOrder.Count; index++)
            {
                int row = appearOrder[index].Item1, col = appearOrder[index].Item2;
                {
                    float reduce = Math.Min(delta, intensity[row, col]);
                    intensity[row, col] -= reduce;
                    delta -= reduce;
                }
            }
        }

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

                drawnWord += wordGrid.Grid[index.Item1, index.Item2];

                // Finish a word
                if (word.Count == 0)
                {
                    achievingWords.Dequeue();
                    lastDrawnWord = drawnWord;
                    drawnWord = "";

                    Global.achieveSound.Play();
                }

                // Update
                intensity[index.Item1, index.Item2] = Consts.INTENSITY_HOVER_MAX;
            }
            else
            {
                elapsedUpdateTime += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        private void UpdateMoving(GameTime gameTime)
        {
            // Update light effect 
            UpdateIntensity(gameTime, Consts.INTENSITY_HOVER_DELTA);

            // Update on mouse over
            UpdateMouse();

            // Update on keyboard pressed
            UpdateKeyboard();
        }

        private void UpdateKeyboard()
        {
            var pressedKeyCode = PlayerTurn.GetInstance().CurrentPlayer.PlayerController.PressedCharacter();
            pressedCharacter = '\0';
            // The pressed key is a character
            if (pressedKeyCode != null)
            {
                // Set the character at selected index
                if (selectedIndex != null)
                    wordGrid.Grid[selectedIndex.Item1, selectedIndex.Item2] = pressedCharacter = Char.ToUpper(pressedKeyCode[0]);
            }
        }

        private void UpdateMouse()
        {
            Tuple<int, int> index = Utils.GetIndexOfMouse(MouseController.GetInstance().GetCurrentMousePosition());
            if (index != null)
                intensity[index.Item1, index.Item2] = Consts.INTENSITY_HOVER_MAX;

            selectedIndex = PlayerTurn.GetInstance().CurrentPlayer.PlayerController.SelectedIndex();
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
        private bool IsIntensityAllZeros()
        {
            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                    if (intensity[row, col] != 0)
                        return false;
            return true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (entityPhase)
            {
                case Phase.IN_GAME_LOADING:
                    DrawAllTilesAndEffects(gameTime, spriteBatch);
                    if (IsIntensityAllZeros() == true)
                        entityPhase = Phase.IN_GAME_LOADING_FINISHED;
                    break;
                case Phase.IN_GAME_ACHIEVING:
                    DrawAllTilesAndEffects(gameTime, spriteBatch);
                    if (IsIntensityAllZeros() == true)
                       entityPhase = Phase.IN_GAME_ACHIEVING_FINISHED;
                    break;
                case Phase.IN_GAME_MOVING:
                case Phase.END_GAME:
                    DrawAllTilesAndEffects(gameTime, spriteBatch);
                    // Player has ended turn
                    if (selectedIndex != null && pressedCharacter != '\0')
                        entityPhase = Phase.IN_GAME_MOVING_FINISHED;
                    break;
                case Phase.IN_GAME_ACHIEVING_FINISHED:
                case Phase.IN_GAME_MOVING_FINISHED:
                    DrawAllTilesAndEffects(gameTime, spriteBatch);
                    break;
                case Phase.END_GAME_ANIMATING:
                    DrawAllTilesAndEffects(gameTime, spriteBatch);
                    if (IsIntensityAllZeros() == true)
                        entityPhase = Phase.END_GAME_ANIMATING_FINISHED;
                    break;
            }
        }

        private void DrawAllTilesAndEffects(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int row = 0; row < wordGrid.NumberOfRows; row++)
                for (int col = 0; col < wordGrid.NumberOfColumns; col++)
                    DrawTileAndEffects(gameTime, spriteBatch, col, row);
        }

        private void DrawTileAndEffects(GameTime gameTime, SpriteBatch spriteBatch, int col, int row)
        {
            // Position of tile
            float top = this.top + row * tileHeight;
            float left = this.left + col * tileWidth;

            var tiles = TileSpriteContainer.GetInstance();

            // Draw corresponding tile
            var tile = tiles.GetTileSprite(wordGrid.Grid[row, col]);

            if (entityPhase == Phase.IN_GAME_LOADING)
            {
                tile.Draw(gameTime, spriteBatch, left, top, Consts.INTENSITY_LOADING_MAX - intensity[row, col]);
            }
            else if (entityPhase == Phase.END_GAME_ANIMATING)
            {
                tile.Draw(gameTime, spriteBatch, left, top, intensity[row, col]);
            }
            else
            {
                tile.Draw(gameTime, spriteBatch, left, top);

                // Draw light effects
                if (entityPhase == Phase.IN_GAME_MOVING)
                {
                    tile = tiles.GetTileSprite(Consts.LIGHT_BLUE);
                    if (selectedIndex != null && selectedIndex.Item1 == row && selectedIndex.Item2 == col)
                        tile.Draw(gameTime, spriteBatch, left, top, Consts.INTENSITY_SELECTED);
                    else
                        tile.Draw(gameTime, spriteBatch, left, top, intensity[row, col]);
                }
                else if (entityPhase == Phase.IN_GAME_ACHIEVING)
                {
                    tile = tiles.GetTileSprite(Consts.LIGHT_BLUE);
                    tile.Draw(gameTime, spriteBatch, left, top, intensity[row, col]);
                }
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
            Global.UpdatePhase(Phase.IN_GAME_ACHIEVING);

            // Draw immediately
            elapsedUpdateTime = Consts.DRAWING_EFFECT_TIME;
            drawnWord = "";
        }
    }
}
