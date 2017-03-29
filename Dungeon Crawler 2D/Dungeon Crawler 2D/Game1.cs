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
            room = new Room(exTex, playerTex, player, r);
<<<<<<< HEAD
=======

            
>>>>>>> origin/master

            Viewport view = GraphicsDevice.Viewport;
            cam = new Camera2D(view, room.tileList);
        }
        protected override void Update(GameTime gameTime)
        {
<<<<<<< HEAD
            //playerCharacter.Update(gameTime);

            cam.SetPosition(room.playerChar.playerPos);
            room.Update(gameTime);
=======

            room.Update(gameTime);
            
            cam.position = new Vector2(MathHelper.Lerp(cam.position.X, playerPos.X, 0),
            MathHelper.Lerp(cam.position.Y, playerPos.Y, 0));

>>>>>>> origin/master
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.GetTransform());

<<<<<<< HEAD
=======
            spriteBatch.End();


            spriteBatch.Begin();

>>>>>>> origin/master
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
