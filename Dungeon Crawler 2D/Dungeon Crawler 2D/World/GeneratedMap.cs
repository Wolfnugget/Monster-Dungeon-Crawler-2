﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.World
{
    class GeneratedMap: Map
    {
        public GeneratedMap(TextureManager textures, ContentManager content)
            : base(textures, content)
        {
            Random rand = new Random();
            Point dimensions = new Point(rand.Next(80, 120), rand.Next(80, 120));

            rooms.Add(Location.Dungeon, new GeneratedDungeon(dimensions , textures, content));
            currentLocation = Location.Dungeon;
        }

        protected override void ChangeArea(Point RoomDirection, TileType entrance)
        {
            throw new NotImplementedException();
        }
    }
}
