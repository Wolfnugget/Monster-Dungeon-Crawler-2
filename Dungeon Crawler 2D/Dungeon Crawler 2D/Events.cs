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

    public enum PlayerEventType
    {
        CheckDirection
    }

    public delegate void PlayerEventHandler(object Object, PlayerEventArgs args);

    public delegate void MapEventHandler(object Object, MapEventArgs args);

    public class PlayerEventArgs : EventArgs
    {
        public Vector2 Position { get; set; }
        public Point Direction { get; set; }
        public PlayerEventType EventType;

        public PlayerEventArgs(PlayerEventType EventType)
        {
            this.EventType = EventType;
        }
    }

    public class MapEventArgs : EventArgs
    {
        public Vector2 Position { get; set; }
        public MapEventType EventType;

        public MapEventArgs(MapEventType EventType)
        {
            this.EventType = EventType;
        }
    }
}
