using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon_Crawler_2D.Menus;

namespace Dungeon_Crawler_2D
{
    class HUDManager
    {
        public Texture2D pixelTex;

        private TextureManager textures;

        public StatScreen statScreen;
        public bool battleWon;
        public int gainedXp;

        private Rectangle leftBarRect;
        private Rectangle rightBarRect;
        private Rectangle bottomBarRect;
        private Rectangle topBarRect;
        private Rectangle bottomMiddleBarRect;

        public int windowWidth;
        public int windowHeight;
        public int sideBarWidth;
        public int statBarWidth;

        //Specialeffekt-relaterat
        private float flashTimer;
        private bool flashTimerToggle;
        public float flashAlpha;
        private float abilityButtonAlpha;

        private float textScale;
        private float textScaleTimer;
        
        public string turnEvents;

        public Object.Player player;

        public HUDManager(TextureManager textures, GraphicsDevice graphicsDevice, ContentManager content, Object.Player player, int windowWidth, int windowHeight)
        {
            this.textures = textures;
            this.player = player;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            pixelTex = new Texture2D(graphicsDevice, 1, 1);
            pixelTex.SetData<Color>(new Color[] { Color.White });

            flashTimer = 0;
            flashTimerToggle = false;
            abilityButtonAlpha = 1f;

            turnEvents = "Plan your move...";
            textScaleTimer = 10;

            //Sätter mått
            sideBarWidth = windowWidth / 10;
            statBarWidth = sideBarWidth / 3;

            leftBarRect = new Rectangle(0, 0, sideBarWidth, windowHeight);
            rightBarRect = new Rectangle(windowWidth - sideBarWidth, 0, sideBarWidth, windowHeight);
            bottomBarRect = new Rectangle(leftBarRect.Width, windowHeight - (sideBarWidth * 2), windowWidth - (sideBarWidth * 2), sideBarWidth * 2);
            topBarRect = new Rectangle(leftBarRect.Width, 0, windowWidth - (leftBarRect.Width * 2), sideBarWidth);
            bottomMiddleBarRect = new Rectangle(leftBarRect.Width * 2, windowHeight - bottomBarRect.Height, windowWidth - (leftBarRect.Width * 4), bottomBarRect.Height);

            statScreen = new StatScreen(this, textures);
        }

