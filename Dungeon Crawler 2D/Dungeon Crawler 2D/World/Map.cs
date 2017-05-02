using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Dungeon_Crawler_2D.World
{
    public enum TileType
    {
        None,
        Wall,
        VerticalWall,
        HorizontalWall,
        basic,
        Stairs,
        Portal,
        ExitPortal,
        TopRightCorner,
        TopLeftCorner,
        BottomRightCorner,
        BottomLeftCorner,
        MonsterTile,
        NorthExit,
        SouthExit,
        WestExit,
        EastExit
    }

    public abstract class Map
    {
        public List<Area> rooms;
        public int currentRoom;
        protected TextureManager textures;
        protected Random rand = new Random();

        public Map(TextureManager textures)
        {
            this.textures = textures;
            rooms = new List<Area>();
        }

        public virtual void Update(GameTime gameTime, Vector2 cameraCenter)
        {
            rooms[currentRoom].Update(gameTime, cameraCenter);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            rooms[currentRoom].Draw(spriteBatch);
        }

        /// <summary>
        /// hämtar spelaren start position.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPlayerStart()
        {
            return rooms[currentRoom].playerStart;
        }

        /// <summary>
        /// Kolla om det går att gå i en viss riktning och sedan kallar ett event om det går.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        protected void CheckMovement(Vector2 position, Point direction)
        {
            Vector2 targetPosition = rooms[currentRoom].GetTargetTileCenter(position, direction);

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
            TileType type = rooms[currentRoom].GetTileType(position); //hämtar tile typen från room.

            if (type == TileType.NorthExit)
            {
                ChangeRoom(new Point(0, -1), TileType.SouthExit);
            }
            else if (type == TileType.SouthExit)
            {
                ChangeRoom(new Point(0, 1), TileType.NorthExit);
            }
            else if (type == TileType.EastExit)
            {
                ChangeRoom(new Point(1, 0), TileType.WestExit);
            }
            else if (type == TileType.WestExit)
            {
                ChangeRoom(new Point(-1, 0), TileType.EastExit);
            }
            else if (type == TileType.MonsterTile)
            {
                if (rand.Next(0,100) <= 10)
                {
                    MapEventArgs args = new MapEventArgs(MapEventType.StartCombat);
                    args.enemy = EnemyType.zombie;
                    OnEvent(args);
                }
            }
        }

        /// <summary>
        /// Ändrar rum om spelaren går in i en dörr.
        /// </summary>
        /// <param name="RoomDirection"></param>
        /// <param name="entrance"></param>
        protected abstract void ChangeRoom(Point RoomDirection, TileType entrance);

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

    }
}
