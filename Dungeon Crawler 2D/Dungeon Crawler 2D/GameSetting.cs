using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawler_2D
{
    public enum GameState
    {
        Explore,
        Battle,
        Inventory
    }

    public static class GameSettings
    {
        public static int windowHeight;
        public static int windowWidth;

        public static GameState gameState;

        public static void SetDefault()
        {
            windowHeight = 800;
            windowWidth = 1200;
        }

        private static Keys _up = Keys.W;
        private static Keys _Right = Keys.D;
        private static Keys _Down = Keys.S;
        private static Keys _Left = Keys.A;

        public static Keys Up { get { return _up; } }
        public static Keys Right { get { return _Right; } }
        public static Keys Down { get { return _Down; } }
        public static Keys Left { get { return _Left; } }
    }
}
