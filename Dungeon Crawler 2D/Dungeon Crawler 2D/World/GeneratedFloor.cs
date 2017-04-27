﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.World
{
    class GeneratedFloor: Floor
    {
        int floor;
        Random rand;

        public GeneratedFloor(Point dimensions, TextureManager textures, int floor, TileType entry)
            : base(textures)
        {
            rand = new Random();
        }

        private void GenerateFloor(TileType entry)
        {
            Point start = GenerateStart(entry);

        }

        private Point GenerateStart(TileType entry)
        {
            Point start = new Point(rand.Next(2, tiles.GetLength(0) - 3), rand.Next(2, tiles.GetLength(1) - 3));
            tiles[start.X, start.Y] = new Tile(entry, true);

            return start;
        }

        private void GenerateCorridor()
        {

        }

        private int GenerateRoom(Point dimensions)
        {
            int numberOfEntrences = 1;

            return numberOfEntrences;
        }

        private void MakeEntrence(Point entrenceTile, Point direction)
        {
            tiles[entrenceTile.X, entrenceTile.Y] = new Tile(TileType.basic, true);

            if (direction.X == 0)
            {
                if (direction.Y == 1)
                {
                    tiles[entrenceTile.X - 1, entrenceTile.Y] = new Tile(TileType.TopRightCorner, false);
                    tiles[entrenceTile.X + 1, entrenceTile.Y] = new Tile(TileType.TopLeftCorner, false);
                }
                else
                {
                    tiles[entrenceTile.X - 1, entrenceTile.Y] = new Tile(TileType.BottomRightCorner, false);
                    tiles[entrenceTile.X + 1, entrenceTile.Y] = new Tile(TileType.BottomLeftCorner, false);
                }
            }
            else
            {
                if (direction.X == 1)
                {
                    tiles[entrenceTile.X, entrenceTile.Y - 1] = new Tile(TileType.BottomLeftCorner, false);
                    tiles[entrenceTile.X, entrenceTile.Y + 1] = new Tile(TileType.BottomRightCorner, false);
                }
                else
                {
                    tiles[entrenceTile.X, entrenceTile.Y - 1] = new Tile(TileType.TopRightCorner, false);
                    tiles[entrenceTile.X, entrenceTile.Y + 1] = new Tile(TileType.TopLeftCorner, false);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}