using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.Object
{
    class Player : Actor
    {
        public Player(Texture2D texture, Vector2 position, float speed, Point startingFrame, Point frameSize, Point frames, float frameTime = 0.3f)
            : base(texture, position, speed, startingFrame, frameSize, frames, frameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (!moving)
            {
                CheckInput();
            }

            base.Update(gameTime);
        }

        void CheckInput()
        {
            direction = new Vector2(0, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                direction.Y = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                direction.Y = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                direction.X = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                direction.X = -1;
            }


        }
    }
}
