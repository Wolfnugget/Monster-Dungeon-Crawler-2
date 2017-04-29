using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using Dungeon_Crawler_2D.Menus;

namespace Dungeon_Crawler_2D
{
    public enum GameState
    {
        Menu,
        Explore,
        Battle
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        private World.Map map;
        private Object.Player player;

        private Camera2D cam;
        private TextureManager textures;
        private HUDManager hud;

        private int windowHeight;
        private int windowWidth;

        World.GeneratedDungeon genTest;

        GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            ScreenManager.Instance.Initialize();
            ScreenManager.Instance.Dimensions = new Vector2(1200, 800);
            graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.Dimensions.X;
            graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.Dimensions.Y;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager.Instance.LoadContent(Content);

            textures = new TextureManager(Content);


            //map = new World.GeneratedMap(textures, 20, 4);
            map = new World.RandomGeneratedMap(textures);
            map.Event += HandleEvents;
            player = new Object.Player(textures.playerSpriteSheet, textures, map.GetPlayerStart(), 100, new Point(16, 16), new Point(2, 0), 0.3f);
            player.Action += HandleEvents;
            
            Viewport view = GraphicsDevice.Viewport;
            float zoom = 5f;
            windowWidth = graphics.PreferredBackBufferWidth = 1200;
            windowHeight = graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();

            gameState = GameState.Explore; //Vilken gamestate spelet startas i.

            hud = new HUDManager(gameState, textures, GraphicsDevice, Content, player, windowWidth, windowHeight);
            cam = new Camera2D(hud, view, windowWidth, windowHeight, map, zoom);
        }
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                gameState = GameState.Explore;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                gameState = GameState.Battle;
            }

            if (gameState == GameState.Menu)
            {
                ScreenManager.Instance.Update(gameTime);
            }
            else if (gameState == GameState.Explore)
            {
                player.Update(gameTime);
                map.Update(gameTime, player.position);
                cam.SetPosition(player.position);
            }
            else if (gameState == GameState.Battle)
            {

            }
            
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.Black);

            

            if (gameState == GameState.Menu)
            {
                spriteBatch.Begin();
                ScreenManager.Instance.Draw(spriteBatch);


                spriteBatch.End();
            }
            else if (gameState == GameState.Battle)
            {

            }
            else if (gameState == GameState.Explore)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara beroende av kameran (allt utom tex healthbars eller poäng)
                map.Draw(spriteBatch);
                player.Draw(spriteBatch);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara oberoende av kameran (tex healthbars eller poäng)
                hud.Draw(spriteBatch);
                if (player.showStats == true)
                {
                    player.statScreen.Draw(spriteBatch);
                }


                spriteBatch.End();
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
