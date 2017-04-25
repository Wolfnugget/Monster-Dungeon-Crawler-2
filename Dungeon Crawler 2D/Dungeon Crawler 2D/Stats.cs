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
        maxHealth, health, maxMana, mana, strength, inteligence, dextarity, luck
    }

    class Stats
    {
        
        

        int maxHealth, health, maxMana, mana, strength, inteligence, dextarity, luck;
        List<Effect> activeEffects;
        TextureManager textures;

        public Stats(TextureManager textures, int maxHealth, int maxMana, int strength, int inteligence, int dextarity, int luck)
        {
            this.textures = textures;
            this.health = maxHealth;
            maxHealth = health;
            this.maxMana = maxMana;
            mana = maxMana;
            this.strength = strength;
            this.inteligence = inteligence;
            this.dextarity = dextarity;
            this.luck = luck;
            activeEffects = new List<Effect>(4);//number goes up 
        }

        public void AddStats(int addition, Stat stat)
        {
            if (stat == Stat.maxHealth)
            {
                maxHealth += addition;
            }
            if (stat == Stat.health)
            {
                health += addition;
            }
            if (stat == Stat.maxMana)
            {
                maxMana += addition;
            }
            if (stat == Stat.mana)
            {
                mana += addition;
            }
            if (stat == Stat.strength)
            {
                strength += addition;
            }
            if (stat == Stat.inteligence)
            {
                inteligence += addition;
            }
            if (stat == Stat.dextarity)
            {
                dextarity += addition;
            }
            if (stat == Stat.luck)
            {
                luck += addition;
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
                        e.timer = length;
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

        public void Buff(int multiplication, Stat stat)
        {
            if (stat == Stat.health)
            {
                health *= multiplication;
            }
            if (stat == Stat.mana)
            {
                mana *= multiplication;
            }
            if (stat == Stat.strength)
            {
                strength *= multiplication;
            }
            if (stat == Stat.inteligence)
            {
                inteligence *= multiplication;
            }
            if (stat == Stat.dextarity)
            {
                dextarity *= multiplication;
            }
            if (stat == Stat.luck)
            {
                luck *= multiplication;
            }
        }
    }
}
