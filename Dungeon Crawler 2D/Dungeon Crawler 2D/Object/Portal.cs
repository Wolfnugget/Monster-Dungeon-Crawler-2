using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.Object
{
    class Portal: Animated
    {
        bool active;

        int activationStage;

        double activationTimer;

        public Portal(Texture2D texture, Vector2 position, bool active)
            : base(texture, position, new Point(16, 16), new Point(3, 0), 0.2f)
        {
            this.active = active;
            if (active)
            {
                startingFrame.Y = 2;
            }
            else
            {
                startingFrame.Y = 0;
            }
            activationStage = 4;
        }

        public override void Update(GameTime gameTime)
        {
            if (activationStage < 4)
            {
                activationTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (activationTimer <= 0)
                {
                    activationStage++;
                    activationTimer = 0.3f;
                    
                }
                frame.Y = 1;
                frame.X = activationStage;
            }
            else
            {
                base.Update(gameTime);
            }
        }

        public override void Funktion()
        {
            active = true;
            activationStage = 0;
            startingFrame.Y = 2;
            activationTimer = 0.3f;
        }

        private void Activating()
        {

        }
    }
}
