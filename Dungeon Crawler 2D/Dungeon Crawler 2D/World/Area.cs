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
    public abstract class Area
    {
        public Tile[,] tiles;
        protected Point tileSize = new Point(16, 16);
        public Vector2 playerStart;
        protected TextureManager textures;

        protected Random rand;

        protected TileSet tileSet;

        protected Dictionary<Point, Object.Object> gameObjects;

        List<Point> ObjectsToRender;

        public bool ExitPortalOpen;

        private int renderDistanceX, renderDistanceY, screenCenterX, screenCenterY,
            yStart, xStart, yMax, xMax;

        public Area(TextureManager textures, ContentManager content)
        {
            this.textures = textures;

            renderDistanceX = 10;
            renderDistanceY = 10;

            rand = new Random();

            gameObjects = new Dictionary<Point, Object.Object>();
            PickTileSet(content);

            ObjectsToRender = new List<Point>();
        }

        protected abstract void PickTileSet(ContentManager content);

        public void Update(GameTime gameTime, Vector2 cameraCenter)
        {
            GetTileAtPosition(cameraCenter, out screenCenterX, out screenCenterY);
            UpdateAndRenderRange();

            for (int y = yStart; y < yMax; y++)
                for (int x = xStart; x < xMax; x++)
                {
                    if (gameObjects.ContainsKey(new Point(x, y)))
                    {
                        gameObjects[new Point(x, y)].Update(gameTime); ;
                    }
                }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            ObjectsToRender.Clear();

            for (int y = yStart; y < yMax; y++)
                for (int x = xStart; x < xMax; x++)
                {
                    tiles[y, x].Draw(GetTileRectangle(x, y), spriteBatch);
                    if (gameObjects.ContainsKey(new Point(x, y)))
                    {
                        ObjectsToRender.Add(new Point(x, y));
                    }
                }

            for (int i = 0; i < ObjectsToRender.Count; i++)
            {
                gameObjects[ObjectsToRender[i]].Draw(spriteBatch);
            }
        }

        private void UpdateAndRenderRange()
        {
            yStart = Math.Max(0, screenCenterY - renderDistanceY);
            xStart = Math.Max(0, screenCenterX - renderDistanceX);
            yMax = Math.Min(tiles.GetLength(0), screenCenterY + renderDistanceY);
            xMax = Math.Min(tiles.GetLength(1), screenCenterX + renderDistanceX);
        }

        /// <summary>
        /// Ger centrum av en tile i riktning från en annan tile med en viss position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Vector2 GetTargetTileCenter(Vector2 position, Point direction)
        {
            int x, y;

            GetTileAtPosition(position, out x, out y);
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

        public void GetTileAtPosition(Vector2 position, out int tileX, out int tileY)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    if (GetTileRectangle(x, y).Contains(position))
                    {
                        tileX = x;
                        tileY = y;
                        return;
                    }
                }
            }
            tileX = 0;
            tileY = 0;
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

        public void BossDies(int bX, int bY)
        {
            for (int y = 1; y < tiles.GetLength(0); y++)
                for (int x = 1; x < tiles.GetLength(1); x++)
                {
                    if (tiles[y, x].type == TileType.ExitPortal)
                    {
                        gameObjects[new Point(x, y)].Funktion();
                    }
                }

            gameObjects.Remove(new Point(bX, bY));
            ExitPortalOpen = true;
        }

        public bool CheckIfTileContainsObject(int x, int y, out Object.Object obj)
        {
            if (gameObjects.TryGetValue(new Point(x, y), out obj))
            {
                if (obj is Object.Potion)
                {
                    gameObjects.Remove(new Point(x, y));
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
