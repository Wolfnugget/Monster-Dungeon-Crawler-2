using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Dungeon_Crawler_2D
{
    public enum EnemyType
    {
        zombie
    }

    class Enemy
    {
        TextureManager textures;
        public Stats stats;
        Random rand = new Random();
        public Abilities ability;
        Object.Player player;
        EnemyType theEnemy;

        public Enemy(TextureManager textures, EnemyType theEnemy, Object.Player player)
        {
            this.textures = textures;
            this.theEnemy = theEnemy;
            this.player = player;
            ability = new Abilities(UsedBy.enemy);
            GiveStats();
        }

        public void Update()
        {
            if (theEnemy == EnemyType.zombie)
            {
                if (rand.Next(0,2) == 1)
                {
                    ability.Ability(this, player, UsedAbility.Hit);
                }
                else
                {
                    ability.Ability(this, player, UsedAbility.Defence);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void GiveStats()
        {
            if (theEnemy == EnemyType.zombie)
            {
                int health = rand.Next(100, 150) + 20 * player.stats.CheckStat(Stat.level);
                int mana = rand.Next(100, 150) + 10 * player.stats.CheckStat(Stat.level);

                int strenght = rand.Next(6, 15) + 3 * player.stats.CheckStat(Stat.level);
                int inteligence = rand.Next(6, 15) + 3 * player.stats.CheckStat(Stat.level);
                int dextarity = rand.Next(6, 15) + 3 * player.stats.CheckStat(Stat.level);
                int luck = rand.Next(6, 15) + 3 * player.stats.CheckStat(Stat.level);
                int speed = rand.Next(6, 15) + 3 * player.stats.CheckStat(Stat.level);
                stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, 0, 0);
            }
        }
        
    }
}
