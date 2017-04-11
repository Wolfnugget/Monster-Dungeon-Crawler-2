using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawler_2D
{
    class BarManager
    {
        public SpriteFont comicSans;
        private Object.Player player;

        public BarManager(ContentManager content, Object.Player player)
        {
            this.player = player;

            comicSans = content.Load<SpriteFont>("textFont1");

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(comicSans, "Health: " + "XXXXX", new Vector2(2, 0), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(comicSans, "Mana: " + "YYYYY", new Vector2(2, 32), Color.Blue, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(comicSans, "Xp: " + "ZZZZZ", new Vector2(2, 64), Color.Green, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }
    }
}
