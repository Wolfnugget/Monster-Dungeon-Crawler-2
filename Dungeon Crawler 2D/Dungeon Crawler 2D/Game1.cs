using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Dungeon_Crawler_2D
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random rand;

        World.Room room;
        //MapSystem.Room room;
        PlayerCharacter player;
        Camera2D cam;
        TextureManager textures;

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
            
            textures = new TextureManager(Content);
            rand = new Random();
            int roomNr = rand.Next(1, 4);
            //room = new MapSystem.Room(exTex, playerTex, "Maps/" + "StartRoom/" + roomNr + ".txt",0);
            room = new World.Room("Maps/StartRoom/" + 5 + ".txt", new Point(-1, 0), textures);
            player = new PlayerCharacter(textures.player, room.PlayerStart, 5, 0, 0);

            Viewport view = GraphicsDevice.Viewport;
            float zoom = 8f;
            windowWidth = graphics.PreferredBackBufferWidth = 1200;
            windowHeight = graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            cam = new Camera2D(view, windowWidth, windowHeight, room, zoom);
        }
        protected override void Update(GameTime gameTime)
        {
            //room.Update(gameTime);
            player.Update(gameTime);
            base.Update(gameTime);
            cam.SetPosition(player.playerPos);
            Console.WriteLine(player.playerPos);
            Console.WriteLine(cam.transform);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.GetTransformation(GraphicsDevice));
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

            room.Draw(spriteBatch);
            player.Draw(spriteBatch);
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
