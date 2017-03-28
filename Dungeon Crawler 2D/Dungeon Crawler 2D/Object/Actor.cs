﻿using System;
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
        protected Vector2 position, destination, direction;

        Point startingFrame, frame, frames, frameSize;
        float frameTime, frameDuration;
        Vector2 origin;
        protected SpriteEffects effect;
        Color color;

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

        public void Move(GameTime gameTime)
        {
            if (Vector2.Distance(position, destination) > 1 &&
                Vector2.Distance(position + direction, destination) < Vector2.Distance(position, destination))
            {
                position += speed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                position = destination;
                direction = new Vector2(0, 0);
                moving = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, srcRec,  color, 0, origin, 1, effect, 1);
        }
    }
}
