using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D
{
    public enum Effects
    {
        poison, bleed, confusion, none
    }
    public enum Stat
    {
        maxHealth, health, maxMana, mana, strength, intelligence, accuracy, luck, speed, maxXp, xp, level
    }

    class Stats
    {
        
        private int maxHealth, health, maxMana, mana, strength, intelligence, accuracy, luck, speed, maxXp, xp, level;
        public int upgrade;
        private List<Effect> activeEffects;
        private TextureManager textures;

        public Stats(TextureManager textures, int maxHealth, int health, int maxMana, int mana, int strength, 
            int intelligence, int accuracy, int luck, int speed, int maxXp, int xp, int level)
        {
            this.textures = textures;
            this.maxHealth = maxHealth;
            this.health = health;
            this.maxMana = maxMana;
            this.mana = mana;
            this.strength = strength;
            this.intelligence = intelligence;
            this.accuracy = accuracy;
            this.luck = luck;
            this.speed = speed;
            this.maxXp = maxXp;
            this.xp = xp;
            this.level = level;
            upgrade = 0;
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
                case Stat.intelligence:
                    intelligence += addition;
                    break;
                case Stat.accuracy:
                    accuracy += addition;
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
                    while (xp >= maxXp)
                    {
                        xp -= maxXp;
                        LevelUpp();
                    }
                    break;
                case Stat.level:
                    LevelUpp();
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
                case Stat.intelligence:
                    return intelligence;
                case Stat.accuracy:
                    return accuracy;
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
            level += 1;
            if (level <= 5)
            {
                upgrade += 3;
                ChangeStat(Stat.maxHealth, 10);
                ChangeStat(Stat.health, 20);
                ChangeStat(Stat.maxMana, 10);
                ChangeStat(Stat.mana, 20);
                ChangeStat(Stat.maxXp, 10);
            }
            else if (level <= 10)
            {
                upgrade += 5;
                ChangeStat(Stat.maxHealth, 15);
                ChangeStat(Stat.health, 25);
                ChangeStat(Stat.maxMana, 15);
                ChangeStat(Stat.mana, 25);
                ChangeStat(Stat.maxXp, 20);
            }
            else if (level <= 15)
            {
                upgrade += 7;
                ChangeStat(Stat.maxHealth, 20);
                ChangeStat(Stat.health, 30);
                ChangeStat(Stat.maxMana, 20);
                ChangeStat(Stat.mana, 30);
                ChangeStat(Stat.maxXp, 30);
            }
            else if (level > 15)
            {
                upgrade += 10;
                ChangeStat(Stat.maxHealth, 20);
                ChangeStat(Stat.health, 40);
                ChangeStat(Stat.maxMana, 20);
                ChangeStat(Stat.mana, 40);
                ChangeStat(Stat.maxXp, 40);
            }
            
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
                if (e.effect == effect && e.timer > 0)
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

        public void DrawEffect(SpriteBatch spriteBatch, Effects effect, UsedBy on)
        {
            foreach (Effect e in activeEffects)
            {
                if (e.effect == effect)
                {
                    e.Draw(spriteBatch, on);
                }
            }
        }

        public void RestoreHealthAndMana()
        {
            health = maxHealth;
            mana = maxMana;
        }

    }
}
