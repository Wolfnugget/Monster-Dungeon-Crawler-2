using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.Object
{
    class Potion: Animated
    {
        public TypeOfPotion potionType;

        bool animate;

        float startY;

        public Potion(Texture2D texture, Vector2 position, TypeOfPotion potionType)
            : base(texture, position, new Point(16, 16), new Point(3, 0), 0.2f)
        {
            this.potionType = potionType;
            startY = position.Y;

            switch (potionType)
            {
                case (TypeOfPotion.health):
                    startingFrame.Y = 0;
                    animate = true;
                    break;
                case (TypeOfPotion.mana):
                    startingFrame.Y = 1;
                    animate = true;
                    break;
                case (TypeOfPotion.maxHealth):
                    frame.Y = 2;
                    frame.X = 0;
                    animate = false;
                    break;
                case (TypeOfPotion.maxMana):
                    frame.Y = 3;
                    frame.X = 0;
                    animate = false;
                    break;
                case (TypeOfPotion.intelligence):
                    frame.Y = 4;
                    frame.X = 0;
                    animate = false;
                    break;
                case (TypeOfPotion.luck):
                    frame.Y = 5;
                    frame.X = 0;
                    animate = false;
                    break;
                case (TypeOfPotion.speed):
                    frame.Y = 6;
                    frame.X = 0;
                    animate = false;
                    break;
                case (TypeOfPotion.strength):
                    frame.Y = 7;
                    frame.X = 0;
                    animate = false;
                    break;
                case (TypeOfPotion.accuracy):
                    frame.Y = 8;
                    frame.X = 0;
                    animate = false;
                    break;
                case (TypeOfPotion.xp):
                    frame.Y = 9;
                    frame.X = 0;
                    animate = false;
                    break;
                case (TypeOfPotion.level):
                    frame.Y = 10;
                    frame.X = 0;
                    animate = false;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            position.Y = startY + 2 *  (float)Math.Sin(3 * gameTime.TotalGameTime.TotalSeconds);

            if (animate)
            {
                Animate(gameTime);
            }
        }
    }
}
