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
        public Texture2D playerSpriteSheet, playerBattleAnimations, whiteSquare,
            poisonIcon, bleedIcon, confusionIcon, barsSheet, battleBackGround, battleBackGround2, 
            battleBackGround3, statPointIcon, strengthIcon, accuracyIcon, speedIcon, intelligenceIcon, 
            luckIcon, potion, demon, zombie, warlock, portal, hitAnimation, magicAnimation, defenceAnimation, 
            dodgeAnimation, poisonHitAnimation, missAnimation, poison, bleed, confusion; //osv


        public SpriteFont comicSans; //osv



        public TextureManager(ContentManager content)
        {
            
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

            //Living
            demon = content.Load<Texture2D>("Textures/Monsters/Demon");
            zombie = content.Load<Texture2D>("Textures/Monsters/zombie_spritesheet");
            warlock = content.Load<Texture2D>("Textures/Monsters/warlock_spritesheet");
            playerSpriteSheet = content.Load<Texture2D>("PlayerSpriteSheet");
            playerBattleAnimations = content.Load<Texture2D>("Battle/Animations/PlayerBattleSpriteSheet");

            //Object
            portal = content.Load<Texture2D>("Textures/Object/Portal");
            potion = content.Load<Texture2D>("Textures/Object/Items");

            //Attack animations
            hitAnimation = content.Load<Texture2D>("Battle/Animations/Attack_Animation");
            magicAnimation = content.Load<Texture2D>("Battle/Animations/Magic_spritesheet");
            defenceAnimation = content.Load<Texture2D>("Battle/Animations/Shield_Animation (1)");
            //dodgeAnimation = content.Load<Texture2D>("Battle/Animations/NULL");
            poisonHitAnimation = content.Load<Texture2D>("Battle/Animations/PoisonHit_spritesheet");
            //missAnimation = content.Load<Texture2D>("Battle/Animations/null");

            //Status Animation
            //poison = content.Load<Texture2D>("Battle/Animations/null");
            //bleed = content.Load<Texture2D>("Battle/Animations/null");
            confusion = content.Load<Texture2D>("Battle/Animations/Confusion_Animation");
        }
    }
}
