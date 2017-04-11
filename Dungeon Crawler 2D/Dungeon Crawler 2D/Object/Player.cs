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
        public int health;
        public int mana;
        public int xp;
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
            Point direction = new Point(0, 0);

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                direction.Y = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                direction.Y = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) //Detta är en "if" så man ska kunna gå diagonalt. Map klassen har stöd för det.
            {
                direction.X = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                direction.X = -1;
            }

            //Här ska frames bytas senare när vi har en sprite

            if (direction != new Point(0, 0))
            {
                Console.WriteLine("input Working: " + direction);
                ActorEventArgs args = new ActorEventArgs(PlayerEventType.CheckDirection);
                args.Direction = direction;
                args.Position = position;
                OnAction(args);
            }
        }
    }
}
