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

        void GenerateMap(int numberOfRooms, TextureManager textures)
        {
            rooms = new List<Room>();
            int roomsAdded = 0;

            rooms.Add(new Room(GetRandomRoomPath("Maps/StartRoom"), new Point(0, 0), textures));
            roomsAdded++;

            HashSet<int> excludeRoom = new HashSet<int>();
            excludeRoom.Clear();
            HashSet<int> excludeExit = new HashSet<int>();
            int addingExitsTo = 0;

            while (roomsAdded < numberOfRooms)
            {
                int exitsToAdd = 0;
                excludeExit.Clear();

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

                while (exitsToAdd > 0 && roomsAdded < numberOfRooms)
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
                excludeRoom.Add(addingExitsTo);
                addingExitsTo = GetRandomNumberExcluding(excludeRoom, 0, roomsAdded - 1);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            rooms[currentRoom].Draw(spriteBatch);
        }

        private void RemoveOneWayDoors()
        {
            for (int i = 0; i < rooms.Count;i++)
            {

            }
        }

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

        public Vector2 GetPlayerStart()
        {
            return rooms[currentRoom].PlayerStart;
        }

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

        private void TileCheck(Vector2 position)
        {
            TileType type = rooms[currentRoom].GetTileType(position);

            Console.WriteLine("Tile Check");

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

        public void OnEvent(MapEventArgs e)
        {
            Event?.Invoke(this, e);
        }

        public void PlayerEvent(PlayerEventArgs args)
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
