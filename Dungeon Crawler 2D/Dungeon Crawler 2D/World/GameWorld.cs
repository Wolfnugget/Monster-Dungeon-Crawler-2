using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Dungeon_Crawler_2D.World
{
    class GameWorld: Map
    {
        public GameWorld(TextureManager textures, ContentManager content)
            : base(textures, content)
        {

            currentLocation = Location.Overworld;
            rooms.Add(Location.Overworld, new PreMadeArea("Maps/Overworld/Main.txt", textures, content));
        }

        protected override void ChangeArea(TileType entrance)
        {
            MapEventArgs args = new MapEventArgs(MapEventType.ChangeRoom);

            if (currentLocation == Location.Overworld)
            {
                Point dimensions = new Point(rand.Next(80, 120), rand.Next(80, 120));
                currentLocation = Location.Dungeon;
                if (rooms.ContainsKey(Location.Dungeon))
                {
                    rooms[Location.Dungeon] = new GeneratedDungeon(dimensions, textures, content);
                }
                else
                {
                    rooms.Add(Location.Dungeon, new GeneratedDungeon(dimensions, textures, content));
                }
            }
            else
            {
                currentLocation = Location.Overworld;
            }
            args.Position = rooms[currentLocation].GetTileCenterOfType(entrance);
            OnEvent(args);
        }
    }
}
