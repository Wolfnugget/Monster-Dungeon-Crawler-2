using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Dungeon_Crawler_2D
{
    class Tile
    {
        Texture2D tex;
        Vector2 pos;
        Color color;
        Rectangle rect;
        bool passable;

        public Tile(Texture2D tex, Vector2 pos, Color color)
        {
            this.tex = tex;
            this.pos = pos;
            this.color = color;
            if (color == Color.Black)
                passable = false;
            else
                passable = true;
            rect = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Width);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sp)
        {
            sp.Draw(tex, pos, color);
        }


    }
}