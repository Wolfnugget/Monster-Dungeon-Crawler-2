using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawler_2D
{
    public enum Effects
    {
        poison, bleed, confusion, strenghtBuff, intelligenceBuff, dexterityBuff, luckBuff, none
    }
    public enum Stat
    {
        maxHealth, health, maxMana, mana, strength, inteligence, dexterity, luck, speed, maxXp, xp, level
    }
    public enum EffectPoint
    {
        attack, endPhase, startPhase
    }

    class Stats
    {
        
        private int maxHealth, health, maxMana, mana, strength, inteligence, dextarity, luck, speed, maxXp, xp, level;
        public int uppgrade;
        private List<Effect> activeEffects;
        private TextureManager textures;

        public Stats(TextureManager textures, int maxHealth, int health, int maxMana, int mana, int strength, 
            int inteligence, int dextarity, int luck, int speed, int maxXp, int xp, int level)
        {
            this.textures = textures;
            this.maxHealth = maxHealth;
            this.health = health;
            this.maxMana = maxMana;
            this.mana = mana;
            this.strength = strength;
            this.inteligence = inteligence;
            this.dextarity = dextarity;
            this.luck = luck;
            this.speed = speed;
            this.maxXp = maxXp;
            this.xp = xp;
            this.level = level;
            uppgrade = 0;
            activeEffects = new List<Effect>(8);//number goes up with amount of effects
        }

        // used when a stat goes upp or down
        public void ChangeStat(Stat stat, int addition)
        {
            switch (stat)
            {
                case Stat.maxHealth:
                    maxHealth += addition;
                    break;
                case Stat.health:
                    health += addition;
                    if (health <= 0)
                    {
                        health = 0;
                    }
                    if (health >= maxHealth)
                    {
                        health = maxHealth;
                    }
                    break;
                case Stat.maxMana:
                    maxMana += addition;
                    break;
                case Stat.mana:
                    mana += addition;
                    if (mana <= 0)
                    {
                        mana = 0;
                    }
                    if (mana >= maxMana)
                    {
                        mana = maxMana;
                    }
                    break;
                case Stat.strength:
                    strength += addition;
                    break;
                case Stat.inteligence:
                    inteligence += addition;
                    break;
                case Stat.dexterity:
                    dextarity += addition;
                    break;
                case Stat.luck:
                    luck += addition;
                    break;
                case Stat.speed:
                    speed += addition;
                    break;
                case Stat.maxXp:
                    maxXp += addition;
                    break;
                case Stat.xp:
                    xp += addition;
                    if (xp >= maxXp)
                    {
                        xp -= maxXp;
                        LevelUpp();
                        maxXp += 10;
                    }
                    if (xp <= 0)
                    {
                        xp = 0;
                    }
                    break;
            }
        }
        
        public int CheckStat(Stat stat)
        {
            switch (stat)
            {
                case Stat.maxHealth:
                    return maxHealth;
                case Stat.health:
                    return health;
                case Stat.maxMana:
                    return maxMana;
                case Stat.mana:
                    return mana;
                case Stat.strength:
                    return strength;
                case Stat.inteligence:
                    return inteligence;
                case Stat.dexterity:
                    return dextarity;
                case Stat.luck:
                    return luck;
                case Stat.speed:
                    return speed;
                case Stat.maxXp:
                    return maxXp;
                case Stat.xp:
                    return xp;
                case Stat.level:
                    return level;
                default :
                    return 0;
            }
            
            
        }

        public void LevelUpp()
        {
            if (level <= 5)
            {
                uppgrade += 3;
            }
            else if (level <= 10)
            {
                uppgrade += 5;
            }
            else if (level <= 15)
            {
                uppgrade += 7;
            }
            else if (level > 15)
            {
                uppgrade += 10;
            }
            level += 1;
        }

        public void AddEffect(int length, Effects effect, int power)
        {
            if (CheckEffects(effect) == false)
            {
                activeEffects.Add(new Effect(textures, effect, length, this, power));
            }
            else
            {
                foreach (Effect e in activeEffects)
                {
                    if (e.effect == effect)
                    {
                        e.timer += length;
                        e.power = power;
                    }
                }
            }
        }

        public void UpdateEffects()
        {
            foreach (Effect e in activeEffects)
            {
                e.Update();
            }
        }

        public bool CheckEffects(Effects effect)
        {
            foreach(Effect e in activeEffects)
            {
                if (e.effect == effect)
                {
                    return true;
                }
            }
            return false;
        }

        public int CheckEffectTime(Effects effect)
        {
            foreach (Effect e in activeEffects)
            {
                if (e.effect == effect)
                {
                    if (e.timer <= 0)
                    {
                        e.timer = 0;
                        
                    }
                    return e.timer;                   
                }
            }
            return 0;
        }

        public int CheckBuff(Stat stat) //needs to be changed
        {
            switch (stat)
            {
                case Stat.strength:
                    foreach (Effect e in activeEffects)
                    {   
                        if (e.effect == Effects.strenghtBuff)
                        {
                            return e.power;
                        }
                    }
                    return 1;
                case Stat.inteligence:
                    foreach (Effect e in activeEffects)
                    {
                        if (e.effect == Effects.intelligenceBuff)
                        {
                            return e.power;
                        }
                    }
                    return 1;
                case Stat.dexterity:
                    foreach (Effect e in activeEffects)
                    {
                        if (e.effect == Effects.dexterityBuff || e.effect == Effects.confusion)
                        {
                            return e.power;
                        }
                    }
                    return 1;
                case Stat.luck:
                    foreach (Effect e in activeEffects)
                    {
                        if (e.effect == Effects.luckBuff)
                        {
                            return e.power;
                        }
                    }
                    return 1;
                default :
                    return 0; //should never happen. just there bc it needs to be
            }
        }
    }
}
