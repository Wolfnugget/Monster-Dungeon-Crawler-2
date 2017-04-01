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

        MapSystem.Room room;
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
            IsMouseVisible = true;
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
            int roomNr = rand.Next(1, 4);
            room = new MapSystem.Room(exTex, playerTex, "Maps/" + "StartRoom/" + roomNr + ".txt", 0);
            Viewport view = GraphicsDevice.Viewport;
            cam = new Camera2D(view, room.tileList, 2.5f);
        }
        protected override void Update(GameTime gameTime)
        {
            //playerCharacter.Update(gameTime);

            cam.SetPosition(room.playerChar.playerPos);
            room.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.GetTransformation(GraphicsDevice));
            room.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
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
