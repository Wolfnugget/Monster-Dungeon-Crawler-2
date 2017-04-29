using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.World
{
    public class GeneratedDungeon: Area
    {
        byte[,] tileNumBP;

        List<int> roomRegions, mazeRegions;

        int totalRegions { get { return roomRegions.Count + mazeRegions.Count; } }

        public GeneratedDungeon(Point dimensions, TextureManager textures)
            : base(textures)
        {
            if (dimensions.X % 2 == 0)
            {
                dimensions.X++;
            }
            if (dimensions.Y % 2 == 0)
            {
                dimensions.Y++;
            }

            tiles = new Tile[dimensions.Y, dimensions.X];

            tileNumBP = new byte[dimensions.Y, dimensions.X];
            GenerateDungeon();
        }

        private void GenerateDungeon()
        {
            Point maxRoomDimensions = new Point(12, 12), minRoomDimensions = new Point(8, 8);
            roomRegions = new List<int>();
            mazeRegions = new List<int>();

            GenerateNumericalRooms(maxRoomDimensions, minRoomDimensions);

            FillEmptySpace();
            ConnectRegions();
            RemoveDeadEnds();
            //DebugNumericalTileArray();
            ConvertBPToTiles();
        }

        private void GenerateNumericalRooms(Point maxRoomDimensions, Point minRoomDimensions)
        {
            List<Rectangle> roomsToMake = new List<Rectangle>();

            int failedTries, maxFailedTries;
            Random rand = new Random();

            failedTries = 0;
            maxFailedTries = 100;

            int x, y, height, width, maxRooms;

            maxRooms = 100;
            Rectangle room;

            bool fail;

            while (failedTries < maxFailedTries && roomsToMake.Count < maxRooms)
            {
                width = (int)Math.Round((double)rand.Next(minRoomDimensions.X, maxRoomDimensions.X) / 2) * 2 + 1;
                height = (int)Math.Round((double)rand.Next(minRoomDimensions.Y, maxRoomDimensions.Y) / 2) * 2 + 1;
                x = (int)Math.Round((double)rand.Next(0, tiles.GetLength(1) - 1 - width) / 2) * 2;
                y = (int)Math.Round((double)rand.Next(0, tiles.GetLength(0) - 1 - height) / 2) * 2;

                fail = false;

                room = new Rectangle(x, y, width, height);

                for (int i = 0; i < roomsToMake.Count; i++)
                {
                    if (room.Intersects(roomsToMake[i]))
                    {
                        fail = true;
                        failedTries++;
                        break;
                    }
                }
                if (!fail)
                {
                    roomsToMake.Add(room);
                    failedTries = 0;
                }
            }

            for (int i = 1; i <= roomsToMake.Count; i++)
            {
                GenerateNumericalRoom(roomsToMake[i - 1], i);
                roomRegions.Add(i);
            }
        }

        private void GenerateNumericalRoom(Rectangle room, int region)
        {
            for (int x = room.X + 1; x < (room.X + room.Width) - 1; x++)
            {
                for (int y = room.Y + 1; y < (room.Y + room.Height) - 1; y++)
                {
                    tileNumBP[y, x] = (byte)region;
                }
            }
        }

        private void FillEmptySpace()
        {
            int mazeRegion = roomRegions.Last() + 1;
            for (int y = 1; y < tiles.GetLength(0) - 1; y += 2)
                for (int x = 1; x < tiles.GetLength(1) - 1; x += 2)
                {
                    if (tileNumBP[y, x] == 0 &&
                        !SampleAdjacentNumericalTiles(new Point(x, y)))
                    {;
                        if (GenerateMaze(new Point(x, y), (byte)mazeRegion))
                        {
                            mazeRegions.Add(mazeRegion);
                            mazeRegion++;
                        }
                    }
                }
        }

        private bool GenerateMaze(Point start, byte region)
        {
            List<Point> floodList = new List<Point>();
            floodList.Clear();
            tileNumBP[start.Y, start.X] = region;
            floodList.Add(start);

            Random rand = new Random();

            HashSet<int> exludePaths = new HashSet<int>();

            int dir, index, connections;
            bool tileAdded;

            connections = 0;

            while (floodList.Count > 0)
            {
                exludePaths.Clear();
                tileAdded = false;
                index = floodList.Count - 1;
                while (exludePaths.Count < 4 && !tileAdded)
                {
                    dir = GeneratorUtility.GetRandomNumberExcluding(rand, exludePaths, 0, 3);
                    if (dir == 0)
                    {
                        if (floodList[index].Y > 1 && floodList[index].X
                            > 0 && floodList[index].X < tiles.GetLength(1) - 1)
                        {
                            if (SampleDirectionReturnFalseIfNotSameNumber(floodList[index], new Point(0, -1), 0, 2))
                            {
                                tileNumBP[floodList[index].Y - 1, floodList[index].X] = region;
                                floodList.Add(floodList[index] + new Point(0, -1));
                                tileAdded = true;
                                connections++;
                            }
                        }
                    }
                    else if (dir == 1)
                    {
                        if (floodList[index].Y > 0 && floodList[index].Y
                            < tiles.GetLength(0) - 1 && floodList[index].X < tiles.GetLength(1) - 2)
                        {
                            if (SampleDirectionReturnFalseIfNotSameNumber(floodList[index], new Point(1, 0), 0, 2))
                            {
                                tileNumBP[floodList[index].Y, floodList[index].X + 1] = region;
                                floodList.Add(floodList[index] + new Point(1, 0));
                                tileAdded = true;
                                connections++;
                            }
                        }
                    }
                    else if (dir == 2)
                    {
                        if (floodList[index].X > 0 && floodList[index].Y
                            < tiles.GetLength(0) - 2 && floodList[index].X < tiles.GetLength(1) - 1)
                        {
                            if (SampleDirectionReturnFalseIfNotSameNumber(floodList[index], new Point(0, 1), 0, 2))
                            {
                                tileNumBP[floodList[index].Y + 1, floodList[index].X] = region;
                                floodList.Add(floodList[index] + new Point(0, 1));
                                tileAdded = true;
                                connections++;
                            }
                        }
                    }
                    else if (dir == 3)
                    {
                        if (floodList[index].X > 1 && floodList[index].Y
                            < tiles.GetLength(0) - 1 && floodList[index].Y > 0)
                        {
                            if (SampleDirectionReturnFalseIfNotSameNumber(floodList[index], new Point(-1, 0), 0, 2))
                            {
                                tileNumBP[floodList[index].Y, floodList[index].X - 1] = region;
                                floodList.Add(floodList[index] + new Point(-1, 0));
                                tileAdded = true;
                                connections++;
                            }
                        }
                    }
                    exludePaths.Add(dir);
                }
                if (!tileAdded)
                {
                    floodList.RemoveAt(index);
                }
            }

            if (connections == 0)
            {
                tileNumBP[start.Y, start.X] = 0;
                return false;
            }
            return true;
        }

        private bool SampleDirectionReturnFalseIfNotSameNumber(Point tile, Point direction, byte testFor, int steps)
        {
            if (direction.X == 0)
            {
                int step = direction.Y * steps;
                while (step != 0)
                {
                    for (int t = -1; t < 2; t++)
                    {
                        if (tileNumBP[tile.Y + step, tile.X + t] != testFor)
                        {
                            return false;
                        }
                    }
                    step -= direction.Y;
                }
            }
            else
            {
                int step = direction.X * steps;
                while (step != 0)
                {
                    for (int t = -1; t < 2; t++)
                    {
                        if (tileNumBP[tile.Y + t, tile.X + step] != testFor)
                        {
                            return false;
                        }
                    }
                    step -= direction.X;
                }
            }
            return true;
        }

        private bool SampleAdjacentNumericalTiles(Point tile)
        {
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                {
                    if (tileNumBP[tile.Y + y, tile.X + x] != 0)
                    {
                        return true;
                    }
                }

            return false;
        }

        private void ConnectRegions()
        {
            byte r1, r2;

            List<Connector> connections = new List<Connector>();

            for (int y = 1; y < tileNumBP.GetLength(0) - 1; y++)
            {
                for (int x = 1; x < tileNumBP.GetLength(1) - 1; x++)
                {
                    if (tileNumBP[y, x] == 0 && ConnectionCheck(x, y,out r1,out r2))
                    {
                        connections.Add(new Connector(x, y, r1, r2));
                    }
                }
            }

            List<byte> connectedRegions = new List<byte>();

            Random rand = new Random();
            byte connectingTo = 1;
            connectedRegions.Add(1);
            List<int> possibleConnections = new List<int>();

            int index;

            HashSet<int> exlude = new HashSet<int>();

            while (connectedRegions.Count < totalRegions)
            {
                possibleConnections.Clear();
                for (int i = 0; i < connections.Count; i++)
                {
                    if ((connections[i].Region1 == connectingTo || connections[i].Region2 == connectingTo) &&
                        (connectedRegions.Contains(connections[i].Region1) != connectedRegions.Contains(connections[i].Region2)))
                    {
                        possibleConnections.Add(i);
                    }
                }
                if (possibleConnections.Count > 0)
                {
                    index = rand.Next(0, possibleConnections.Count - 1);
                    tileNumBP[connections[possibleConnections[index]].Y, connections[possibleConnections[index]].X] = (byte)(totalRegions + 1);
                    if (connectingTo == connections[possibleConnections[index]].Region1)
                    {
                        connectedRegions.Add(connections[possibleConnections[index]].Region2);
                    }
                    else
                    {
                        connectedRegions.Add(connections[possibleConnections[index]].Region1);
                    }
                    connections.RemoveAt(possibleConnections[index]);
                }
                else
                {
                    for (int i = 0; i < connectedRegions.Count; i++)
                    {
                        if (connectedRegions[i] == connectingTo)
                        {
                            exlude.Add(i);
                            break;
                        }
                    }
                }

                index = GeneratorUtility.GetRandomNumberExcluding(rand, exlude, 0, connectedRegions.Count - 1);

                connectingTo = connectedRegions[index];
            }

            int extraConnectorPercentage = 3;
            for (int i = 0; i < connections.Count; i++)
            {
                if (rand.Next(0, 100) < extraConnectorPercentage)
                {
                    tileNumBP[connections[i].Y, connections[i].X] = (byte)(totalRegions + 1);
                }
            }

            //Fortsätt här
        }

        private bool ConnectionCheck(int x, int y, out byte r1, out byte r2)
        {
            if (tileNumBP[y - 1, x] != tileNumBP[y + 1, x] &&
                tileNumBP[y - 1, x] != 0 &&
                tileNumBP[y + 1, x] != 0)
            {
                r1 = tileNumBP[y - 1, x];
                r2 = tileNumBP[y + 1, x];
                return true;
            }
            else if (tileNumBP[y, x - 1] != tileNumBP[y, x + 1] &&
                tileNumBP[y, x - 1] != 0 &&
                tileNumBP[y, x + 1] != 0)
            {
                r1 = tileNumBP[y, x - 1];
                r2 = tileNumBP[y, x + 1];
                return true;
            }

            r1 = 0;
            r2 = 0;
            return false;
        }

        private struct Connector
        {
            public int X, Y;
            public byte Region1, Region2;

            public Connector(int X, int Y, byte Region1, byte Region2)
            {
                this.X = X;
                this.Y = Y;
                this.Region1 = Region1;
                this.Region2 = Region2;
            }
        }

        private void RemoveDeadEnds()
        {
            bool foundDeadEnd = true;

            while (foundDeadEnd)
            {
                foundDeadEnd = false;
                for (int y = 1; y < tileNumBP.GetLength(0) - 1; y++)
                    for (int x = 1; x < tileNumBP.GetLength(1) - 1; x++)
                    {
                        if (tileNumBP[y, x] != 0 && numberOfAdjacentWalls(x, y) > 2)
                        {
                            tileNumBP[y, x] = 0;
                            foundDeadEnd = true;
                        }
                    }
            }
        }

        private int numberOfAdjacentWalls(int x, int y)
        {
            int count = 0;

            if (tileNumBP[y - 1, x] == 0)
            {
                count++;
            }
            if (tileNumBP[y + 1, x] == 0)
            {
                count++;
            }
            if (tileNumBP[y, x - 1] == 0)
            {
                count++;
            }
            if (tileNumBP[y, x + 1] == 0)
            {
                count++;
            }

            return count;
        }

        private void ConvertBPToTiles()
        {
            Random rand = new Random();
            List<int> monsterRegions = new List<int>();
            List<Point> possibleStartTiles = new List<Point>();

            int minMonsterRooms = roomRegions.Count / 4;
            int maxMonsterRooms = (int)(roomRegions.Count / 1.5f);

            monsterRegions = GeneratorUtility.GetRandomListofIntFromList(rand, roomRegions, minMonsterRooms, maxMonsterRooms);

            bool pasable;
            TileType tileType;

            for (int y = 0; y < tileNumBP.GetLength(0); y++)
                for (int x = 0; x < tileNumBP.GetLength(1); x++)
                {
                    tileType = TilePicker(x, y, out pasable);
                    if (pasable)
                    {
                        if (tileNumBP[y, x] == 1)
                        {
                            possibleStartTiles.Add(new Point(x, y));
                        }
                        else if (monsterRegions.Contains(tileNumBP[y, x]))
                        {
                            tileType = TileType.MonsterTile;
                        }
                    }
                    tiles[y, x] = new Tile(tileType, pasable);
                }

            Point startTile = possibleStartTiles[rand.Next(0, possibleStartTiles.Count - 1)];
            playerStart = GetTileCenter(startTile.X, startTile.Y);

            DebugNumericalTileArrayContainedInList(monsterRegions);
        }

        private TileType TilePicker(int x, int y, out bool pasable)
        {
            if (tileNumBP[y, x] == 0)
            {
                if (y == 0)
                {
                    if (x == 0)
                    {
                        pasable = false;
                        return TileType.TopLeftCorner;
                    }
                    else if (x == tileNumBP.GetLength(1) - 1)
                    {
                        pasable = false;
                        return TileType.BottomLeftCorner;
                    }
                    else
                    {
                        pasable = false;
                        return TileType.VerticalWall;
                    }
                }
                else if (y == tileNumBP.GetLength(0) - 1)
                {
                    if (x == 0)
                    {
                        pasable = false;
                        return TileType.TopRightCorner;
                    }
                    else if (x == tileNumBP.GetLength(1) - 1)
                    {
                        pasable = false;
                        return TileType.BottomRightCorner;
                    }
                    else
                    {
                        pasable = false;
                        return TileType.VerticalWall;
                    }
                }
                else if (x == 0 || x == tileNumBP.GetLength(1) - 1)
                {
                    pasable = false;
                    return TileType.HorizontalWall;
                }
                else
                {
                    if (tileNumBP[y - 1, x] == 0 && tileNumBP[y + 1, x] == 0)
                    {
                        pasable = false;
                        return TileType.VerticalWall;
                    }
                    else if (tileNumBP[y, x - 1] == 0 && tileNumBP[y, x + 1] == 0)
                    {
                        pasable = false;
                        return TileType.HorizontalWall;
                    }
                    else if (tileNumBP[y, x + 1] == 0 && tileNumBP[y + 1, x] == 0)
                    {
                        pasable = false;
                        return TileType.TopLeftCorner;
                    }
                    else if (tileNumBP[y, x - 1] == 0 && tileNumBP[y + 1, x] == 0)
                    {
                        pasable = false;
                        return TileType.TopRightCorner;
                    }
                    else if (tileNumBP[y - 1, x] == 0 && tileNumBP[y, x + 1] == 0)
                    {
                        pasable = false;
                        return TileType.BottomLeftCorner;
                    }
                    else if (tileNumBP[y - 1, x] == 0 && tileNumBP[y, x - 1] == 0)
                    {
                        pasable = false;
                        return TileType.BottomRightCorner;
                    }
                    else
                    {
                        pasable = false;
                        return TileType.Wall;
                    }
                }

            }
            else
            {
                pasable = true;
                return TileType.basic;
            }
        }

        #region Debug
        private void DebugNumericalTileArray()
        {

            for (int y = 0; y < tileNumBP.GetLength(0); y++)
            {
                for (int x = 0; x < tileNumBP.GetLength(1); x++)
                {
                    if (tileNumBP[y, x] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0");
                    }
                    else if (tileNumBP[y, x] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("1");
                    }
                    else if (roomRegions.Contains(tileNumBP[y, x]))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("1");
                    }
                    else if (mazeRegions.Contains(tileNumBP[y, x]))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("1");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("1");
                    }
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void DebugNumericalTileArrayContainedInList(List<int> list)
        {

            for (int y = 0; y < tileNumBP.GetLength(0); y++)
            {
                for (int x = 0; x < tileNumBP.GetLength(1); x++)
                {
                    if (tileNumBP[y, x] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0");
                    }
                    else if (list.Contains(tileNumBP[y, x]))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("1");
                    }
                    else if (roomRegions.Contains(tileNumBP[y, x]))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("1");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("1");
                    }
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion
    }
}
