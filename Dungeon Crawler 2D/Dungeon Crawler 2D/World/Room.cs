using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.World
{
    class Room
    {
        Tile[,] tiles;
        public Vector2 PlayerStart;

        public Point roomCoords;

        public bool northExit, southExit, westExit, eastExit;

        public Room(string roomPath, TextureManager textures, Point roomCoords)
        {
            List<string> roomBluePrint = new List<string>();
            StreamReader sr = new StreamReader(roomPath);
            while (!sr.EndOfStream)
            {
                roomBluePrint.Add(sr.ReadLine());
            }

            int x = (roomBluePrint.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)).Length;
            this.roomCoords = roomCoords;

            tiles = new Tile[x, roomBluePrint.Count];

            for (int y = 0; y < roomBluePrint.Count; y++)
            {
                x = 0;
                while (x < roomBluePrint[y].Length)
                {
                    if (roomBluePrint[y][x] == '0')
                    {
                        tiles[y, x] = new Tile(new Vector2(x * textures.basicTile.Width, y * textures.basicTile.Height)
                            , textures.basicTile, TileType.basic);
                    }
                    else if (roomBluePrint[y][x] == 'X')
                    {
                        tiles[y, x] = new Tile(new Vector2(x * textures.wall.Width, y * textures.wall.Height)
                            , textures.wall, TileType.Wall);
                    }
                    else if (roomBluePrint[y][x] == 'N')
                    {
                        tiles[y, x] = new Tile(new Vector2(x * textures.door.Width, y * textures.door.Height)
                            , textures.door, TileType.NorthExit);
                        northExit = true;
                    }
                    else if (roomBluePrint[y][x] == 'S')
                    {
                        tiles[y, x] = new Tile(new Vector2(x * textures.door.Width, y * textures.door.Height)
                            , textures.door, TileType.SouthExit);
                        southExit = true;
                    }
                    else if (roomBluePrint[y][x] == 'W')
                    {
                        tiles[y, x] = new Tile(new Vector2(x * textures.door.Width, y * textures.door.Height)
                            , textures.door, TileType.WestExit);
                        westExit = true;
                    }
                    else if (roomBluePrint[y][x] == 'E')
                    {
                        tiles[y, x] = new Tile(new Vector2(x * textures.door.Width, y * textures.door.Height)
                            , textures.door, TileType.EastExit);
                        eastExit = true;
                    }
                    else if (roomBluePrint[y][x] == 'C')
                    {
                        PlayerStart = new Vector2(x * textures.basicTile.Width, y * textures.basicTile.Height);
                        tiles[y, x] = new Tile(new Vector2(x * textures.basicTile.Width, y * textures.basicTile.Height)
                            , textures.basicTile, TileType.basic);
                    }
                    x++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
