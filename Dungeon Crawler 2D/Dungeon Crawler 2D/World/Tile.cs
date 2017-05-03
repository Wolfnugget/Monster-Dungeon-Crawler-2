using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Dungeon_Crawler_2D.World
{
    public class Tile
    {
        public TileType type;
        public bool pasable;
        Texture2D texture;

        public Tile(TileType type, Texture2D texture, bool pasable)
        {
            this.type = type;
            this.texture = texture;
            this.pasable = pasable;
        }

        public void Draw(Rectangle rectangle, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        public void ChangeTexture(Texture2D texture)
        {
            this.texture = texture;
        }
    }
}
