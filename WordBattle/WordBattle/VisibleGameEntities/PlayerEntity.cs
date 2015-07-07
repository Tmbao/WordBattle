using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using WordBattleCore.PlayerDataTypes;
using Microsoft.Xna.Framework;
using WordBattle.InvisibleGameEntities;
using WordBattle.Utilities;
using WordBattle.VisibleGameEntities;
using WordBattle.ControllerGameEntities;
using WordBattlePlayer;

namespace WordBattle.VisibleGameEntities
{
    public class PlayerEntity : VisibleGameEntity
    {
        float left, top;

        public float Top
        {
            get { return top; }
            set { top = value; }
        }

        public float Left
        {
            get { return left; }
            set { left = value; }
        }

        int width, height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        PlayerRecord playerRecord;

        public PlayerRecord PlayerRecord
        {
            get { return playerRecord; }
            set { playerRecord = value; }
        }

        Sprite2D playerImage, playerImageHighlighter;

        Phase entityPhase;

        public Phase EntityPhase
        {
            get { return entityPhase; }
            set { entityPhase = value; }
        }

        GameController playerController;

        public GameController PlayerController
        {
            get { return playerController; }
            set { playerController = value; }
        }

        List<int> appearOrder;

        public PlayerEntity(float left, float top, int width, int height, string playerName, string playerImage)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.increasingScore = 0;

            this.entityPhase = Phase.IN_GAME_ACHIEVING_FINISHED;

            this.playerRecord = new PlayerRecord();
            this.playerRecord.PlayerName = playerName;
            this.playerRecord.PlayerScore = 0;

            this.playerImage = new Sprite2D(0, 0, Utils.LoadTextures(Utils.GetImageFileName(playerImage)));
            this.playerImageHighlighter = new Sprite2D(0, 0, Utils.LoadTextures(Utils.GetImageFileName(Consts.LIGHT)));

            intensity = new float[Consts.MAX_NAME_LENGTH];
            appearOrder = new List<int>();
            for (int index = 0; index < intensity.Length; index++)
            {
                intensity[index] = Consts.INTENSITY_LOADING_MAX;
                appearOrder.Add(index);
            }

            // Shuffle the order
            Random rand = new Random();
            for (int index = 0; index < intensity.Length; index++)
            {
                int i = rand.Next(appearOrder.Count);
                int j = rand.Next(appearOrder.Count);

                // Swap i and j
                var temp = appearOrder[i];
                appearOrder[i] = appearOrder[j];
                appearOrder[j] = temp;
            }
        }

        int increasingScore;

        public void IncreaseScore(int increase)
        {
            increasingScore += increase;
            entityPhase = Phase.IN_GAME_ACHIEVING;
        }

        public override void Update(GameTime gameTime)
        {
            switch (entityPhase)
            {
                case Phase.IN_GAME_LOADING:
                    UpdateLoading(gameTime);
                    break;
                case Phase.IN_GAME_ACHIEVING:
                    UpdateAchieving(gameTime);
                    break;
                case Phase.IN_GAME_MOVING:
                    playerController.Update(gameTime);
                    break;
            }
            playerImage.Update(gameTime);
            playerImageHighlighter.Update(gameTime);
            base.Update(gameTime);
        }

        float[] intensity;

        private void UpdateLoading(GameTime gameTime)
        {
            UpdateIntensity();
        }

        private void UpdateIntensity()
        {
            float delta = Consts.INTENSITY_LOADING_DELTA;
            for (int index = 0; index < appearOrder.Count; index++)
            {
                float reduce = Math.Min(delta, intensity[appearOrder[index]]);
                intensity[appearOrder[index]] -= reduce;
                delta -= reduce;
            }
        }

        int elapsedUpdateTime;

        private void UpdateAchieving(GameTime gameTime)
        {
            if (elapsedUpdateTime >= Consts.SCORE_EFFECT_TIME)
            {
                elapsedUpdateTime = 0;

                if (increasingScore > 0)
                {
                    playerRecord.PlayerScore++;
                    increasingScore--;
                }
                else if (increasingScore < 0)
                {
                    playerRecord.PlayerScore--;
                    increasingScore++;
                }
            }
            else
                elapsedUpdateTime += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (entityPhase)
            {
                case Phase.IN_GAME_LOADING:
                    DrawPlayerImage(gameTime, spriteBatch, GetImageIntensity());
                    DrawLoadingPlayerName(gameTime, spriteBatch);
                    DrawLoadingPlayerScore(gameTime, spriteBatch);

                    if (IsIntensityAllZeroes())
                        entityPhase = Phase.IN_GAME_LOADING_FINISHED;

                    break;
                default:
                    DrawPlayerImage(gameTime, spriteBatch);
                    DrawPlayerName(gameTime, spriteBatch);
                    DrawPlayerScore(gameTime, spriteBatch);

                    // Finish achieving phase
                    if (entityPhase == Phase.IN_GAME_ACHIEVING && increasingScore == 0)
                        entityPhase = Phase.IN_GAME_ACHIEVING_FINISHED;

                    break;
            }
        }

