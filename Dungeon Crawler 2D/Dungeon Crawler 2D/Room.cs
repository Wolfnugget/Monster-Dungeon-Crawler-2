using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;


namespace Dungeon_Crawler_2D
{
    public class Room
    {
        public Tile[,] tiles;
        public List<String> tileList;
        public PlayerCharacter playerChar;

        public Room(Texture2D tileTex, Texture2D charTex, PlayerCharacter playerChar, String map)
        {
            tileList = new List<String>();
            StreamReader sr;
            sr = new StreamReader(map);
            while (!sr.EndOfStream)
            {
                tileList.Add(sr.ReadLine());
            }
            sr.Close();
            int x = 0;
            int y = 0;
            for (int i = 0; i < tileList.Count; i++)
            {
                for (int j = 0; j < tileList[i].Length; j++)
                {
                    x++;
                    y++;
                }
            }
            tiles = new Tile[x, y];


            for (int i = 0; i < tileList.Count; i++)
            {
                for (int j = 0; j < tileList[i].Length; j++)
                {
                    if (tileList[i][j] == 'C')
                        this.playerChar = new PlayerCharacter(charTex, new Vector2(charTex.Width * j, charTex.Height * i), 5, 0, 0, this);

                    else if (tileList[i][j] == 'X')
                        tiles[i, j] = new Tile(tileTex, new Vector2(tileTex.Width* j, tileTex.Height* i), Color.Black, 1);
                    else if (tileList[i][j] == 'r')
                    {
                        //something randomly generated 
                    }
                    else if (tileList[i][j] == 'e')
                    {
                        //enemy

                    }
                    else if (tileList[i][j] == 'N')
                    {
                        tiles[i, j] = new Tile(tileTex, new Vector2(tileTex.Width * j, tileTex.Height * i), Color.Green, 2);
                    }
                        
                    
                    if (tileList[i][j] != 'X' && tileList[i][j] != 'N')
                        tiles[i, j] = new Tile(tileTex, new Vector2(tileTex.Width * j, tileTex.Height * i), Color.Beige, 0);
                    
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            playerChar.Update(gameTime);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tileList.Count; i++)
                for (int j = 0; j < tileList[i].Length; j++)
                    if (tiles[i,j] != null)
                        tiles[i, j].Draw(spriteBatch);
            playerChar.Draw(spriteBatch);
        }
    }
}
