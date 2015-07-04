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
        TilingGrid tilingGrid;

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

            tilingGrid = TilingGrid.GetInstance();
            tilingGrid.Load(
                Consts.GRID_LEFT, Consts.GRID_TOP, 
                Consts.TILE_WIDTH, Consts.TILE_HEIGHT, 
                Utils.GetMapFileName(Consts.MAP_NAME));

            Global.CurrentPhase = Phase.ACHIEVING;

            Queue<Queue<Tuple<int, int>>> words = new Queue<Queue<Tuple<int, int>>>();
            Queue<Tuple<int, int>> word = new Queue<Tuple<int, int>>();
            word.Enqueue(new Tuple<int, int>(1, 1));
            word.Enqueue(new Tuple<int, int>(1, 2));
            word.Enqueue(new Tuple<int, int>(1, 3));
            word.Enqueue(new Tuple<int, int>(1, 4));
            word.Enqueue(new Tuple<int, int>(2, 4));
            word.Enqueue(new Tuple<int, int>(3, 4));
            
            words.Enqueue(word);

            tilingGrid.ShowWords(words);
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
            tilingGrid.Update(gameTime);

            // Check current Phase
            switch (Global.CurrentPhase)
            {
                case Phase.IN_GAME:
                    UpdateGame(gameTime);
                    break;
                case Phase.ACHIEVING:
                    UpdateDrawing(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateDrawing(GameTime gameTime)
        {
            return;
            throw new NotImplementedException();
        }

        private void UpdateGame(GameTime gameTime)
        {
            return;
            throw new NotImplementedException();
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
                Utilities.PSBlendState.Multiply,
                null,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Global.MainCamera.WVP);
            tilingGrid.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    class Temp : BlendState
    {

    }
}
