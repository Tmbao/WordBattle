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
        MenuContainer menuContainer;
        TrieDictionary dictionary;
        WordGrid gridMap;
        Sprite2D background;
        LogoPanel logoPanel;
        TilingGrid tilingGrid;
        PlayerTurn playerTurn;
        GameNotification notification;

        MouseController mouseController;
        KeyboardController keyboardController;

        GameMode gameMode;

        string p1Name, p2Name, roomName;

        public WordBattleGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = Consts.ContentDirectory;

            IsFixedTimeStep = false;

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

            // Initialize menu
            menuContainer = MenuContainer.GetInstance();

            // Initialize background
            background = new Sprite2D(0, 0, Utils.LoadTextures(Utils.GetImageFileName("Background")));

            // Initialize notification
            notification = GameNotification.GetInstance();

            // Initialize controller
            mouseController = MouseController.GetInstance();
            keyboardController = KeyboardController.GetInstance();

            // Just for testing
            Global.UpdatePhase(Phase.MENU_LOADING);
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
                case Phase.MENU_LOADING:
                    if (menuContainer.EnityPhase == Phase.MENU_LOADING_FINISHED)
                        Global.UpdatePhase(Phase.MENU);
                    break;
                case Phase.MENU:
                    if (menuContainer.GetSelectedMode() != GameMode.NONE)
                    {
                        gameMode = menuContainer.GetSelectedMode();
                        p1Name = menuContainer.GetPlayer1Name();
                        p2Name = menuContainer.GetPlayer2Name();
                        roomName = menuContainer.GetRoomName();
                        Global.UpdatePhase(Phase.MENU_SELECTED_ANIMATING);
                    }
                    break;
                case Phase.MENU_SELECTED_ANIMATING:
                    if (menuContainer.EnityPhase == Phase.MENU_SELECTED_ANIMATING_FINISHED &&
                        logoPanel.EntityPhase == Phase.MENU_SELECTED_ANIMATING_FINISHED)
                    {
                        CreateNewGame();
                        notification.PushMessage("READY");
                        Global.UpdatePhase(Phase.IN_GAME_LOADING);
                    }
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
                    if (tilingGrid.LastDrawnWord.Length > 0)
                    {
                        playerTurn.CurrentPlayer.IncreaseScore(tilingGrid.LastDrawnWord.Length);
                        notification.PushMessage(tilingGrid.LastDrawnWord);

                        // Clear last drawn word
                        tilingGrid.LastDrawnWord = "";
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

            keyboardController.Update(gameTime);
            mouseController.Update(gameTime);
            
            // Check current phase
            switch (Global.CurrentPhase)
            {
                case Phase.MENU_LOADING:
                case Phase.MENU:
                case Phase.MENU_SELECTED_ANIMATING:
                    UpdateMenu(gameTime);
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

        private void UpdateMenu(GameTime gameTime)
        {
            menuContainer.Update(gameTime);
            logoPanel.Update(gameTime);
        }

        Vector3 logoTranslation;

        private void CreateNewGame()
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
                p1Name, "Single");
            p1.PlayerController = PlayerGameController.GetInstance();

            PlayerEntity p2;

            switch (gameMode)
            {
                case GameMode.SINGLE:
                    p2 = new PlayerEntity(
                        Consts.PLAYER2_PANEL_LEFT, Consts.PLAYER2_PANEL_TOP, 
                        Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT, 
                        "COMPUTER", "AI");
                    p2.PlayerController = new RandomAIPlayer();
                    break;

                case GameMode.MULTI:
                case GameMode.NETWORK:
                default:
                    // Online mode is under construction
                    p2 = new PlayerEntity(
                        Consts.PLAYER2_PANEL_LEFT, Consts.PLAYER2_PANEL_TOP,
                        Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                        p2Name, "Single");

                    break;
            }

            playerTurn = PlayerTurn.GetInstance();
            playerTurn.NewGame(p1, p2, 0);
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

            spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                //BlendState.AlphaBlend,
                Utilities.PSBlendState.Multiply,
                null,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Global.MainCamera.WVP);

            switch (Global.CurrentPhase)
            {
                case Phase.MENU_LOADING:
                case Phase.MENU:
                case Phase.MENU_SELECTED_ANIMATING:
                    logoPanel.Draw(gameTime, spriteBatch);
                    background.Draw(gameTime, spriteBatch);
                    menuContainer.Draw(gameTime, spriteBatch);
                    break;
                case Phase.IN_GAME_LOADING:
                case Phase.IN_GAME_MOVING:
                case Phase.IN_GAME_ACHIEVING:
                    logoPanel.Draw(gameTime, spriteBatch);
                    background.Draw(gameTime, spriteBatch);
                    playerTurn.Draw(gameTime, spriteBatch);
                    tilingGrid.Draw(gameTime, spriteBatch);
                    notification.Draw(gameTime, spriteBatch);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
