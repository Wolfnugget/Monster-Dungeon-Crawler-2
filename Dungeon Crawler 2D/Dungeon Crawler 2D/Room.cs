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
    class Room
    {
        Tile[,] tiles;
        List<String> tileList;

        public Room(Texture2D tileTex, int roomNr)
        {
            tileList = new List<String>();

            StreamReader sr = new StreamReader(@"Map1.txt");
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
                    if (tileList[i][j] == '0')
                    {
                        Vector2 pos = new Vector2(17 * j, 17 * i);
                        tiles[j, i] = new Tile(tileTex, pos, Color.Black);
                    }
                    else if (tileList[i][j] == 'c')
                    {
                        //makes a chest
                    }
                    else if (tileList[i][j] == 'x')
                    {
                        //the former room
                        //where you begin when entering the room
                    }
                    else if (tileList[i][j] == 'r')
                    {
                        //something randomly generated 
                    }
                    else if (tileList[i][j] == 'e')
                    {
                        //enemy

                    }
                    else if (tileList[i][j] == 'n')
                    {
                        //next room
                    }



                    if (tileList[i][j] != '0' || tileList[i][j] != '1')
                    {
                        Vector2 pos = new Vector2(17 * j, 17 * i);
                        tiles[j, i] = new Tile(tileTex, pos, Color.Beige);
                    }
                }
            }
        }


        public void Draw(SpriteBatch sp)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(sp);
            }
        }

    }
}
