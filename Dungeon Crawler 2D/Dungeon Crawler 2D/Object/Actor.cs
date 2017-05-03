using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.Object
{
    public abstract class Actor: Animated
    {
        protected float speed;
        protected Vector2 destination;

        protected bool moving;

        public Actor(Texture2D texture, Vector2 position, float speed, Point frameSize, Point frames, float frameTime)
            : base(texture, position, frameSize, frames, frameTime)
        {
            this.speed = speed;
            moving = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (moving)
            {
                Animate(gameTime);
                Move(gameTime);
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

        public ActorEventHandler Action;

        public void OnAction(ActorEventArgs e)
        {
            //Om detta vissar fel så måste du uppdatera visual studio. Det är korrekt, ändra inte.
            Action.Invoke(this, e);
        }
    }
}
