using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WordBattle.VisibleGameEntities;
using WordBattle.InvisibleGameEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WordBattle.VisibleGameEntities
{
    class PlayerTurn : VisibleGameEntity
    {
        private static PlayerTurn instance;

        public static PlayerTurn GetInstance()
        {
            if (instance == null)
                instance = new PlayerTurn();
            return instance;
        }

        private PlayerTurn()
        {
            players = new PlayerEntity[2];
        }

        public void NewGame(PlayerEntity p1, PlayerEntity p2, int turn)
        {
            players[0] = p1;
            players[1] = p2;
            this.turn = turn;
        }

        int turn;
        PlayerEntity[] players;

        public PlayerEntity[] Players
        {
            get { return players; }
            private set { players = value; }
        }

        public PlayerEntity CurrentPlayer
        {
            get { return players[turn]; }
            private set { players[turn] = value; }
        }

        public void UpdatePhase(Phase phase)
        {
            if (players[0] != null)
                players[0].EntityPhase = phase;
            if (players[1] != null)
                players[1].EntityPhase = phase;
        }

        public override void Update(GameTime gameTime)
        {
            switch (Global.CurrentPhase)
            {
                case Phase.IN_GAME_END_TURN:
                    // Change turn
                    turn ^= 1;
                    Global.UpdatePhase(Phase.IN_GAME_MOVING);

                    // This is not the first move any more
                    WordBattleCore.GridEntities.WordGrid.GetInstance().FirstMove = false;
                    break;
                case Phase.IN_GAME_MOVING:
                    CurrentPlayer.Update(gameTime);
                    break;
                case Phase.IN_GAME_ACHIEVING:
                    players[0].Update(gameTime);
                    players[1].Update(gameTime);
                    break;
                case Phase.IN_GAME_LOADING:
                    players[0].Update(gameTime);
                    if (players[0].EntityPhase == Phase.IN_GAME_LOADING_FINISHED)
                        players[1].Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (Global.CurrentPhase) {
                case Phase.IN_GAME_LOADING:
                    players[0].Draw(gameTime, spriteBatch);
                    players[1].Draw(gameTime, spriteBatch);
                    break;
                case Phase.IN_GAME_MOVING:
                case Phase.IN_GAME_ACHIEVING:
                    players[0].Draw(gameTime, spriteBatch);
                    players[1].Draw(gameTime, spriteBatch);
                    CurrentPlayer.DrawHighlightName(gameTime, spriteBatch);
                    break;
            }
            
            
        }
    }
}
