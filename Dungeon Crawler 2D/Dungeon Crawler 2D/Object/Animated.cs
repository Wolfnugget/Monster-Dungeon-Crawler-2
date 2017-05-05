using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;

namespace Dungeon_Crawler_2D.Object
{
    public abstract class Animated: Object
    {

        protected Point startingFrame, frame, frames, frameSize;
        float frameTime, frameDuration;
        protected Vector2 origin;
        protected SpriteEffects effect;

        private Rectangle srcRec
        {
            get
            {
                return new Rectangle(frame.X * frameSize.Y,
                    frame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y);
            }
        }

        public Animated(Texture2D texture, Vector2 position, Point frameSize, Point frames, float frameTime)
            : base(texture, position)
        {
            this.position = position;
            this.frameSize = frameSize;
            this.frameTime = frameTime;
            frame = startingFrame;
            this.frames = frames;

            origin = new Vector2(frameSize.X / 2, frameSize.Y / 2);
        }

        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, srcRec, Color.White, 0, origin, 1, effect, 1);
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
    }
}
