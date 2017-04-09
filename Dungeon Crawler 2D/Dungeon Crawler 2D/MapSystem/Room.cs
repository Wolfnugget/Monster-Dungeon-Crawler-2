//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using System.IO;


//namespace Dungeon_Crawler_2D.MapSystem
//{
//    public class Room
//    {
//        public Tile[,] tiles;
//        public List<String> tileList;
//        public PlayerCharacter playerChar;
//        public int roomNr;
//        protected Texture2D tileTex, charTex;

//        public Room(Texture2D tileTex, Texture2D charTex, String map, int roomNr)
//        {
//            this.roomNr = roomNr;
//            this.tileTex = tileTex;
//            this.charTex = charTex;

//            tileList = new List<String>();
//            StreamReader sr;
//            sr = new StreamReader(map);
//            while (!sr.EndOfStream)
//            {
//                tileList.Add(sr.ReadLine());
//            }
//            sr.Close();
//            int x = 0;
//            int y = 0;
//            for (int i = 0; i < tileList.Count; i++)
//            {
//                for (int j = 0; j < tileList[i].Length; j++)
//                {
//                    x++;
//                    y++;
//                }
//            }
//            tiles = new Tile[x, y];

//            LoadRoom(); // ska inte kallas på här. Är bara här för testning!
            
//        }

//        public void Update(GameTime gameTime)
//        {
//            playerChar.Update(gameTime);
//        }


//        public void Draw(SpriteBatch spriteBatch)
//        {
//            for (int i = 0; i < tileList.Count; i++)
//                for (int j = 0; j < tileList[i].Length; j++)
//                    if (tiles[i,j] != null)
//                        tiles[i, j].Draw(spriteBatch);
//            playerChar.Draw(spriteBatch);
//        }

//        public void LoadRoom()
//        {
//            for (int i = 0; i < tileList.Count; i++)
//            {
//                for (int j = 0; j < tileList[i].Length; j++)
//                {
//                    if (tileList[i][j] == 'C')
//                        //playerChar = new PlayerCharacter(charTex, new Vector2(charTex.Width * j, charTex.Height * i), 5, 0, 0);

//                    else if (tileList[i][j] == 'X')
//                        tiles[i, j] = new Tile(tileTex, new Vector2(tileTex.Width* j, tileTex.Height* i), Color.Black, 1);
//                    else if (tileList[i][j] == 'R')
//                    {
//                        //something randomly generated 
//                    }
//                    else if (tileList[i][j] == 'E')
//                    {
//                        //Where you enter from
//                    }
//                    else if (tileList[i][j] == 'N')
//                    {
//                        tiles[i, j] = new Tile(tileTex, new Vector2(tileTex.Width * j, tileTex.Height * i), Color.Green, 2);
//                    }
//                    else if (tileList[i][j] == '1')
//                    {
//                        tiles[i, j] = new Tile(tileTex, new Vector2(tileTex.Width * j, tileTex.Height * i), Color.Red, 3);
//                    }


//                    if (tileList[i][j] != 'X' && tileList[i][j] != 'N' && tileList[i][j] != '1' 
//                        && tileList[i][j] != 'R' && tileList[i][j] != '1' && tileList[i][j] != '1')
//                        tiles[i, j] = new Tile(tileTex, new Vector2(tileTex.Width * j, tileTex.Height * i), Color.Beige, 0);
                    
//                }
//            }
//        }
//    }
//}
