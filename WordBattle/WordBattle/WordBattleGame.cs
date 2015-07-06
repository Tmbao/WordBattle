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
using WordBattleCore.GridDataTypes;
using WordBattle.ControllerGameEntities;

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
        WordGrid gridMap;
        Sprite2D background;
        LogoPanel logoPanel;
        TilingGrid tilingGrid;
        PlayerTurn playerTurn;

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

            logoPanel = LogoPanel.GetInstance();

            background = new Sprite2D(0, 0, Utils.LoadSprite(Utils.GetImageFileName("Background")));

            // Just for testing
            Global.CurrentPhase = Phase.PRE_GAME;
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
                case Phase.IN_GAME_MOVING:
                    switch (tilingGrid.EntityPhase)
                    {
                        case Phase.IN_GAME_MOVING_FINISHED:
                            // Update achieving here

                            // Just for testing
                            Global.CurrentPhase = Phase.IN_GAME_END_TURN;
                            playerTurn.Update(gameTime);

                            break;
                    }

                    break;
            }
            
            // Check current Phase
            switch (Global.CurrentPhase)
            {
                case Phase.PRE_GAME:
                    UpdatePreGame(gameTime);
                    break;
                case Phase.IN_GAME_MOVING:
                    UpdateGameMoving(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdatePreGame(GameTime gameTime)
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
                "PLAYER1");
            p1.PlayerController = PlayerGameController.GetInstance();

            var p2 = new PlayerEntity(
                Consts.PLAYER2_PANEL_LEFT, Consts.PLAYER2_PANEL_TOP,
                Consts.PLAYER_PANEL_WIDTH, Consts.PLAYER_PANEL_HEIGHT,
                "PLAYER2");
            p2.PlayerController = PlayerGameController.GetInstance();

            playerTurn = PlayerTurn.GetInstance();
            playerTurn.NewGame(p1, p2, 0);

            Global.CurrentPhase = Phase.IN_GAME_MOVING;
        }

        private void UpdateGameMoving(GameTime gameTime)
        {
            playerTurn.Update(gameTime);
            tilingGrid.Update(gameTime);
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
                // BlendState.AlphaBlend,
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

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
