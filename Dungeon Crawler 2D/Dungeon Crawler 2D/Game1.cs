using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Dungeon_Crawler_2D
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D exTex;
        Texture2D playerTex;
        Vector2 playerPos;
        Rectangle playerHitBox;
        Random rand;

        Room room;
        PlayerCharacter player;
        Camera2D cam;

        //PlayerCharacter playerCharacter;

        //Object.Player player;

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

            cam = new Camera2D(5.0f, 0.0f, Vector2.Zero);
            //playerPos = new Vector2(50, 50);
            //playerHitBox = new Rectangle((int)playerPos.X, (int)playerPos.Y, playerTex.Width, playerTex.Height);

            //playerCharacter = new PlayerCharacter(playerTex, playerPos, playerHitBox, 5, 0, 0);


            //playerCharacter = new PlayerCharacter(playerTex, playerPos, playerHitBox, 5, 0, 0);

            //player = new Object.Player(playerTex, playerPos, 2, new Point(0, 0), new Point(16, 16), new Point(0, 0));


        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            playerTex = Content.Load<Texture2D>("Player");
            exTex = Content.Load<Texture2D>("Example");
            rand = new Random();
            int r = rand.Next(0, 3);
            player = new PlayerCharacter(playerTex, Vector2.Zero, 0, 0, 0, room);
            room = new Room(exTex, playerTex, player, r);
<<<<<<< HEAD
            player = new PlayerCharacter(playerTex, Vector2.Zero, 0, 0, 0, room);
=======

            
>>>>>>> origin/master

        }
        protected override void Update(GameTime gameTime)
        {
            //playerCharacter.Update(gameTime);
<<<<<<< HEAD

            player.Update(gameTime);


            cam.position = new Vector2(MathHelper.Lerp(cam.position.X, playerPos.X, 0),
            MathHelper.Lerp(cam.position.Y, playerPos.Y, 0));

=======
            room.Update(gameTime);
>>>>>>> origin/master
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                null, null, null, null, cam.GetTransformation(GraphicsDevice));

            spriteBatch.End();


            spriteBatch.Begin();
            room.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public bool CheckTile(Vector2 direction, out Vector2 destination)
        {

            destination = new Vector2(0, 0);
            return false;
        }

    }
}
