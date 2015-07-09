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

        CustomCusor cursor;

        TileButton backButton, soundButton;

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
            //IsMouseVisible = true;
            cursor = CustomCusor.GetInstance();

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

            // Initialize button
            backButton = new TileButton(25, 620, Utils.GetImageFileName("Back"));
            soundButton = new TileButton(70, 620, Utils.GetImageFileName("Sound"));

            // Load sound effects
            Global.clickSound = Content.Load<SoundEffect>(@"Sound\click");
            Global.achieveSound = Content.Load<SoundEffect>(@"Sound\achieve");
            Global.themeSong = Content.Load<Song>(@"Sound\theme");

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
                    CheckMenuLoading();
                    break;
                case Phase.MENU:
                    CheckMenu();
                    break;
                case Phase.MENU_SELECTED_ANIMATING:
                    CheckMenuSelectedAnimating();
                    break;
                case Phase.IN_GAME_LOADING:
                    CheckGameLoading();
                    break;
                case Phase.IN_GAME_MOVING:
                case Phase.END_GAME:
                    CheckGameMoving();
                    break;
                case Phase.IN_GAME_ACHIEVING:
                    CheckGameAchieving(gameTime);
                    break;
                case Phase.END_GAME_ANIMATING:
                    CheckEndGameAnimating(gameTime);
                    break;
            }

            keyboardController.Update(gameTime);
            mouseController.Update(gameTime);
            cursor.Update(gameTime);
            
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
                case Phase.END_GAME:
                    UpdateGameMoving(gameTime);
                    break;
                case Phase.IN_GAME_ACHIEVING:
                    UpdateGameAchieving(gameTime);
                    break;
                case Phase.END_GAME_ANIMATING:
                    UpdateEndGameAnimating(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void CheckEndGameAnimating(GameTime gameTime)
        {
            if (tilingGrid.EntityPhase == Phase.END_GAME_ANIMATING_FINISHED &&
                playerTurn.EntityPhase == Phase.END_GAME_ANIMATING_FINISHED &&
                logoPanel.EntityPhase == Phase.END_GAME_ANIMATING_FINISHED)
            {
                Global.UpdatePhase(Phase.MENU_LOADING);
            }

        }

        private void UpdateEndGameAnimating(GameTime gameTime)
        {
            tilingGrid.Update(gameTime);
            playerTurn.Update(gameTime);
            notification.Update(gameTime);

            if (tilingGrid.EntityPhase == Phase.END_GAME_ANIMATING_FINISHED)
                logoPanel.Update(gameTime);
        }

        private void CheckMenuLoading()
        {
            if (menuContainer.EnityPhase == Phase.MENU_LOADING_FINISHED)
                Global.UpdatePhase(Phase.MENU);
        }

        private void CheckMenu()
        {
            gameMode = menuContainer.GetSelectedMode();
            if (gameMode != GameMode.NONE)
            {
                p1Name = menuContainer.GetPlayer1Name();
                p2Name = menuContainer.GetPlayer2Name();
                roomName = menuContainer.GetRoomName();
                Global.UpdatePhase(Phase.MENU_SELECTED_ANIMATING);
            }

            if (backButton.IsClicked)
                Exit();
        }

        private void CheckMenuSelectedAnimating()
        {
            if (menuContainer.EnityPhase == Phase.MENU_SELECTED_ANIMATING_FINISHED &&
                logoPanel.EntityPhase == Phase.MENU_SELECTED_ANIMATING_FINISHED)
            {
                CreateNewGame();
                notification.PushMessage("READY");
                Global.UpdatePhase(Phase.IN_GAME_LOADING);
            }
        }

        private void CheckGameLoading()
        {
            // Add update logic here

            // Finished loading 
            if (tilingGrid.EntityPhase == Phase.IN_GAME_LOADING_FINISHED)
            {
                notification.PushMessage("GO");
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Stop();
                MediaPlayer.Play(Global.themeSong);
                Global.UpdatePhase(Phase.IN_GAME_MOVING);
            }
        }

        private void CheckGameMoving()
        {
            if (soundButton.IsClicked)
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Stop();
                else
                    MediaPlayer.Play(Global.themeSong);
            }

            if (backButton.IsClicked)
            {
                if (gameMode == GameMode.NETWORK)
                {
                    PlayerGameControllerOnline.SendMessage(new PlayerGameControllerOnline.Message { RoomId=roomName, Turn = -2 });
                }
                tilingGrid.InitializeAnimating();
                playerTurn.InitializeAnimating();
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Stop();
                Global.UpdatePhase(Phase.END_GAME_ANIMATING);
                return;
            }
            switch (tilingGrid.EntityPhase)
            {
                case Phase.IN_GAME_MOVING_FINISHED:
                    // Show achieved words

                    var index = tilingGrid.SelectedIndex;
                    var correctedWords = gridMap.GetCorrectedWords(index);

                    tilingGrid.AchieveWords(correctedWords);
                    break;
            }
        }

        private void CheckGameAchieving(GameTime gameTime)
        {
            // Add update logic here
            if (soundButton.IsClicked)
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Stop();
                else
                    MediaPlayer.Play(Global.themeSong);
            }
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
                playerTurn.CurrentPlayer.EntityPhase == Phase.IN_GAME_ACHIEVING_FINISHED)
            {
                Global.CurrentPhase = Phase.IN_GAME_END_TURN;
                playerTurn.Update(gameTime);
            }
        }

        private void UpdateMenu(GameTime gameTime)
        {
            menuContainer.Update(gameTime);
            logoPanel.Update(gameTime);
            notification.Update(gameTime);

            if (Global.CurrentPhase == Phase.MENU)
                backButton.Update(gameTime);
        }

        private void CreateNewGame()
        {
            gridMap.IntializeNewMap();

            tilingGrid = TilingGrid.GetInstance();
            tilingGrid.Load(
                Consts.GRID_LEFT, Consts.GRID_TOP,
                Consts.TILE_WIDTH, Consts.TILE_HEIGHT);

            PlayerEntity p1 = null, p2 = null;

            switch (gameMode)
            {
                case GameMode.SINGLE:
                    p1 = new PlayerEntity(
                        Consts.PLAYER1_PANEL_LEFT, Consts.PLAYER1_PANEL_TOP,
                        Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                        p1Name, "Single");
                    p1.PlayerController = PlayerGameController.GetInstance();
                    p2 = new PlayerEntity(
                        Consts.PLAYER2_PANEL_LEFT, Consts.PLAYER2_PANEL_TOP, 
                        Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT, 
                        "COMPUTER", "AI");
                    p2.PlayerController = new RandomAIPlayer();
                    break;

                case GameMode.MULTI:
                    p1 = new PlayerEntity(
                        Consts.PLAYER1_PANEL_LEFT, Consts.PLAYER1_PANEL_TOP,
                        Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                        p1Name, "Single");
                    p1.PlayerController = PlayerGameController.GetInstance();
                    p2 = new PlayerEntity(
                        Consts.PLAYER2_PANEL_LEFT, Consts.PLAYER2_PANEL_TOP,
                        Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                        p2Name, "Single");
                    p2.PlayerController = PlayerGameController.GetInstance();
                    break;

                case GameMode.NETWORK:
                    PlayerGameControllerOnline.RoomId = roomName;
                    PlayerGameControllerOnline.MyName = p1Name;
                    PlayerGameControllerOnline.Connect();

                    int myTurn = PlayerGameControllerOnline.Turn;
                    if (myTurn == 0)
                    {
                        p1 = new PlayerEntity(
                            Consts.PLAYER1_PANEL_LEFT, Consts.PLAYER1_PANEL_TOP,
                            Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                            PlayerGameControllerOnline.MyName, "Single");
                        p1.PlayerController = PlayerGameControllerOnline.GetInstance(ControllerOwner.ME);
                        p2 = new PlayerEntity(
                            Consts.PLAYER2_PANEL_LEFT, Consts.PLAYER2_PANEL_TOP,
                            Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                            PlayerGameControllerOnline.OpName, "Single");
                        p2.PlayerController = PlayerGameControllerOnline.GetInstance(ControllerOwner.OPPONENT);
                    }
                    else if (myTurn == 1)
                    {
                        p2 = new PlayerEntity(
                            Consts.PLAYER1_PANEL_LEFT, Consts.PLAYER1_PANEL_TOP,
                            Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                            PlayerGameControllerOnline.MyName, "Single");
                        p2.PlayerController = PlayerGameControllerOnline.GetInstance(ControllerOwner.ME);
                        p1 = new PlayerEntity(
                            Consts.PLAYER2_PANEL_LEFT, Consts.PLAYER2_PANEL_TOP,
                            Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                            PlayerGameControllerOnline.OpName, "Single");
                        p1.PlayerController = PlayerGameControllerOnline.GetInstance(ControllerOwner.OPPONENT);
                    }

                    // Online mode is under construction
                    break;

                default:
                    break;
            }

            playerTurn = PlayerTurn.GetInstance();
            playerTurn.NewGame(p1, p2, 0);
        }

        private void UpdateGameLoading(GameTime gameTime)
        {
            notification.Update(gameTime);
            playerTurn.Update(gameTime);
            if (playerTurn.EntityPhase == Phase.IN_GAME_LOADING_FINISHED)
                tilingGrid.Update(gameTime);
        }

        private void UpdateGameMoving(GameTime gameTime)
        {
            notification.Update(gameTime);
            playerTurn.Update(gameTime);
            tilingGrid.Update(gameTime);
            backButton.Update(gameTime);
            soundButton.Update(gameTime);
        }

        private void UpdateGameAchieving(GameTime gameTime)
        {
            tilingGrid.Update(gameTime);
            playerTurn.Update(gameTime);
            notification.Update(gameTime);
            soundButton.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            switch (Global.CurrentPhase)
            {
                case Phase.MENU_LOADING:
                case Phase.MENU:
                case Phase.MENU_SELECTED_ANIMATING:
                    spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        //BlendState.AlphaBlend,
                        Utilities.PSBlendState.Multiply,
                        null,
                        DepthStencilState.None,
                        RasterizerState.CullNone,
                        null,
                        Global.MainCamera.WVP);
                    cursor.Draw(gameTime, spriteBatch);
                    logoPanel.Draw(gameTime, spriteBatch);
                    background.Draw(gameTime, spriteBatch);
                    menuContainer.Draw(gameTime, spriteBatch);
                    notification.Draw(gameTime, spriteBatch);
                    backButton.Draw(gameTime, spriteBatch);

                    spriteBatch.End();
                    break;

                case Phase.IN_GAME_LOADING:
                case Phase.IN_GAME_MOVING:
                case Phase.IN_GAME_ACHIEVING:
                case Phase.END_GAME:
                case Phase.END_GAME_ANIMATING:
                    spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        //BlendState.AlphaBlend,
                        Utilities.PSBlendState.Multiply,
                        null,
                        DepthStencilState.None,
                        RasterizerState.CullNone,
                        null,
                        Global.MainCamera.WVP);
                    cursor.Draw(gameTime, spriteBatch);
                    logoPanel.Draw(gameTime, spriteBatch);
                    background.Draw(gameTime, spriteBatch);
                    playerTurn.Draw(gameTime, spriteBatch);
                    tilingGrid.Draw(gameTime, spriteBatch);
                    notification.Draw(gameTime, spriteBatch);
                    backButton.Draw(gameTime, spriteBatch);

                    if (Global.CurrentPhase == Phase.IN_GAME_MOVING ||
                        Global.CurrentPhase == Phase.IN_GAME_ACHIEVING ||
                        Global.CurrentPhase == Phase.END_GAME)
                        soundButton.Draw(gameTime, spriteBatch);

                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
