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
    public class PreMadeArea: Area
    {
        public Point roomCoords;
        public bool[] doors;

        public PreMadeArea(string roomPath, Point roomCoords, TextureManager textures)
            : base(textures)
        {
            List<string>  roomBluePrint = new List<string>();
            doors = new bool[4] { false, false, false, false};
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
                            tiles[i, j] = new Tile(TileType.basic, true);
                            break;
                        case 'X':
                            tiles[i, j] = new Tile(TileType.Wall, false);
                            break;
                        case 'H':
                            tiles[i, j] = new Tile(TileType.HorizontalWall, false);
                            break;
                        case 'V':
                            tiles[i, j] = new Tile(TileType.VerticalWall, false);
                            break;
                        case 'J':
                            tiles[i, j] = new Tile(TileType.TopRightCorner, false);
                            break;
                        case 'L':
                            tiles[i, j] = new Tile(TileType.TopLeftCorner, false);
                            break;
                        case 'j':
                            tiles[i, j] = new Tile(TileType.BottomRightCorner, false);
                            break;
                        case 'l':
                            tiles[i, j] = new Tile(TileType.BottomLeftCorner, false);
                            break;
                        case 'N':
                            tiles[i, j] = new Tile(TileType.NorthExit, true);
                            doors[0] = true;
                            break;
                        case 'E':
                            tiles[i, j] = new Tile(TileType.EastExit, true);
                            doors[1] = true;
                            break;
                        case 'S':
                            tiles[i, j] = new Tile(TileType.SouthExit, true);
                            doors[2] = true;
                            break;
                        case 'W':
                            tiles[i, j] = new Tile(TileType.WestExit, true);
                            doors[3] = true;
                            break;
                        case 'C':
                            tiles[i, j] = new Tile(TileType.basic, true);
                            playerStart = GetTileCenter(j, i);
                            break;
                        case '1':
                            tiles[i, j] = new Tile(TileType.MonsterTile, true);
                            break;
                        case 'P':
                            tiles[i, j] = new Tile(TileType.basic, true);
                            //kod för en potion som antingen helar eller ger mana till spelaren / en pickupp
                            break;
                        case 'T':
                            tiles[i, j] = new Tile(TileType.basic, true);
                            //kod för treasure chest som ger en item
                            break;
                        case 'R':
                            tiles[i, j] = new Tile(TileType.basic, true);
                            //behöver ändras till en random funktion som kan vara chest, enemy, potion eller något annat
                            break;
                    }
                }
            }
        }
    }
}
