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
    public class SplashScreen : GameScreen
    {
        SpriteFont font;
        List<FadeAnimation> fade;
        List<Texture2D> images;

        FileManager fileManager;
        int imageNumber;

        public override void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(Content, graphicsDevice);
            if (font == null)
                font = this.content.Load<SpriteFont>("SplashFont");

            imageNumber = 0;
            fileManager = new FileManager();
            fade = new List<FadeAnimation>();
            images = new List<Texture2D>();

            fileManager.LoadContent("Menus/Load/Splash.txt", attributes, contents);


            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Image":
                            images.Add(content.Load<Texture2D>(contents[i][j]));
                            fade.Add(new FadeAnimation());
                            break;
                    }
                }
            }

            for (int i = 0; i < attributes.Count; i++)
            {
                fade[i].LoadContent(content, images[i], "", new Vector2(ScreenManager.Instance.Dimensions.X / 2 - images[i].Width / 2, ScreenManager.Instance.Dimensions.Y / 2 - images[i].Height / 2));
                fade[i].Scale = 1.0f;
                fade[i].IsActive = true;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            fade[imageNumber].Update(gameTime);

            if (fade[imageNumber].Alpha == 0.0f)
                imageNumber++;

            if (imageNumber >= fade.Count - 1 || InputManager.KeyPressed(Keys.Enter))
            {
                if (fade[imageNumber].Alpha != 1.0f)
                    ScreenManager.Instance.AddScreen(new TitleScreen(), fade[imageNumber].Alpha);
                else
                    ScreenManager.Instance.AddScreen(new TitleScreen());
            }

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            fade[imageNumber].Draw(spriteBatch);
        }
    }
}