        public void Update()
        {
            #region Test statIncrese / Decrease
            if (Keyboard.GetState().IsKeyDown(Keys.V))
            {
                player.stats.AddEffect(1, Effects.poison, 1);
                player.stats.AddEffect(1, Effects.bleed, 1);
                player.stats.AddEffect(1, Effects.confusion, 1);

                player.stats.ChangeStat(Stat.health, 1);
                player.stats.ChangeStat(Stat.mana, 1);
                player.stats.ChangeStat(Stat.xp, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                player.stats.AddEffect(-1, Effects.poison, 1);
                player.stats.AddEffect(-1, Effects.bleed, 1);
                player.stats.AddEffect(-1, Effects.confusion, 1);

                player.stats.ChangeStat(Stat.health, -1);
                player.stats.ChangeStat(Stat.mana, -1);
                player.stats.ChangeStat(Stat.xp, -1);
            }
            #endregion

            #region Icon flash
            // making icons flash
            if (flashTimer <= 0)
            {
                flashTimerToggle = false;
            }
            else if (flashTimer >= 20)
            {
                flashTimerToggle = true;
            }

            if (flashTimerToggle == false)
            {
                flashTimer += 1;
            }
            else
            {
                flashTimer -= 1;
            }
            flashAlpha = flashTimer * 0.05f;

            #endregion 

            #region Scale Text in info-box
            // making text scale up in info-box
            if (textScaleTimer < 20 && textScaleTimer > 0)
            {
                textScaleTimer += 1;
            }
            else if (textScaleTimer >= 20)
            {
                textScaleTimer = 20;
            }

            textScale = textScaleTimer * 0.1f;

            #endregion
            
        }

        public void DrawExplore(SpriteBatch spriteBatch)
        {
            #region Sidebar for stats
            spriteBatch.Draw(pixelTex, leftBarRect, Color.Black);
            spriteBatch.Draw(pixelTex, rightBarRect, Color.Black);
            #endregion 

            #region lines to separate regions
            spriteBatch.Draw(pixelTex, new Rectangle(sideBarWidth, 0, statBarWidth / 20, windowHeight), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(windowWidth - sideBarWidth - (statBarWidth / 20), 0, statBarWidth / 20, windowHeight), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(0, statBarWidth * 8, sideBarWidth, statBarWidth / 20), Color.White);
            #endregion

            #region health bar
            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2),
                (windowHeight / 2) - statBarWidth,
                statBarWidth,
                (windowHeight / 2)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.health) * (windowHeight / 2) / (player.stats.CheckStat(Stat.maxHealth))),
                statBarWidth,
                (player.stats.CheckStat(Stat.health) * (windowHeight / 2) / player.stats.CheckStat(Stat.maxHealth))),
                new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);
            #endregion

            #region mana bar
            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2),
                (windowHeight / 2) - statBarWidth,
                statBarWidth,
                (windowHeight / 2)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.mana) * (windowHeight / 2) / (player.stats.CheckStat(Stat.maxMana))),
                statBarWidth,
                (player.stats.CheckStat(Stat.mana) * (windowHeight / 2) / player.stats.CheckStat(Stat.maxMana))),
                new Rectangle(8, 0, 8, textures.barsSheet.Height), Color.White);
            #endregion

            #region experience bar
            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2),
                sideBarWidth,
                statBarWidth,
                windowHeight - sideBarWidth - statBarWidth),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.xp) * (windowHeight - sideBarWidth - statBarWidth) / (player.stats.CheckStat(Stat.maxXp))),
                statBarWidth,
                (player.stats.CheckStat(Stat.xp) * (windowHeight - sideBarWidth - statBarWidth) / (player.stats.CheckStat(Stat.maxXp)))),
                new Rectangle(16, 0, 8, textures.barsSheet.Height), Color.White);
            #endregion

            #region Effect display
            spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), statBarWidth, statBarWidth, statBarWidth), Color.White * 0.2f);
            spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), statBarWidth + (statBarWidth * 2), statBarWidth, statBarWidth), Color.White * 0.2f);
            spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), statBarWidth + (statBarWidth * 4), statBarWidth, statBarWidth), Color.White * 0.2f);

            if (player.stats.CheckEffectTime(Effects.poison) > 0)
            {
                spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), statBarWidth, statBarWidth, statBarWidth), Color.White * flashAlpha);
            }

            if (player.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), statBarWidth + (statBarWidth * 2), statBarWidth, statBarWidth), Color.White * flashAlpha);
            }

            if (player.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2), statBarWidth + (statBarWidth * 4), statBarWidth, statBarWidth), Color.White * flashAlpha);
            }
            #endregion

            #region Text to bars
            #region Hp
            Vector2 textSizeHPNr = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.health));
            Vector2 originHPNr = new Vector2(textSizeHPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.health),
                new Vector2(leftBarRect.X + (sideBarWidth / 4), windowHeight - statBarWidth),
                Color.Red, 0, originHPNr, 2, SpriteEffects.None, 0);

            Vector2 textSizeHPTxt = textures.comicSans.MeasureString("HP");
            Vector2 originHPTxt = new Vector2(textSizeHPTxt.X * 0.5f, textSizeHPTxt.Y * 0.8f);
            spriteBatch.DrawString(textures.comicSans, ("HP"),
                new Vector2(leftBarRect.X + (sideBarWidth / 4), (windowHeight / 2) - statBarWidth),
                Color.Red, 0, originHPTxt, 2, SpriteEffects.None, 0);
            #endregion

            #region Mana
            Vector2 textSizeMPNr = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.mana));
            Vector2 originMPNr = new Vector2(textSizeMPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.mana),
                new Vector2(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Blue, 0, originMPNr, 2, SpriteEffects.None, 0);

            Vector2 textSizeMPTxt = textures.comicSans.MeasureString("MP");
            Vector2 originMPTxt = new Vector2(textSizeMPTxt.X * 0.5f, textSizeMPTxt.Y * 0.8f);
            spriteBatch.DrawString(textures.comicSans, ("MP"),
                new Vector2(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2), (windowHeight / 2) - statBarWidth),
                Color.Blue, 0, originMPTxt, 2, SpriteEffects.None, 0);
            #endregion

            #region experience
            Vector2 textSizeXPNr = textures.comicSans.MeasureString("XP: " + player.stats.CheckStat(Stat.xp));
            Vector2 originXPNr = new Vector2(textSizeXPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, "XP: " + player.stats.CheckStat(Stat.xp),
                new Vector2(rightBarRect.X + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Green, 0, originXPNr, 2, SpriteEffects.None, 0);
            #endregion
            #endregion

            #region Text to icons
            #region poison
            if (player.stats.CheckEffectTime(Effects.poison) > 0)
            {
                Vector2 textSizePoison = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.poison));
                Vector2 originPoison = new Vector2(textSizePoison.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.poison),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), statBarWidth + statBarWidth),
                    Color.Green, 0, originPoison, 2, SpriteEffects.None, 0);
            }
            #endregion

            #region bleed
            if (player.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                Vector2 textSizeBleed = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.bleed));
                Vector2 originBleed = new Vector2(textSizeBleed.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.bleed),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), statBarWidth + (statBarWidth * 3)),
                    Color.Red, 0, originBleed, 2, SpriteEffects.None, 0);
            }
            #endregion

            #region confusion
            if (player.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                Vector2 textSizeConfusion = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.confusion));
                Vector2 originConfusion = new Vector2(textSizeConfusion.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.confusion),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), statBarWidth + (statBarWidth * 5)),
                    Color.Yellow, 0, originConfusion, 2, SpriteEffects.None, 0);
            }
            #endregion
            #endregion

            #region level
            Vector2 textSizeXpLevel = textures.comicSans.MeasureString("lvl " + player.stats.CheckStat(Stat.level));
            Vector2 originXpLevel = new Vector2(textSizeXpLevel.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, "lvl " + player.stats.CheckStat(Stat.level),
                new Vector2(rightBarRect.X + (sideBarWidth / 2),
                statBarWidth), Color.GhostWhite, 0, originXpLevel, 3, SpriteEffects.None, 0);

            #endregion

            #region StatpointsPopup
            if (player.stats.upgrade > 0 && GameSettings.gameState == GameState.Explore)
            { 
                
                Vector2 textSizeUpgradeNotification = textures.comicSans.MeasureString("Q: Statpoints ready for usage!");
                Vector2 originUpgradeNotification = new Vector2(textSizeUpgradeNotification.X * 0.5f, 0);

                spriteBatch.DrawString(textures.comicSans, "Q: Statpoints ready for usage!",
                        new Vector2(windowWidth / 2, windowHeight - (statBarWidth * 2)), Color.Yellow * flashAlpha,
                        0, originUpgradeNotification, 3, SpriteEffects.None, 0);
            }

            if (GameSettings.gameState == GameState.Inventory && statScreen.showSummary == false)
            {

                Vector2 textSizeUpgradeNotification = textures.comicSans.MeasureString("E: Apply Statpoints to selected Stat!");
                Vector2 originUpgradeNotification = new Vector2(textSizeUpgradeNotification.X * 0.5f, 0);

                spriteBatch.DrawString(textures.comicSans, "E: Apply Statpoints to selected Stat!",
                        new Vector2(windowWidth / 2, windowHeight - (statBarWidth * 2)), Color.Yellow * flashAlpha,
                        0, originUpgradeNotification, 3, SpriteEffects.None, 0);
            }
            #endregion
        }

        public void DrawInventory(SpriteBatch spriteBatch)
        {
            statScreen.Draw(spriteBatch);
        }

        public void DrawBattle(SpriteBatch spriteBatch, Combat combat)
        {
            spriteBatch.Draw(textures.battleBackGround3, new Rectangle(leftBarRect.X + leftBarRect.Width,
                topBarRect.Y + topBarRect.Height,
                windowWidth - (sideBarWidth * 2),
                windowHeight - topBarRect.Height - bottomBarRect.Height),
                Color.White);

            #region stat bar
            spriteBatch.Draw(pixelTex, leftBarRect, Color.Black);
            spriteBatch.Draw(pixelTex, rightBarRect, Color.Black);
            spriteBatch.Draw(pixelTex, bottomBarRect, Color.Black);
            spriteBatch.Draw(pixelTex, topBarRect, Color.Black);
            #endregion

            #region lines to seperate sektions
            spriteBatch.Draw(pixelTex, new Rectangle(sideBarWidth, 0, statBarWidth / 20, windowHeight - bottomBarRect.Height), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(windowWidth - sideBarWidth - (statBarWidth / 20), 0, statBarWidth / 20, windowHeight - bottomBarRect.Height), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(sideBarWidth, topBarRect.Height, topBarRect.Width, statBarWidth / 20), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(0, windowHeight - bottomBarRect.Height, windowWidth, statBarWidth / 20), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(sideBarWidth * 2, windowHeight - bottomBarRect.Height, statBarWidth / 20, bottomBarRect.Height), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(windowWidth - (sideBarWidth * 2) - (statBarWidth / 20), windowHeight - bottomBarRect.Height, statBarWidth / 20, bottomBarRect.Height), Color.White);
            #endregion

            #region effect display
            #region player's
            spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 4), sideBarWidth, sideBarWidth / 2, sideBarWidth / 2), Color.White * 0.2f);
            spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 4), sideBarWidth * 2, sideBarWidth / 2, sideBarWidth / 2), Color.White * 0.2f);
            spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 4), sideBarWidth * 3, sideBarWidth / 2, sideBarWidth / 2), Color.White * 0.2f);

            if (player.stats.CheckEffectTime(Effects.poison) > 0)
            {
                spriteBatch.Draw(textures.poisonIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 4), sideBarWidth, sideBarWidth / 2, sideBarWidth / 2), Color.White * flashAlpha);
            }

            if (player.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                spriteBatch.Draw(textures.bleedIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 4), sideBarWidth * 2, sideBarWidth / 2, sideBarWidth / 2), Color.White * flashAlpha);
            }

            if (player.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                spriteBatch.Draw(textures.confusionIcon, new Rectangle(leftBarRect.X + (sideBarWidth / 4), sideBarWidth * 3, sideBarWidth / 2, sideBarWidth / 2), Color.White * flashAlpha);
            }
            #endregion

            #region enemy's
            spriteBatch.Draw(textures.poisonIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth, sideBarWidth / 2, sideBarWidth / 2), Color.White * 0.3f);
            spriteBatch.Draw(textures.bleedIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth * 2, sideBarWidth / 2, sideBarWidth / 2), Color.White * 0.3f);
            spriteBatch.Draw(textures.confusionIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth * 3, sideBarWidth / 2, sideBarWidth / 2), Color.White * 0.3f);

            if (combat.enemy.stats.CheckEffectTime(Effects.poison) > 0)
            {
                spriteBatch.Draw(textures.poisonIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth, sideBarWidth / 2, sideBarWidth / 2), Color.White * flashAlpha);
            }

            if (combat.enemy.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                spriteBatch.Draw(textures.bleedIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth * 2, sideBarWidth / 2, sideBarWidth / 2), Color.White * flashAlpha);
            }

            if (combat.enemy.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                spriteBatch.Draw(textures.confusionIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth * 3, sideBarWidth / 2, sideBarWidth / 2), Color.White * flashAlpha);
            }
            #endregion
            #endregion

            #region bars
            #region health
            #region player's
            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - bottomBarRect.Height + statBarWidth,
                statBarWidth,
                bottomBarRect.Height - (statBarWidth * 2)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth))),
                statBarWidth,
                (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth)))),
                new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);
            #endregion

            #region enemy's
            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X - sideBarWidth + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - bottomBarRect.Height + statBarWidth,
                statBarWidth,
                bottomBarRect.Height - (statBarWidth * 2)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X - sideBarWidth + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (combat.enemy.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (combat.enemy.stats.CheckStat(Stat.maxHealth))),
                statBarWidth,
                (combat.enemy.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (combat.enemy.stats.CheckStat(Stat.maxHealth)))),
                new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);
            #endregion
            #endregion

            #region mana
            #region player's
            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + sideBarWidth + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - bottomBarRect.Height + statBarWidth,
                statBarWidth,
                bottomBarRect.Height - (statBarWidth * 2)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + sideBarWidth + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxMana))),
                statBarWidth,
                (player.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxMana)))),
                new Rectangle(8, 0, 8, textures.barsSheet.Height), Color.White);
            #endregion 

            #region enemy's
            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - bottomBarRect.Height + statBarWidth,
                statBarWidth,
                bottomBarRect.Height - (statBarWidth * 2)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (combat.enemy.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (combat.enemy.stats.CheckStat(Stat.maxMana))),
                statBarWidth,
                (combat.enemy.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (combat.enemy.stats.CheckStat(Stat.maxMana)))),
                new Rectangle(8, 0, 8, textures.barsSheet.Height), Color.White);
            #endregion
            #endregion
            #endregion

            #region text to bars
            #region player
            #region hp
            Vector2 textSizePlayerHPNr = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.health) + "/" + player.stats.CheckStat(Stat.maxHealth));
            Vector2 originPlayerHPNr = new Vector2(textSizePlayerHPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, ("" + player.stats.CheckStat(Stat.health) + "/" + player.stats.CheckStat(Stat.maxHealth)),
                new Vector2(leftBarRect.X + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Red, 0, originPlayerHPNr, 2, SpriteEffects.None, 0);

            Vector2 textSizePlayerHPTxt = textures.comicSans.MeasureString("HP");
            Vector2 originPlayerHPTxt = new Vector2(textSizePlayerHPTxt.X * 0.5f, textSizePlayerHPTxt.Y * 0.8f);
            spriteBatch.DrawString(textures.comicSans, ("HP"),
                new Vector2(leftBarRect.X + (sideBarWidth / 2), windowHeight - bottomBarRect.Height + statBarWidth),
                Color.Red, 0, originPlayerHPTxt, 2, SpriteEffects.None, 0);
            #endregion

            #region mana
            Vector2 textSizePlayerMPNr = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.mana) + "/" + player.stats.CheckStat(Stat.maxMana));
            Vector2 originPlayerMNr = new Vector2(textSizePlayerMPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, ("" + player.stats.CheckStat(Stat.mana) + "/" + player.stats.CheckStat(Stat.maxMana)),
                new Vector2(leftBarRect.X + sideBarWidth + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Blue, 0, originPlayerMNr, 2, SpriteEffects.None, 0);

            Vector2 textSizePlayerMPTxt = textures.comicSans.MeasureString("MP");
            Vector2 originPlayerMPTxt = new Vector2(textSizePlayerMPTxt.X * 0.5f, textSizePlayerMPTxt.Y * 0.8f);
            spriteBatch.DrawString(textures.comicSans, ("MP"),
                new Vector2(leftBarRect.X + sideBarWidth + (sideBarWidth / 2), windowHeight - bottomBarRect.Height + statBarWidth),
                Color.Blue, 0, originPlayerMPTxt, 2, SpriteEffects.None, 0);
            #endregion
            #endregion

            #region enemy's
            #region hp
            Vector2 textSizeEnemyHPNr = textures.comicSans.MeasureString("" + combat.enemy.stats.CheckStat(Stat.health) + "/" + combat.enemy.stats.CheckStat(Stat.maxHealth));
            Vector2 originEnemyHPNr = new Vector2(textSizeEnemyHPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, ("" + combat.enemy.stats.CheckStat(Stat.health) + "/" + combat.enemy.stats.CheckStat(Stat.maxHealth)),
                new Vector2(rightBarRect.X - sideBarWidth + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Red, 0, originEnemyHPNr, 2, SpriteEffects.None, 0);

            Vector2 textSizeEnemyHPTxt = textures.comicSans.MeasureString("HP");
            Vector2 originEnemyHPTxt = new Vector2(textSizeEnemyHPTxt.X * 0.5f, textSizeEnemyHPTxt.Y * 0.8f);
            spriteBatch.DrawString(textures.comicSans, ("HP"),
                new Vector2(rightBarRect.X - sideBarWidth + (sideBarWidth / 2), windowHeight - bottomBarRect.Height + statBarWidth),
                Color.Red, 0, originEnemyHPTxt, 2, SpriteEffects.None, 0);
            #endregion

            #region mana
            Vector2 textSizeEnemyMPNr = textures.comicSans.MeasureString("" + combat.enemy.stats.CheckStat(Stat.mana) + "/" + combat.enemy.stats.CheckStat(Stat.maxMana));
            Vector2 originEnemyMPNr = new Vector2(textSizeEnemyMPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, ("" + combat.enemy.stats.CheckStat(Stat.mana) + "/" + combat.enemy.stats.CheckStat(Stat.maxMana)),
                new Vector2(rightBarRect.X + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Blue, 0, originEnemyMPNr, 2, SpriteEffects.None, 0);

            Vector2 textSizeEnemyMPTxt = textures.comicSans.MeasureString("MP");
            Vector2 originEnemyMPTxt = new Vector2(textSizeEnemyMPTxt.X * 0.5f, textSizeEnemyMPTxt.Y) * 0.8f;
            spriteBatch.DrawString(textures.comicSans, ("MP"),
                new Vector2(rightBarRect.X + (sideBarWidth / 2), windowHeight - bottomBarRect.Height + statBarWidth),
                Color.Blue, 0, originEnemyMPTxt, 2, SpriteEffects.None, 0);
            #endregion
            #endregion
            #endregion

            #region text to icons
            #region player's
            #region poison
            if (player.stats.CheckEffectTime(Effects.poison) > 0)
            {
                Vector2 textSizePlayerPoison = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.poison));
                Vector2 originPlayerPoison = new Vector2(textSizePlayerPoison.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.poison),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), sideBarWidth + (sideBarWidth / 2)),
                    Color.Green, 0, originPlayerPoison, 2, SpriteEffects.None, 0);
            }
            #endregion

            #region bleed
            if (player.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                Vector2 textSizePlayerBleed = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.bleed));
                Vector2 originPlayerBleed = new Vector2(textSizePlayerBleed.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.bleed),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), sideBarWidth * 2 + (sideBarWidth / 2)),
                    Color.Red, 0, originPlayerBleed, 2, SpriteEffects.None, 0);
            }
            #endregion

            #region confusion
            if (player.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                Vector2 textSizePlayerConfusion = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.confusion));
                Vector2 originPlayerConfusion = new Vector2(textSizePlayerConfusion.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.confusion),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), sideBarWidth * 3 + (sideBarWidth / 2)),
                    Color.Yellow, 0, originPlayerConfusion, 2, SpriteEffects.None, 0);
            }
            #endregion
            #endregion

            #region enemy's
            #region poison
            if (combat.enemy.stats.CheckEffectTime(Effects.poison) > 0)
            {
                Vector2 textSizeEnemyPoison = textures.comicSans.MeasureString("" + combat.enemy.stats.CheckEffectTime(Effects.poison));
                Vector2 originEnemyPoison = new Vector2(textSizeEnemyPoison.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + combat.enemy.stats.CheckEffectTime(Effects.poison),
                    new Vector2(rightBarRect.X + (sideBarWidth / 2), sideBarWidth + (sideBarWidth / 2)),
                    Color.Green, 0, originEnemyPoison, 2, SpriteEffects.None, 0);
            }
            #endregion

            #region bleed
            if (combat.enemy.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                Vector2 textSizeEnemyBleed = textures.comicSans.MeasureString("" + combat.enemy.stats.CheckEffectTime(Effects.bleed));
                Vector2 originEnemyBleed = new Vector2(textSizeEnemyBleed.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + combat.enemy.stats.CheckEffectTime(Effects.bleed),
                    new Vector2(rightBarRect.X + (sideBarWidth / 2), sideBarWidth * 2 + (sideBarWidth / 2)),
                    Color.Red, 0, originEnemyBleed, 2, SpriteEffects.None, 0);
            }
            #endregion

            #region confusion
            if (combat.enemy.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                Vector2 textSizeEnemyConfusion = textures.comicSans.MeasureString("" + combat.enemy.stats.CheckEffectTime(Effects.confusion));
                Vector2 originEnemyConfusion = new Vector2(textSizeEnemyConfusion.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + combat.enemy.stats.CheckEffectTime(Effects.confusion),
                    new Vector2(rightBarRect.X + (sideBarWidth / 2), sideBarWidth * 3 + (sideBarWidth / 2)),
                    Color.Yellow, 0, originEnemyConfusion, 2, SpriteEffects.None, 0);
            }
            #endregion
            #endregion
            #endregion

            #region ability buttons

            Vector2 buttonSize = new Vector2(sideBarWidth * 2, sideBarWidth / 2);
            Rectangle qAbility = new Rectangle(bottomMiddleBarRect.X + (sideBarWidth / 2), bottomMiddleBarRect.Y + (sideBarWidth / 4), (int)buttonSize.X, (int)buttonSize.Y);
            Rectangle wAbility = new Rectangle(bottomMiddleBarRect.X + bottomMiddleBarRect.Width - (sideBarWidth / 2) - (int)buttonSize.X, bottomMiddleBarRect.Y + (sideBarWidth / 4), (int)buttonSize.X, (int)buttonSize.Y);
            Rectangle eAbility = new Rectangle(bottomMiddleBarRect.X + (sideBarWidth / 2), bottomMiddleBarRect.Y + bottomMiddleBarRect.Height - ((int)buttonSize.Y) - (sideBarWidth / 4), (int)buttonSize.X, (int)buttonSize.Y);
            Rectangle rAbility = new Rectangle(bottomMiddleBarRect.X + bottomMiddleBarRect.Width - (sideBarWidth / 2) - (int)buttonSize.X, bottomMiddleBarRect.Y + bottomMiddleBarRect.Height - (int)buttonSize.Y - (sideBarWidth / 4), (int)buttonSize.X, (int)buttonSize.Y);

            //outlines
            spriteBatch.Draw(textures.whiteSquare, qAbility, Color.White);
            spriteBatch.Draw(textures.whiteSquare, wAbility, Color.White);
            spriteBatch.Draw(textures.whiteSquare, eAbility, Color.White);
            spriteBatch.Draw(textures.whiteSquare, rAbility, Color.White);

            //inner box
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(qAbility.X + (statBarWidth / 20), qAbility.Y + (statBarWidth / 20), qAbility.Width - (statBarWidth / 10), qAbility.Height - (statBarWidth / 10)), Color.Black);
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(wAbility.X + (statBarWidth / 20), wAbility.Y + (statBarWidth / 20), wAbility.Width - (statBarWidth / 10), wAbility.Height - (statBarWidth / 10)), Color.Black);
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(eAbility.X + (statBarWidth / 20), eAbility.Y + (statBarWidth / 20), eAbility.Width - (statBarWidth / 10), eAbility.Height - (statBarWidth / 10)), Color.Black);
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(rAbility.X + (statBarWidth / 20), rAbility.Y + (statBarWidth / 20), rAbility.Width - (statBarWidth / 10), rAbility.Height - (statBarWidth / 10)), Color.Black);

            //text
            Vector2 textSizeAbility;
            Vector2 originTextAbility;

            textSizeAbility = (textures.comicSans.MeasureString("Q: Hit attack"));
            originTextAbility = textSizeAbility * 0.5f;
            spriteBatch.DrawString(textures.comicSans, "Q: Hit attack", new Vector2(qAbility.X + (qAbility.Width / 2), qAbility.Y + (qAbility.Height / 2)),
                Color.Yellow, 0, originTextAbility, 2, SpriteEffects.None, 0);

            if (player.abilities.CheckCost(UsedAbility.Magic) > player.stats.CheckStat(Stat.mana))
            {
                abilityButtonAlpha = 0.2f;
            }
            textSizeAbility = (textures.comicSans.MeasureString("W: Magic attack"));
            originTextAbility = textSizeAbility * 0.5f;
            spriteBatch.DrawString(textures.comicSans, "W: Magic attack", new Vector2(wAbility.X + (wAbility.Width / 2), wAbility.Y + (wAbility.Height / 2)),
                Color.Yellow * abilityButtonAlpha, 0, originTextAbility, 2, SpriteEffects.None, 0);
            abilityButtonAlpha = 1f;

            textSizeAbility = (textures.comicSans.MeasureString("E: Dodge"));
            originTextAbility = textSizeAbility * 0.5f;
            spriteBatch.DrawString(textures.comicSans, "E: Dodge", new Vector2(eAbility.X + (eAbility.Width / 2), eAbility.Y + (eAbility.Height / 2)),
                Color.Yellow, 0, originTextAbility, 2, SpriteEffects.None, 0);

            if (player.abilities.CheckCost(UsedAbility.PoisonHit) > player.stats.CheckStat(Stat.mana))
            {
                abilityButtonAlpha = 0.2f;
            }
            textSizeAbility = (textures.comicSans.MeasureString("R: Poison Hit"));
            originTextAbility = textSizeAbility * 0.5f;
            spriteBatch.DrawString(textures.comicSans, "R: Poison Hit", new Vector2(rAbility.X + (rAbility.Width / 2), rAbility.Y + (rAbility.Height / 2)),
                Color.Yellow * abilityButtonAlpha, 0, originTextAbility, 2, SpriteEffects.None, 0);
            abilityButtonAlpha = 1f;

            #endregion

            #region battleText

            //får texten plats i rutan?
            if (textures.comicSans.MeasureString(turnEvents).X * 2 <= topBarRect.Width - statBarWidth)
            {
                Vector2 textSizeInfo = textures.comicSans.MeasureString(turnEvents);
                Vector2 originInfo = new Vector2(textSizeInfo.X * 0.5f, textSizeInfo.Y * 0.5f);
                spriteBatch.DrawString(textures.comicSans, turnEvents,
                    new Vector2(topBarRect.X + (topBarRect.Width / 2), topBarRect.Height / 2),
                    Color.Yellow, 0, originInfo, textScale, SpriteEffects.None, 0);
            }
            //annars... dela upp strängen efter 70 karaktärer, minus hur långt bak det sista "mellanslaget" fanns i strängen.
            else
            {
                int chunksize = 70;
                int lastSpace = 0;
                int rowNumber = 0;
                int stringLength = turnEvents.Length;
                Vector2 textSizeInfo;
                Vector2 originInfo;
                for (int i = 0; i < stringLength; i += chunksize)
                {
                    if (i + chunksize > stringLength)
                    {
                        chunksize = stringLength - i;
                    }
                    lastSpace = chunksize - turnEvents.Substring(i, chunksize).LastIndexOf(' ');

                    if (i >= lastSpace)
                    {
                        textSizeInfo = textures.comicSans.MeasureString(turnEvents.Substring(i, chunksize));
                        originInfo = new Vector2(textSizeInfo.X * 0.5f, textSizeInfo.Y * 0.5f);

                        spriteBatch.DrawString(textures.comicSans, turnEvents.Substring(i, chunksize),
                            new Vector2(topBarRect.X + (topBarRect.Width / 2), (topBarRect.Height / 3) + ((topBarRect.Height / 3) * rowNumber)),
                            Color.Yellow, 0, originInfo, textScale, SpriteEffects.None, 0);
                        rowNumber += 1;
                    }
                    else
                    {
                        textSizeInfo = textures.comicSans.MeasureString(turnEvents.Substring(i, chunksize - lastSpace));
                        originInfo = new Vector2(textSizeInfo.X * 0.5f, textSizeInfo.Y * 0.5f);

                        spriteBatch.DrawString(textures.comicSans, turnEvents.Substring(i, chunksize - lastSpace),
                            new Vector2(topBarRect.X + (topBarRect.Width / 2), (topBarRect.Height / 3) + ((topBarRect.Height / 3) * rowNumber)),
                            Color.Yellow, 0, originInfo, textScale, SpriteEffects.None, 0);
                        i -= lastSpace;
                        rowNumber += 1;
                    }
                }
            }
            #endregion

        }

        public void CombatText(int combatLine, Enemy enemy)
        {
            textScaleTimer = 10;
            switch (combatLine)
            {
                case 0:
                    turnEvents = "And nothing happened that round";
                    break;
                case 1:
                    turnEvents = "Viking defends against " + enemy.theEnemy + "'s " +
                        enemy.ability.usedAbility + ", blocking " + player.abilities.power
                        + " out of " + enemy.ability.power + " damage!";
                    break;
                case 2:
                    turnEvents = enemy.theEnemy + " defends against the Viking's " +
                        player.abilities.usedAbility + ", blocking " + enemy.ability.power
                        + " out of " + player.abilities.power + " damage!";
                    break;
                case 3:
                    turnEvents = "The Viking misses but the " + enemy.theEnemy + " attacks using " +
                        enemy.ability.usedAbility + ", dealing " + enemy.ability.power + " damage!";
                    break;
                case 4:
                    turnEvents = enemy.theEnemy + " misses but the Viking attacks using " +
                        player.abilities.usedAbility + ", dealing " + player.abilities.power + " damage!";
                    break;
                case 5:
                    turnEvents = "The Viking kills the " + enemy.theEnemy + " before it can attack!";
                    break;
                case 6:
                    turnEvents = "The Viking attacks the " + enemy.theEnemy + " using " +
                        player.abilities.usedAbility + " for " + player.abilities.power +
                        " damage! While " + enemy.theEnemy + " attacks for " + enemy.ability.power +
                        " damage, using " + enemy.ability.usedAbility;
                    break;
                case 7:
                    turnEvents = enemy.theEnemy + " kills the Viking before he can even attack!";
                    break;
                case 8:
                    turnEvents = enemy.theEnemy + " attacks the Viking using " +
                        enemy.ability.usedAbility + " for " + enemy.ability.power +
                        " damage! While the Viking attacks for " + player.abilities.power +
                        " damage, using " + player.abilities.usedAbility;
                    break;
                case 9:
                    turnEvents = "The viking attacks itself in confusion";
                    break;
                case 10:
                    turnEvents = enemy.theEnemy + " attacks itself in confusion";
                    break;
                case 11:
                    turnEvents = "The Viking kills itself in confusion";
                    break;
                case 12:
                    turnEvents = enemy.theEnemy + " kills itself in confusion";
                    break;
                case 13:
                    turnEvents = "The Viking defended against all damage done by " + 
                        enemy.theEnemy + "'s " + enemy.ability.usedAbility;
                    break;
                case 14:
                    turnEvents = enemy.theEnemy + " defended against all damage done by the viking's " 
                        + player.abilities.usedAbility;
                    break;
                case 15:
                    turnEvents = "something";
                    break;
                case 16:
                    turnEvents = "something";
                    break;
                case 17:
                    turnEvents = "something";
                    break;
            }
        }

        public void HandleCombatSummary(bool result, int xp)
        {
            this.battleWon = result;
            this.gainedXp = xp;
        }
    }
}
