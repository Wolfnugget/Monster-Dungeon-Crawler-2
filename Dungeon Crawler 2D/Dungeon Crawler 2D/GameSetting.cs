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

        private static Keys _up = Keys.Up;
        private static Keys _Right = Keys.Right;
        private static Keys _Down = Keys.Down;
        private static Keys _Left = Keys.Left;

        private static Keys _aup = Keys.W;
        private static Keys _aRight = Keys.D;
        private static Keys _aDown = Keys.S;
        private static Keys _aLeft = Keys.A;

        private static Keys _openMenu = Keys.Escape;

        public static Keys Up { get { return _up; } }
        public static Keys Right { get { return _Right; } }
        public static Keys Down { get { return _Down; } }
        public static Keys Left { get { return _Left; } }

        public static Keys Alt_Up { get { return _aup; } }
        public static Keys Alt_Right { get { return _aRight; } }
        public static Keys Alt_Down { get { return _aDown; } }
        public static Keys Alt_Left { get { return _aLeft; } }

        public static Keys OpenMenu { get { return _openMenu; } }
    }
}
