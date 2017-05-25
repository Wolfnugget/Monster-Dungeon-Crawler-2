using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Dungeon_Crawler_2D.Menus
{
    public abstract class GameScreen
    {
        protected ContentManager content;
        protected List<List<string>> attributes, contents;



        public virtual void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
        }

        public virtual void UnloadContent()
        {
            content.Unload();
            attributes.Clear();
            contents.Clear();
        }
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

    }
}
