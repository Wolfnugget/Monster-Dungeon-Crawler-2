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
        Hit, Defence, Magic, Dodge, Miss, PoisonHit//osv
    }

    class Abilities
    {
        public UsedAbility usedAbility;
        private UsedBy usedBy;
        public Effects effect;
        public int power;
        private Random rand = new Random();

        public Abilities(UsedBy usedBy)
        {
            this.usedBy = usedBy;
        }

        public void Ability(Enemy enemy, Object.Player player, UsedAbility ability)
        {

            #region Hit
            if (ability == UsedAbility.Hit)
            {
                if (usedBy == UsedBy.player)
                {
                    int accuracy = player.stats.CheckStat(Stat.dexterity) + 
                        (player.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                    if (accuracy >= 50)
                    {
                        usedAbility = UsedAbility.Hit;
                        if (accuracy >= 100)
                        {
                            power = player.stats.CheckStat(Stat.strength);
                            effect = Effects.bleed;
                        }
                        else
                        {
                            power = player.stats.CheckStat(Stat.strength);
                            effect = Effects.none;
                        }
                    }
                    else { usedAbility = UsedAbility.Miss; power = 0; effect = Effects.none; }
                }
                else
                {
                    int accuracy = enemy.stats.CheckStat(Stat.dexterity) + 
                        (enemy.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                    if (accuracy >= 50)
                    {
                        usedAbility = UsedAbility.Hit;
                        if (accuracy >= 100)
                        {
                            power = enemy.stats.CheckStat(Stat.strength) * 2;
                            effect = Effects.bleed;
                        }
                        else
                        {
                            power = enemy.stats.CheckStat(Stat.strength);
                            effect = Effects.none;
                        }
                    }
                    else { usedAbility = UsedAbility.Miss; power = 0; effect = Effects.none; }
                }
            }
            #endregion

            #region Defence

            if (ability == UsedAbility.Defence)
            {
                effect = Effects.none;
                if (usedBy == UsedBy.player)
                {
                    usedAbility = UsedAbility.Defence;
                    power = (player.stats.CheckStat(Stat.strength) / 2) + (player.stats.CheckStat(Stat.dexterity) / 2);
                }
                else
                {
                    usedAbility = UsedAbility.Defence;
                    power = (enemy.stats.CheckStat(Stat.strength) / 2) + (enemy.stats.CheckStat(Stat.dexterity) / 2);
                }
            }
            #endregion

            #region Magic
            if (ability == UsedAbility.Magic)
            {
                if (usedBy == UsedBy.player)
                {
                    int accuracy = player.stats.CheckStat(Stat.dexterity) + 
                        (player.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 80);
                    if (accuracy >= 50)
                    {
                        usedAbility = UsedAbility.Magic;
                        if (accuracy >= 100)
                        {
                            power = player.stats.CheckStat(Stat.inteligence) + player.stats.CheckStat(Stat.luck);
                            effect = Effects.confusion;
                        }
                        else
                        {
                            power = player.stats.CheckStat(Stat.inteligence) + player.stats.CheckStat(Stat.luck);
                            effect = Effects.none;
                        }
                    }
                    else { usedAbility = UsedAbility.Miss; power = 0; effect = Effects.none; }
                }
                else
                {
                    int accuracy = enemy.stats.CheckStat(Stat.dexterity) + 
                        (enemy.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                    if (accuracy >= 50)
                    {
                        this.usedAbility = UsedAbility.Magic;
                        if (accuracy >= 100)
                        {
                            power = enemy.stats.CheckStat(Stat.inteligence) + enemy.stats.CheckStat(Stat.luck);
                            effect = Effects.confusion;
                        }
                        else
                        {
                            power = enemy.stats.CheckStat(Stat.inteligence) + enemy.stats.CheckStat(Stat.luck);
                            effect = Effects.none;
                        }
                    }
                    else { this.usedAbility = UsedAbility.Miss; power = 0; effect = Effects.none; }
                }
            }
            #endregion

            #region Dodge
            if (ability == UsedAbility.Dodge)
            {
                effect = Effects.none;
                if (usedBy == UsedBy.player)
                {
                    int accuracy = player.stats.CheckStat(Stat.dexterity) + 
                        (player.stats.CheckStat(Stat.luck)) + rand.Next(0, 80);
                    if (accuracy >= 50)
                    {
                        usedAbility = UsedAbility.Dodge;
                    }
                    else usedAbility = UsedAbility.Miss;
                }
                else
                {
                    int accuracy = enemy.stats.CheckStat(Stat.dexterity) + 
                        (enemy.stats.CheckStat(Stat.luck)) + rand.Next(0, 80);
                    if (accuracy >= 50)
                    {
                        usedAbility = UsedAbility.Dodge;
                    }
                    else usedAbility = UsedAbility.Miss;
                }
            }
            #endregion

            #region Poison Hit
            if (ability == UsedAbility.PoisonHit)
            {
                if (usedBy == UsedBy.player)
                {
                    int accuracy = player.stats.CheckStat(Stat.dexterity) + 
                        (player.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                    if (accuracy >= 50)
                    {
                        usedAbility = UsedAbility.PoisonHit;
                        if (accuracy >= 100)
                        {
                            power = player.stats.CheckStat(Stat.strength);
                            effect = Effects.poison;
                        }
                        else
                        {
                            power = player.stats.CheckStat(Stat.strength) / 2;
                            effect = Effects.poison;
                        }
                    }
                    else { usedAbility = UsedAbility.Miss; power = 0; effect = Effects.none; }
                }
                else
                {
                    int accuracy = enemy.stats.CheckStat(Stat.dexterity) + 
                        (enemy.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                    if (accuracy >= 50)
                    {
                        usedAbility = UsedAbility.PoisonHit;
                        if (accuracy >= 100)
                        {
                            power = enemy.stats.CheckStat(Stat.strength);
                            effect = Effects.poison;
                        }
                        else
                        {
                            power = enemy.stats.CheckStat(Stat.strength) / 2;
                            effect = Effects.poison;
                        }
                    }
                    else { usedAbility = UsedAbility.Miss; power = 0; effect = Effects.none; }
                }
            }
            #endregion

        }

    }
}

