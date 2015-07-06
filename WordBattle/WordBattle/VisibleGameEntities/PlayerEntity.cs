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

        public PlayerEntity(float left, float top, int width, int height, string playerName)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.increasingScore = 0;

            this.playerRecord = new PlayerRecord();
            this.playerRecord.PlayerName = playerName;
            this.playerRecord.PlayerScore = 0;
        }

        int increasingScore;

        public void IncreaseScore(int increase)
        {
            increasingScore += increase;
            entityPhase = Phase.IN_GAME_ACHIEVING;
        }

        public override void Update(GameTime gameTime)
        {
            playerController.Update(gameTime);
            switch (entityPhase)
            {
                case Phase.IN_GAME_ACHIEVING:
                    UpdateAchieving(gameTime);
                    break;
            }            
            base.Update(gameTime);
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
                else
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
            DrawPlayerName(gameTime, spriteBatch);
            DrawPlayerScore(gameTime, spriteBatch);
        }

        private void DrawPlayerScore(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = AllTileSprites.GetInstance();

            string score = playerRecord.PlayerScore.ToString();
            while (score.Length < 6)
                score = "0" + score;

            float shift = (width - Utils.GetTextWidth(score, Consts.PLAYER_FONT_SIZE, Consts.PLAYER_TEXT_SPACING)) / 2;
            for (int index = 0; index < score.Length; index++)
            {
                tiles.GetTileSprite(score[index]).Draw(gameTime, spriteBatch,
                    left + shift + index * Consts.PLAYER_FONT_SIZE + index * Consts.PLAYER_TEXT_SPACING,
                    top + Consts.PANEL_COMPONENT_SPACING + Consts.PLAYER_FONT_SIZE + Consts.PANEL_COMPONENT_SPACING,
                    1,
                    (float)Consts.PLAYER_FONT_SIZE / Consts.TILE_WIDTH);
            }

            // Finish achieving phase
            if (entityPhase == Phase.IN_GAME_ACHIEVING && increasingScore == 0)
                entityPhase = Phase.IN_GAME_ACHIEVING_FINISHED;

        }

        private void DrawPlayerName(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var tiles = AllTileSprites.GetInstance();

            float shift = (width - Utils.GetTextWidth(playerRecord.PlayerName, Consts.PLAYER_FONT_SIZE, Consts.PLAYER_TEXT_SPACING)) / 2;
            for (int index = 0; index < playerRecord.PlayerName.Length; index++)
            {
                tiles.GetTileSprite(playerRecord.PlayerName[index]).Draw(gameTime, spriteBatch,
                    left + shift + index * Consts.PLAYER_FONT_SIZE + index * Consts.PLAYER_TEXT_SPACING,
                    top + Consts.PANEL_COMPONENT_SPACING,
                    1,
                    (float) Consts.PLAYER_FONT_SIZE / Consts.TILE_WIDTH);
            }
        }
    }
}
