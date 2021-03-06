﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Dungeon_Crawler_2D.World
{
    public enum TileTexture
    {
        Floor_Tile,
        Floor_Boss_Tile,
        Enemy_Tile,
        Wall_NorthEast_Corner,
        Wall_NorthWest_Corner,
        Wall_SouthEast_Corner,
        Wall_SouthWest_Corner,
        Wall_Corner_Bottom_Left,
        Wall_Corner_Bottom_Right,
        Wall_Corner_Top_Left,
        Wall_Corner_Top_Right,
        Wall_Vertical,
        Wall_Horizontal,
        Wall_Single,
        Single_Corner_Top,
        Single_Corner_Right,
        Single_Corner_Left,
        Single_Corner_Bottom,
        Grass_Tile,
        Tex_Between_Walls,
        Boss_Tile,
        Cross_Wall
    }

    public enum TileSets
    {
        Castle,
        Dungeon,
        Overworld
    }

    public class TileSet
    {
        Dictionary<TileTexture, List<Texture2D>> textureSet;

        Texture2D nullTex;

        public TileSet(ContentManager content, TileSets tileSet)
        {
            string path = "Textures/TileSet/" + tileSet.ToString();
            LoadTextures(content, path);

            nullTex = content.Load<Texture2D>("Example");
        }

        private void LoadTextures(ContentManager content, string path)
        {
            List<string> files = new List<string>();

            List<TileTexture> enumList = new List<TileTexture>();
            textureSet = new Dictionary<TileTexture, List<Texture2D>>();
            Texture2D texture;

            foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Content/" + path).Select(Path.GetFileNameWithoutExtension))
            {
                files.Add(file);
            }

            files.Sort();
            List<string> filesSorted = files.Distinct().ToList();

            foreach (TileTexture tt in Enum.GetValues(typeof(TileTexture)))
            {
                enumList.Add(tt);
            }

            for (int i = 0; i < filesSorted.Count; i++)
            {
                for (int j = 0; j < enumList.Count; j++)
                {
                    if (filesSorted[i].Contains(enumList[j].ToString()))
                    {
                        texture = content.Load<Texture2D>(path + "/" + filesSorted[i]);
                        if (textureSet.ContainsKey(enumList[j]))
                        {
                            textureSet[enumList[j]].Add(texture);
                        }
                        else
                        {
                            textureSet.Add(enumList[j], new List<Texture2D>());
                            textureSet[enumList[j]].Add(texture);
                        }
                    }
                }
            }
        }

        public Texture2D GetTexture(TileTexture tileTexture, int region)
        {
            if (textureSet.ContainsKey(tileTexture))
            {
                if (region == 0)
                {
                    return textureSet[tileTexture][0];
                }
                else
                {
                    int index = 0;
                    for (int c = 0; c < region; c++)
                    {
                        index++;
                        if (index >= textureSet[tileTexture].Count)
                        {
                            index = 0;
                        }
                    }

                    return textureSet[tileTexture][index];
                }
            }

            return nullTex;
        }
    }
}
