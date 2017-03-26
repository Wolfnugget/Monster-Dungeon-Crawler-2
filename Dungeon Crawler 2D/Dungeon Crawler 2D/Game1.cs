using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawler_2D
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D playerTex;
        Vector2 pos;
        Rectangle hitBox;
        int health;
        int mana;
        int xp;

        PlayerCharacter playerCharacter;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTex = Content.Load<Texture2D>("character");

            pos = new Vector2(50, 50);
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, playerTex.Width, playerTex.Height);
            //characters = new Characters(tex, pos, hitBox, health, mana, xp);
            playerCharacter = new PlayerCharacter(playerTex, pos, hitBox, health, mana, xp);

            








        }
        protected override void Update(GameTime gameTime)
        {
            playerCharacter.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            

            playerCharacter.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
