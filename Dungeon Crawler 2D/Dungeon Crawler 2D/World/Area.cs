using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.World
{
    public abstract class Area
    {
        public Tile[,] tiles;
        protected Point tileSize = new Point(16, 16);
        public Vector2 playerStart;
        TextureManager textures;

        public Area(TextureManager textures)
        {
            this.textures = textures;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    spriteBatch.Draw(TextureByType(tiles[y, x].type), GetTileRectangle(x, y), Color.White);
                }
        }

        private Texture2D TextureByType(TileType type)
        {
            switch (type)
            {
                case TileType.BottomLeftCorner:
                    return textures.wallBLeftCorner;
                case TileType.BottomRightCorner:
                    return textures.wallBRightCorner;
                case TileType.TopLeftCorner:
                    return textures.wallTLeftCorner;
                case TileType.TopRightCorner:
                    return textures.wallTRightCorner;
                case TileType.Wall:
                    return textures.horizontalWall;
                case TileType.VerticalWall:
                    return textures.vericalWall;
                case TileType.HorizontalWall:
                    return textures.horizontalWall;
                case TileType.NorthExit:
                    return textures.northDoor;
                case TileType.EastExit:
                    return textures.eastDoor;
                case TileType.SouthExit:
                    return textures.southDoor;
                case TileType.WestExit:
                    return textures.westDoor;
                default:
                    return textures.basicTile;
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
                    if (GetTileRectangle(x, y).Contains(position))
                    {
                        if (tiles[y + direction.Y, x + direction.X].pasable &&
                            tiles[y, x + direction.X].pasable &&
                            tiles[y + direction.Y, x].pasable)
                        {
                            return GetTileCenter(x + direction.X, y + direction.Y);
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
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    if (tiles[y, x].type == type)
                    {
                        return GetTileCenter(x, y);
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
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    if (tiles[y, x].type == typeOfExit)
                    {
                        tiles[y, x].pasable = false;
                    }
                }
            }
        }

        public TileType GetTileType(Vector2 position)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    if (GetTileRectangle(x, y).Contains(position))
                    {
                        return tiles[y, x].type;
                    }
                }
            }
            return TileType.None;
        }

        protected Rectangle GetTileRectangle(int x, int y)
        {
            return new Rectangle(x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y);
        }

        protected Vector2 GetTileCenter(int x, int y)
        {
            Rectangle tileRectangle = GetTileRectangle(x, y);

            return new Vector2(tileRectangle.X + (tileSize.X / 2), tileRectangle.Y + (tileSize.Y / 2));
        }
    }
}
