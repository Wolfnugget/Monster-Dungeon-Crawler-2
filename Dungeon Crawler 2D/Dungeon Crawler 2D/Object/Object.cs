using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.Object
{
    public abstract class Object
    {
        protected Texture2D texture;
        public Vector2 position;

        public Object(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void Funktion()
        {

        }
    }
}
