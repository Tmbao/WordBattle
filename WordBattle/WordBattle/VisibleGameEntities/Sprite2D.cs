using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WordBattle.VisibleGameEntities
{
    class Sprite2D : VisibleGameEntity
    {
        private List<Texture2D> textures;

        public List<Texture2D> Textures
        {
            get { return textures; }
            private set { textures = value; }
        }

        public int NumberOfTextures
        {
            get { return textures.Count; }
        }

        int currentTexture;

        int width, height;

        public int Height
        {
            get { return height; }
            private set { height = value; }
        }

        public int Width
        {
            get { return width; }
            private set { width = value; }
        }

        public Sprite2D(int width, int height, List<Texture2D> textures)
        {
            // Copy parameters
            this.width = width;
            this.height = height;
            this.textures = textures;
            
            // Default values
            if (width == 0)
                this.width = textures[0].Width;
            if (height == 0)
                this.height = textures[0].Height;

            this.currentTexture = 0;
        }

        public override void Update(GameTime gameTime)
        {
            currentTexture = (currentTexture + 1) % textures.Count;

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float left, float top, float intensity, float scale)
        {
            spriteBatch.Draw(textures[currentTexture],
                    new Vector2(left, top), // position
                    new Rectangle(0, 0, width, height), // source rectangle
                    Color.White * intensity, // color
                    0, // rotation
                    Vector2.Zero, // origin
                    scale, // scale
                    SpriteEffects.None, // effects
                    1); // layer depth
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float left, float top, float intensity)
        {
            Draw(gameTime, spriteBatch, left, top, intensity, 1);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float left, float top)
        {
            Draw(gameTime, spriteBatch, left, top, 1, 1);
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime, spriteBatch, 0, 0, 1, 1);
        }
    }
}
