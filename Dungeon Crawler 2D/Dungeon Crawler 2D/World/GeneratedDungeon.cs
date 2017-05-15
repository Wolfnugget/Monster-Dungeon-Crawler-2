using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.World
{
    public class GeneratedDungeon: Area
    {
        byte[,] dungeonBP;

        List<byte> roomRegions, mazeRegions, monsterRegions, wallRegions;

        Dictionary<byte, int> roomLengthFromStart; 

        int bossRoom;

        int totalRegions { get { return roomRegions.Count + mazeRegions.Count; } }

        public GeneratedDungeon(Point dimensions, TextureManager textures, ContentManager content)
            : base(textures, content)
        {
            NewDungeon(dimensions, content, false);
        }

        public void NewDungeon(Point dimensions, ContentManager content, bool repickTileSet)
        {
            if (dimensions.X % 2 == 0)
            {
                dimensions.X++;
            }
            if (dimensions.Y % 2 == 0)
            {
                dimensions.Y++;
            }
            if (repickTileSet)
            {
                PickTileSet(content);
            }

            tiles = new Tile[dimensions.Y, dimensions.X];

            dungeonBP = new byte[dimensions.Y, dimensions.X];
            GenerateDungeon();
            ExitPortalOpen = false;
        }

        protected override void PickTileSet(ContentManager content)
        {
            List<TileSets> tileSetsList = new List<TileSets>();

            foreach (TileSets tS in Enum.GetValues(typeof(TileSets)))
            {
                if (tS != TileSets.Overworld)
                {
                    tileSetsList.Add(tS);
                }
            }

            int index = rand.Next(0, tileSetsList.Count);
            tileSet = new TileSet(content, tileSetsList[index]);
        }

        private void GenerateDungeon()
        {
            Point maxRoomDimensions = new Point(12, 12), minRoomDimensions = new Point(8, 8);
            roomRegions = new List<byte>();
            mazeRegions = new List<byte>();
            wallRegions = new List<byte>();
            wallRegions.Add(0);

            GenerateNumericalRooms(maxRoomDimensions, minRoomDimensions);

            FillEmptySpace();
            ConnectRegions();
            RemoveDeadEnds();
            ConvertBPToTiles();
            AddEnemies();
        }

        private void GenerateNumericalRooms(Point maxRoomDimensions, Point minRoomDimensions)
        {
            List<Rectangle> roomsToMake = new List<Rectangle>();

            int failedTries, maxFailedTries;

            failedTries = 0;
            maxFailedTries = 500;

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

            for (byte i = 1; i <= roomsToMake.Count; i++)
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
                    dungeonBP[y, x] = (byte)region;
                }
            }
        }

        private void FillEmptySpace()
        {
            int mazeRegion = roomRegions.Last() + 1;
            for (int y = 1; y < tiles.GetLength(0) - 1; y += 2)
                for (int x = 1; x < tiles.GetLength(1) - 1; x += 2)
                {
                    if (dungeonBP[y, x] == 0 &&
                        !SampleAdjacentNumericalTiles(new Point(x, y)))
                    {;
                        if (GenerateMaze(new Point(x, y), (byte)mazeRegion))
                        {
                            mazeRegions.Add((byte)mazeRegion);
                            mazeRegion++;
                        }
                    }
                }
        }

        private bool GenerateMaze(Point start, byte region)
        {
            List<Point> floodList = new List<Point>();
            floodList.Clear();
            dungeonBP[start.Y, start.X] = region;
            floodList.Add(start);

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
                                dungeonBP[floodList[index].Y - 1, floodList[index].X] = region;
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
                                dungeonBP[floodList[index].Y, floodList[index].X + 1] = region;
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
                                dungeonBP[floodList[index].Y + 1, floodList[index].X] = region;
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
                                dungeonBP[floodList[index].Y, floodList[index].X - 1] = region;
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
                dungeonBP[start.Y, start.X] = 0;
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
                        if (dungeonBP[tile.Y + step, tile.X + t] != testFor)
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
                        if (dungeonBP[tile.Y + t, tile.X + step] != testFor)
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
                    if (dungeonBP[tile.Y + y, tile.X + x] != 0)
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

            for (int y = 1; y < dungeonBP.GetLength(0) - 1; y++)
            {
                for (int x = 1; x < dungeonBP.GetLength(1) - 1; x++)
                {
                    if (dungeonBP[y, x] == 0 && ConnectionCheck(x, y,out r1,out r2))
                    {
                        connections.Add(new Connector(x, y, r1, r2));
                    }
                }
            }

            RemoveUnconnectableRegions(connections);

            List<byte> connectedRegions = new List<byte>();

            byte connectingTo = 1;
            connectedRegions.Add(1);
            List<int> possibleConnections = new List<int>();

            int index;

            HashSet<int> exlude = new HashSet<int>();

            while (connectedRegions.Count < totalRegions)
            {
                index = GeneratorUtility.GetRandomNumberExcluding(rand, exlude, 0, connectedRegions.Count - 1);
                connectingTo = connectedRegions[index];

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
                    dungeonBP[connections[possibleConnections[index]].Y, connections[possibleConnections[index]].X] = connections[possibleConnections[index]].Region1;
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
            }

            int extraConnectorPercentage = 3;
            for (int i = 0; i < connections.Count; i++)
            {
                if (rand.Next(0, 100) < extraConnectorPercentage)
                {
                    dungeonBP[connections[i].Y, connections[i].X] = connections[i].Region1;
                }
            }

            //Fortsätt här
        }

        private bool ConnectionCheck(int x, int y, out byte r1, out byte r2)
        {
            if (dungeonBP[y - 1, x] != dungeonBP[y + 1, x] &&
                dungeonBP[y - 1, x] != 0 &&
                dungeonBP[y + 1, x] != 0)
            {
                r1 = dungeonBP[y - 1, x];
                r2 = dungeonBP[y + 1, x];
                return true;
            }
            else if (dungeonBP[y, x - 1] != dungeonBP[y, x + 1] &&
                dungeonBP[y, x - 1] != 0 &&
                dungeonBP[y, x + 1] != 0)
            {
                r1 = dungeonBP[y, x - 1];
                r2 = dungeonBP[y, x + 1];
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

        private void RemoveUnconnectableRegions(List<Connector> connectors)
        {
            List<byte> regionsWithoutConnector = new List<byte>();

            regionsWithoutConnector.AddRange(mazeRegions);
            regionsWithoutConnector.AddRange(roomRegions);

            for (int i = 0; i < connectors.Count; i++)
            {
                if (regionsWithoutConnector.Contains(connectors[i].Region1))
                {
                    regionsWithoutConnector.Remove(connectors[i].Region1);
                }
                else if (regionsWithoutConnector.Contains(connectors[i].Region2))
                {
                    regionsWithoutConnector.Remove(connectors[i].Region2);
                }
            }

            for (int y = 1; y < dungeonBP.GetLength(0); y++)
                for (int x = 1; x < dungeonBP.GetLength(1); x++)
                {
                    if (regionsWithoutConnector.Contains(dungeonBP[y, x]))
                    {
                        dungeonBP[y, x] = 0;
                    }
                }

            for (int i = 0; i < regionsWithoutConnector.Count; i++)
            {
                if (mazeRegions.Contains(regionsWithoutConnector[i]))
                {
                    mazeRegions.Remove(regionsWithoutConnector[i]);
                }
                else if (roomRegions.Contains(regionsWithoutConnector[i]))
                {
                    roomRegions.Remove(regionsWithoutConnector[i]);
                }
            }
        }

        private void RemoveDeadEnds()
        {
            bool foundDeadEnd = true;

            while (foundDeadEnd)
            {
                foundDeadEnd = false;
                for (int y = 1; y < dungeonBP.GetLength(0) - 1; y++)
                    for (int x = 1; x < dungeonBP.GetLength(1) - 1; x++)
                    {
                        if (dungeonBP[y, x] != 0 && numberOfAdjacentWalls(x, y) > 2)
                        {
                            dungeonBP[y, x] = 0;
                            foundDeadEnd = true;
                        }
                    }
            }
        }

        private int numberOfAdjacentWalls(int x, int y)
        {
            int count = 0;

            if (dungeonBP[y - 1, x] == 0)
            {
                count++;
            }
            if (dungeonBP[y + 1, x] == 0)
            {
                count++;
            }
            if (dungeonBP[y, x - 1] == 0)
            {
                count++;
            }
            if (dungeonBP[y, x + 1] == 0)
            {
                count++;
            }

            return count;
        }

        private void ConvertBPToTiles()
        {
            int minMonsterRooms = roomRegions.Count / 4;
            int maxMonsterRooms = (int)(roomRegions.Count / 1.5f);

            List<byte> temp = roomRegions;

            temp.RemoveAt(0);

            monsterRegions = GeneratorUtility.GetRandomListofByteFromList
                (rand, temp, minMonsterRooms, maxMonsterRooms);

            TileTexture textureType;

            Point startTile = GetStartTile();
            playerStart = GetTileCenter(startTile.X, startTile.Y);

            bossRoom = FindRoomFurthestFromStar(startTile.X, startTile.Y);

            for (int y = 0; y < dungeonBP.GetLength(0); y++)
                for (int x = 0; x < dungeonBP.GetLength(1); x++)
                {
                    textureType = TilePicker(x, y);
                    if (dungeonBP[y, x] == 0)
                    {
                        tiles[y, x] = new Tile(TileType.Wall, tileSet.GetTexture(TilePicker(x, y), dungeonBP[y, x]), false);
                    }
                    else
                    {
                        if (monsterRegions.Contains(dungeonBP[y, x]))
                        {
                            tiles[y, x] = new Tile(TileType.MonsterTile, tileSet.GetTexture(TilePicker(x, y), dungeonBP[y, x]), true);
                        }
                        else
                        {
                            tiles[y, x] = new Tile(TileType.basic, tileSet.GetTexture(TilePicker(x, y), dungeonBP[y, x]), true);
                        }
                    }
                }

            tiles[startTile.Y, startTile.X].type = TileType.Portal;
            gameObjects.Add(new Point(startTile.X, startTile.Y), new Object.Portal(textures.portal, playerStart, true));
        }

        private TileTexture TilePicker(int x, int y)
        {
            if (dungeonBP[y, x] == 0)
            {
                if (y == 0)
                {
                    if (x == 0)
                    {
                        return TileTexture.Wall_NorthWest_Corner;
                    }
                    else if (x == dungeonBP.GetLength(1) - 1)
                    {
                        return TileTexture.Wall_SouthWest_Corner;
                    }
                    else
                    {
                        return TileTexture.Wall_Vertical;
                    }
                }
                else if (y == dungeonBP.GetLength(0) - 1)
                {
                    if (x == 0)
                    {
                        return TileTexture.Wall_NorthEast_Corner;
                    }
                    else if (x == dungeonBP.GetLength(1) - 1)
                    {
                        return TileTexture.Wall_SouthEast_Corner;
                    }
                    else
                    {
                        return TileTexture.Wall_Vertical;
                    }
                }
                else if (x == 0 || x == dungeonBP.GetLength(1) - 1)
                {
                    return TileTexture.Wall_Horizontal;
                }
                else
                {
                    if (wallRegions.Contains(dungeonBP[y - 1, x] ) && wallRegions.Contains(dungeonBP[y + 1, x]))
                    {
                        return TileTexture.Wall_Vertical;
                    }
                    else if (wallRegions.Contains(dungeonBP[y, x - 1]) && wallRegions.Contains(dungeonBP[y, x + 1]))
                    {
                        return TileTexture.Wall_Horizontal;
                    }
                    else if (wallRegions.Contains(dungeonBP[y, x + 1]) && wallRegions.Contains(dungeonBP[y + 1, x]))
                    {
                        return TileTexture.Wall_Corner_Top_Left;
                    }
                    else if (wallRegions.Contains(dungeonBP[y, x - 1]) && wallRegions.Contains(dungeonBP[y + 1, x]))
                    {
                        return TileTexture.Wall_Corner_Top_Right;
                    }
                    else if (wallRegions.Contains(dungeonBP[y - 1, x]) && wallRegions.Contains(dungeonBP[y, x + 1]))
                    {
                        return TileTexture.Wall_Corner_Bottom_Left;
                    }
                    else if (wallRegions.Contains(dungeonBP[y - 1, x])&& wallRegions.Contains(dungeonBP[y, x - 1]))
                    {
                        return TileTexture.Wall_Corner_Bottom_Right;
                    }
                    else
                    {
                        return TileTexture.Single_Corner;
                    }
                }

            }
            else
            {
                if (dungeonBP[y, x] == bossRoom)
                {
                    return TileTexture.Boss_Tile;
                }
                else if (monsterRegions.Contains(dungeonBP[y, x]))
                {
                    return TileTexture.Enemy_Tile;
                }
                return TileTexture.Floor_Tile;
            }
        }

        private Point GetStartTile()
        {
            List<Point> possibleStartTiles = new List<Point>();

            for (int y = 0; y < dungeonBP.GetLength(0); y++)
                for (int x = 0; x < dungeonBP.GetLength(1); x++)
                {
                    if (dungeonBP[y, x] == 1)
                        possibleStartTiles.Add(new Point(x, y));
                }

            return possibleStartTiles[rand.Next(0, possibleStartTiles.Count - 1)];
        }

        private int FindRoomFurthestFromStar(int startX, int startY)
        {
            int currentMatch = 0;
            int index, currentMatchLength = 0, processingLength;
            List<Point> tilesInRegion = new List<Point>();

            roomLengthFromStart = new Dictionary<byte, int>();

            for (int i = 1; i < roomRegions.Count; i++)
            {
                tilesInRegion.Clear();
                for (int y = 0; y < dungeonBP.GetLength(0); y++)
                    for (int x = 0; x < dungeonBP.GetLength(1); x++)
                    {
                        if (dungeonBP[y, x] == roomRegions[i])
                        {
                            tilesInRegion.Add(new Point(x, y));
                        }
                    }

                index = rand.Next(0, tilesInRegion.Count - 1);
                processingLength = FindShortestPath(startX, startY, tilesInRegion[index].X, tilesInRegion[index].Y);

                roomLengthFromStart.Add(roomRegions[i], processingLength);

                if (currentMatch == 0)
                {
                    currentMatch = roomRegions[i];
                    currentMatchLength = processingLength;
                }
                else if (processingLength > currentMatchLength)
                {
                    currentMatch = roomRegions[i];
                    currentMatchLength = processingLength;
                }
            }
            return currentMatch;
        }

        private int FindShortestPath(int startX, int startY, int destX, int destY)
        {
            Dictionary<Point, PathNode> visitedTiles = new Dictionary<Point, PathNode>();
            Queue<Point> processing = new Queue<Point>();
            processing.Enqueue(new Point(startX, startY));

            PathNode processingNode;

            visitedTiles.Add(new Point(startX, startY), new PathNode(new Point (startX, startY),new Point(startX, startY), 0));
            List<Point> possibleMoves = new List<Point>();

            while (processing.Count > 0)
            {
                processingNode = visitedTiles[processing.Dequeue()];
                if (processingNode.tile.X == destX && processingNode.tile.Y == destY)
                {
                    return processingNode.movesTo;
                }
                possibleMoves = GetPossibleMoves(processingNode.tile);
                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    if (!visitedTiles.ContainsKey(possibleMoves[i]))
                    {
                        visitedTiles.Add(new Point(possibleMoves[i].X, possibleMoves[i].Y),
                            new PathNode(new Point(possibleMoves[i].X, possibleMoves[i].Y),
                            new Point(processingNode.tile.X, processingNode.tile.Y), processingNode.movesTo + 1));
                        processing.Enqueue(possibleMoves[i]);
                    }
                }
            }
            return 0;
        }

        private List<Point> GetPossibleMoves(Point from)
        {
            List<Point> possible = new List<Point>();

            if (from.Y < dungeonBP.GetLength(0) - 2 && dungeonBP[from.Y + 1, from.X] != 0)
            {
                possible.Add(new Point(from.X, from.Y + 1));
            }
            if (from.Y > 1 && dungeonBP[from.Y - 1, from.X] != 0)
            {
                possible.Add(new Point(from.X, from.Y - 1));
            }
            if (from.X < dungeonBP.GetLength(1) - 2 && dungeonBP[from.Y, from.X + 1] != 0)
            {
                possible.Add(new Point(from.X + 1, from.Y));
            }
            if (from.X > 1 && dungeonBP[from.Y, from.X - 1] != 0)
            {
                possible.Add(new Point(from.X - 1, from.Y));
            }

            return possible;
        }

        private struct PathNode
        {
            public Point tile;
            public Point prior;
            public int movesTo;

            public PathNode(Point tile, Point prior, int movesTo)
            {
                this.tile = tile;
                this.prior = prior;
                this.movesTo = movesTo;
            }
        }

        private void AddEnemies()
        {
            int enemyPercentageInMaze = 1;
            int monsterTilePercentageInMaze = 2;
            int enemyPercentageNormalRooms = 2;
            int enemyPercentageInMonsterRooms = 5;
            int potionPercentageInNormalRooms = 1;
            int potionPercentageInMonsterRooms = 2;
            int tierCount = 5;

            int index;

            Dictionary<int, List<byte>> tieredRoomDictionary = GetTieredRoomDictionaryBasedOnLengthFromStart(tierCount);

            List<Point> potentialBossTiles = new List<Point>();
            List<TypeOfPotion> potionTypeList = new List<TypeOfPotion>();

            foreach (TypeOfPotion tP in Enum.GetValues(typeof(TypeOfPotion)))
            {
                potionTypeList.Add(tP);
            }

            for (int y = 1; y < tiles.GetLength(0); y++)
                for (int x = 1; x < tiles.GetLength(1); x++)
                {
                    if (dungeonBP[y, x] > 1)
                    {
                        if (mazeRegions.Contains(dungeonBP[y, x]))
                        {
                            if (rand.Next(0, 100) < enemyPercentageInMaze)
                            {

                            }
                            if (rand.Next(0, 100) < monsterTilePercentageInMaze)
                            {
                                tiles[y, x].type = TileType.MonsterTile;
                                tiles[y, x].ChangeTexture(tileSet.GetTexture(TileTexture.Enemy_Tile, dungeonBP[y, x]));
                            }
                        }
                        else if (dungeonBP[y, x] == bossRoom)
                        {
                            potentialBossTiles.Add(new Point(x, y));
                        }
                        else if (tiles[y, x].type == TileType.MonsterTile)
                        {
                            if (rand.Next(0, 100) < enemyPercentageInMonsterRooms)
                            {

                            }
                            if (rand.Next(0, 100) < potionPercentageInMonsterRooms)
                            {
                                index = rand.Next(0, potionTypeList.Count);
                                gameObjects.Add(new Point(x, y), new Object.Potion(textures.potion, GetTileCenter(x, y), potionTypeList[index]));
                            }
                        }
                        else
                        {
                            if (rand.Next(0, 100) < enemyPercentageNormalRooms)
                            {

                            }
                            if (rand.Next(0, 100) < potionPercentageInNormalRooms)
                            {
                                index = rand.Next(0, potionTypeList.Count);
                                gameObjects.Add(new Point(x, y), new Object.Potion(textures.potion, GetTileCenter(x, y), potionTypeList[index]));
                            }
                        }
                    }
                }

            index = rand.Next(0, potentialBossTiles.Count - 1);
            gameObjects.Add(potentialBossTiles[index],
                new Object.Monster(textures.demon, GetTileCenter(potentialBossTiles[index].X, potentialBossTiles[index].Y),
                0, tileSize, new Point(2, 0), 0.4f, true));

            tiles[potentialBossTiles[index].Y, potentialBossTiles[index].X].type = TileType.Boss;

            index = rand.Next(0, potentialBossTiles.Count - 1);
            tiles[potentialBossTiles[index].Y, potentialBossTiles[index].X].type = TileType.ExitPortal;

            gameObjects.Add(potentialBossTiles[index],
                new Object.Portal(textures.portal, GetTileCenter(potentialBossTiles[index].X, potentialBossTiles[index].Y), false));

        }

        //private Object.Potion CreatePotion(int roomTier)
        //{
        //    Object.Potion potion;

        //    Dictionary<TypeOfPotion, int> potionTypeWeighted = new Dictionary<TypeOfPotion, int>();

        //    potionTypeWeighted.Add(TypeOfPotion.health, 30);
        //    potionTypeWeighted.Add(TypeOfPotion.mana, 30);
        //    potionTypeWeighted.Add(TypeOfPotion.xp, 15);
        //    potionTypeWeighted.Add(TypeOfPotion.strength, 10);
        //    potionTypeWeighted.Add(TypeOfPotion.accuracy, 10);
        //    potionTypeWeighted.Add(TypeOfPotion.luck, 10);
        //    potionTypeWeighted.Add(TypeOfPotion.intelligence, 10);
        //    potionTypeWeighted.Add(TypeOfPotion.speed, 10);
        //    potionTypeWeighted.Add(TypeOfPotion.maxHealth, 10);
        //    potionTypeWeighted.Add(TypeOfPotion.maxMana, 10);
        //    potionTypeWeighted.Add(TypeOfPotion.level, 5);

        //    int defaultWeight, weightToAdd;

        //    foreach (TypeOfPotion tP in Enum.GetValues(typeof(TypeOfPotion)))
        //    {
        //        defaultWeight = potionTypeWeighted[tP];
        //        weightToAdd = defaultWeight + 2

        //        potionTypeWeighted[tP] = 
        //    }

        //    return potion;
        //}

        private Dictionary<int, List<byte>> GetTieredRoomDictionaryBasedOnLengthFromStart(int tierCount)
        {
            List<int> tileCountToSort = new List<int>();

            for (int i = 1; i < roomRegions.Count; i++)
            {
                tileCountToSort.Add(roomLengthFromStart[roomRegions[i]]);
            }

            tileCountToSort.Sort();

            List<List<int>> tileCountSortedDividedIntoLists =
                GeneratorUtility.DivideIntListIntoListOfIntList(tileCountToSort, tierCount);

            Dictionary<int, List<byte>> dictionaryToReturn = new Dictionary<int, List<byte>>();

            for (int i = 0; i < tileCountSortedDividedIntoLists.Count; i++)
            {
                dictionaryToReturn[i + 1] = new List<byte>();
                for (int y = 0; y < tileCountSortedDividedIntoLists[i].Count; y++)
                {
                    dictionaryToReturn[i + 1].Add((byte)tileCountSortedDividedIntoLists[i][y]);
                }
            }

            return dictionaryToReturn;
        }

        #region Debug
        private void DebugNumericalTileArray()
        {

            for (int y = 0; y < dungeonBP.GetLength(0); y++)
            {
                for (int x = 0; x < dungeonBP.GetLength(1); x++)
                {
                    if (dungeonBP[y, x] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0");
                    }
                    else if (dungeonBP[y, x] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("1");
                    }
                    else if (dungeonBP[y, x] == bossRoom)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("B");
                    }
                    else if (roomRegions.Contains(dungeonBP[y, x]))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("1");
                    }
                    else if (mazeRegions.Contains(dungeonBP[y, x]))
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

            for (int y = 0; y < dungeonBP.GetLength(0); y++)
            {
                for (int x = 0; x < dungeonBP.GetLength(1); x++)
                {
                    if (dungeonBP[y, x] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0");
                    }
                    else if (list.Contains(dungeonBP[y, x]))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("1");
                    }
                    else if (roomRegions.Contains(dungeonBP[y, x]))
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
