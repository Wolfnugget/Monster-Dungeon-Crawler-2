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
        public Texture2D playerSpriteSheet, whiteSquare,
            poisonIcon, bleedIcon, confusionIcon, barsSheet, battleBackGround, battleBackGround2, 
            battleBackGround3, statPointIcon, strengthIcon, accuracyIcon, speedIcon, intelligenceIcon, 
            luckIcon, potion, demon, zombie, portal; //osv


        public SpriteFont comicSans; //osv



        public TextureManager(ContentManager content)
        {
            playerSpriteSheet = content.Load<Texture2D>("PlayerSpriteSheet");
            whiteSquare = content.Load<Texture2D>("Example");
            battleBackGround = content.Load<Texture2D>("Battle/Dungeon_Wallpaper");
            battleBackGround2 = content.Load<Texture2D>("Battle/Dungeon_WallpaperSmall");
            battleBackGround3 = content.Load<Texture2D>("Battle/Dungeon_WallpaperSmall2");

            comicSans = content.Load<SpriteFont>("textFont1");
            
            //Sidebar Icons
            poisonIcon = content.Load<Texture2D>("Battle/Icons/Poison");
            bleedIcon = content.Load<Texture2D>("Battle/Icons/Blood_Loss");
            confusionIcon = content.Load<Texture2D>("Battle/Icons/Confusion_Icon");

            //Sidebar-sheet
            barsSheet = content.Load<Texture2D>("Bars/BarsSheet");

            //Stat icons
            statPointIcon = content.Load<Texture2D>("StatIcons/StatPoints_Icon");
            strengthIcon = content.Load<Texture2D>("StatIcons/Strength_Icon");
            accuracyIcon = content.Load<Texture2D>("StatIcons/Accuracy_Icon");
            speedIcon = content.Load<Texture2D>("StatIcons/Speed_Icon");
            intelligenceIcon = content.Load<Texture2D>("StatIcons/Intelligence_Icon");
            luckIcon = content.Load<Texture2D>("StatIcons/Luck_Icon");

            //Monster
            demon = content.Load<Texture2D>("Textures/Monsters/Demon");
            zombie = content.Load<Texture2D>("Textures/Monsters/zombie spritesheet");

            //Object
            portal = content.Load<Texture2D>("Textures/Object/Portal");
            potion = content.Load<Texture2D>("Textures/Object/Items");
        }
    }
}
