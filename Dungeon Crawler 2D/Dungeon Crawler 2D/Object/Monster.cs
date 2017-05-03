using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.Object
{
    public class Monster: Actor
    {
        public bool boss;

        public Monster(Texture2D texture, Vector2 position, float speed, Point frameSize, Point frames, float frameTime,
            bool boss)
            : base(texture, position, speed, frameSize, frames, frameTime)
        {
            this.boss = boss;
        }

        public override void Update(GameTime gameTime)
        {
            if (speed == 0)
            {
                Animate(gameTime);
            }
            else
            {
                base.Update(gameTime);
            }
        }
    }
}
