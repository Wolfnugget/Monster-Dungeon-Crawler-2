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
    public class TitleScreen : GameScreen
    {
        SpriteFont font;
        MenuManager menu;

        public override void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(Content, graphicsDevice);
            if (font == null)
                font = this.content.Load<SpriteFont>("MenuFont");
            menu = new MenuManager();
            menu.LoadContent(content, "Title");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            menu.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            menu.Draw(spriteBatch);
        }
    }
}
