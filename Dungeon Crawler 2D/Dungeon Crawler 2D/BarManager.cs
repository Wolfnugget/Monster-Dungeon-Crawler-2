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
        private Texture2D pixelTex;
        private SpriteFont comicSans;

        private int windowWidth;
        private int windowHeight;
        public int sideBarWidth;
        private int statBarWidth;

        private Object.Player player;

        public BarManager(GraphicsDevice graphicsDevice, ContentManager content, Object.Player player, int windowWidth, int windowHeight)
        {
            this.player = player;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            comicSans = content.Load<SpriteFont>("textFont1");

            pixelTex = new Texture2D(graphicsDevice, 1, 1);
            pixelTex.SetData<Color>(new Color[] { Color.White });

            sideBarWidth = windowWidth / 10;
            statBarWidth = sideBarWidth / 3;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //side-bars där stats visas
            spriteBatch.Draw(pixelTex, new Rectangle(0, 0, sideBarWidth, windowHeight), Color.Black);
            spriteBatch.Draw(pixelTex, new Rectangle(windowWidth - sideBarWidth, 0, sideBarWidth, windowHeight), Color.Black);


            //health-bar
            spriteBatch.Draw(pixelTex, new Rectangle((sideBarWidth / 2) - (statBarWidth / 2), windowHeight - sideBarWidth - player.stats.CheckStat(Stat.maxHealth), statBarWidth, player.stats.CheckStat(Stat.maxHealth)), Color.Gray);
            spriteBatch.Draw(pixelTex, new Rectangle((sideBarWidth / 2) - (statBarWidth / 2), windowHeight - sideBarWidth - player.stats.CheckStat(Stat.maxHealth), statBarWidth, player.stats.CheckStat(Stat.health)), Color.Red);

            //mana-bar
            spriteBatch.Draw(pixelTex, new Rectangle(windowWidth - sideBarWidth + (sideBarWidth / 2) - (statBarWidth / 2), windowHeight - sideBarWidth - player.stats.CheckStat(Stat.maxMana), statBarWidth, player.stats.CheckStat(Stat.maxMana)), Color.Gray);
            spriteBatch.Draw(pixelTex, new Rectangle(windowWidth - sideBarWidth + (sideBarWidth / 2) - (statBarWidth / 2), windowHeight - sideBarWidth - player.stats.CheckStat(Stat.maxMana), statBarWidth, player.stats.CheckStat(Stat.mana)), Color.Blue);

            //text
            Vector2 textSizeHP = comicSans.MeasureString("HP: " + player.stats.CheckStat(Stat.health));
            Vector2 originHP = new Vector2(textSizeHP.X * 0.5f, 0);

            Vector2 textSizeMP = comicSans.MeasureString("MP: " + player.stats.CheckStat(Stat.mana));
            Vector2 originMP = new Vector2(textSizeHP.X * 0.5f, 0);

            spriteBatch.DrawString(comicSans, "HP: " + player.stats.CheckStat(Stat.health), new Vector2((sideBarWidth / 2), windowHeight - sideBarWidth), Color.Red, 0, originHP, 2, SpriteEffects.None, 0);
            spriteBatch.DrawString(comicSans, "MP: " + player.stats.CheckStat(Stat.mana), new Vector2(windowWidth - (sideBarWidth / 2), windowHeight - sideBarWidth), Color.Blue, 0, originMP, 2, SpriteEffects.None, 0);
        }
    }
}
