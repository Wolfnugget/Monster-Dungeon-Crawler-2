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
    class PlayerCharacter:Characters
    {
        

        int health;
        int mana;
        int xp;



        public PlayerCharacter(Texture2D tex, Vector2 pos, Rectangle hitBox, int health, int mana, int xp):
            base(tex, pos, hitBox, health, mana, xp)
        {
            this.tex = tex;
            this.pos = pos;
            this.hitBox = hitBox;
            this.health = 5;
            this.mana = 0;
            this.xp = xp;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                pos.Y -= 3;

                //Animation for Up
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                pos.Y += 3;

                //Animation for Down
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                pos.X -= 3;

                //Animation for Left
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                pos.X += 3;

                //Animation for Right
            }
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, tex.Width, tex.Height);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, hitBox, Color.White, rotation, origin, scale, sEffect, 1);
            base.Draw(spriteBatch);
        }

    }
}
