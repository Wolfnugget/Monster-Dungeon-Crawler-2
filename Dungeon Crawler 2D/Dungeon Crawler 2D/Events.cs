using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D
{
    public enum MapEventType
    {
        ChangeRoom,
        Move
    }

    public delegate void MapEventHandler(MapEventType eventType, Vector2 position);
}
