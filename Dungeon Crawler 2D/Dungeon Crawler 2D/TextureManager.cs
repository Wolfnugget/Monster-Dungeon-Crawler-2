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
        public Texture2D playerSpriteSheet, horizontalWall, vericalWall,
            wallTRightCorner, wallTLeftCorner, wallBRightCorner, wallBLeftCorner,
            northDoor, southDoor, eastDoor, westDoor,
            basicTile; //osv

        public TextureManager(ContentManager content)
        {
            playerSpriteSheet = content.Load<Texture2D>("PlayerSpriteSheet");
            basicTile = content.Load<Texture2D>("Textures/Dungeon/SmallTiles");
            horizontalWall = content.Load<Texture2D>("Textures/Dungeon/HorizontalWall");
            vericalWall = content.Load<Texture2D>("Textures/Dungeon/VerticalWall");
            northDoor = content.Load<Texture2D>("Textures/Dungeon/NorthDoor");
            southDoor = content.Load<Texture2D>("Textures/Dungeon/SouthDoor");
            eastDoor = content.Load<Texture2D>("Textures/Dungeon/EastDoor");
            westDoor =  content.Load<Texture2D>("Textures/Dungeon/WestDoor");
            wallTRightCorner = content.Load<Texture2D>("Textures/Dungeon/WallTRightCorner");
            wallTLeftCorner = content.Load<Texture2D>("Textures/Dungeon/WallTLeftCorner");
            wallBRightCorner = content.Load<Texture2D>("Textures/Dungeon/WallBRightCorner");
            wallBLeftCorner = content.Load<Texture2D>("Textures/Dungeon/WallBLeftCorner");
        }
    }
}
