﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawler_2D
{
    class Characters
    {
        Texture2D tex;
        Vector2 pos;
        Rectangle hitBox;
        Game1 game;

        protected Vector2 origin;
        protected float rotation, frameTime, scale;
        protected Point startingFrame, frame, frameSize;
        protected Rectangle srcRec
        {
            get { return new Rectangle(frame.X * frameSize.X,
            frame.Y * frameSize.Y, frameSize.X, frameSize.Y); }
        }
        protected Color color;
        protected SpriteEffects sEffect;

        int health;
        int mana;
        int xp;

        public Characters(Texture2D tex, Vector2 pos, Rectangle hitBox, int health, int mana, int xp)
        {
            this.tex = tex;
            this.pos = pos;
            this.hitBox = hitBox;
            this.health = health;
            this.mana = mana;
            this.xp = xp;
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void PlayerMove()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                //Animation for Up
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //Animation for Down
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //Animation for Left
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                //Animation for Right
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, srcRec, color, rotation, origin, scale, sEffect, 1);
        }
    }
}
