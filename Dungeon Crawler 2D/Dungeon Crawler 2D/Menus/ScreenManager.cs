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
    public class ScreenManager
    {
        #region Variables

        //Creating custom ContentManager//

        ContentManager content;

        //Current Screen that is being displayed//

        GameScreen currentScreen;

        //The new screen that will be taking effect//

        GameScreen newScreen;

        //ScreenManager Instance//

        private static ScreenManager instance;


        ///////Screen Stack////////

        Stack<GameScreen> screenStack = new Stack<GameScreen>();


        ////Screen Height and Width/////
        Vector2 dimensions;

        bool transition;

        FadeAnimation fade;

        Texture2D fadeTexture, nullTexture;

        InputManager inputManager;

        public GraphicsDevice GraphicsDevice;

        public SpriteBatch SpriteBatch;

        #endregion

        #region Properties
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();
                return instance;
            }
        }


        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Texture2D NullTexture
        {
            get { return nullTexture; }
        }

        #endregion


        #region Main Methods

        public void AddScreen(GameScreen screen, InputManager inputManager)
        {
            transition = true;
            newScreen = screen;

            fade.IsActive = true;
            fade.Alpha = 0.0f;
            fade.ActivateValue = 1.0f;

        }

        public void AddScreen(GameScreen screen, float alpha)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.ActivateValue = 1.0f;
            if (alpha != 1.0f)
                fade.Alpha = 1.0f - alpha;
            else
                fade.Alpha = alpha;
            fade.Increase = true;
        }


        public void Initialize()
        {
            currentScreen = new SplashScreen();
            fade = new FadeAnimation();
        }
        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content, inputManager, graphicsDevice);
            inputManager = new InputManager();

            nullTexture = content.Load<Texture2D>("Textures/Menu/nullTexture");
            fadeTexture = content.Load<Texture2D>("Textures/Menu/FadeTexture");
            fade.LoadContent(content, fadeTexture, "", Vector2.Zero);
            fade.Scale = dimensions.X;
        }
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            if (!transition)
                currentScreen.Update(gameTime);
            else
                Transition(gameTime, inputManager, graphicsDevice);
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            currentScreen.Draw(spriteBatch, gameTime);
            if (transition)
                fade.Draw(spriteBatch);
        }

        #endregion

        #region Private Methods

        private void Transition(GameTime gameTime, InputManager inputManager, GraphicsDevice graphicsDevice)
        {
            fade.Update(gameTime);
            if (fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScreen);
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent(content, inputManager, graphicsDevice);
            }
            else if (fade.Alpha == 0.0f)
            {
                transition = false;
                fade.IsActive = false;
            }
        }

        #endregion
    }
}
