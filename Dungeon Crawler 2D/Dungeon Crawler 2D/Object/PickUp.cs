using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.Object
{
    class PickUp: Animated
    {
        public Dungeon_Crawler_2D.PickUp potionType;

        bool animate;

        float startY;

        public PickUp(Texture2D texture, Vector2 position, Dungeon_Crawler_2D.PickUp potionType)
            : base(texture, position, new Point(16, 16), new Point(3, 0), 0.2f)
        {
            this.potionType = potionType;
            startY = position.Y;

            switch (potionType)
            {
                case (Dungeon_Crawler_2D.PickUp.health):
                    startingFrame.Y = 0;
                    animate = true;
                    break;
                case (Dungeon_Crawler_2D.PickUp.mana):
                    startingFrame.Y = 1;
                    animate = true;
                    break;
                case (Dungeon_Crawler_2D.PickUp.maxHealth):
                    frame.Y = 2;
                    frame.X = 0;
                    animate = false;
                    break;
                case (Dungeon_Crawler_2D.PickUp.maxMana):
                    frame.Y = 3;
                    frame.X = 0;
                    animate = false;
                    break;
                case (Dungeon_Crawler_2D.PickUp.intelligence):
                    frame.Y = 4;
                    frame.X = 0;
                    animate = false;
                    break;
                case (Dungeon_Crawler_2D.PickUp.luck):
                    frame.Y = 5;
                    frame.X = 0;
                    animate = false;
                    break;
                case (Dungeon_Crawler_2D.PickUp.speed):
                    frame.Y = 6;
                    frame.X = 0;
                    animate = false;
                    break;
                case (Dungeon_Crawler_2D.PickUp.strength):
                    frame.Y = 7;
                    frame.X = 0;
                    animate = false;
                    break;
                case (Dungeon_Crawler_2D.PickUp.accuracy):
                    frame.Y = 8;
                    frame.X = 0;
                    animate = false;
                    break;
                case (Dungeon_Crawler_2D.PickUp.xp):
                    frame.Y = 9;
                    frame.X = 0;
                    animate = false;
                    break;
                case (Dungeon_Crawler_2D.PickUp.level):
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
