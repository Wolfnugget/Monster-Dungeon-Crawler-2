using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Dungeon_Crawler_2D.Menus
{
    public enum GameState
    {
        Explore,
        Battle
    }
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

        GameState gameState;






        public override void LoadContent(ContentManager Content, InputManager inputManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(Content, inputManager, graphicsDevice);

            spriteBatch = new SpriteBatch(graphicsDevice);

            GameSettings.SetDefault();

            textures = new TextureManager(Content);

            world = new World.GameWorld(textures, Content);

            world.Event += HandleEvents;
            player = new Object.Player(textures.playerSpriteSheet, textures, world.GetPlayerStart(), 100, new Point(16, 16), new Point(2, 0), 0.1f);
            player.Action += HandleEvents;

            Viewport view = graphicsDevice.Viewport;
            float zoom = 6f;

            hud = new HUDManager(gameState, textures, graphicsDevice, Content, player, GameSettings.windowWidth, GameSettings.windowHeight);
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

            if (gameState == GameState.Explore)
            {
                player.Update(gameTime);
                world.Update(gameTime, player.position);
                hud.Update(gameState);
                cam.SetPosition(player.position);
            }
            else if (gameState == GameState.Battle)
            {
                hud.Update(gameState);
                combat.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            if (gameState == GameState.Explore)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara beroende av kameran (allt utom tex healthbars eller poäng)
                world.Draw(spriteBatch);
                player.Draw(spriteBatch);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara oberoende av kameran (tex healthbars eller poäng)
                hud.DrawExplore(spriteBatch);
                if (hud.showStats == true)
                {
                    hud.statScreen.Draw(spriteBatch);
                }
                if (hud.showSummary == true)
                {
                    hud.DrawCombatSummary(spriteBatch);
                }
                spriteBatch.End();
            }

            else if (gameState == GameState.Battle)
            {

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam.GetTransform());

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara beroende av kameran (allt utom tex healthbars eller poäng)

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                //OBS!! Skriv bara här om ni vill att det som ritas ut ska vara oberoende av kameran (tex healthbars eller poäng)
                combat.Draw(spriteBatch, gameTime);

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
            }
            else if (args.EventType == MapEventType.StartCombat)
            {
                gameState = GameState.Battle;
                combat.StartCombat(args.enemy);
            }
            else if (args.EventType == MapEventType.PotionPickup)
            {
                player.UsePotion(args.potionType);
            }
        }

        private void HandleCombat(BattleEvensArgs args)
        {
            hud.showSummary = true;
            if (hud.battleWon == true)
            {
                gameState = GameState.Explore;
            }


            if (args.enemyType == EnemyType.boss)
            {
                world.WorldAction(World.WorldTrigger.BossDied, player.position);
            }
        }
    }
}
