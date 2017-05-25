using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Dungeon_Crawler_2D.World
{
    public enum TileType
    {
        None,
        Wall,
        basic,
        Stairs,
        Portal,
        ExitPortal,
        MonsterTile,
        NorthExit,
        SouthExit,
        WestExit,
        EastExit,
        Boss
    }

    public enum Location
    {
        Overworld,
        Dungeon
    }

    public enum WorldTrigger
    {
        BossDied
    }

    public abstract class Map
    {
        public Dictionary<Location, Area> rooms;
        public Location currentLocation;
        protected TextureManager textures;
        protected Random rand = new Random();
        protected ContentManager content;

        protected int randomEncounterChance;

        public Map(TextureManager textures, ContentManager content)
        {
            this.textures = textures;
            this.content = content;
            rooms = new Dictionary<Location, Area>();

            randomEncounterChance = 5;
        }

        public virtual void Update(GameTime gameTime, Vector2 cameraCenter)
        {
            rooms[currentLocation].Update(gameTime, cameraCenter);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            rooms[currentLocation].Draw(spriteBatch);
        }

        /// <summary>
        /// hämtar spelaren start position.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPlayerStart()
        {
            return rooms[currentLocation].playerStart;
        }

        /// <summary>
        /// Kolla om det går att gå i en viss riktning och sedan kallar ett event om det går.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        protected void CheckMovement(Vector2 position, Point direction)
        {
            Vector2 targetPosition = rooms[currentLocation].GetTargetTileCenter(position, direction);

            if (targetPosition != position)
            {
                MapEventArgs args = new MapEventArgs(MapEventType.Move);
                args.Position = targetPosition;
                OnEvent(args);
            }
        }

        /// <summary>
        /// kolla vad det är för typ av tile spelaren hamnat på och sedan gör något baserat på det.
        /// </summary>
        /// <param name="position"></param>
        private void TileCheck(Vector2 position)
        {
            Object.Object objOnPosition;

            int x, y;
            rooms[currentLocation].GetTileAtPosition(position, out x, out y); //hämtar tile typen från room.

            if (rooms[currentLocation].CheckIfTileContainsObject(x, y, out objOnPosition))
            {
                if (objOnPosition is Object.PickUp)
                {
                    Object.PickUp potion = (Object.PickUp)objOnPosition;
                    MapEventArgs args = new MapEventArgs(MapEventType.PotionPickup);
                    args.potionType = potion.potionType;
                    OnEvent(args);
                }
            }

            if (rooms[currentLocation].tiles[y, x].type == TileType.Portal)
            {
                ChangeArea(TileType.Portal);
            }
            else if (rooms[currentLocation].tiles[y, x].type == TileType.ExitPortal
                && rooms[currentLocation].ExitPortalOpen)
            {
                ChangeArea(TileType.Portal);
            }
            else if (rooms[currentLocation].tiles[y, x].type == TileType.MonsterTile)
            {
                if (rand.Next(0,100) < randomEncounterChance)
                {
                    MapEventArgs args = new MapEventArgs(MapEventType.StartCombat);
                    if (rand.Next(0,100)< 50) { args.enemy = EnemyType.zombie; }
                    else { args.enemy = EnemyType.warlock; }
                    OnEvent(args);
                }
            }
            else if (rooms[currentLocation].tiles[y, x].type == TileType.Boss &&
                rooms[currentLocation].CheckIfTileContainsObject(x, y, out objOnPosition))
            {
                MapEventArgs args = new MapEventArgs(MapEventType.StartCombat);
                args.enemy = EnemyType.boss;
                OnEvent(args);
            }
        }

        /// <summary>
        /// Ändrar rum om spelaren går in i en dörr.
        /// </summary>
        /// <param name="RoomDirection"></param>
        /// <param name="entrance"></param>
        protected abstract void ChangeArea(TileType entrance);

        public MapEventHandler Event;

        /// <summary>
        /// Kallar event.
        /// </summary>
        /// <param name="e"></param>
        public void OnEvent(MapEventArgs e)
        {
            //Om detta vissar fel så måste du uppdatera visual studio. Det är korrekt, ändra inte.
            Event.Invoke(this, e);
        }

        /// <summary>
        /// Om map tar emot ett event från spelaren, t.ex om spelaren
        /// försöker gå i en viss riktning eller hamnat på en ny tile.
        /// </summary>
        /// <param name="args"></param>
        public void PlayerEvent(ActorEventArgs args)
        {
            if (args.EventType == PlayerEventType.CheckDirection)
            {
                CheckMovement(args.Position, args.Direction);
            }
            else if (args.EventType == PlayerEventType.EnterTile)
            {
                TileCheck(args.Position);
            }
        }

        public void WorldAction(WorldTrigger trigger, Vector2 playerPosition)
        {
            int x, y;
            rooms[currentLocation].GetTileAtPosition(playerPosition, out x, out y);

            if (trigger == WorldTrigger.BossDied)
            {
                rooms[currentLocation].BossDies(x, y);
            }
        }
    }
}
