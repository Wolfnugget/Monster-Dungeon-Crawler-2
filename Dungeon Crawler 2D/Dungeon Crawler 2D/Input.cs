using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dungeon_Crawler_2D
{
    public static class Input
    {
        public static bool Up { get { return Keyboard.GetState().IsKeyDown(Keys.W); } }
        public static bool Down { get { return Keyboard.GetState().IsKeyDown(Keys.S); } }
        public static bool Right { get { return Keyboard.GetState().IsKeyDown(Keys.D); } }
        public static bool Left { get { return Keyboard.GetState().IsKeyDown(Keys.A); } }
    }
}
