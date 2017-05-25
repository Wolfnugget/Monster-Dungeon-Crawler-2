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

        private static Keys _aUp = Keys.W;
        private static Keys _aRight = Keys.D;
        private static Keys _aDown = Keys.S;
        private static Keys _aLeft = Keys.A;

        private static Keys _ability1 = Keys.Q;
        private static Keys _ability2 = Keys.W;
        private static Keys _ability3 = Keys.E;
        private static Keys _ability4 = Keys.R;

        private static Keys _continue = Keys.Space;
        private static Keys _use = Keys.E;

        private static Keys _openInventory = Keys.Q;
        private static Keys _openMenu = Keys.Escape;

        public static Keys Up { get { return _up; } }
        public static Keys Right { get { return _Right; } }
        public static Keys Down { get { return _Down; } }
        public static Keys Left { get { return _Left; } }

        public static Keys Alt_Up { get { return _aUp; } }
        public static Keys Alt_Right { get { return _aRight; } }
        public static Keys Alt_Down { get { return _aDown; } }
        public static Keys Alt_Left { get { return _aLeft; } }

        public static Keys Ability_1 { get { return _ability1; } }
        public static Keys Ability_2 { get { return _ability2; } }
        public static Keys Ability_3 { get { return _ability3; } }
        public static Keys Ability_4 { get { return _ability4; } }

        public static Keys Continue { get { return _continue; } }
        public static Keys Use { get { return _use; } }

        public static Keys OpenInventory { get { return _openInventory; } }
        public static Keys OpenMenu { get { return _openMenu; } }
    }
}
