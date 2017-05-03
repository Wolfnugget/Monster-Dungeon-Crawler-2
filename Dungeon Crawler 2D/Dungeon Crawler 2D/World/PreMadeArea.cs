using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Dungeon_Crawler_2D.World
{
    public class PreMadeArea: Area
    {
        public PreMadeArea(string roomPath, Point areaCoords, TextureManager textures, ContentManager content)
            : base(textures, content)
        {
            List<string>  roomBluePrint = new List<string>();
            doors = new bool[4] { false, false, false, false};
            StreamReader sr = new StreamReader(roomPath);
            while (!sr.EndOfStream)
            {
                roomBluePrint.Add(sr.ReadLine());
            }

            int x = (roomBluePrint.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)).Length;
            this.areaCoords = areaCoords;

            tiles = new Tile[roomBluePrint.Count, x];

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    switch (roomBluePrint[i][j])
                    {
                        
                    }
                }
            }
        }
    }
}
