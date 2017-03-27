﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawler_2D
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D playerTex;
        Vector2 playerPos;
        Rectangle playerHitBox;

        PlayerCharacter playerCharacter;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.ApplyChanges();

            playerPos = new Vector2(50, 50);
            playerHitBox = new Rectangle((int)playerPos.X, (int)playerPos.Y, playerTex.Width, playerTex.Height);

            playerCharacter = new PlayerCharacter(playerTex, playerPos, playerHitBox, 5, 0, 0);
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTex = Content.Load<Texture2D>("Player");
        }
        protected override void Update(GameTime gameTime)
        {
            playerCharacter.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            playerCharacter.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
