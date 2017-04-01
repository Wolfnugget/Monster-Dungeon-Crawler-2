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
        Wall,
        basic,
        NorthExit,
        SouthExit,
        WestExit,
        EastExit
    }

    class Map
    {
        List<Room> rooms;
        Point currentRoom;

        Random rand;

        public Map(TextureManager textures, int minNumberOfRooms, int maxNumberOfRoomsOffset = 0)
        {
            rand = new Random();
            GenerateMap(textures, rand.Next(minNumberOfRooms, minNumberOfRooms + maxNumberOfRoomsOffset));
            currentRoom = new Point(0, 0);
        }

        void GenerateMap(TextureManager textures, int numberOfRooms)
        {
            rooms = new List<Room>();
            int roomsAdded = 0;

            rooms.Add(new Room(GetRandomRoomPath("Maps/StartRoom"), textures, new Point(0, 0)));
            roomsAdded++;
            int addingExitsTo = 0;
            HashSet<int> excludeRoom = new HashSet<int>();

            while (roomsAdded < numberOfRooms)
            {
                int exitsToAdd = 0;
                HashSet<int> excludeExit = new HashSet<int>();

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
                        rooms.Add(new Room(GetRandomRoomPath("Maps/North"), textures,
                            rooms[addingExitsTo].roomCoords + new Point(0, -1)));
                        excludeExit.Add(1);
                        roomsAdded++;
                        exitsToAdd--;
                    }
                    else if (r == 2)
                    {
                        rooms.Add(new Room(GetRandomRoomPath("Maps/South"), textures,
                            rooms[addingExitsTo].roomCoords + new Point(0, -1)));
                        excludeExit.Add(2);
                        roomsAdded++;
                        exitsToAdd--;
                    }
                    else if (r == 3)
                    {
                        rooms.Add(new Room(GetRandomRoomPath("Maps/East"), textures,
                            rooms[addingExitsTo].roomCoords + new Point(0, -1)));
                        excludeExit.Add(3);
                        roomsAdded++;
                        exitsToAdd--;
                    }
                    else if (r == 4)
                    {
                        rooms.Add(new Room(GetRandomRoomPath("Maps/West"), textures,
                            rooms[addingExitsTo].roomCoords + new Point(0, -1)));
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
            foreach (Room room in rooms)
            {
                if (room.roomCoords == currentRoom)
                {
                    room.Draw(spriteBatch);
                }
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
            var range = Enumerable.Range(min, max).Where(i => !exclude.Contains(i));

            Random random = new Random();
            int index = random.Next(min, max - exclude.Count);

            return range.ElementAt(index);
        }

        private string GetRandomRoomPath(string FolderPath)
        {
            List<string> roomPaths = new List<string>();
            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + FolderPath).Select(Path.GetFileName))
            {
                roomPaths.Add(file);
            }
            rand.Next(0, roomPaths.Count - 1);

            return roomPaths[rand.Next(0, roomPaths.Count - 1)];
        }


    }
}
