using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawler_2D
{
    public class PlayerCharacter : Characters
    {
        Texture2D playerTex;
        public Vector2 playerPos;
        Vector2 previousPlayerPos;
        string playerDirection = null;
        Rectangle playerHitBox;
        Room room;

        int health;
        int mana;
        int xp;

        public PlayerCharacter(Texture2D tex, Vector2 pos, int health, int mana, int xp, Room room) :
            base(tex, pos, health, mana, xp)
        {
            this.playerTex = tex;
            this.playerPos = pos;
            this.playerHitBox = hitBox;
            this.room = room;

            this.health = health;
            this.mana = mana;
            this.xp = xp;

            previousPlayerPos = playerPos;
        }

        public override void Update(GameTime gameTime)
        {
            //uppdaterar previousPlayerPos för varje gång spelaren flyttar till en ny ruta
            if (playerPos.X - previousPlayerPos.X == 16
                || playerPos.Y - previousPlayerPos.Y == 16
                || previousPlayerPos.X - playerPos.X == 16
                || previousPlayerPos.Y - playerPos.Y == 16)
            {
                previousPlayerPos = playerPos;
                playerDirection = null;
            }

            //Kollar när det är tillåtet att ändra stringen: "playerDirection", som kontrollerar om spelaren ska flytta sig, och i vilken riktning
            if (Keyboard.GetState().IsKeyDown(Keys.W) && playerPos == previousPlayerPos
                || playerDirection == "down" && Keyboard.GetState().IsKeyDown(Keys.W) && !Keyboard.GetState().IsKeyDown(Keys.S))
            {
                playerDirection = "up";
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && playerPos == previousPlayerPos
                || playerDirection == "up" && Keyboard.GetState().IsKeyDown(Keys.S) && !Keyboard.GetState().IsKeyDown(Keys.W))
            {
                playerDirection = "down";
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A) && playerPos == previousPlayerPos
                || playerDirection == "right" && Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerDirection = "left";
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) && playerPos == previousPlayerPos
                || playerDirection == "left" && Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerDirection = "right";
            }

            //flyttar spelaren vidare i en riktning tills nästa lediga ruta då en knap inte trycks ner
            // OBS!!! Vill ni göra något som bara ska ske när spelaren rör sig i en riktning. t.ex animationer, ÄNDRA DÅ HÄR! vid "//Animation för ...". 
            if (playerDirection == "up")
            {
                playerPos.Y -= 2;
                //Animation for Up
            }
            else if (playerDirection == "down")
            {
                playerPos.Y += 2;
                //Animation for Down
            }
            else if (playerDirection == "left")
            {
                playerPos.X -= 2;
                //Animation for Left
            }
            else if (playerDirection == "right")
            {
                playerPos.X += 2;
                //Animation for Right
            }

            playerHitBox = new Rectangle((int)playerPos.X, (int)playerPos.Y, playerTex.Width, playerTex.Height);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTex, playerPos, null, Color.White, rotation, origin, scale, sEffect, layer);

            base.Draw(spriteBatch);
        }

    }
}