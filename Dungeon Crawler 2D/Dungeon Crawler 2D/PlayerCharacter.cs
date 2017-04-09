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
        Rectangle playerHitBox;

        public enum MoveDirection
        {
            Up,
            Down,
            Left,
            Right,
            none
        }
        MoveDirection currentMovement;

        int health;
        int mana;
        int xp;
        World.Map map;

        public PlayerCharacter(Texture2D tex, Vector2 pos, int health, int mana, int xp, World.Map map): // Add conection to map
            base(tex, pos, health, mana, xp)
        {
            playerTex = tex;
            playerPos = pos;
            playerHitBox = hitBox;

            this.health = health;
            this.mana = mana;
            this.xp = xp;
            this.map = map;

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
                currentMovement = MoveDirection.none;
            }

            //Kollar när det är tillåtet att ändra stringen: "playerDirection", som kontrollerar om spelaren ska flytta sig, och i vilken riktning
            //if (playerPos == previousPlayerPos)
            //{
            //    if (Keyboard.GetState().IsKeyDown(Keys.W) && map.CheckMovement(playerPos, new Point(0, -1)))
            //    {
            //        currentMovement = MoveDirection.Up;
            //    }
            //    else if (Keyboard.GetState().IsKeyDown(Keys.S) && map.CheckMovement(playerPos, new Point(0, 1)))
            //    {
            //        currentMovement = MoveDirection.Down;
            //    }
            //    else if (Keyboard.GetState().IsKeyDown(Keys.A) && map.CheckMovement(playerPos, new Point(-1, 0)))
            //    {
            //        currentMovement = MoveDirection.Left;
            //    }
            //    else if (Keyboard.GetState().IsKeyDown(Keys.D) && map.CheckMovement(playerPos, new Point(1, 0)))
            //    {
            //        currentMovement = MoveDirection.Right;
            //    }
            //}
            
            
            //flyttar spelaren vidare i en riktning tills nästa lediga ruta då en knap inte trycks ner
            // OBS!!! Vill ni göra något som bara ska ske när spelaren rör sig i en riktning. t.ex animationer, ÄNDRA DÅ HÄR! vid "//Animation för ...". 
            if (currentMovement == MoveDirection.Up)
            {
                playerPos.Y -= 2;
                //Animation for Up
            }
            else if (currentMovement == MoveDirection.Down)
            {
                playerPos.Y += 2;
                //Animation for Down
            }
            else if (currentMovement == MoveDirection.Left)
            {
                playerPos.X -= 2;
                //Animation for Left
            }
            else if (currentMovement == MoveDirection.Right)
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