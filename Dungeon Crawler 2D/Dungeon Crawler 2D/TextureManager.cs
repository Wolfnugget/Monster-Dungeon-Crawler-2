using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D
{
    class TextureManager
    {
        public Texture2D player, wall, basicTile; //osv

        public TextureManager(ContentManager content)
        {
            player = content.Load<Texture2D>("Player");
            basicTile = content.Load<Texture2D>("Example");
            wall = content.Load<Texture2D>("Example");
        }
    }
}
