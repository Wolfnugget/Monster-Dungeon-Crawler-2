using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.World
{
    class GeneratedDungeon: Area
    {
        byte[,] tileNumBP;

        public GeneratedDungeon(Point dimensions, TextureManager textures, int floor, TileType entry)
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


        }

        private void GenerateDungeonNumericalBlueprint()
        {
            
        }

        private void GenerateNumericalRooms(Point maxRoomDimensions, Point minRoomDimensions)
        {
            List<Rectangle> roomsToMake = new List<Rectangle>();

            int freeTiles, targetUsedTiles, failedTries, maxFailedTries;
            Random rand = new Random();

            freeTiles = tiles.GetLength(0) * tiles.GetLength(1);
            targetUsedTiles = (int)(freeTiles * 0.2f);
            failedTries = 0;
            maxFailedTries = 100;

            int x, y, height, width;
            Rectangle room = new Rectangle();

            bool fail;

            while (failedTries < maxFailedTries && freeTiles > targetUsedTiles)
            {
                width = (rand.Next(minRoomDimensions.X, maxRoomDimensions.X) / 2) * 2 + 1;
                height = (rand.Next(minRoomDimensions.Y, maxRoomDimensions.Y) / 2) * 2 + 1;
                x = (rand.Next(1, tiles.GetLength(1) - 1 - width) / 2) * 2 + 1;
                y = (rand.Next(1, tiles.GetLength(0) - 1 - height) / 2) * 2 + 1;

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
                    freeTiles -= room.Height * room.Width;
                    failedTries = 0;
                }
            }

            for (int i = 0; i < roomsToMake.Count; i++)
            {
                GenerateNumericalRoomBlueprint(roomsToMake[i], (byte)i);
            }
        }

        private void GenerateNumericalRoomBlueprint(Rectangle room, byte number)
        {
            for (int x = room.X; x < (room.X + room.Width); x++)
            {
                for (int y = room.X; y < (room.Y + room.Height); y++)
                {
                    tileNumBP[y, x] = number;
                }
            }
        }

        private void FillEmptySpace()
        {
            for (int y = 1; y < tiles.GetLength(0) - 1; y += 2)
                for (int x = 1; x < tiles.GetLength(1) - 1; x += 2)
                {
                    if (tileNumBP[y, x] == 0 &&
                        SampleAdjacentNumericalTiles(new Point(x, y)))
                    {

                    }
                }
        }

        private void GenerateMaze(Point start)
        {
            List<Point> floodList = new List<Point>();
            tiles[start.Y, start.X] = new Tile(TileType.basic, true);
            floodList.Add(start);

            HashSet<int> exludePaths = new HashSet<int>();

            int dir, index;
            bool tileAdded;

            while (floodList.Count > 0)
            {
                exludePaths.Clear();
                tileAdded = false;
                index = floodList.Count - 1;
                while (exludePaths.Count < 4 || tileAdded)
                {
                    dir = GeneratorUtility.GetRandomNumberExcluding(exludePaths, 0, 3);
                    if (dir == 0)
                    {
                        if (floodList[index].Y > 1 && floodList[index].X
                            > 1 && floodList[index].X < tiles.GetLength(1) - 2)
                        {
                            if (!TestDirection(floodList[index], new Point(0, -1)))
                            {
                                tiles[floodList[index].Y - 1, floodList[index].X] = new Tile(TileType.basic, true);
                                floodList.Add(floodList[index] + new Point(0, -1));
                                tileAdded = true;
                            }
                        }
                    }
                    else if (dir == 1)
                    {
                        if (floodList[index].Y > 1 && floodList[index].Y
                            < tiles.GetLength(0) - 2 && floodList[index].X < tiles.GetLength(1) - 2)
                        {
                            if (!TestDirection(floodList[index], new Point(1, 0)))
                            {
                                tiles[floodList[index].Y, floodList[index].X + 1] = new Tile(TileType.basic, true);
                                floodList.Add(floodList[index] + new Point(1, 0));
                                tileAdded = true;
                            }
                        }
                    }
                    else if (dir == 2)
                    {
                        if (floodList[index].X > 1 && floodList[index].X
                            < tiles.GetLength(1) - 2 && floodList[index].Y > 1)
                        {
                            if (!TestDirection(floodList[index], new Point(1, 0)))
                            {
                                tiles[floodList[index].Y + 1, floodList[index].X] = new Tile(TileType.basic, true);
                                floodList.Add(floodList[index] + new Point(0, 1));
                                tileAdded = true;
                            }
                        }
                    }
                    else if (dir == 3)
                    {
                        if (floodList[index].X > 1 && floodList[index].Y
                            < tiles.GetLength(0) - 2 && floodList[index].Y > 1)
                        {
                            if (!TestDirection(floodList[index], new Point(1, 0)))
                            {
                                tiles[floodList[index].Y, floodList[index].X - 1] = new Tile(TileType.basic, true);
                                floodList.Add(floodList[index] + new Point(-1, 0));
                                tileAdded = true;
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
        }

        private bool TestDirection(Point tile, Point direction)
        {
            if (direction.X == 0)
            {
                for (int t = -1; t < 2; t++)
                {
                    if (tileNumBP[tile.Y + direction.Y, tile.X + t] != 0)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int t = -1; t < 2; t++)
                {
                    if (tileNumBP[tile.Y + t, tile.X + direction.X] != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
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
    }
}
