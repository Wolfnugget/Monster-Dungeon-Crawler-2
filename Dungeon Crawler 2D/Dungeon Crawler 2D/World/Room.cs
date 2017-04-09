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
    public class Room
    {
        public Tile[,] tiles;
        public Vector2 PlayerStart;
        public List<string> roomBluePrint;
        TextureManager textures;

        public Point roomCoords;

        public bool northExit, southExit, westExit, eastExit;

        public Room(string roomPath, Point roomCoords, TextureManager textures)
        {
            this.roomBluePrint = new List<string>();
            this.textures = textures;
            StreamReader sr = new StreamReader(roomPath);
            while (!sr.EndOfStream)
            {
                roomBluePrint.Add(sr.ReadLine());
            }

            int x = (roomBluePrint.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)).Length;
            this.roomCoords = roomCoords;

            tiles = new Tile[roomBluePrint.Count, x];

            Console.WriteLine("Y = " + roomBluePrint.Count + "x: " + x);

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (roomBluePrint[i][j] == '0')
                    {
                        tiles[i, j] = new Tile(new Vector2(j * textures.basicTile.Width, i * textures.basicTile.Height)
                            , textures.basicTile, TileType.basic, Color.Beige);
                    }
                    else if (roomBluePrint[i][j] == 'X')
                    {
                        tiles[i, j] = new Tile(new Vector2(j * textures.wall.Width, i * textures.wall.Height)
                            , textures.wall, TileType.Wall, Color.Black);
                    }
                    else if (roomBluePrint[i][j] == 'N')
                    {
                        tiles[i, j] = new Tile(new Vector2(j * textures.door.Width, i * textures.door.Height)
                            , textures.door, TileType.NorthExit, Color.Green);
                        northExit = true;
                    }
                    else if (roomBluePrint[i][j] == 'S')
                    {
                        tiles[i, j] = new Tile(new Vector2(j * textures.door.Width, i * textures.door.Height)
                            , textures.door, TileType.SouthExit, Color.Green);
                        southExit = true;
                    }
                    else if (roomBluePrint[i][j] == 'W')
                    {
                        tiles[i, j] = new Tile(new Vector2(j * textures.door.Width, i * textures.door.Height)
                            , textures.door, TileType.WestExit, Color.Green);
                        westExit = true;
                    }
                    else if (roomBluePrint[i][j] == 'E')
                    {
                        tiles[i, j] = new Tile(new Vector2(j * textures.door.Width, i * textures.door.Height)
                            , textures.door, TileType.EastExit, Color.Green);
                        eastExit = true;
                    }
                    else if (roomBluePrint[i][j] == 'C')
                    {
                        PlayerStart = new Vector2(j * textures.basicTile.Width, i * textures.basicTile.Height);
                        tiles[i, j] = new Tile(new Vector2(j * textures.basicTile.Width, i * textures.basicTile.Height)
                            , textures.basicTile, TileType.basic, Color.Beige);
                    }
                }
            }
        }

        void LoadRoom()
        {
            // this will load the room, this is so not all rooms get loaded at the same time.
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        public Vector2 GetTargetTileCenter(Vector2 position, Point direction)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    if (tiles[y, x].tileRectangle.Contains(position))
                    {
                        if (tiles[y + direction.Y, x + direction.X].pasable)
                        {
                            return tiles[y + direction.Y, x + direction.X].TileCenter;
                        }
                        else
                        {
                            return position;
                        }
                    }
                }
            }

            return position;
        }

        public TileType GetTileType(Vector2 pos)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i,j].tileRectangle.Contains(pos))
                    {
                        return tiles[i,j].type;
                    }
                }
            }
            return TileType.Wall;
        }

    }
}
