using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawler_2D
{
    public class Characters
    {
        protected Texture2D tex;
        protected Vector2 pos;
        protected Rectangle hitBox;

        protected int health;
        protected int mana;
        protected int xp;

        protected Vector2 origin;
        protected float rotation, frameTime, scale, layer;
        protected Point startingFrame, frame, frameSize;
        protected Rectangle srcRec
        {
            get { return new Rectangle(frame.X * frameSize.X,
            frame.Y * frameSize.Y, frameSize.X, frameSize.Y); }
        }
        protected Color color;
        protected SpriteEffects sEffect;
        
        public Characters(Texture2D tex, Vector2 pos, int health, int mana, int xp)
        {
            this.tex = tex;
            this.pos = pos;
            this.hitBox = hitBox;

            this.health = health;
            this.mana = mana;
            this.xp = xp;

            //Behövs ge värden, default är 0. är scale 0 blir texturen osynlig... (Ta bort detta meddelande)
            rotation = 0;
            scale = 1;
            layer = 1;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
        

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
