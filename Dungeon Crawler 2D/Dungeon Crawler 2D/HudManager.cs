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

        //Specialeffekt-relaterat
        private float flashTimer;
        private bool flashTimerToggle;
        private float flashAlpha;

        private float textScale;
        private float textScaleTimer;

        //TEMP!! Data som kommer att hämtas från player/enemy-klassen
        private KeyboardState currentState, previousState;
        private bool turn;
        private string turnEvents;


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

            flashTimer = 0;
            flashTimerToggle = false;

            textScaleTimer = 0;
            turn = true;
        }

        public void Update(GameState gameState)
        {
            this.gameState = gameState;
            
            if (Keyboard.GetState().IsKeyDown(Keys.V))
            {
                player.stats.AddEffect(1, Effects.poison, 1);
                player.stats.AddEffect(1, Effects.bleed, 1);
                player.stats.AddEffect(1, Effects.confusion, 1);

                player.stats.ChangeStat(Stat.health, 1);
                player.stats.ChangeStat(Stat.mana, 1);
                player.stats.ChangeStat(Stat.xp, 1);
            }

            // Temporära button-mappings för att testa vad som sker när olika värden ändras
            //-------------------------------------------------------------------
            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                player.stats.AddEffect(-1, Effects.poison, 1);
                player.stats.AddEffect(-1, Effects.bleed, 1);
                player.stats.AddEffect(-1, Effects.confusion, 1);

                player.stats.ChangeStat(Stat.health, -1);
                player.stats.ChangeStat(Stat.mana, -1);
                player.stats.ChangeStat(Stat.xp, -1);
            }

            previousState = currentState;
            currentState = Keyboard.GetState();
            if (currentState.IsKeyDown(Keys.R) && previousState.IsKeyUp(Keys.R))
            {
                Enemy enemy = new Enemy(textures, EnemyType.zombie, player);
                CombatText(3, enemy);
                textScaleTimer = 10;
            }
            //---------------------------------------------------------------------
            
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
        }

        public void DrawExplore(SpriteBatch spriteBatch)
        {
            //Sätter mått
            sideBarWidth = windowWidth / 10;
            statBarWidth = sideBarWidth / 3;

            leftBarRect = new Rectangle(0, 0, sideBarWidth, windowHeight);
            rightBarRect = new Rectangle(windowWidth - sideBarWidth, 0, sideBarWidth, windowHeight);

            //side-bars där stats visas
            spriteBatch.Draw(pixelTex, leftBarRect, Color.Black);
            spriteBatch.Draw(pixelTex, rightBarRect, Color.Black);

            //linjer som separerar olika sektioner
            spriteBatch.Draw(pixelTex, new Rectangle(sideBarWidth, 0, statBarWidth / 20, windowHeight), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(windowWidth - sideBarWidth - (statBarWidth / 20), 0, statBarWidth / 20, windowHeight), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(0, statBarWidth * 8, sideBarWidth, statBarWidth / 20), Color.White);

            //health-bar
            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.maxHealth) * (statBarWidth / 20)),
                statBarWidth,
                player.stats.CheckStat(Stat.maxHealth) * (statBarWidth / 20)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.health) * (statBarWidth / 20)),
                statBarWidth,
                player.stats.CheckStat(Stat.health) * (statBarWidth / 20)),
                new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);

            //mana-bar
            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.maxMana) * (statBarWidth / 20)),
                statBarWidth,
                player.stats.CheckStat(Stat.maxMana) * (statBarWidth / 20)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.mana) * (statBarWidth / 20)),
                statBarWidth,
                player.stats.CheckStat(Stat.mana) * statBarWidth / 20),
                new Rectangle(8, 0, 8, textures.barsSheet.Height), Color.White);

            //experience-bar
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

            //effects-display
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

            //text till bars
            //HP
            Vector2 textSizeHPNr = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.health));
            Vector2 originHPNr = new Vector2(textSizeHPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.health),
                new Vector2(leftBarRect.X + (sideBarWidth / 4), windowHeight - statBarWidth),
                Color.Red, 0, originHPNr, 2, SpriteEffects.None, 0);

            Vector2 textSizeHPTxt = textures.comicSans.MeasureString("HP");
            Vector2 originHPTxt = new Vector2(textSizeHPTxt.X * 0.5f, textSizeHPTxt.Y * 0.8f);
            spriteBatch.DrawString(textures.comicSans, ("HP"),
                new Vector2(leftBarRect.X + (sideBarWidth / 4), windowHeight - statBarWidth - (player.stats.CheckStat(Stat.maxHealth) * (statBarWidth / 20))),
                Color.Red, 0, originHPTxt, 2, SpriteEffects.None, 0);

            //MP
            Vector2 textSizeMPNr = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.mana));
            Vector2 originMPNr = new Vector2(textSizeMPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckStat(Stat.mana),
                new Vector2(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Blue, 0, originMPNr, 2, SpriteEffects.None, 0);

            Vector2 textSizeMPTxt = textures.comicSans.MeasureString("MP");
            Vector2 originMPTxt = new Vector2(textSizeMPTxt.X * 0.5f, textSizeMPTxt.Y * 0.8f);
            spriteBatch.DrawString(textures.comicSans, ("MP"),
                new Vector2(leftBarRect.X + (sideBarWidth / 4) + (sideBarWidth / 2), windowHeight - statBarWidth - (player.stats.CheckStat(Stat.maxMana) * (statBarWidth / 20))),
                Color.Blue, 0, originMPTxt, 2, SpriteEffects.None, 0);

            //XP
            Vector2 textSizeXPNr = textures.comicSans.MeasureString("XP: " + player.stats.CheckStat(Stat.xp));
            Vector2 originXPNr = new Vector2(textSizeXPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, "XP: " + player.stats.CheckStat(Stat.xp),
                new Vector2(rightBarRect.X + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Green, 0, originXPNr, 2, SpriteEffects.None, 0);

            //text till icons
            if (player.stats.CheckEffectTime(Effects.poison) > 0)
            {
                Vector2 textSizePoison = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.poison));
                Vector2 originPoison = new Vector2(textSizePoison.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.poison),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), statBarWidth + statBarWidth),
                    Color.Green, 0, originPoison, 2, SpriteEffects.None, 0);
            }

            if (player.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                Vector2 textSizeBleed = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.bleed));
                Vector2 originBleed = new Vector2(textSizeBleed.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.bleed),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), statBarWidth + (statBarWidth * 3)),
                    Color.Red, 0, originBleed, 2, SpriteEffects.None, 0);
            }

            if (player.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                Vector2 textSizeConfusion = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.confusion));
                Vector2 originConfusion = new Vector2(textSizeConfusion.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.confusion),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), statBarWidth + (statBarWidth * 5)),
                    Color.Yellow, 0, originConfusion, 2, SpriteEffects.None, 0);
            }

            //visar experience level
            Vector2 textSizeXpLevel = textures.comicSans.MeasureString("lvl " + player.stats.CheckStat(Stat.level));
            Vector2 originXpLevel = new Vector2(textSizeXpLevel.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, "lvl " + player.stats.CheckStat(Stat.level),
                new Vector2(rightBarRect.X + (sideBarWidth / 2),
                statBarWidth), Color.GhostWhite, 0, originXpLevel, 3, SpriteEffects.None, 0);
        }

        public void DrawBattle(SpriteBatch spriteBatch, Combat combat)
        {
            //Sätter mått
            sideBarWidth = windowWidth / 10;
            statBarWidth = sideBarWidth / 3;

            leftBarRect = new Rectangle(0, 0, sideBarWidth, windowHeight);
            rightBarRect = new Rectangle(windowWidth - sideBarWidth, 0, sideBarWidth, windowHeight);
            bottomBarRect = new Rectangle(leftBarRect.Width, windowHeight - (sideBarWidth * 2), windowWidth - (sideBarWidth * 2), sideBarWidth * 2);
            topBarRect = new Rectangle(0, 0, windowWidth, sideBarWidth);

            //sektioner där stats visas
            spriteBatch.Draw(pixelTex, leftBarRect, Color.Black);
            spriteBatch.Draw(pixelTex, rightBarRect, Color.Black);
            spriteBatch.Draw(pixelTex, bottomBarRect, Color.Black);
            spriteBatch.Draw(pixelTex, topBarRect, Color.Black);

            //Linjer som separerar sektioner
            spriteBatch.Draw(pixelTex, new Rectangle(sideBarWidth, 0, statBarWidth / 20, windowHeight - bottomBarRect.Height), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(windowWidth - sideBarWidth - (statBarWidth / 20), 0, statBarWidth / 20, windowHeight - bottomBarRect.Height), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(sideBarWidth, topBarRect.Height, topBarRect.Width - (sideBarWidth * 2), statBarWidth / 20), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(0, windowHeight - bottomBarRect.Height, windowWidth, statBarWidth / 20), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(sideBarWidth * 2, windowHeight - bottomBarRect.Height, statBarWidth / 20, bottomBarRect.Height), Color.White);
            spriteBatch.Draw(pixelTex, new Rectangle(windowWidth - (sideBarWidth * 2) - (statBarWidth / 20), windowHeight - bottomBarRect.Height, statBarWidth / 20, bottomBarRect.Height), Color.White);

            //effects-display
            //spelaren
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

            //fienden
            spriteBatch.Draw(textures.poisonIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth, sideBarWidth / 2, sideBarWidth / 2), Color.White * 0.3f);
            spriteBatch.Draw(textures.bleedIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth * 2, sideBarWidth / 2, sideBarWidth / 2), Color.White * 0.3f);
            spriteBatch.Draw(textures.confusionIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth * 3, sideBarWidth / 2, sideBarWidth / 2), Color.White * 0.3f);

            if (player.stats.CheckEffectTime(Effects.poison) > 0)
            {
                spriteBatch.Draw(textures.poisonIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth, sideBarWidth / 2, sideBarWidth / 2), Color.White * flashAlpha);
            }

            if (player.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                spriteBatch.Draw(textures.bleedIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth * 2, sideBarWidth / 2, sideBarWidth / 2), Color.White * flashAlpha);
            }

            if (player.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                spriteBatch.Draw(textures.confusionIcon, new Rectangle(rightBarRect.X + (sideBarWidth / 4), sideBarWidth * 3, sideBarWidth / 2, sideBarWidth / 2), Color.White * flashAlpha);
            }

            //health-bars
            //spelaren
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

            //fienden
            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X - sideBarWidth + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - bottomBarRect.Height + statBarWidth,
                statBarWidth,
                bottomBarRect.Height - (statBarWidth * 2)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X - sideBarWidth + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth))),
                statBarWidth,
                (player.stats.CheckStat(Stat.health) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxHealth)))),
                new Rectangle(0, 0, 8, textures.barsSheet.Height), Color.White);

            //mana-bars
            //spelaren
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

            //fienden
            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - bottomBarRect.Height + statBarWidth,
                statBarWidth,
                bottomBarRect.Height - (statBarWidth * 2)),
                new Rectangle(24, 0, 8, textures.barsSheet.Height), Color.White);

            spriteBatch.Draw(textures.barsSheet, new Rectangle(rightBarRect.X + (sideBarWidth / 2) - (statBarWidth / 2),
                windowHeight - statBarWidth - (player.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxMana))),
                statBarWidth,
                (player.stats.CheckStat(Stat.mana) * (bottomBarRect.Height - (statBarWidth * 2)) / (player.stats.CheckStat(Stat.maxMana)))),
                new Rectangle(8, 0, 8, textures.barsSheet.Height), Color.White);


            //text till bars
            //spelaren
            //HP
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

            //MP
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

            //fienden
            //HP
            Vector2 textSizeEnemyHPNr = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.health) + "/" + player.stats.CheckStat(Stat.maxHealth));
            Vector2 originEnemyHPNr = new Vector2(textSizeEnemyHPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, ("" + player.stats.CheckStat(Stat.health) + "/" + player.stats.CheckStat(Stat.maxHealth)),
                new Vector2(rightBarRect.X - sideBarWidth + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Red, 0, originEnemyHPNr, 2, SpriteEffects.None, 0);

            Vector2 textSizeEnemyHPTxt = textures.comicSans.MeasureString("HP");
            Vector2 originEnemyHPTxt = new Vector2(textSizeEnemyHPTxt.X * 0.5f, textSizeEnemyHPTxt.Y * 0.8f);
            spriteBatch.DrawString(textures.comicSans, ("HP"),
                new Vector2(rightBarRect.X - sideBarWidth + (sideBarWidth / 2), windowHeight - bottomBarRect.Height + statBarWidth),
                Color.Red, 0, originEnemyHPTxt, 2, SpriteEffects.None, 0);

            //MP
            Vector2 textSizeEnemyMPNr = textures.comicSans.MeasureString("" + player.stats.CheckStat(Stat.mana) + "/" + player.stats.CheckStat(Stat.maxMana));
            Vector2 originEnemyMPNr = new Vector2(textSizeEnemyMPNr.X * 0.5f, 0);
            spriteBatch.DrawString(textures.comicSans, ("" + player.stats.CheckStat(Stat.mana) + "/" + player.stats.CheckStat(Stat.maxMana)),
                new Vector2(rightBarRect.X + (sideBarWidth / 2), windowHeight - statBarWidth),
                Color.Blue, 0, originEnemyMPNr, 2, SpriteEffects.None, 0);

            Vector2 textSizeEnemyMPTxt = textures.comicSans.MeasureString("MP");
            Vector2 originEnemyMPTxt = new Vector2(textSizeEnemyMPTxt.X * 0.5f, textSizeEnemyMPTxt.Y) * 0.8f;
            spriteBatch.DrawString(textures.comicSans, ("MP"),
                new Vector2(rightBarRect.X + (sideBarWidth / 2), windowHeight - bottomBarRect.Height + statBarWidth),
                Color.Blue, 0, originEnemyMPTxt, 2, SpriteEffects.None, 0);

            //text till icons
            //spelaren
            if (player.stats.CheckEffectTime(Effects.poison) > 0)
            {
                Vector2 textSizePlayerPoison = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.poison));
                Vector2 originPlayerPoison = new Vector2(textSizePlayerPoison.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.poison),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), sideBarWidth + (sideBarWidth / 2)),
                    Color.Green, 0, originPlayerPoison, 2, SpriteEffects.None, 0);
            }

            if (player.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                Vector2 textSizePlayerBleed = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.bleed));
                Vector2 originPlayerBleed = new Vector2(textSizePlayerBleed.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.bleed),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), sideBarWidth * 2 + (sideBarWidth / 2)),
                    Color.Red, 0, originPlayerBleed, 2, SpriteEffects.None, 0);
            }

            if (player.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                Vector2 textSizePlayerConfusion = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.confusion));
                Vector2 originPlayerConfusion = new Vector2(textSizePlayerConfusion.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.confusion),
                    new Vector2(leftBarRect.X + (sideBarWidth / 2), sideBarWidth * 3 + (sideBarWidth / 2)),
                    Color.Yellow, 0, originPlayerConfusion, 2, SpriteEffects.None, 0);
            }

            //fienden
            if (player.stats.CheckEffectTime(Effects.poison) > 0)
            {
                Vector2 textSizeEnemyPoison = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.poison));
                Vector2 originEnemyPoison = new Vector2(textSizeEnemyPoison.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.poison),
                    new Vector2(rightBarRect.X + (sideBarWidth / 2), sideBarWidth + (sideBarWidth / 2)),
                    Color.Green, 0, originEnemyPoison, 2, SpriteEffects.None, 0);
            }

            if (player.stats.CheckEffectTime(Effects.bleed) > 0)
            {
                Vector2 textSizeEnemyBleed = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.bleed));
                Vector2 originEnemyBleed = new Vector2(textSizeEnemyBleed.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.bleed),
                    new Vector2(rightBarRect.X + (sideBarWidth / 2), sideBarWidth * 2 + (sideBarWidth / 2)),
                    Color.Red, 0, originEnemyBleed, 2, SpriteEffects.None, 0);
            }

            if (player.stats.CheckEffectTime(Effects.confusion) > 0)
            {
                Vector2 textSizeEnemyConfusion = textures.comicSans.MeasureString("" + player.stats.CheckEffectTime(Effects.confusion));
                Vector2 originEnemyConfusion = new Vector2(textSizeEnemyConfusion.X * 0.5f, 0);
                spriteBatch.DrawString(textures.comicSans, "" + player.stats.CheckEffectTime(Effects.confusion),
                    new Vector2(rightBarRect.X + (sideBarWidth / 2), sideBarWidth * 3 + (sideBarWidth / 2)),
                    Color.Yellow, 0, originEnemyConfusion, 2, SpriteEffects.None, 0);
            }

            //text för övre rutan
            if (turn == false)
            {
                Vector2 textSizeInfo = textures.comicSans.MeasureString(turnEvents);
                Vector2 originInfo = new Vector2(textSizeInfo.X * 0.5f, textSizeInfo.Y * 0.5f);
                spriteBatch.DrawString(textures.comicSans, turnEvents,
                    new Vector2(topBarRect.Width / 2, topBarRect.Height / 2),
                    Color.Green, 0, originInfo, textScale, SpriteEffects.None, 0);
            }
        }

        public void CombatText(int combatLine, Enemy enemy)
        {
            textScaleTimer = 10;
            switch (combatLine)
            {
                case '0':
                    turnEvents = "And nothing happened that round";
                    turn = false;
                    break;
                case '1':
                    turnEvents = "Viking defends against " + enemy.theEnemy + "'s " + 
                        enemy.ability.usedAbility + " blocking " + player.abilities.power 
                        + " out of " + enemy.ability.power + " damage!";
                    turn = false;
                    break;
                case '2':
                    turnEvents = enemy.theEnemy + " defends against the Viking's " + 
                        player.abilities.usedAbility + " blocking " + enemy.ability.power 
                        + " out of " + player.abilities.power + " damage!";
                    turn = false;
                    break;
                case '3':
                    turnEvents = "The Viking Misses but the " + enemy.theEnemy + " attacks using " + 
                        enemy.ability.usedAbility + " dealing " + enemy.ability.power + " damage!";
                    turn = false;
                    break;
                case '4':
                    turnEvents = enemy.theEnemy + " misses but the Viking attacks using " +
                        player.abilities.usedAbility + " dealing " + player.abilities.power + " damage!";
                    turn = false;
                    break;
                case '5':
                    turnEvents = "The Viking kills the " + enemy.theEnemy + " before it can attack!";
                    turn = false;
                    break;
                case'6':
                    turnEvents = "The Viking hits the " + enemy.theEnemy + " using " + 
                        player.abilities.usedAbility + " for " + player.abilities.power + 
                        " damage! While " + enemy.theEnemy + " attacks for " + enemy.ability.power + 
                        " damage using " + enemy.ability.usedAbility;
                    turn = false;
                    break;
                case'7':
                    turnEvents = enemy.theEnemy + " kills the Viking before he can even attack!";
                    turn = false;
                    break;
                case '8':
                    turnEvents = enemy.theEnemy +" hits the Viking using " +
                        enemy.ability.usedAbility + " for " + enemy.ability.power +
                        " damage! While the Viking attacks for " + player.abilities.power +
                        " damage using " + player.abilities.usedAbility;
                    turn = false;
                    break;
            }
            
        }

    }
}
