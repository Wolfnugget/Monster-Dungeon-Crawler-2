using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Dungeon_Crawler_2D
{
    public enum GameState
    {
        Explore,
        Battle,
        Menu
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private World.Map map;
        private Object.Player player;

        private Camera2D cam;
        private TextureManager textures;
        private BarManager bars;

        private int windowHeight;
        private int windowWidth;

        GameState gameState;

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
            
            map = new World.GeneratedMap(textures, 20, 2);
            map.Event += HandleEvents;
            player = new Object.Player(textures.player, map.GetPlayerStart(), 128, new Point(0, 0), new Point(16, 16), new Point(0, 0));
            player.Action += HandleEvents;

            bars = new BarManager(Content, player);

            Viewport view = GraphicsDevice.Viewport;
            float zoom = 5f;
            windowWidth = graphics.PreferredBackBufferWidth = 1200;
            windowHeight = graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            gameState = GameState.Explore; //Vilken gamestate spelet startas i.

            cam = new Camera2D(view, windowWidth, windowHeight, map, zoom);
        }
        protected override void Update(GameTime gameTime)
        {
            if (gameState == GameState.Explore)
            {
                player.Update(gameTime);
                cam.SetPosition(player.position);
            }
            else if (gameState == GameState.Battle)
            {

            }
            else if (gameState == GameState.Menu)
            {

            }
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.Black);

            if (gameState == GameState.Explore)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara beroende av kameran (allt utom tex healthbars eller poäng)
                map.Draw(spriteBatch);
                player.Draw(spriteBatch);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara oberoende av kameran (tex healthbars eller poäng)
                bars.Draw(spriteBatch);

                spriteBatch.End();
            }
            else if (gameState == GameState.Battle)
            {

            }
            else if (gameState == GameState.Battle)
            {

            }

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
