using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawler_2D
{
    public enum UsedBy
    {
        player, enemy
    }

    public enum UsedAbility
    {
        Hit, Defence, Magic, Dodge, miss//osv
    }

    class Abilities
    {
        public UsedAbility usedAbility;
        private UsedBy usedBy;
        public int power;
        private Random rand = new Random();

        public Abilities(UsedBy usedBy)
        {
            this.usedBy = usedBy;
        }


        public void Hit(Enemy enemy, Object.Player player)
        {
            if (usedBy == UsedBy.player)
            {
                int accuracy = player.stats.CheckStat(Stat.dexterity) + (player.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                if (accuracy >= 50)
                {
                    usedAbility = UsedAbility.Hit;
                    if (accuracy >= 100)
                    {
                        power = player.stats.CheckStat(Stat.strength);
                        enemy.stats.AddEffect(3, Effects.bleed, 1);
                    }
                    else
                    {
                        power = player.stats.CheckStat(Stat.strength);
                    }
                }
                else { usedAbility = UsedAbility.miss; power = 0; }
            }
            else
            {
                int accuracy = enemy.stats.CheckStat(Stat.dexterity) + (enemy.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                if (accuracy >= 50)
                {
                    usedAbility = UsedAbility.Hit;
                    if (accuracy >= 100)
                    {
                        power = enemy.stats.CheckStat(Stat.strength) * 2;
                        player.stats.AddEffect(3, Effects.bleed, 1);
                    }
                    else
                    {
                        power = enemy.stats.CheckStat(Stat.strength);
                    }
                }
                else { usedAbility = UsedAbility.miss; power = 0; }
            }
        }

        public void Defence(Enemy enemy, Object.Player player)
        {
            usedAbility = UsedAbility.Defence;
            if (usedBy == UsedBy.player)
            {
                power = (player.stats.CheckStat(Stat.strength) / 2) + (player.stats.CheckStat(Stat.dexterity) / 2);
            }
            else
            {
                power = (enemy.stats.CheckStat(Stat.strength) / 2) + (enemy.stats.CheckStat(Stat.dexterity) / 2);
            }
        }


        public void Magic(Enemy enemy, Object.Player player)
        {
            if (usedBy == UsedBy.player)
            {
                int accuracy = player.stats.CheckStat(Stat.dexterity) + (player.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 80);
                if (accuracy >= 50)
                {
                    usedAbility = UsedAbility.Magic;
                    if (accuracy >= 100)
                    {
                        power = player.stats.CheckStat(Stat.inteligence) + player.stats.CheckStat(Stat.luck);
                        enemy.stats.AddEffect(3, Effects.confusion, 1);
                    }
                    else
                    {
                        power = player.stats.CheckStat(Stat.inteligence) + player.stats.CheckStat(Stat.luck);
                    }
                }
                else { usedAbility = UsedAbility.miss; power = 0; }
            }
            else
            {
                int accuracy = enemy.stats.CheckStat(Stat.dexterity) + (enemy.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                if (accuracy >= 50)
                {
                    usedAbility = UsedAbility.Magic;
                    if (accuracy >= 100)
                    {
                        power = enemy.stats.CheckStat(Stat.inteligence) + enemy.stats.CheckStat(Stat.luck);
                        enemy.stats.AddEffect(3, Effects.confusion, 1);
                    }
                    else
                    {
                        power = enemy.stats.CheckStat(Stat.inteligence) + enemy.stats.CheckStat(Stat.luck);
                    }
                }
                else { usedAbility = UsedAbility.miss; power = 0; }
            }
        }


        public void Dodge(Enemy enemy, Object.Player player)
        {
            if (usedBy == UsedBy.player)
            {
                int accuracy = player.stats.CheckStat(Stat.dexterity) + (player.stats.CheckStat(Stat.luck)) + rand.Next(0, 80);
                if (accuracy >= 50)
                {
                    usedAbility = UsedAbility.Dodge;
                }
                else usedAbility = UsedAbility.miss;
            }
            else
            {
                int accuracy = enemy.stats.CheckStat(Stat.dexterity) + (enemy.stats.CheckStat(Stat.luck)) + rand.Next(0, 80);
                if (accuracy >= 50)
                {
                    usedAbility = UsedAbility.Dodge;
                }
                else usedAbility = UsedAbility.miss;
            }
        }

        public void PoisonHit(Enemy enemy, Object.Player player)
        {
            if (usedBy == UsedBy.player)
            {
                int accuracy = player.stats.CheckStat(Stat.dexterity) + (player.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                if (accuracy >= 50)
                {
                    usedAbility = UsedAbility.Hit;
                    if (accuracy >= 100)
                    {
                        power = player.stats.CheckStat(Stat.strength);
                        enemy.stats.AddEffect(3, Effects.poison, 1);
                    }
                    else
                    {
                        power = player.stats.CheckStat(Stat.strength) / 2;
                        enemy.stats.AddEffect(3, Effects.poison, 1);
                    }
                }
                else { usedAbility = UsedAbility.miss; power = 0; }
            }
            else
            {
                int accuracy = enemy.stats.CheckStat(Stat.dexterity) + (enemy.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                if (accuracy >= 50)
                {
                    usedAbility = UsedAbility.Hit;
                    if (accuracy >= 100)
                    {
                        power = enemy.stats.CheckStat(Stat.strength);
                        player.stats.AddEffect(3, Effects.poison, 1);
                    }
                    else
                    {
                        power = enemy.stats.CheckStat(Stat.strength) / 2;
                        player.stats.AddEffect(3, Effects.poison, 1);
                    }
                }
                else { usedAbility = UsedAbility.miss; power = 0; }
            }
        }
    }
}

