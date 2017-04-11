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

        World.Map map;
        Object.Player player;

        
        //MapSystem.Room room;
        //PlayerCharacter player;
        Camera2D cam;
        TextureManager textures;
        BarManager bars;

        int windowHeight;
        int windowWidth;

        //PlayerCharacter playerCharacter;

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
            //room = new World.Room("Maps/StartRoom/" + 5 + ".txt", new Point(-1, 0), textures);
            //player = new PlayerCharacter(textures.player, room.PlayerStart, 5, 0, 0);
            
            map = new World.Map(textures, 4, 2);
            map.Event += HandleEvents;
            player = new Object.Player(textures.player, map.GetPlayerStart(), 64, new Point(0, 0), new Point(16, 16), new Point(0, 0));
            player.Action += HandleEvents;

            bars = new BarManager(Content, player);

            Viewport view = GraphicsDevice.Viewport;
            float zoom = 5f;
            windowWidth = graphics.PreferredBackBufferWidth = 1200;
            windowHeight = graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            cam = new Camera2D(view, windowWidth, windowHeight, map, zoom);
        }
        protected override void Update(GameTime gameTime)
        {
            //room.Update(gameTime);
            
            player.Update(gameTime);
            base.Update(gameTime);
            
            //OBS!! Låt kameran alltid uppdateras sist
            cam.SetPosition(player.position);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

            //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara beroende av kameran (allt utom tex healthbars eller poäng)
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara oberoende av kameran (tex healthbars eller poäng)
            bars.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Tar hand om events.
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="args"></param>
        public void HandleEvents(object Object, EventArgs args)
        {
            if (Object is Object.Player)
            {
                HandlePlayer((ActorEventArgs)args);
            }
            else if (Object is World.Map)
            {
                HandleMap((MapEventArgs)args);
            }
        }

        /// <summary>
        /// Allt som skickas av player via events.
        /// </summary>
        /// <param name="args"></param>
        private void HandlePlayer(ActorEventArgs args)
        {
            if (args.EventType == PlayerEventType.CheckDirection) //spelaren försöker gå i en viss riktning.
            {
                map.PlayerEvent(args);
            }
            else if (args.EventType == PlayerEventType.EnterTile) //spelaren är framme på en tile.
            {
                map.PlayerEvent(args);
            }
        }

        /// <summary>
        /// allt som skickas från map via events.
        /// </summary>
        /// <param name="args"></param>
        private void HandleMap(MapEventArgs args)
        {
            if (args.EventType == MapEventType.Move) //Spelaren ska börja gå till nästa en viss tile.
            {
                player.SetDestination(args.Position);
            }
            else if (args.EventType == MapEventType.ChangeRoom)
            {
                player.SetPosition(args.Position);
            }
        }
    }
}
