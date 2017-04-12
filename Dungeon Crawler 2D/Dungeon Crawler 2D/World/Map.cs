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
    /// <summary>
    /// Tile typer, dörrar väggar osv.
    /// </summary>
    public enum TileType
    {
        None,
        Wall,
        basic,
        MonsterTile,
        NorthExit,
        SouthExit,
        WestExit,
        EastExit
    }

    public class Map
    {
        public List<Room> rooms;
        public int currentRoom;
        Random rand;
        TextureManager textures;

        public Map(TextureManager textures ,int minNumberOfRooms, int maxNumberOfRoomsOffset = 0)
        {
            this.textures = textures;
            rand = new Random();
            GenerateMap(rand.Next(minNumberOfRooms, minNumberOfRooms + maxNumberOfRoomsOffset), textures);
        }

        /// <summary>
        /// Genererar rum.
        /// </summary>
        /// <param name="numberOfRooms"></param>
        /// <param name="textures"></param>
        void GenerateMap(int numberOfRooms, TextureManager textures)
        {
            rooms = new List<Room>();
            int roomsAdded = 0;

            rooms.Add(new Room(GetRandomRoomPath("Maps/StartRoom"), new Point(0, 0), textures));
            roomsAdded++;

            HashSet<int> excludeRoom = new HashSet<int>(); //Rum vars dörrar har genererats.
            excludeRoom.Clear();
            HashSet<int> excludeExit = new HashSet<int>(); //dörrar som är klara(leder till ett rum).
            int addingExitsTo = 0; //rum som arbetas med.

            while (roomsAdded < numberOfRooms)
            {
                int exitsToAdd = 0;
                excludeExit.Clear();

                //kollar hur vilka dörrar som finns i rummet.
                if (rooms[addingExitsTo].northExit &&
                    !CheckIfRoomExists(rooms[addingExitsTo].roomCoords, new Point(0, -1)))
                {
                    exitsToAdd++;
                }
                else
                {
                    excludeExit.Add(1);
                }
                if (rooms[addingExitsTo].southExit &&
                    !CheckIfRoomExists(rooms[addingExitsTo].roomCoords, new Point(0, 1)))
                {
                    exitsToAdd++;
                }
                else
                {
                    excludeExit.Add(2);
                }
                if (rooms[addingExitsTo].eastExit &&
                    !CheckIfRoomExists(rooms[addingExitsTo].roomCoords, new Point(1, 0)))
                {
                    exitsToAdd++;
                }
                else
                {
                    excludeExit.Add(3);
                }
                if (rooms[addingExitsTo].westExit &&
                    !CheckIfRoomExists(rooms[addingExitsTo].roomCoords, new Point(-1, 0)))
                {
                    exitsToAdd++;
                }
                else
                {
                    excludeExit.Add(4);
                }

                while (exitsToAdd > 0 && roomsAdded < numberOfRooms) //genererar rum för varje dör, eller tills det inte ska genereras fler rum.
                {
                    int r = GetRandomNumberExcluding(excludeExit, 1, 4);
                    if (r == 1)
                    {
                        rooms.Add(new Room(GetRandomRoomPath("Maps/South"),
                            rooms[addingExitsTo].roomCoords + new Point(0, -1), textures));
                        excludeExit.Add(1);
                        roomsAdded++;
                        exitsToAdd--;
                    }
                    else if (r == 2)
                    {
                        rooms.Add(new Room(GetRandomRoomPath("Maps/North"),
                            rooms[addingExitsTo].roomCoords + new Point(0, 1), textures));
                        excludeExit.Add(2);
                        roomsAdded++;
                        exitsToAdd--;
                    }
                    else if (r == 3)
                    {
                        rooms.Add(new Room(GetRandomRoomPath("Maps/West"),
                            rooms[addingExitsTo].roomCoords + new Point(1, 0), textures));
                        excludeExit.Add(3);
                        roomsAdded++;
                        exitsToAdd--;
                    }
                    else if (r == 4)
                    {
                        rooms.Add(new Room(GetRandomRoomPath("Maps/East"),
                            rooms[addingExitsTo].roomCoords + new Point(-1, 0), textures));
                        excludeExit.Add(4);
                        roomsAdded++;
                        exitsToAdd--;
                    }
                }
                excludeRoom.Add(addingExitsTo); //lägger till rummet som har genererats så att den inte försöker generera till det rummet igen.
                addingExitsTo = GetRandomNumberExcluding(excludeRoom, 0, roomsAdded - 1); //tar ett nytt random rum som det sedan ska genereras nya rum till baserat på antal dörrar.
            }
            RemoveOneWayDoors();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            rooms[currentRoom].Draw(spriteBatch);
        }

        /// <summary>
        /// Tar bort dörrar som inte leder någonstans
        /// </summary>
        private void RemoveOneWayDoors()
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].northExit)
                {
                    bool foundEntance = false;
                    for (int y = 0; y < rooms.Count; y++)
                    {
                        if (rooms[y].roomCoords ==
                            (rooms[i].roomCoords + new Point(0, -1)))
                        {
                            foundEntance = true;
                            if (!rooms[y].southExit)
                            {
                                rooms[i].RemoveExit(TileType.NorthExit);
                                break;
                            }
                        }
                    }
                    if (!foundEntance)
                    {
                        rooms[i].RemoveExit(TileType.NorthExit);
                    }
                }
                if (rooms[i].eastExit)
                {
                    bool foundEntance = false;
                    for (int y = 0; y < rooms.Count; y++)
                    {
                        if (rooms[y].roomCoords ==
                            (rooms[i].roomCoords + new Point(1, 0)))
                        {
                            foundEntance = true;
                            if (!rooms[y].westExit)
                            {
                                rooms[i].RemoveExit(TileType.EastExit);
                                break;
                            }
                        }
                    }
                    if (!foundEntance)
                    {
                        rooms[i].RemoveExit(TileType.EastExit);
                    }
                }
                if (rooms[i].southExit)
                {
                    bool foundEntance = false;
                    for (int y = 0; y < rooms.Count; y++)
                    {
                        if (rooms[y].roomCoords ==
                            (rooms[i].roomCoords + new Point(0, 1)))
                        {
                            foundEntance = true;
                            if (!rooms[y].northExit)
                            {
                                rooms[i].RemoveExit(TileType.SouthExit);
                                break;
                            }
                        }
                    }
                    if (!foundEntance)
                    {
                        rooms[i].RemoveExit(TileType.SouthExit);
                    }
                }
                if (rooms[i].westExit)
                {
                    bool foundEntance = false;
                    for (int y = 0; y < rooms.Count; y++)
                    {
                        if (rooms[y].roomCoords ==
                            (rooms[i].roomCoords + new Point(-1, 0)))
                        {
                            foundEntance = true;
                            if (!rooms[y].eastExit)
                            {
                                rooms[i].RemoveExit(TileType.WestExit);
                                break;
                            }
                        }
                    }
                    if (!foundEntance)
                    {
                        rooms[i].RemoveExit(TileType.WestExit);
                    }
                }
            }
        }

        /// <summary>
        /// Kolla om ett rum finns i riktning från ett annat rum.
        /// </summary>
        /// <param name="checkingFrom"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private bool CheckIfRoomExists(Point checkingFrom, Point direction)
        {
            Point roomToCheck = checkingFrom + direction;

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].roomCoords == roomToCheck)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Genererar ett tal mellan min och max, utesluter talen i listan exclude.
        /// </summary>
        /// <param name="exclude"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int GetRandomNumberExcluding(HashSet<int> exclude, int min, int max)
        {
            HashSet<int> range = new HashSet<int>();

            for (int number = min; number <= max; number++)
            {
                if (!exclude.Contains(number))
                {
                    range.Add(number);
                }
            }

            Random random = new Random();
            int index = random.Next(0, range.Count());

            return range.ElementAt(index);
        }

        /// <summary>
        /// väljer en random fil i en folder från spelets bas folder.
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <returns></returns>
        private string GetRandomRoomPath(string FolderPath)
        {
            List<string> roomPaths = new List<string>();
            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + FolderPath).Select(Path.GetFileName))
            {
                roomPaths.Add(FolderPath + "/" + file);
            }
            rand.Next(0, roomPaths.Count);

            return roomPaths[rand.Next(0, roomPaths.Count - 1)];
        }

        /// <summary>
        /// hämtar spelaren start position.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPlayerStart()
        {
            return rooms[currentRoom].PlayerStart;
        }

        /// <summary>
        /// Kolla om det går att gå i en viss riktning och sedan kallar ett event om det går.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        private void CheckMovement(Vector2 position, Point direction)
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
        }

        /// <summary>
        /// Ändrar rum om spelaren går in i en dörr.
        /// </summary>
        /// <param name="RoomDirection"></param>
        /// <param name="entrance"></param>
        private void ChangeRoom(Point RoomDirection, TileType entrance)
        {
            Point newRoomCoords = rooms[currentRoom].roomCoords + RoomDirection;

            Console.WriteLine("change room from: " + rooms[currentRoom].roomCoords + " to: " + newRoomCoords);

            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].roomCoords == newRoomCoords)
                {
                    currentRoom = i;
                    MapEventArgs args = new MapEventArgs(MapEventType.ChangeRoom);
                    args.Position = rooms[currentRoom].GetTileCenterOfType(entrance);
                    OnEvent(args);
                    CheckMovement(args.Position, RoomDirection);
                    break;
                }
            }
        }

        public MapEventHandler Event;

        /// <summary>
        /// Kallar event.
        /// </summary>
        /// <param name="e"></param>
        public void OnEvent(MapEventArgs e)
        {
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
