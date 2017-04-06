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

        int windowHeight;
        int windowWidth;

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
            IsMouseVisible = true;
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
            float zoom = 10f;
            windowWidth = graphics.PreferredBackBufferWidth = 1200;
            windowHeight = graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            cam = new Camera2D(view, windowWidth, windowHeight, room.tileList, zoom);
        }
        protected override void Update(GameTime gameTime)
        {
            room.Update(gameTime);
            base.Update(gameTime);
            cam.SetPosition(room.playerChar.playerPos);
            Console.WriteLine(room.playerChar.playerPos);
            Console.WriteLine(cam.transform);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.GetTransformation(GraphicsDevice));
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

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
