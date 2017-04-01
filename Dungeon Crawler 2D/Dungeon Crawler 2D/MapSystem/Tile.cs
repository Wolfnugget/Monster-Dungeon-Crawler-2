using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Dungeon_Crawler_2D.MapSystem
{
    public class Tile
    {
        protected Texture2D tex;
        protected Vector2 pos;
        protected Color color;
        public Rectangle rect;
        public int tileType;

        public Tile(Texture2D tex, Vector2 pos, Color color, int tileType)
        {
            this.tex = tex;
            this.pos = pos;
            this.color = color;
            this.tileType = tileType;
            rect = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Width);
        }

        public void Update(GameTime gameTime)
        {
            //om en tile ska ha en animation
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Draw(tex, pos, color);
        }


    }
}