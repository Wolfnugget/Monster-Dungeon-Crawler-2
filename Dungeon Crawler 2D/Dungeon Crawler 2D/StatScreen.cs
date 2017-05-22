using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawler_2D
{
    class StatScreen
    {
        private KeyboardState previousState, currentState;
        private HUDManager hud;
        private TextureManager textures;
        private Rectangle statScreenRect;
        private Vector2 selectionCoords;

        private int coordinateXMultiplier;
        private int coordinateYMultiplier;

        public bool showSummary;

        public StatScreen(HUDManager hud, TextureManager textures)
        {
            this.hud = hud; ;
            this.textures = textures;

            statScreenRect = new Rectangle((hud.windowWidth / 2) - ((hud.sideBarWidth * 3) / 2),
                (hud.windowHeight / 2) - (hud.sideBarWidth * 2), hud.sideBarWidth * 3, hud.sideBarWidth * 2);

            selectionCoords = new Vector2(statScreenRect.X + ((statScreenRect.Width / 7) * 3), statScreenRect.Y + (statScreenRect.Height / 10));
            coordinateXMultiplier = 1;
            coordinateYMultiplier = 10;
            showSummary = false;
        }

        public void Update()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();

            //Stänga Battle-summerings-pop-upen
            if (showSummary == true && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                showSummary = false;
                GameSettings.gameState = GameState.Explore;
            }

            //Inventory-knappen
            if (currentState.IsKeyDown(Keys.Q) && previousState.IsKeyUp(Keys.Q) && showSummary == false)
            {
                if (GameSettings.gameState == GameState.Explore)
                {
                    GameSettings.gameState = GameState.Inventory;
                }
                else if (GameSettings.gameState == GameState.Inventory)
                {
                    GameSettings.gameState = GameState.Explore;
                }
            }

            if (GameSettings.gameState == GameState.Inventory && showSummary == false)
            {
                statScreenRect = new Rectangle((hud.windowWidth / 2) - ((hud.sideBarWidth * 3) / 2),
                (hud.windowHeight / 2) - (hud.sideBarWidth * 2), hud.sideBarWidth * 3, hud.sideBarWidth * 2);

                selectionCoords = new Vector2(statScreenRect.X + ((statScreenRect.Width / 7) * coordinateXMultiplier),
                    statScreenRect.Y + (statScreenRect.Height / coordinateYMultiplier));

                //styr selectionCoordinates
                if (coordinateYMultiplier == 10 && currentState.IsKeyDown(Keys.S) && previousState.IsKeyUp(Keys.S))
                {
                    coordinateYMultiplier = 2;
                }
                if (coordinateYMultiplier == 2 && currentState.IsKeyDown(Keys.W) && previousState.IsKeyUp(Keys.W))
                {
                    coordinateYMultiplier = 10;
                }
                if (coordinateXMultiplier > 1 && currentState.IsKeyDown(Keys.A) && previousState.IsKeyUp(Keys.A))
                {
                    coordinateXMultiplier -= 2;
                }
                if (coordinateXMultiplier < 5 && currentState.IsKeyDown(Keys.D) && previousState.IsKeyUp(Keys.D))
                {
                    coordinateXMultiplier += 2;
                }

                //kollar om en stat ska uppgraderas
                if (hud.player.stats.upgrade > 0)
                {
                    //strength
                    if (new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 3),
                    statScreenRect.Y + (statScreenRect.Height / 10),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7).Contains(selectionCoords.X, selectionCoords.Y) && currentState.IsKeyDown(Keys.E) && previousState.IsKeyUp(Keys.E))
                    {
                        hud.player.stats.upgrade -= 1;
                        hud.player.stats.ChangeStat(Stat.strength, 1);
                    }

                    //accuracy
                    if (new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 5),
                    statScreenRect.Y + (statScreenRect.Height / 10),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7).Contains(selectionCoords.X, selectionCoords.Y) && currentState.IsKeyDown(Keys.E) && previousState.IsKeyUp(Keys.E))
                    {
                        hud.player.stats.upgrade -= 1;
                        hud.player.stats.ChangeStat(Stat.accuracy, 1);
                    }

                    //speed
                    if (new Rectangle(statScreenRect.X + (statScreenRect.Width / 7),
                    statScreenRect.Y + (statScreenRect.Height / 2),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7).Contains(selectionCoords.X, selectionCoords.Y) && currentState.IsKeyDown(Keys.E) && previousState.IsKeyUp(Keys.E))
                    {
                        hud.player.stats.upgrade -= 1;
                        hud.player.stats.ChangeStat(Stat.speed, 1);
                    }

                    //intelligence
                    if (new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 3),
                    statScreenRect.Y + (statScreenRect.Height / 2),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7).Contains(selectionCoords.X, selectionCoords.Y) && currentState.IsKeyDown(Keys.E) && previousState.IsKeyUp(Keys.E))
                    {
                        hud.player.stats.upgrade -= 1;
                        hud.player.stats.ChangeStat(Stat.intelligence, 1);
                    }

                    //luck
                    if (new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 5),
                    statScreenRect.Y + (statScreenRect.Height / 2),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7).Contains(selectionCoords.X, selectionCoords.Y) && currentState.IsKeyDown(Keys.E) && previousState.IsKeyUp(Keys.E))
                    {
                        hud.player.stats.upgrade -= 1;
                        hud.player.stats.ChangeStat(Stat.luck, 1);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (showSummary == false)
            {
                spriteBatch.Draw(hud.pixelTex, statScreenRect, Color.Black);

                spriteBatch.Draw(hud.pixelTex, new Rectangle(statScreenRect.X, statScreenRect.Y, statScreenRect.Width, statScreenRect.Width / 100), Color.White);
                spriteBatch.Draw(hud.pixelTex, new Rectangle(statScreenRect.X, statScreenRect.Y + statScreenRect.Height - (statScreenRect.Width / 100), statScreenRect.Width, statScreenRect.Width / 100), Color.White);
                spriteBatch.Draw(hud.pixelTex, new Rectangle(statScreenRect.X, statScreenRect.Y, statScreenRect.Width / 100, statScreenRect.Height), Color.White);
                spriteBatch.Draw(hud.pixelTex, new Rectangle(statScreenRect.X + statScreenRect.Width - (statScreenRect.Width / 100), statScreenRect.Y, statScreenRect.Width / 100, statScreenRect.Height), Color.White);

                #region icons

                //icons
                //Upgrade
                spriteBatch.Draw(textures.statPointIcon, new Rectangle(statScreenRect.X + (statScreenRect.Width / 7),
                    statScreenRect.Y + (statScreenRect.Height / 10),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7),
                    null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

                //Strength
                spriteBatch.Draw(textures.strengthIcon, new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 3),
                    statScreenRect.Y + (statScreenRect.Height / 10),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7),
                    null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

                //Accuracy
                spriteBatch.Draw(textures.accuracyIcon, new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 5),
                    statScreenRect.Y + (statScreenRect.Height / 10),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7),
                    null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

                //Speed
                spriteBatch.Draw(textures.speedIcon, new Rectangle(statScreenRect.X + (statScreenRect.Width / 7),
                    statScreenRect.Y + (statScreenRect.Height / 2),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7),
                    null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

                //Intelligence
                spriteBatch.Draw(textures.intelligenceIcon, new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 3),
                    statScreenRect.Y + (statScreenRect.Height / 2),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7),
                    null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

                //Luck
                spriteBatch.Draw(textures.luckIcon, new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 5),
                    statScreenRect.Y + (statScreenRect.Height / 2),
                    statScreenRect.Width / 7,
                    statScreenRect.Width / 7),
                    null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

                //Marker icon
                spriteBatch.Draw(textures.whiteSquare, new Rectangle((int)selectionCoords.X, (int)selectionCoords.Y, statScreenRect.Width / 7, statScreenRect.Width / 7), Color.White * hud.flashAlpha);

                #endregion

                #region value-text
                //text
                //upgrade
                Vector2 textSizeUpgrade = textures.comicSans.MeasureString(hud.player.stats.upgrade.ToString());
                Vector2 originUpgrade = new Vector2(textSizeUpgrade.X * 0.5f, (textSizeUpgrade.Y * 0.8f) - textSizeUpgrade.Y);

                spriteBatch.DrawString(textures.comicSans, hud.player.stats.upgrade.ToString(),
                        new Vector2(statScreenRect.X + ((statScreenRect.Width / 14) * 3), statScreenRect.Y + ((statScreenRect.Height / 10) * 3)), Color.Green,
                        0, originUpgrade, 2, SpriteEffects.None, 0);

                //strength
                Vector2 textSizeStrength = textures.comicSans.MeasureString(hud.player.stats.CheckStat(Stat.strength).ToString());
                Vector2 originStrength = new Vector2(textSizeStrength.X * 0.5f, (textSizeStrength.Y * 0.8f) - textSizeStrength.Y);

                spriteBatch.DrawString(textures.comicSans, hud.player.stats.CheckStat(Stat.strength).ToString(),
                        new Vector2(statScreenRect.X + ((statScreenRect.Width / 14) * 7), statScreenRect.Y + ((statScreenRect.Height / 10) * 3)), Color.Red,
                        0, originStrength, 2, SpriteEffects.None, 0);

                //Accuracy
                Vector2 textSizeDexterity = textures.comicSans.MeasureString(hud.player.stats.CheckStat(Stat.accuracy).ToString());
                Vector2 originDexterity = new Vector2(textSizeDexterity.X * 0.5f, (textSizeDexterity.Y * 0.8f) - textSizeDexterity.Y);

                spriteBatch.DrawString(textures.comicSans, hud.player.stats.CheckStat(Stat.accuracy).ToString(),
                        new Vector2(statScreenRect.X + ((statScreenRect.Width / 14) * 11), statScreenRect.Y + ((statScreenRect.Height / 10) * 3)), Color.White,
                        0, originDexterity, 2, SpriteEffects.None, 0);

                //Speed
                Vector2 textSizeSpeed = textures.comicSans.MeasureString(hud.player.stats.CheckStat(Stat.speed).ToString());
                Vector2 originSpeed = new Vector2(textSizeSpeed.X * 0.5f, (textSizeSpeed.Y * 0.8f) - textSizeSpeed.Y);

                spriteBatch.DrawString(textures.comicSans, hud.player.stats.CheckStat(Stat.speed).ToString(),
                        new Vector2(statScreenRect.X + ((statScreenRect.Width / 14) * 3), statScreenRect.Y + ((statScreenRect.Height / 10) * 7)), Color.Blue,
                        0, originSpeed, 2, SpriteEffects.None, 0);

                //Intelligence
                Vector2 textSizeIntelligence = textures.comicSans.MeasureString(hud.player.stats.CheckStat(Stat.intelligence).ToString());
                Vector2 originIntelligence = new Vector2(textSizeIntelligence.X * 0.5f, (textSizeIntelligence.Y * 0.8f) - textSizeIntelligence.Y);

                spriteBatch.DrawString(textures.comicSans, hud.player.stats.CheckStat(Stat.intelligence).ToString(),
                        new Vector2(statScreenRect.X + ((statScreenRect.Width / 14) * 7), statScreenRect.Y + ((statScreenRect.Height / 10) * 7)), Color.Yellow,
                        0, originIntelligence, 2, SpriteEffects.None, 0);

                //Luck
                Vector2 textSizeLuck = textures.comicSans.MeasureString(hud.player.stats.CheckStat(Stat.luck).ToString());
                Vector2 originLuck = new Vector2(textSizeLuck.X * 0.5f, (textSizeLuck.Y * 0.8f) - textSizeLuck.Y);

                spriteBatch.DrawString(textures.comicSans, hud.player.stats.CheckStat(Stat.luck).ToString(),
                        new Vector2(statScreenRect.X + ((statScreenRect.Width / 14) * 11), statScreenRect.Y + ((statScreenRect.Height / 10) * 7)), Color.LightGreen,
                        0, originLuck, 2, SpriteEffects.None, 0);

                #endregion

                //Ritar ut små gröna plustecken när man kan uppgradera en stat
                if (hud.player.stats.upgrade > 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if (i == 0 && j == 0)
                            {
                                //excluding the first stat icon (upgrade)
                            }
                            else
                            {
                                spriteBatch.DrawString(textures.comicSans, "+",
                                new Vector2(statScreenRect.X + (statScreenRect.Width / 4) + (((statScreenRect.Width / 7) * 2) * i),
                                statScreenRect.Y + (statScreenRect.Height / 20) + (((statScreenRect.Height / 10) * 4) * j)),
                                Color.Green, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
                            }
                        }
                    }
                }
            }
        }

        public void DrawCombatSummary(SpriteBatch spriteBatch)
        {
            Rectangle whiteBox = new Rectangle((hud.windowWidth / 2) - ((hud.sideBarWidth * 3) / 2), (hud.windowHeight / 2) - (hud.sideBarWidth * 2), hud.sideBarWidth * 3, hud.sideBarWidth * 2);
            Rectangle blackBox = new Rectangle(whiteBox.X + (hud.statBarWidth / 20), whiteBox.Y + (hud.statBarWidth / 20), whiteBox.Width - (hud.statBarWidth / 10), whiteBox.Height - (hud.statBarWidth / 10));

            Vector2 textSizeSummary;
            Vector2 originSummaryText;

            spriteBatch.Draw(textures.whiteSquare, whiteBox, Color.White);
            spriteBatch.Draw(textures.whiteSquare, blackBox, Color.Black);

            if (hud.battleWon == true)
            {
                textSizeSummary = textures.comicSans.MeasureString("BATTLE WON!!");
                originSummaryText = textSizeSummary * 0.5f;
                spriteBatch.DrawString(textures.comicSans, "BATTLE WON!!", new Vector2(whiteBox.X + (whiteBox.Width / 2), whiteBox.Y + (whiteBox.Height / 3)), Color.Yellow, 0, originSummaryText, 3, SpriteEffects.None, 0);

                textSizeSummary = textures.comicSans.MeasureString("Experience gained: " + hud.gainedXp);
                originSummaryText = textSizeSummary * 0.5f;
                spriteBatch.DrawString(textures.comicSans, "Experience gained: " + hud.gainedXp, new Vector2(whiteBox.X + (whiteBox.Width / 2), whiteBox.Y + ((whiteBox.Height / 3) * 2)), Color.Yellow, 0, originSummaryText, 2, SpriteEffects.None, 0);

                textSizeSummary = textures.comicSans.MeasureString("Press Space");
                originSummaryText = textSizeSummary * 0.5f;
                spriteBatch.DrawString(textures.comicSans, "Press Space", new Vector2(whiteBox.X + (whiteBox.Width / 2), whiteBox.Y + ((whiteBox.Height / 5) * 4)), Color.Yellow, 0, originSummaryText, 2, SpriteEffects.None, 0);
            }

            else
            {
                textSizeSummary = textures.comicSans.MeasureString("BATTLE LOST...");
                originSummaryText = textSizeSummary * 0.5f;
                spriteBatch.DrawString(textures.comicSans, "BATTLE LOST...", new Vector2(whiteBox.X + (whiteBox.Width / 2), whiteBox.Y + (whiteBox.Height / 3)), Color.Yellow, 0, originSummaryText, 3, SpriteEffects.None, 0);

                textSizeSummary = textures.comicSans.MeasureString("Play New Game?");
                originSummaryText = textSizeSummary * 0.5f;
                spriteBatch.DrawString(textures.comicSans, "Play New Game?", new Vector2(whiteBox.X + (whiteBox.Width / 2), whiteBox.Y + ((whiteBox.Height / 3) * 2)), Color.Yellow, 0, originSummaryText, 2, SpriteEffects.None, 0);

                textSizeSummary = textures.comicSans.MeasureString("Press Space");
                originSummaryText = textSizeSummary * 0.5f;
                spriteBatch.DrawString(textures.comicSans, "Press Space", new Vector2(whiteBox.X + (whiteBox.Width / 2), whiteBox.Y + ((whiteBox.Height / 5) * 4)), Color.Yellow, 0, originSummaryText, 2, SpriteEffects.None, 0);
            }
        }
    }
}
