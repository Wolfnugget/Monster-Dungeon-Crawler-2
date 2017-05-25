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
        Move,
        StartCombat,
        PotionPickup
    }

    public enum PlayerEventType
    {
        CheckDirection,
        EnterTile
    }

    public enum EndCombat
    {
        Won, 
        Lost
    }

    public delegate void ActorEventHandler(object Object, ActorEventArgs args);

    public delegate void MapEventHandler(object Object, MapEventArgs args);

    public delegate void CombatEventHandler(object Object, BattleEvensArgs args);

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
        public EnemyType enemy;
        public PickUp potionType;


        public MapEventArgs(MapEventType EventType)
        {
            this.EventType = EventType;
        }
    }

    public class BattleEvensArgs : EventArgs
    {
        public EndCombat result;
        public int xp;
        public Dictionary<Stat, int> statReward;
        public EnemyType enemyType;

        public BattleEvensArgs()
        {
            statReward = new Dictionary<Stat, int>();
        }
    }
}
