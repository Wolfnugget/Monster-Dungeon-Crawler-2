using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawler_2D
{
    public enum Effects
    {
        poison, bleed, confusion, strenghtBuff
    }
    public enum Stat
    {
        maxHealth, health, maxMana, mana, strength, inteligence, dextarity, luck, experience
    }

    class Stats
    {
        
        private int maxHealth, health, maxMana, mana, strength, inteligence, dextarity, luck, experience;
        private List<Effect> activeEffects;
        private TextureManager textures;

        public Stats(TextureManager textures, int maxHealth, int health, int maxMana, int mana, int strength, 
            int inteligence, int dextarity, int luck, int experience)
        {
            this.textures = textures;
            this.health = maxHealth;
            this.health = health;
            this.maxMana = maxMana;
            this.mana = mana;
            this.strength = strength;
            this.inteligence = inteligence;
            this.dextarity = dextarity;
            this.luck = luck;
            this.experience = experience;
            activeEffects = new List<Effect>(4);//number goes up with amount of effects
        }

        // used when a stat goes upp or down
        public void ChangeStat(int addition, Stat stat)
        {
            switch (stat)
            {
                case Stat.maxHealth:
                    maxHealth += addition;
                    break;
                case Stat.health:
                    health += addition;
                    break;
                case Stat.maxMana:
                    maxMana += addition;
                    break;
                case Stat.mana:
                    mana += addition;
                    break;
                case Stat.strength:
                    strength += addition;
                    break;
                case Stat.inteligence:
                    inteligence += addition;
                    break;
                case Stat.dextarity:
                    dextarity += addition;
                    break;
                case Stat.luck:
                    luck += addition;
                    break;
                case Stat.experience:
                    experience += addition;
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
                case Stat.dextarity:
                    return dextarity;
                case Stat.luck:
                    return luck;
                default :
                    return experience;
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
                    return e.timer;
                }
            }
            return 0;
        }

        public void Buff(int multiplication, Stat stat) //needs to be changed
        {
            switch (stat)
            {
                case Stat.strength:
                    strength *= multiplication;
                    break;
                case Stat.inteligence:
                    inteligence *= multiplication;
                    break;
                case Stat.dextarity:
                    dextarity *= multiplication;
                    break;
                case Stat.luck:
                    luck *= multiplication;
                    break;
            }
        }


    }
}
