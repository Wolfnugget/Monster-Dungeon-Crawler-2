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
        public Vector2 playerStart;

        public Point roomCoords;

        public bool northExit, southExit, westExit, eastExit;

        Point tileSize = new Point(16, 16);

        public Room(string roomPath, Point roomCoords, TextureManager textures)
        {
            List<string>  roomBluePrint = new List<string>();
            StreamReader sr = new StreamReader(roomPath);
            while (!sr.EndOfStream)
            {
                roomBluePrint.Add(sr.ReadLine());
            }

            int x = (roomBluePrint.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)).Length;
            this.roomCoords = roomCoords;

            tiles = new Tile[roomBluePrint.Count, x];

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    switch (roomBluePrint[i][j])
                    {
                        case '0':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.basicTile, TileType.basic);
                            break;
                        case 'X':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.horizontalWall, TileType.Wall);
                            break;
                        case 'H':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.horizontalWall, TileType.Wall);
                            break;
                        case 'V':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.vericalWall, TileType.Wall);
                            break;
                        case 'J':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.wallTRightCorner, TileType.Wall);
                            break;
                        case 'L':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.wallTLeftCorner, TileType.Wall);
                            break;
                        case 'j':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.wallBRightCorner, TileType.Wall);
                            break;
                        case 'l':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.wallBLeftCorner, TileType.Wall);
                            break;
                        case 'N':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.northDoor, TileType.NorthExit);
                            northExit = true;
                            break;
                        case 'E':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.eastDoor, TileType.EastExit);
                            eastExit = true;
                            break;
                        case 'S':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.southDoor, TileType.SouthExit);
                            southExit = true;
                            break;
                        case 'W':
                            tiles[i, j] = new Tile(new Vector2(j * tileSize.X, i * tileSize.Y)
                            , textures.westDoor, TileType.WestExit);
                            westExit = true;
                            break;
                        case 'C':
                            tiles[i, j] = new Tile(new Vector2(j * textures.basicTile.Width, i * textures.basicTile.Height)
                            , textures.basicTile, TileType.basic);
                            playerStart = tiles[i, j].TileCenter;
                            break;
                        case '1':
                            tiles[i, j] = new Tile(new Vector2(j * textures.basicTile.Width, i * textures.basicTile.Height)
                            , textures.basicTile, TileType.MonsterTile);
                            break;
                        case 'P':
                            tiles[i, j] = new Tile(new Vector2(j * textures.basicTile.Width, i * textures.basicTile.Height)
                            , textures.basicTile, TileType.basic);
                            //kod för en potion som antingen helar eller ger mana till spelaren / en pickupp
                            break;
                        case 'T':
                            tiles[i, j] = new Tile(new Vector2(j * textures.basicTile.Width, i * textures.basicTile.Height)
                            , textures.basicTile, TileType.basic);
                            //kod för treasure chest som ger en item
                            break;
                        case 'R':
                            tiles[i, j] = new Tile(new Vector2(j * textures.basicTile.Width, i * textures.basicTile.Height)
                            , textures.basicTile, TileType.basic);
                            //behöver ändras till en random funktion som kan vara chest, enemy, potion eller något annat
                            break;
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

        /// <summary>
        /// Ger centrum av en tile i riktning från en annan tile med en viss position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector2 GetTargetTileCenter(Vector2 position, Point direction)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    if (tiles[y, x].tileRectangle.Contains(position))
                    {
                        if (tiles[y + direction.Y, x + direction.X].pasable &&
                            tiles[y, x + direction.X].pasable &&
                            tiles[y + direction.Y, x].pasable)
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

        /// <summary>
        /// Hämtar ett tile center av den första tilen av en viss typ.
        /// Används för att hitta en viss ingång till ett rum.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Vector2 GetTileCenterOfType(TileType type)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j].type == type)
                    {
                        return tiles[i, j].TileCenter;
                    }
                }
            }

            return new Vector2(0, 0);
        }

        /// <summary>
        /// Stänger en dörr.
        /// </summary>
        /// <param name="typeOfExit"></param>
        public void RemoveExit(TileType typeOfExit)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[i, y].type == typeOfExit)
                    {
                        tiles[i, y].pasable = false;
                    }
                }
            }
        }

        public TileType GetTileType(Vector2 position)
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i,j].tileRectangle.Contains(position))
                    {
                        return tiles[i,j].type;
                    }
                }
            }
            return TileType.None;
        }
    }
}
