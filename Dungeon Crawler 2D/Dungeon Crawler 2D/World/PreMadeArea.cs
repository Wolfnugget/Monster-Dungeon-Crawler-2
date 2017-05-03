using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Dungeon_Crawler_2D.World
{
    public class PreMadeArea: Area
    {
        public PreMadeArea(string roomPath, TextureManager textures, ContentManager content)
            : base(textures, content)
        {
            List<string>  roomBluePrint = new List<string>();

            StreamReader sr = new StreamReader(roomPath);
            while (!sr.EndOfStream)
            {
                roomBluePrint.Add(sr.ReadLine());
            }

            int length = (roomBluePrint.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)).Length;

            tiles = new Tile[roomBluePrint.Count, length];

            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    switch (roomBluePrint[y][x])
                    {
                        case ('G'):
                            tiles[y, x] = new Tile(TileType.basic, tileSet.GetTexture(TileTexture.Grass_Tile, 0), true);
                            break;
                        case ('R'):
                            tiles[y, x] = new Tile(TileType.Wall, tileSet.GetTexture(TileTexture.Wall_NorthEast_Corner, 0), false);
                            break;
                        case ('r'):
                            tiles[y, x] = new Tile(TileType.Wall, tileSet.GetTexture(TileTexture.Wall_SouthEast_Corner, 0), false);
                            break;
                        case ('L'):
                            tiles[y, x] = new Tile(TileType.Wall, tileSet.GetTexture(TileTexture.Wall_NorthWest_Corner, 0), false);
                            break;
                        case ('l'):
                            tiles[y, x] = new Tile(TileType.Wall, tileSet.GetTexture(TileTexture.Wall_SouthWest_Corner, 0), false);
                            break;
                        case ('P'):
                            tiles[y, x] = new Tile(TileType.Portal, tileSet.GetTexture(TileTexture.Grass_Tile, 0), false);
                            gameObjects.Add(new Point(x, y), new Object.Portal(textures.portal, GetTileCenter(x, y), true));
                            break;
                    }
                }
            }
        }

        protected override void PickTileSet(ContentManager content)
        {
            tileSet = new TileSet(content, TileSets.Overworld);
        }
    }
}
