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
        private float colorChange;
        private Vector2 selectionCoords;

        public StatScreen(HUDManager hud, TextureManager textures)
        {
            this.hud = hud; ;
            this.textures = textures;

            statScreenRect = new Rectangle((hud.windowWidth / 2) - ((hud.sideBarWidth * 3) / 2),
                (hud.windowHeight / 2) - (hud.sideBarWidth * 2), hud.sideBarWidth * 3, hud.sideBarWidth * 2);

            selectionCoords = new Vector2(statScreenRect.X, statScreenRect.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            statScreenRect = new Rectangle((hud.windowWidth / 2) - ((hud.sideBarWidth * 3) / 2),
                (hud.windowHeight / 2) - (hud.sideBarWidth * 2), hud.sideBarWidth * 3, hud.sideBarWidth * 2);
            colorChange = 1f;

            spriteBatch.Draw(hud.pixelTex, statScreenRect, Color.Black);

            spriteBatch.Draw(hud.pixelTex, new Rectangle(statScreenRect.X, statScreenRect.Y, statScreenRect.Width, statScreenRect.Width / 100), Color.White);
            spriteBatch.Draw(hud.pixelTex, new Rectangle(statScreenRect.X, statScreenRect.Y + statScreenRect.Height - (statScreenRect.Width / 100), statScreenRect.Width, statScreenRect.Width / 100), Color.White);
            spriteBatch.Draw(hud.pixelTex, new Rectangle(statScreenRect.X, statScreenRect.Y, statScreenRect.Width / 100, statScreenRect.Height), Color.White);
            spriteBatch.Draw(hud.pixelTex, new Rectangle(statScreenRect.X + statScreenRect.Width - (statScreenRect.Width / 100), statScreenRect.Y, statScreenRect.Width / 100, statScreenRect.Height), Color.White);

            #region icons

            //icons
            //Upgrade
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(statScreenRect.X + (statScreenRect.Width / 7), 
                statScreenRect.Y + (statScreenRect.Height / 10), 
                statScreenRect.Width / 7, 
                statScreenRect.Width / 7),
                null, Color.Green, 0, Vector2.Zero, SpriteEffects.None, 0);

            //Strength
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 3),
                statScreenRect.Y + (statScreenRect.Height / 10),
                statScreenRect.Width / 7,
                statScreenRect.Width / 7),
                null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);

            //Dexterity
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 5),
                statScreenRect.Y + (statScreenRect.Height / 10),
                statScreenRect.Width / 7,
                statScreenRect.Width / 7),
                null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

            //Speed
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(statScreenRect.X + (statScreenRect.Width / 7),
                statScreenRect.Y + (statScreenRect.Height / 2),
                statScreenRect.Width / 7,
                statScreenRect.Width / 7),
                null, Color.LightBlue, 0, Vector2.Zero, SpriteEffects.None, 0);

            //Intelligence
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 3),
                statScreenRect.Y + (statScreenRect.Height / 2),
                statScreenRect.Width / 7,
                statScreenRect.Width / 7),
                null, Color.Yellow, 0, Vector2.Zero, SpriteEffects.None, 0);

            //Luck
            spriteBatch.Draw(textures.whiteSquare, new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 5),
                statScreenRect.Y + (statScreenRect.Height / 2),
                statScreenRect.Width / 7,
                statScreenRect.Width / 7),
                null, Color.LightGreen, 0, Vector2.Zero, SpriteEffects.None, 0);
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

            //Dexterity
            Vector2 textSizeDexterity = textures.comicSans.MeasureString(hud.player.stats.CheckStat(Stat.dexterity).ToString());
            Vector2 originDexterity = new Vector2(textSizeDexterity.X * 0.5f, (textSizeDexterity.Y * 0.8f) - textSizeDexterity.Y);

            spriteBatch.DrawString(textures.comicSans, hud.player.stats.CheckStat(Stat.dexterity).ToString(),
                    new Vector2(statScreenRect.X + ((statScreenRect.Width / 14) * 11), statScreenRect.Y + ((statScreenRect.Height / 10) * 3)), Color.White,
                    0, originDexterity, 2, SpriteEffects.None, 0);

            //Speed
            Vector2 textSizeSpeed = textures.comicSans.MeasureString(hud.player.stats.CheckStat(Stat.speed).ToString());
            Vector2 originSpeed = new Vector2(textSizeSpeed.X * 0.5f, (textSizeSpeed.Y * 0.8f) - textSizeSpeed.Y);

            spriteBatch.DrawString(textures.comicSans, hud.player.stats.CheckStat(Stat.speed).ToString(),
                    new Vector2(statScreenRect.X + ((statScreenRect.Width / 14) * 3), statScreenRect.Y + ((statScreenRect.Height / 10) * 7)), Color.LightBlue,
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

            spriteBatch.Draw(textures.whiteSquare, new Rectangle((int)selectionCoords.X, (int)selectionCoords.Y, 40, 40), Color.Orange);


            //Ritar ut små gröna plustecken när man kan uppgradera en stat
            if (hud.player.stats.upgrade > 0)
            {
                previousState = currentState;
                currentState = Keyboard.GetState();

                if (new Rectangle(statScreenRect.X + ((statScreenRect.Width / 7) * 3),
                statScreenRect.Y + (statScreenRect.Height / 10),
                statScreenRect.Width / 7,
                statScreenRect.Width / 7).Contains(selectionCoords.X, selectionCoords.Y) && currentState.IsKeyDown(Keys.Enter) && previousState.IsKeyUp(Keys.Enter))
                {
                    hud.player.stats.upgrade -= 1;
                    hud.player.stats.ChangeStat(Stat.strength, 1);
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        spriteBatch.DrawString(textures.comicSans, "+", 
                            new Vector2(statScreenRect.X + (statScreenRect.Width / 4) + (((statScreenRect.Width / 7) * 2) * i), 
                            statScreenRect.Y + (statScreenRect.Height / 5) + (((statScreenRect.Height / 10) * 4) * j)),
                            Color.Green, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
                    }
                }
            }
        }
    }
}
