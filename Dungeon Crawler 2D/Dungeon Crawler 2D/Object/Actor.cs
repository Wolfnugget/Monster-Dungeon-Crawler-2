using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.Object
{
    abstract class Actor
    {
        Texture2D texture;
        protected float speed;
        public Vector2 position;
        protected Vector2 destination;

        protected Point startingFrame, frame, frames, frameSize;
        float frameTime, frameDuration;
        Vector2 origin;
        protected SpriteEffects effect;

        protected bool moving;

        private Rectangle srcRec
        {
            get
            {
                return new Rectangle(frame.X * frameSize.Y,
                    frame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y);
            }
        }

        public Actor(Texture2D texture, Vector2 position, float speed, Point frameSize, Point frames, float frameTime)
        {
            this.texture = texture;
            this.position = position;
            this.speed = speed;
            this.frameSize = frameSize;
            this.frameTime = frameTime;
            frame = startingFrame;
            this.frames = frames;
            moving = false;

            origin = new Vector2(frameSize.X / 2, frameSize.Y / 2);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (moving)
            {
                Animate(gameTime);
                Move(gameTime);
            }
        }

        protected void Animate(GameTime gameTime)
        {
            frameDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (frame.Y < startingFrame.Y || frame.Y > startingFrame.Y + frames.Y)
            {
                frame.Y = startingFrame.Y;
            }
            if (frame.X < startingFrame.X || frame.X > startingFrame.X + frames.X)
            {
                frame.X = startingFrame.X;
            }
            if (frameDuration <= 0)
            {
                frameDuration = frameTime;
                if (frame.X < startingFrame.X + frames.X)
                {
                    frame.X++;
                }
                else if (frame.Y < startingFrame.Y + frames.Y)
                {
                    frame.X = startingFrame.X;
                    frame.Y++;
                }
                else
                {
                    frame = startingFrame;
                }
            }
        }

        /// <summary>
        /// Sätter positionen actorn ska röra sig till.
        /// </summary>
        /// <param name="destination"></param>
        public void SetDestination(Vector2 destination)
        {
            this.destination = destination;
            moving = true;
        }

        /// <summary>
        /// Förflytta till position.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void Move(GameTime gameTime)
        {
            Vector2 dir = new Vector2(destination.X - position.X, destination.Y - position.Y);
            Vector2 norm = Vector2.Normalize(dir);

            //Kolla avstånd till målet eller om actorn råkat gå för långt.
            if (Vector2.Distance(position, destination) > speed * gameTime.ElapsedGameTime.TotalSeconds)
            {
                position += speed * norm * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else //är framme och ska sluta röra sig. Kallar ett event att actorn är framme på en tile.
            {
                position = destination;
                moving = false;
                ActorEventArgs args = new ActorEventArgs(PlayerEventType.EnterTile);
                args.Position = position;
                OnAction(args);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, srcRec,  Color.White, 0, origin, 1, effect, 1);
        }

        public ActorEventHandler Action;

        public void OnAction(ActorEventArgs e)
        {
            //Om detta vissar fel så måste du uppdatera visual studio. Det är korrekt, ändra inte.
            Action.Invoke(this, e);
        }
    }
}
