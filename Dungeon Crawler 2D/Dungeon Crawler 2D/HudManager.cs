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
    class HUDManager
    {
        private GameState gameState;

        private Texture2D pixelTex;

        private TextureManager textures;

        private Rectangle leftBarRect;
        private Rectangle rightBarRect;
        private Rectangle bottomBarRect;
        private Rectangle topBarRect;
        
        private int windowWidth;
        private int windowHeight;
        public int sideBarWidth;
        private int statBarWidth;

        private Object.Player player;

        public HUDManager(GameState gameState, TextureManager textures, GraphicsDevice graphicsDevice, ContentManager content, Object.Player player, int windowWidth, int windowHeight)
        {
            this.gameState = gameState;
            this.textures = textures;
            this.player = player;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            
            pixelTex = new Texture2D(graphicsDevice, 1, 1);
            pixelTex.SetData<Color>(new Color[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
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

            #region explore

            if (gameState == GameState.Explore)
            {
                //Sätter mått
                sideBarWidth = windowWidth / 10;
                statBarWidth = sideBarWidth / 3;

                leftBarRect = new Rectangle(0, 0, sideBarWidth, windowHeight);
                rightBarRect = new Rectangle(windowWidth - sideBarWidth, 0, sideBarWidth, windowHeight);

                //side-bars där stats visas
                spriteBatch.Draw(pixelTex, leftBarRect, Color.Black);
                spriteBatch.Draw(pixelTex, rightBarRect, Color.Black);

                //health-bar
                spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2), 
                    windowHeight - statBarWidth - (player.stats.CheckStat(Stat.maxHealth) * 2), 
                    statBarWidth, 
                    player.stats.CheckStat(Stat.maxHealth) * 2), 
                    new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

                spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2), 
                    windowHeight - statBarWidth - (player.stats.CheckStat(Stat.health) * 2),
                    statBarWidth, 
                    player.stats.CheckStat(Stat.health) * 2), 
                    new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);

                //mana-bar
                spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2), 
                    windowHeight - statBarWidth - (player.stats.CheckStat(Stat.maxMana) * 2),
                    statBarWidth, 
                    player.stats.CheckStat(Stat.maxMana) * 2), 
                    new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

                spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2), 
                    windowHeight - statBarWidth - (player.stats.CheckStat(Stat.mana) * 2),
                    statBarWidth, 
                    player.stats.CheckStat(Stat.mana) * 2), 
                    new Rectangle(8, 0, 8, textures.barsSheet.Height), Color.White);

                //spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2),
                //    windowHeight - bottomBarRect.Height + statBarWidth,
                //    statBarWidth,
                //    bottomBarRect.Height - (statBarWidth * 2)),
                //    new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

                //spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2),
                //    windowHeight - statBarWidth - (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth))),
                //    statBarWidth,
                //    (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth)))),
                //    new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);


                //experience-bar
                spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), 
                    sideBarWidth, 
                    statBarWidth, 
                    windowHeight - sideBarWidth - statBarWidth),
                    new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

                spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), 
                    (windowHeight / 2) - (player.stats.CheckStat(Stat.xp) * 4), 
                    statBarWidth, 
                    player.stats.CheckStat(Stat.xp) * 4),
                    new Rectangle(16, 0, 8, textures.barsSheet.Height), Color.White);
                
                //effects-display
                spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth, statBarWidth, statBarWidth), Color.White * 0.3f);
                spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 2), statBarWidth, statBarWidth), Color.White * 0.3f);
                spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 4), statBarWidth, statBarWidth), Color.White * 0.3f);

                if (player.stats.CheckEffectTime(Effects.poison) > 0)
                {
                    spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth, statBarWidth, statBarWidth), Color.White);
                }

                if (player.stats.CheckEffectTime(Effects.bleed) > 0)
                {
                    spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 2), statBarWidth, statBarWidth), Color.White);
                }

                if (player.stats.CheckEffectTime(Effects.confusion) > 0)
                {
                    spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 4), statBarWidth, statBarWidth), Color.White);
                }

                //text till bars
                Vector2 textSizeHP = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.health));
                Vector2 originHP = new Vector2(textSizeHP.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.health),
                    new Vector2(leftBarRect.X + (sideBarWidth / 4), windowHeight - statBarWidth),
                    Color.Red, 0, originHP, 2, SpriteEffects.None, 0);

                Vector2 textSizeMP = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.mana));
                Vector2 originMP = new Vector2(textSizeMP.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.mana),
                    new Vector2(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2), windowHeight - statBarWidth),
                    Color.Blue, 0, originMP, 2, SpriteEffects.None, 0);

                Vector2 textSizeXP = textures.comicSans.MeasureString("XP: " + player.stats.CheckStat(Stat.xp));
                Vector2 originXP = new Vector2(textSizeXP.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "XP: " + player.stats.CheckStat(Stat.xp),
                    new Vector2(rightBarRect.X + (sideBarWidth / 2), windowHeight - statBarWidth),
                    Color.Green, 0, originXP, 2, SpriteEffects.None, 0);

                //text till icons
                Vector2 textSizePoison = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.poison));
                Vector2 originPoison = new Vector2(textSizePoison.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.poison),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), sideBarWidth + statBarWidth),
                    Color.Green, 0, originPoison, 2, SpriteEffects.None, 0);

                Vector2 textSizeBleed = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.bleed));
                Vector2 originBleed = new Vector2(textSizeBleed.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.bleed),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), sideBarWidth + (statBarWidth * 3)),
                    Color.Red, 0, originBleed, 2, SpriteEffects.None, 0);

                Vector2 textSizeConfusion = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.confusion));
                Vector2 originConfusion = new Vector2(textSizeConfusion.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.confusion),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), sideBarWidth + (statBarWidth * 5)),
                    Color.Yellow, 0, originConfusion, 2, SpriteEffects.None, 0);

                //visar experience level
                Vector2 textSizeXpLevel = textures.comicSans.MeasureString("lvl " + player.stats.CheckStat(Stat.level));
                Vector2 originXpLevel = new Vector2(textSizeXpLevel.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "lvl " + player.stats.CheckStat(Stat.level), new Vector2(rightBarRect.X + (sideBarWidth / 2),
                    statBarWidth), Color.GhostWhite, 0, originXpLevel, 3, SpriteEffects.None, 0);
            }
            #endregion

            #region battle
            if (gameState == GameState.Battle)
            {
                //Sätter mått
                sideBarWidth = windowWidth / 10;
                statBarWidth = sideBarWidth / 3;

                leftBarRect = new Rectangle(0, 0, sideBarWidth, windowHeight);
                rightBarRect = new Rectangle(windowWidth - sideBarWidth, 0, sideBarWidth, windowHeight);
                bottomBarRect = new Rectangle(0, windowHeight - (sideBarWidth * 2), windowWidth, sideBarWidth * 2);
                topBarRect = new Rectangle(0, 0, windowWidth, sideBarWidth);


                //side-bars där stats visas
                spriteBatch.Draw(pixelTex, leftBarRect, Color.LightBlue);
                spriteBatch.Draw(pixelTex, rightBarRect, Color.LightBlue);
                spriteBatch.Draw(pixelTex, bottomBarRect, Color.LightBlue);
                spriteBatch.Draw(pixelTex, topBarRect, Color.LightBlue);

                //effects-display
                spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth, statBarWidth, statBarWidth), Color.White * 0.3f);
                spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 2), statBarWidth, statBarWidth), Color.White * 0.3f);
                spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 4), statBarWidth, statBarWidth), Color.White * 0.3f);

                if (player.stats.CheckEffectTime(Effects.poison) > 0)
                {
                    spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth, statBarWidth, statBarWidth), Color.White);
                }

                if (player.stats.CheckEffectTime(Effects.bleed) > 0)
                {
                    spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 2), statBarWidth, statBarWidth), Color.White);
                }

                if (player.stats.CheckEffectTime(Effects.confusion) > 0)
                {
                    spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), sideBarWidth + (statBarWidth * 4), statBarWidth, statBarWidth), Color.White);
                }

                //health-bars
                //spelaren
                spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2), 
                    windowHeight - bottomBarRect.Height + statBarWidth,
                    statBarWidth, 
                    bottomBarRect.Height - (statBarWidth * 2)), 
                    new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

                spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2),
                    windowHeight - statBarWidth - (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth))),
                    statBarWidth,
                    (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth)))),
                    new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);

                //fienden
                spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2),
                    windowHeight - bottomBarRect.Height + statBarWidth,
                    statBarWidth,
                    bottomBarRect.Height - (statBarWidth * 2)),
                    new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

                spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2),
                    windowHeight - statBarWidth - (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth))),
                    statBarWidth,
                    (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth)))),
                    new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);

                //mana-bars
                //spelaren
                spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2),
                    windowHeight - bottomBarRect.Height + statBarWidth,
                    statBarWidth, 
                    bottomBarRect.Height - (statBarWidth * 2)), 
                    new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

                spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2),
                    windowHeight - statBarWidth - (player.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxMana))),
                    statBarWidth,
                    (player.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxMana)))), 
                    new Rectangle(8, 0, 8, textures.barsSheet.Height), Color.White);

                //fienden
                spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2),
                    windowHeight - bottomBarRect.Height + statBarWidth,
                    statBarWidth,
                    bottomBarRect.Height - (statBarWidth * 2)),
                    new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

                spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2),
                    windowHeight - statBarWidth - (player.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxMana))),
                    statBarWidth,
                    (player.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxMana)))),
                    new Rectangle(8, 0, 8, textures.barsSheet.Height), Color.White);


                //text till bars
                //spelaren
                Vector2 textSizePlayerHP = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.health) + "/" + player.stats.CheckStat(Stat.maxHealth));
                Vector2 originPlayerHP = new Vector2(textSizePlayerHP.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.health) + "/" + player.stats.CheckStat(Stat.maxHealth),
                    new Vector2(leftBarRect.X + (sideBarWidth / 4), windowHeight - bottomBarRect.Height),
                    Color.Red, 0, originPlayerHP, 1, SpriteEffects.None, 0);

                Vector2 textSizePlayerMP = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.mana) + "/" + player.stats.CheckStat(Stat.maxMana));
                Vector2 originPlayerMP = new Vector2(textSizePlayerMP.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.mana) + "/" + player.stats.CheckStat(Stat.maxMana),
                    new Vector2(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2), windowHeight - bottomBarRect.Height),
                    Color.Blue, 0, originPlayerMP, 1, SpriteEffects.None, 0);

                //fienden
                Vector2 textSizeEnemyHP = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.health) + "/" + player.stats.CheckStat(Stat.maxHealth));
                Vector2 originEnemyHP = new Vector2(textSizeEnemyHP.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.health) + "/" + player.stats.CheckStat(Stat.maxHealth),
                    new Vector2(rightBarRect.X + (sideBarWidth / 4), windowHeight - bottomBarRect.Height),
                    Color.Red, 0, originEnemyHP, 1, SpriteEffects.None, 0);

                Vector2 textSizeEnemyMP = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.mana) + "/" + player.stats.CheckStat(Stat.maxMana));
                Vector2 originEnemyMP = new Vector2(textSizeEnemyMP.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.mana) + "/" + player.stats.CheckStat(Stat.maxMana),
                    new Vector2(rightBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2), windowHeight - bottomBarRect.Height),
                    Color.Blue, 0, originEnemyMP, 1, SpriteEffects.None, 0);
            }
            #endregion
        }
    }
}
