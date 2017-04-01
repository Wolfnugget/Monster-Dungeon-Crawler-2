using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawler_2D
{
    public class PlayerCharacter : Characters
    {
        Texture2D playerTex;
        public Vector2 playerPos;
        Rectangle playerHitBox;
        Room room;

        int health;
        int mana;
        int xp;

        public PlayerCharacter(Texture2D tex, Vector2 pos, int health, int mana, int xp, Room room):
            base(tex, pos, health, mana, xp)
        {
            this.playerTex = tex;
            this.playerPos = pos;
            this.playerHitBox = hitBox;
            this.room = room;

            this.health = health;
            this.mana = mana;
            this.xp = xp;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                playerPos.Y -= 3;
                //Animation for Up
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                playerPos.Y += 3;
                //Animation for Down
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerPos.X -= 3;
                //Animation for Left
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerPos.X += 3;
                //Animation for Right
            }

            playerHitBox = new Rectangle((int)playerPos.X, (int)playerPos.Y, playerTex.Width, playerTex.Height);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTex, playerPos, null, Color.White, rotation, origin, scale, sEffect, layer);

            base.Draw(spriteBatch);
        }

    }
}
