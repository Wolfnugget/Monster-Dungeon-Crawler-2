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
            this.playerTex = tex;
            this.playerPos = pos;
            this.playerHitBox = hitBox;
            this.health = 5;
            this.mana = 0;
            this.xp = xp;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                playerPos.Y -= 3;

                //Animation for Up
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                playerPos.Y += 3;

                //Animation for Down
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerPos.X -= 3;

                //Animation for Left
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerPos.X += 3;

                //Animation for Right
            }
            playerHitBox = new Rectangle((int)playerPos.X, (int)playerPos.Y, playerTex.Width, playerTex.Height);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

    }
}