        private float GetImageIntensity()
        {
            float sum = 0;
            for (int index = 0; index < intensity.Length; index++)
                sum += Consts.INTENSITY_LOADING_MAX - intensity[index];
            return sum / (Consts.INTENSITY_LOADING_MAX * intensity.Length);
        }

        private bool IsIntensityAllZeroes()
        {
            for (int index = 0; index < intensity.Length; index++)
                if (intensity[index] != 0)
                    return false;
            return true;
        }

        private float[] ReverseIntensity()
        {
            float[] _intensity = new float[intensity.Length];

            for (int index = 0; index < intensity.Length; index++)
                _intensity[index] = Consts.INTENSITY_LOADING_MAX - intensity[index];

            return _intensity;
        }

        private void DrawLoadingPlayerScore(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = TileSpriteContainer.GetInstance();

            string score = playerRecord.PlayerScore.ToString();
            while (score.Length < Consts.MAX_SCORE_DIGIT)
                score = "0" + score;

            // Show in the center
            // float shift = (width - Utils.GetTextWidth(score, Consts.PLAYER_FONT_SIZE, Consts.PLAYER_TEXT_SPACING)) / 2;
            // Show in the left
            float shift = 0;
            tiles.DrawText(gameTime, spriteBatch, score,
                left + Consts.PLAYER_IMAGE_WIDTH + Consts.COMPONENT_SPACING + shift,
                top + Consts.COMPONENT_SPACING + Consts.PLAYER_FONT_SIZE + Consts.COMPONENT_SPACING,
                Consts.PLAYER_FONT_SIZE, ReverseIntensity());
        }

        private void DrawLoadingPlayerName(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = TileSpriteContainer.GetInstance();

            // Show in the center
            // float shift = (width - Utils.GetTextWidth(playerRecord.PlayerName, Consts.PLAYER_FONT_SIZE, Consts.PLAYER_TEXT_SPACING)) / 2;
            // Show in the left
            float shift = 0;
            tiles.DrawText(gameTime, spriteBatch, playerRecord.PlayerName,
                left + Consts.PLAYER_IMAGE_WIDTH + Consts.COMPONENT_SPACING + shift,
                top + Consts.COMPONENT_SPACING,
                Consts.PLAYER_FONT_SIZE, ReverseIntensity());
        }

        private void DrawPlayerImage(GameTime gameTime, SpriteBatch spriteBatch, float intensity)
        {
            playerImage.Draw(gameTime, spriteBatch, left, top, intensity, (float)Consts.PLAYER_IMAGE_WIDTH / playerImage.Width);
        }

        private void DrawPlayerImage(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawPlayerImage(gameTime, spriteBatch, 1);
        }

        private void DrawPlayerScore(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = TileSpriteContainer.GetInstance();

            string score = playerRecord.PlayerScore.ToString();
            while (score.Length < Consts.MAX_SCORE_DIGIT)
                score = "0" + score;

            // Show in the center
            // float shift = (width - Utils.GetTextWidth(score, Consts.PLAYER_FONT_SIZE, Consts.PLAYER_TEXT_SPACING)) / 2;
            // Show in the left
            float shift = 0;
            tiles.DrawText(gameTime, spriteBatch, score,
                left + Consts.PLAYER_IMAGE_WIDTH + Consts.COMPONENT_SPACING + shift,
                top + Consts.COMPONENT_SPACING + Consts.PLAYER_FONT_SIZE + Consts.COMPONENT_SPACING,
                Consts.PLAYER_FONT_SIZE);
        }

        private void DrawPlayerName(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = TileSpriteContainer.GetInstance();

            // Show in the center
            // float shift = (width - Utils.GetTextWidth(playerRecord.PlayerName, Consts.PLAYER_FONT_SIZE, Consts.PLAYER_TEXT_SPACING)) / 2;
            // Show in the left
            float shift = 0;
            tiles.DrawText(gameTime, spriteBatch, playerRecord.PlayerName,
                left + Consts.PLAYER_IMAGE_WIDTH + Consts.COMPONENT_SPACING + shift,
                top + Consts.COMPONENT_SPACING,
                Consts.PLAYER_FONT_SIZE);
        }

        public void DrawHighlightName(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerImageHighlighter.Draw(gameTime, spriteBatch, left, top, 1, (float)Consts.PLAYER_IMAGE_WIDTH / playerImageHighlighter.Width);
        }
    }
}
