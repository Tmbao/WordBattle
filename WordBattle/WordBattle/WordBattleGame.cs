using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WordBattle.InvisibleGameEntities;
using WordBattle.VisibleGameEntities;
using WordBattle.Utilities;
using WordBattleCore.GridEntities;
using WordBattle.ControllerGameEntities;
using WordBattleCore.Dictionary;
using WordBattlePlayer.Computer;

namespace WordBattle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class WordBattleGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // My entities
        TrieDictionary dictionary;
        WordGrid gridMap;
        Sprite2D background;
        LogoPanel logoPanel;
        TilingGrid tilingGrid;
        PlayerTurn playerTurn;
        GameNotification notification;

        public WordBattleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Consts.ContentDirectory;

            // Modify screen size
            graphics.PreferredBackBufferWidth = Consts.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Consts.SCREEN_HEIGHT;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Global.Content = Content;

            // Allow mouse
            IsMouseVisible = true;

            // Initialize map
            gridMap = WordGrid.GetInstance();
            gridMap.Load(Content.Load<GridData>(Utils.GetMapFileName(Consts.DEFALT_MAP_NAME)));

            // Initialize dictionary
            dictionary = TrieDictionary.GetInstance();
            dictionary.Load(Content.Load<string[]>(Utils.GetDictionaryFileName(Consts.DEFAULT_DICTIONARY_NAME)));

            // Initialize logo panel
            logoPanel = LogoPanel.GetInstance();

            // Initialize background
            background = new Sprite2D(0, 0, Utils.LoadSprite(Utils.GetImageFileName("Background")));

            // Initialize notification
            notification = GameNotification.GetInstance();

            // Just for testing
            Global.CurrentPhase = Phase.NEW_GAME;
        }

        

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Global.UpdateAll(gameTime);

            // Check and switch into new phase
            switch (Global.CurrentPhase)
            {
                case Phase.NEW_GAME:
                    break;
                case Phase.IN_GAME_LOADING:
                    // Add update logic here

                    // Finished loading 
                    if (tilingGrid.EntityPhase == Phase.IN_GAME_LOADING_FINISHED)
                    {
                        notification.PushMessage("GO");
                        Global.UpdatePhase(Phase.IN_GAME_MOVING);
                    }
                    break;
                case Phase.IN_GAME_MOVING:
                    switch (tilingGrid.EntityPhase)
                    {
                        case Phase.IN_GAME_MOVING_FINISHED:
                            // Show achieved words

                            var index = tilingGrid.SelectedIndex;
                            var correctedWords = gridMap.GetCorrectedWords(index);

                            tilingGrid.AchieveWords(correctedWords);

                            break;
                    }
                    break;

                case Phase.IN_GAME_ACHIEVING:
                    // Add update logic here

                    // Update score
                    if (tilingGrid.LastDrawedWord.Length > 0)
                    {
                        playerTurn.CurrentPlayer.IncreaseScore(tilingGrid.LastDrawedWord.Length);
                        notification.PushMessage(tilingGrid.LastDrawedWord);

                        // Clear last drawed word
                        tilingGrid.LastDrawedWord = "";
                    }

                    // Finished presenting achieved words
                    if (tilingGrid.EntityPhase == Phase.IN_GAME_ACHIEVING_FINISHED && 
                        notification.EntityPhase == Phase.IN_GAME_ACHIEVING_FINISHED &&
                        playerTurn.CurrentPlayer.EntityPhase == Phase.IN_GAME_ACHIEVING_FINISHED)
                    {
                        Global.CurrentPhase = Phase.IN_GAME_END_TURN;
                        playerTurn.Update(gameTime);
                    }

                    break;
            }
            
            // Check current phase
            switch (Global.CurrentPhase)
            {
                case Phase.NEW_GAME:
                    UpdateNewGame(gameTime);
                    break;
                case Phase.IN_GAME_LOADING:
                    UpdateGameLoading(gameTime);
                    break;
                case Phase.IN_GAME_MOVING:
                    UpdateGameMoving(gameTime);
                    break;
                case Phase.IN_GAME_ACHIEVING:
                    UpdateGameAchieving(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateNewGame(GameTime gameTime)
        {
            gridMap.IntializeNewMap();

            tilingGrid = TilingGrid.GetInstance();
            tilingGrid.Load(
                Consts.GRID_LEFT, Consts.GRID_TOP,
                Consts.TILE_WIDTH, Consts.TILE_HEIGHT);

            // Just for testing
            var p1 = new PlayerEntity(
                Consts.PLAYER1_PANEL_LEFT, Consts.PLAYER1_PANEL_TOP,
                Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                "TMBAO", "Human");
            p1.PlayerController = PlayerGameController.GetInstance();

            var p2 = new PlayerEntity(
                Consts.PLAYER2_PANEL_LEFT, Consts.PLAYER2_PANEL_TOP,
                Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                "RANDOMAI", "AI");
            p2.PlayerController = new RandomAIPlayer();

            playerTurn = PlayerTurn.GetInstance();
            playerTurn.NewGame(p1, p2, 0);

            Global.UpdatePhase(Phase.IN_GAME_LOADING);
            notification.PushMessage("READY");
        }

        private void UpdateGameLoading(GameTime gameTime)
        {
            notification.Update(gameTime);
            playerTurn.Update(gameTime);
            if (playerTurn.Players[1].EntityPhase == Phase.IN_GAME_LOADING_FINISHED)
                tilingGrid.Update(gameTime);
        }

        private void UpdateGameMoving(GameTime gameTime)
        {
            notification.Update(gameTime);
            playerTurn.Update(gameTime);
            tilingGrid.Update(gameTime);            
        }

        private void UpdateGameAchieving(GameTime gameTime)
        {
            tilingGrid.Update(gameTime);
            playerTurn.Update(gameTime);
            notification.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                //BlendState.AlphaBlend,
                Utilities.PSBlendState.Multiply,
                null,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Global.MainCamera.WVP);

            background.Draw(gameTime, spriteBatch);
            logoPanel.Draw(gameTime, spriteBatch);
            playerTurn.Draw(gameTime, spriteBatch);
            tilingGrid.Draw(gameTime, spriteBatch);
            notification.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
