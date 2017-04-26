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

        public Tile(TileType type, bool pasable)
        {
            this.type = type;
            this.pasable = pasable;
        }
    }
}
