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
    public class TextureManager
    {
        public Texture2D player, horizontalWall, vericalWall,
            wallRightCorner, wallLeftCorner,
            northDoor, southDoor, eastDoor, westDoor,
            basicTile; //osv

        public TextureManager(ContentManager content)
        {
            player = content.Load<Texture2D>("Player");
            basicTile = content.Load<Texture2D>("Textures/Dungeon/SmallTiles");
            horizontalWall = content.Load<Texture2D>("Textures/Dungeon/HorizontalWall");
            vericalWall = content.Load<Texture2D>("Textures/Dungeon/VerticalWall");
            northDoor = content.Load<Texture2D>("Textures/Dungeon/NorthDoor");
            southDoor = content.Load<Texture2D>("Textures/Dungeon/SouthDoor");
            eastDoor = content.Load<Texture2D>("Textures/Dungeon/EastDoor");
            westDoor =  content.Load<Texture2D>("Textures/Dungeon/WestDoor");
            wallRightCorner = content.Load<Texture2D>("Textures/Dungeon/WallRightCorner");
            wallLeftCorner = content.Load<Texture2D>("Textures/Dungeon/WallLeftCorner");
        }
    }
}
