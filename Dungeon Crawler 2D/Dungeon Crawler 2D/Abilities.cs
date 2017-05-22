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
    public enum UsedBy
    {
        player, enemy
    }

    public enum UsedAbility
    {
        Hit, Defence, Magic, Dodge, Miss, PoisonHit, confusion//osv
    }

    class Abilities
    {
        public UsedAbility usedAbility;
        private UsedBy usedBy;
        public Effects effect;
        public int power;
        private Random rand = new Random();
        protected int frame;
        SpriteEffects spriteFx;
        protected Rectangle srcRec = new Rectangle(0, 0, 16, 16);
        protected TextureManager textures;
        Texture2D texture;
        Vector2 position;

        public Abilities(UsedBy usedBy, TextureManager textures)
        {
            this.usedBy = usedBy;
            this.textures = textures;
        }

        public void Ability(Enemy enemy, Object.Player player, UsedAbility ability)
        {
            #region Hit
            if (ability == UsedAbility.Hit)
            {
                if (usedBy == UsedBy.player)
                {
                    int accuracy = player.stats.CheckStat(Stat.accuracy) + 
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
                    int accuracy = enemy.stats.CheckStat(Stat.accuracy) + 
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
                    power = (player.stats.CheckStat(Stat.strength) / 2) + (player.stats.CheckStat(Stat.accuracy) / 2);
                }
                else
                {
                    usedAbility = UsedAbility.Defence;
                    power = (enemy.stats.CheckStat(Stat.strength) / 2) + (enemy.stats.CheckStat(Stat.accuracy) / 2);
                }
            }
            #endregion

            #region Magic
            if (ability == UsedAbility.Magic)
            {
                if (usedBy == UsedBy.player)
                {
                    int accuracy = player.stats.CheckStat(Stat.accuracy) + 
                        (player.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 80);
                    if (accuracy >= 50)
                    {
                        usedAbility = UsedAbility.Magic;
                        if (accuracy >= 100)
                        {
                            power = player.stats.CheckStat(Stat.intelligence) + player.stats.CheckStat(Stat.luck);
                            effect = Effects.confusion;
                        }
                        else
                        {
                            power = player.stats.CheckStat(Stat.intelligence) + player.stats.CheckStat(Stat.luck);
                            effect = Effects.none;
                        }
                    }
                    else { usedAbility = UsedAbility.Miss; power = 0; effect = Effects.none; }
                }
                else
                {
                    int accuracy = enemy.stats.CheckStat(Stat.accuracy) + 
                        (enemy.stats.CheckStat(Stat.luck) / 2) + rand.Next(0, 100);
                    if (accuracy >= 50)
                    {
                        this.usedAbility = UsedAbility.Magic;
                        if (accuracy >= 100)
                        {
                            power = enemy.stats.CheckStat(Stat.intelligence) + enemy.stats.CheckStat(Stat.luck);
                            effect = Effects.confusion;
                        }
                        else
                        {
                            power = enemy.stats.CheckStat(Stat.intelligence) + enemy.stats.CheckStat(Stat.luck);
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
                    int accuracy = player.stats.CheckStat(Stat.accuracy) + 
                        (player.stats.CheckStat(Stat.luck)) + rand.Next(0, 80);
                    if (accuracy >= 50)
                    {
                        usedAbility = UsedAbility.Dodge;
                    }
                    else usedAbility = UsedAbility.Miss;
                }
                else
                {
                    int accuracy = enemy.stats.CheckStat(Stat.accuracy) + 
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
                    int accuracy = player.stats.CheckStat(Stat.accuracy) + 
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
                    int accuracy = enemy.stats.CheckStat(Stat.accuracy) + 
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

            #region Auto-Miss

            if (ability == UsedAbility.Miss)
            {
                usedAbility = UsedAbility.Miss; 
                power = 0; 
                effect = Effects.none; 
            }

            #endregion
        }

        public int CheckCost(UsedAbility ability)
        {
            switch (ability)
            {
                case UsedAbility.Magic:
                    return 15;
                case UsedAbility.PoisonHit:
                    return 25;
                default: return 0; //bör ej tillkomma
            }
        }

        public void BattleAnimation()
        {
            srcRec.X = (frame % 4) * 16;
            frame++;
        }

        public void Draw(SpriteBatch spriteBatch, UsedBy by)
        {
            if (by == UsedBy.enemy)
            {
                spriteFx = SpriteEffects.FlipHorizontally;
                position = new Vector2(700, 300);
            }
            else { position = new Vector2(325, 300); spriteFx = SpriteEffects.None; }

            switch (usedAbility)
            {
                case UsedAbility.Hit:
                    texture = textures.hitAnimation;
                    break;
                case UsedAbility.Magic:
                    texture = textures.magicAnimation;
                    break;
                case UsedAbility.Defence:
                    texture = textures.defenceAnimation;
                    spriteFx = SpriteEffects.None;
                    break;
                case UsedAbility.Dodge:
                    texture = textures.dodgeAnimation;
                    break;
                case UsedAbility.PoisonHit:
                    texture = textures.poisonHitAnimation;
                    break;
                case UsedAbility.Miss:
                    texture = textures.missAnimation;
                    break;
                case UsedAbility.confusion:
                    texture = textures.confusion;
                    spriteFx = SpriteEffects.None;
                    break;
            }

            try
            {
                spriteBatch.Draw(texture, position, srcRec, Color.White, 0,
                    new Vector2(), 10, spriteFx, 1);
            }
            catch { }
        }
    }
}

