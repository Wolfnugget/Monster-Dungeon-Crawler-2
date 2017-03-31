using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Dungeon_Crawler_2D.World
{
    class Tile
    {
        Rectangle tileRectangle;
        Texture2D texture;
        TileType type;
        Color color;
        public bool pasable;

        public Vector2 TileCenter
        {
            get
            {
                return new Vector2();
            }
        }

        public Tile(Vector2 position, Texture2D texture, TileType type)
        {
            this.texture = texture;
            tileRectangle = new Rectangle((int)position.X, (int)position.Y,
                texture.Width, texture.Height);

            this.type = type;

            if (type == TileType.basic)
            {
                pasable = true;
                color = Color.White;
            }
            else if (type == TileType.Wall)
            {
                pasable = false;
                color = Color.Black;
            }
            else
            {
                pasable = true;
                color = Color.Green;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, tileRectangle, Color.White);
        }
    }
}
