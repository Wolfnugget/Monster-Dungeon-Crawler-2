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
    class StatScreen
    {
        Object.Player player;
        TextureManager textures;
        int screenWidth;
        int screenHeight;

        public StatScreen(Object.Player player, TextureManager textures)
        {
            this.player = player;
            this.textures = textures;
            screenHeight = 0;
            screenWidth = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 textSize = textures.comicSans.MeasureString(player.stats.CheckStat(Stat.maxMana).ToString());
            Vector2 origin = new Vector2(textSize.X * 0.5f, 0);
            screenWidth = 0;
            screenHeight = 0;
            for (int i = 0; i < 20; i++)
            {
                screenWidth += textures.whiteSquare.Width;
                screenHeight += textures.whiteSquare.Height;
                for (int j = 0; j < 15; j++)
                {
                    spriteBatch.Draw(textures.whiteSquare, new Vector2(440 + textures.whiteSquare.Width * i, 240 + textures.whiteSquare.Height * j), Color.Gray);
                }
            }
            spriteBatch.DrawString(textures.comicSans, player.stats.CheckStat(Stat.maxHealth).ToString(),
                    new Vector2(440 + (screenWidth / 4) * 1, 240 + (screenHeight / 3)), Color.Red,
                    0, origin, 2, SpriteEffects.None, 0);
            spriteBatch.DrawString(textures.comicSans, player.stats.CheckStat(Stat.strength).ToString(),
                    new Vector2(440 + (screenWidth / 4) * 2, 240 + (screenHeight / 3)), Color.Red,
                    0, origin, 2, SpriteEffects.None, 0);
            spriteBatch.DrawString(textures.comicSans, player.stats.CheckStat(Stat.dexterity).ToString(),
                    new Vector2(440 + (screenWidth / 4) * 3, 240 + (screenHeight / 3)), Color.Red,
                    0, origin, 2, SpriteEffects.None, 0);

            spriteBatch.DrawString(textures.comicSans, player.stats.CheckStat(Stat.maxMana).ToString(),
                new Vector2(440 + (screenWidth / 4) * 1, 240 + (screenHeight / 6) * 4), Color.Red,
                0, origin, 2, SpriteEffects.None, 0);
            spriteBatch.DrawString(textures.comicSans, player.stats.CheckStat(Stat.inteligence).ToString(),
                new Vector2(440 + (screenWidth / 4) * 2, 240 + (screenHeight / 6) * 4), Color.Red,
                0, origin, 2, SpriteEffects.None, 0);
            spriteBatch.DrawString(textures.comicSans, player.stats.CheckStat(Stat.luck).ToString(),
                new Vector2(440 + (screenWidth / 4) * 3, 240 + (screenHeight / 6) * 4), Color.Red,
                0, origin, 2, SpriteEffects.None, 0);
        }
    }
}
