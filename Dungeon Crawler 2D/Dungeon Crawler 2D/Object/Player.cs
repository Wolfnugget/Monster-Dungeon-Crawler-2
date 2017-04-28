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
        bool alreadyPressed = false;
        public Stats stats;
        public StatScreen statScreen;
        public bool showStats;
        private TextureManager textures;

        public Player(Texture2D texture, TextureManager textures, Vector2 position, float speed, Point frameSize, Point frames, float frameTime = 0.3f)
            : base(texture, position, speed, frameSize, frames, frameTime)
        {
            this.textures = textures;
            stats = new Stats(textures, 100, 100, 100, 100, 10, 10, 10, 10, 10, 0, 1);
            statScreen = new StatScreen(this, textures);
            showStats = false;
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
                startingFrame = new Point(0, 3);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                direction.Y = 1;
                startingFrame = new Point(0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D)) //Detta är en "if" så man ska kunna gå diagonalt. Map klassen har stöd för det.
            {
                direction.X = 1;
                startingFrame = new Point(0, 2);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                direction.X = -1;
                startingFrame = new Point(0, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                showStats = !showStats;
                
            }
                

            //Här ska frames bytas senare när vi har en sprite

            if (direction != new Point(0, 0))
            {
                ActorEventArgs args = new ActorEventArgs(PlayerEventType.CheckDirection);
                args.Direction = direction;
                args.Position = position;
                OnAction(args);
            }
        }
    }
}
