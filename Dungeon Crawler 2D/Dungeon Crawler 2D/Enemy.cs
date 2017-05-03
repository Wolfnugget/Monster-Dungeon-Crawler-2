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
        public EnemyType theEnemy;

        public Enemy(TextureManager textures, EnemyType theEnemy, Object.Player player)
        {
            this.textures = textures;
            this.theEnemy = theEnemy;
            this.player = player;
            ability = new Abilities(UsedBy.enemy);
            GiveStats();
        }

        public bool Update()
        {
            if (theEnemy == EnemyType.zombie)
            {
                if (stats.CheckStat(Stat.mana) >= ability.CheckCost(UsedAbility.PoisonHit))
                {
                    switch (rand.Next(0, 4))
                    {
                        case 0:
                            ability.Ability(this, player, UsedAbility.Hit);
                            break;
                        case 1:
                            ability.Ability(this, player, UsedAbility.PoisonHit);
                            stats.ChangeStat(Stat.mana, -ability.CheckCost(UsedAbility.PoisonHit));
                            break;
                        default:
                            ability.Ability(this, player, UsedAbility.Defence);
                            break;
                    }
                }
                else
                {
                    switch (rand.Next(0, 3))
                    {
                        case 0:
                            ability.Ability(this, player, UsedAbility.Hit);
                            break;
                        default:
                            ability.Ability(this, player, UsedAbility.Defence);
                            break;
                    }
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void GiveStats()
        {
            if (theEnemy == EnemyType.zombie)
            {
                if (player.stats.CheckStat(Stat.level) <= 5)
                {
                    int health = rand.Next(50, 70) + 5 * player.stats.CheckStat(Stat.level);
                    int mana = rand.Next(10, 30) + 2 * player.stats.CheckStat(Stat.level);

                    int strenght = rand.Next(6, 12) + player.stats.CheckStat(Stat.level);
                    int inteligence = 0;
                    int dextarity = rand.Next(3, 8) + player.stats.CheckStat(Stat.level);
                    int luck = 0;
                    int speed = rand.Next(6, 15) + player.stats.CheckStat(Stat.level);
                    int xp = rand.Next(20, 40) + 2 * player.stats.CheckStat(Stat.level);
                    stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, xp, 0);
                }
                else if (player.stats.CheckStat(Stat.level) <= 10)
                {
                    int health = rand.Next(75, 95) + 4 * player.stats.CheckStat(Stat.level);
                    int mana = rand.Next(30, 50) + 3 * player.stats.CheckStat(Stat.level);

                    int strenght = 12 + rand.Next(1, 7) + player.stats.CheckStat(Stat.level);
                    int inteligence = 0;
                    int dextarity = 7 + rand.Next(1, 9) + player.stats.CheckStat(Stat.level);
                    int luck = rand.Next(5, 10);
                    int speed = 7 + rand.Next(1, 6) + player.stats.CheckStat(Stat.level);
                    stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, 0, 0);
                }
                else
                {
                    int health = 150 + rand.Next(0, 50) + 2 * player.stats.CheckStat(Stat.level);
                    int mana = 100 + rand.Next(20, 70) + player.stats.CheckStat(Stat.level);

                    int strenght = 20 + rand.Next(1, 7) + player.stats.CheckStat(Stat.level);
                    int inteligence = 0;
                    int dextarity = rand.Next(5, 10) + player.stats.CheckStat(Stat.level);
                    int luck = rand.Next(5, 20);
                    int speed = rand.Next(10, 20) + player.stats.CheckStat(Stat.level);
                    stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, 0, 0);
                }
            }
        }
    }
}
