using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private TextureManager textures;

        private Rectangle leftSideBarRect;
        private Rectangle rightSideBarRect;
        
        private int windowWidth;
        private int windowHeight;
        public int sideBarWidth;
        private int statBarWidth;

        private Object.Player player;

        public BarManager(TextureManager textures, GraphicsDevice graphicsDevice, ContentManager content, Object.Player player, int windowWidth, int windowHeight)
        {
            this.textures = textures;
            this.player = player;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            comicSans = content.Load<SpriteFont>("textFont1");

            pixelTex = new Texture2D(graphicsDevice, 1, 1);
            pixelTex.SetData<Color>(new Color[] { Color.White });
            
            sideBarWidth = windowWidth / 10;
            statBarWidth = sideBarWidth / 3;

            leftSideBarRect = new Rectangle(0, 0, sideBarWidth, windowHeight);
            rightSideBarRect = new Rectangle(windowWidth - sideBarWidth, 0, sideBarWidth, windowHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //side-bars där stats visas
            spriteBatch.Draw(pixelTex, leftSideBarRect, Color.Black);
            spriteBatch.Draw(pixelTex, rightSideBarRect, Color.Black);
            
            //health-bar
            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftSideBarRect.X + (sideBarWidth / 2)
                - (statBarWidth / 2), windowHeight - sideBarWidth - (player.stats.CheckStat(Stat.maxHealth) * 2)
                , statBarWidth, player.stats.CheckStat(Stat.maxHealth) * 2), new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);
            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftSideBarRect.X + (sideBarWidth / 2)
                - (statBarWidth / 2), windowHeight - sideBarWidth - (player.stats.CheckStat(Stat.health) * 2),
                statBarWidth, player.stats.CheckStat(Stat.health) * 2), new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);

            //mana-bar
            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightSideBarRect.X + (sideBarWidth / 2)
                - (statBarWidth / 2), windowHeight - sideBarWidth - (player.stats.CheckStat(Stat.maxMana) * 2),
                statBarWidth, player.stats.CheckStat(Stat.maxMana) * 2), new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);
            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightSideBarRect.X + (sideBarWidth / 2)
                - (statBarWidth / 2), windowHeight - sideBarWidth - (player.stats.CheckStat(Stat.mana) * 2),
                statBarWidth, player.stats.CheckStat(Stat.mana) * 2), new Rectangle(8, 0, 8, textures.barsSheet.Height), Color.White);
            
            //experience-bar
            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightSideBarRect.X + (sideBarWidth / 2)
                - (statBarWidth / 2), (windowHeight / 2) - (sideBarWidth * 2), statBarWidth, sideBarWidth * 2),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);
            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightSideBarRect.X + (sideBarWidth / 2)
                - (statBarWidth / 2), (windowHeight / 2) - (player.stats.CheckStat(Stat.xp) * 4), statBarWidth, player.stats.CheckStat(Stat.xp) * 4),
                new Rectangle(16, 0, 8, textures.barsSheet.Height), Color.White);


            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                player.stats.AddEffect(1, Effects.poison, 1);
                player.stats.AddEffect(1, Effects.bleed, 1);
                player.stats.AddEffect(1, Effects.confusion, 1);

                player.stats.ChangeStat(Stat.health, 1);
                player.stats.ChangeStat(Stat.mana, 1);
                player.stats.ChangeStat(Stat.xp, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                player.stats.AddEffect(-1, Effects.poison, 1);
                player.stats.AddEffect(-1, Effects.bleed, 1);
                player.stats.AddEffect(-1, Effects.confusion, 1);

                player.stats.ChangeStat(Stat.health, -1);
                player.stats.ChangeStat(Stat.mana, -1);
                player.stats.ChangeStat(Stat.xp, -1);
            }

            //effects-display
            spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftSideBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth, statBarWidth, statBarWidth), Color.White * 0.1f);
            spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftSideBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 2), statBarWidth, statBarWidth), Color.White * 0.1f);
            spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftSideBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 4), statBarWidth, statBarWidth), Color.White * 0.1f);

            if (player.stats.CheckEffectTime(Effects.poison) > 0)
            {
                spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftSideBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth, statBarWidth, statBarWidth), Color.White);
            }

            if (player.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftSideBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 2), statBarWidth, statBarWidth), Color.White);
            }

            if (player.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftSideBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 4), statBarWidth, statBarWidth), Color.White);
            }

            //text till bars
            Vector2 textSizeHP = comicSans.MeasureString("HP: " + player.stats.CheckStat(Stat.health));
            Vector2 originHP = new Vector2(textSizeHP.X * 0.5f, 0);
            spriteBatch.DrawString(comicSans, "HP: " + player.stats.CheckStat(Stat.health),
                new Vector2(leftSideBarRect.X + (sideBarWidth / 2), windowHeight - sideBarWidth),
                Color.Red, 0, originHP, 2, SpriteEffects.None, 0);

            Vector2 textSizeMP = comicSans.MeasureString("MP: " + player.stats.CheckStat(Stat.mana));
            Vector2 originMP = new Vector2(textSizeMP.X * 0.5f, 0);
            spriteBatch.DrawString(comicSans, "MP: " + player.stats.CheckStat(Stat.mana),
                new Vector2(rightSideBarRect.X + (sideBarWidth / 2), windowHeight - sideBarWidth),
                Color.Blue, 0, originMP, 2, SpriteEffects.None, 0);

            Vector2 textSizeXP = comicSans.MeasureString("XP: " + player.stats.CheckStat(Stat.xp));
            Vector2 originXP = new Vector2(textSizeXP.X * 0.5f, 0);
            spriteBatch.DrawString(comicSans, "XP: " + player.stats.CheckStat(Stat.xp),
                new Vector2(rightSideBarRect.X + (sideBarWidth / 2), windowHeight / 2),
                Color.Green, 0, originXP, 2, SpriteEffects.None, 0);

            //text till icons
            Vector2 textSizePoison = comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.poison));
            Vector2 originPoison = new Vector2(textSizePoison.X * 0.5f, 0);
            spriteBatch.DrawString(comicSans, "" + player.stats.CheckEffectTime(Effects.poison),
                new Vector2(leftSideBarRect.X + (sideBarWidth / 2), sideBarWidth + statBarWidth),
                Color.Green, 0, originPoison, 2, SpriteEffects.None, 0);

            Vector2 textSizeBleed = comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.bleed));
            Vector2 originBleed = new Vector2(textSizeBleed.X * 0.5f, 0);
            spriteBatch.DrawString(comicSans, "" + player.stats.CheckEffectTime(Effects.bleed),
                new Vector2(leftSideBarRect.X + (sideBarWidth / 2), sideBarWidth + (statBarWidth * 3)),
                Color.Red, 0, originBleed, 2, SpriteEffects.None, 0);

            Vector2 textSizeConfusion = comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.confusion));
            Vector2 originConfusion = new Vector2(textSizeConfusion.X * 0.5f, 0);
            spriteBatch.DrawString(comicSans, "" + player.stats.CheckEffectTime(Effects.confusion),
                new Vector2(leftSideBarRect.X + (sideBarWidth / 2), sideBarWidth + (statBarWidth * 5)),
                Color.Yellow, 0, originConfusion, 2, SpriteEffects.None, 0);

            //visar experience level
            Vector2 textSizeXpLevel = comicSans.MeasureString("lvl " + "?");
            Vector2 originXpLevel = new Vector2(textSizeXpLevel.X * 0.5f, 0);
            spriteBatch.DrawString(comicSans, "lvl " + "?", new Vector2(rightSideBarRect.X + (sideBarWidth / 2),
                statBarWidth * 2), Color.GhostWhite, 0, originXpLevel, 3, SpriteEffects.None, 0);
        }
    }
}
