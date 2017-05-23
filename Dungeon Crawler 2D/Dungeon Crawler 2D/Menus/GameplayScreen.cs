using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Dungeon_Crawler_2D.Menus
{
    public class GameplayScreen : GameScreen
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private World.Map world;
        private Object.Player player;

        private Camera2D cam;
        private TextureManager textures;
        private HUDManager hud;
        private Combat combat;

        public override void LoadContent(ContentManager Content, InputManager inputManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(Content, inputManager, graphicsDevice);

            spriteBatch = new SpriteBatch(graphicsDevice);

            GameSettings.SetDefault();

            GameSettings.gameState = GameState.Explore;

            textures = new TextureManager(Content);

            world = new World.GameWorld(textures, Content);

            world.Event += HandleEvents;
            player = new Object.Player(textures.playerSpriteSheet, textures, world.GetPlayerStart(), 100, new Point(16, 16), new Point(2, 0), 0.1f);
            player.Action += HandleEvents;

            Viewport view = graphicsDevice.Viewport;
            float zoom = 5f;

            hud = new HUDManager(textures, graphicsDevice, Content, player, GameSettings.windowWidth, GameSettings.windowHeight);
            cam = new Camera2D(hud, view, GameSettings.windowWidth, GameSettings.windowHeight, world, zoom);

            combat = new Combat(player, textures, hud);
            combat.Event += HandleEvents;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(GameSettings.Restart))
            {
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
                Console.WriteLine("P funkar");
            }

            if (GameSettings.gameState == GameState.Explore)
            {
                player.Update(gameTime);
                world.Update(gameTime, player.position);
                hud.Update();
                hud.statScreen.Update();
                cam.SetPosition(player.position);

                
            }
            else if (GameSettings.gameState == GameState.Battle)
            {
                hud.Update();
                combat.Update(gameTime);
            }
            else if (GameSettings.gameState == GameState.Inventory)
            {
                hud.Update();
                hud.statScreen.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (GameSettings.gameState == GameState.Explore)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara beroende av kameran (allt utom tex healthbars eller poäng)
                world.Draw(spriteBatch);
                player.Draw(spriteBatch);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara oberoende av kameran (tex healthbars eller poäng)
                hud.DrawExplore(spriteBatch);
                
                spriteBatch.End();
            }

            else if (GameSettings.gameState == GameState.Battle)
            {

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara beroende av kameran (allt utom tex healthbars eller poäng)

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara oberoende av kameran (tex healthbars eller poäng)
                combat.Draw(spriteBatch, gameTime);

                spriteBatch.End();
            }

            else if (GameSettings.gameState == GameState.Inventory)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara beroende av kameran (allt utom tex healthbars eller poäng)
                world.Draw(spriteBatch);
                player.Draw(spriteBatch);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                hud.DrawExplore(spriteBatch);
                hud.statScreen.Draw(spriteBatch);

                if (hud.statScreen.showSummary == true)
                {
                    hud.statScreen.DrawCombatSummary(spriteBatch);
                }

                spriteBatch.End();
            }
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
            else if (Object is Combat)
            {
                HandleCombat((BattleEvensArgs)args);
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
                world.PlayerEvent(args);
            }
            else if (args.EventType == PlayerEventType.EnterTile) //spelaren är framme på en tile.
            {
                world.PlayerEvent(args);
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
                player.RestoreHpAndMana();
            }
            else if (args.EventType == MapEventType.StartCombat)
            {
                GameSettings.gameState = GameState.Battle;
                combat.StartCombat(args.enemy);
            }
            else if (args.EventType == MapEventType.PotionPickup)
            {
                player.UsePotion(args.potionType);
            }
        }

        private void HandleCombat(BattleEvensArgs args)
        {
            hud.statScreen.showSummary = true;
            if (args.result == EndCombat.Won)
            {
                GameSettings.gameState = GameState.Inventory;
            }
            else if (args.result == EndCombat.Lost)
            {
                hud.statScreen.showSummary = true;
                GameSettings.gameState = GameState.Inventory;
            }

            if (args.enemyType == EnemyType.boss)
            {
                world.WorldAction(World.WorldTrigger.BossDied, player.position);
            }
        }

    //private void debug

    }
}
