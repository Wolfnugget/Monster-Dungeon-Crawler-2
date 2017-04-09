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

        Point startingFrame, frame, frames, frameSize;
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

        public Actor(Texture2D texture, Vector2 position, float speed, Point startingFrame, Point frameSize, Point frames, float frameTime)
        {
            this.texture = texture;
            this.position = position;
            this.speed = speed;
            this.startingFrame = startingFrame;
            this.frameSize = frameSize;
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

        public void SetDestination(Vector2 destination)
        {
            this.destination = destination;
            moving = true;
        }

        public void Move(GameTime gameTime)
        {
            Vector2 dir = new Vector2(destination.X - position.X, destination.Y - position.Y);
            Vector2 norm = Vector2.Normalize(dir);

            if (Vector2.Distance(position, destination) > 1 &&
                Vector2.Distance(position + (norm * speed * (float)gameTime.ElapsedGameTime.TotalSeconds), destination) < Vector2.Distance(position, destination))
            {
                position += speed * norm * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                position = destination;
                moving = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, srcRec,  Color.White, 0, origin, 1, effect, 1);
        }

        public PlayerEventHandler Action;

        public void OnAction(PlayerEventArgs e)
        {
            Action?.Invoke(this, e);
        }
    }
}
