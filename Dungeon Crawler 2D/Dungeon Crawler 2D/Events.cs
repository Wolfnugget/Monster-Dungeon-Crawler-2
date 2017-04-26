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
        CheckDirection,
        EnterTile
    }

    public delegate void ActorEventHandler(object Object, ActorEventArgs args);

    public delegate void MapEventHandler(object Object, MapEventArgs args);

    /// <summary>
    /// All information som kan skickas från spelaren eller annan actor via events.
    /// </summary>
    public class ActorEventArgs : EventArgs
    {
        public Vector2 Position { get; set; }
        public Point Direction { get; set; }
        public PlayerEventType EventType;

        public ActorEventArgs(PlayerEventType EventType)
        {
            this.EventType = EventType;
        }
    }

    /// <summary>
    /// All information som kan skickas från map via event.
    /// </summary>
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
