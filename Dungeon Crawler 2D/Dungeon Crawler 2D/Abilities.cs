﻿using System;
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
        Hit, Defence, Magic, Dodge, Miss, PoisonHit//osv
    }

    class Abilities
    {
        public UsedAbility usedAbility;
        private UsedBy usedBy;
        public Effects effect;
        public int power;
        private Random rand = new Random();
        protected int startingFrame, frame, frames, frameSize;
        SpriteEffects spriteFx;
        double frameTimer, frameInterval;
        protected Rectangle srcRec = new Rectangle(0, 0, 16, 16);
        protected TextureManager textures;

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

        public void BattleAnimation(SpriteBatch spriteBatch, GameTime gameTime, UsedBy by, UsedAbility ability)
        {
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (by == UsedBy.enemy)
            {
                spriteFx = SpriteEffects.FlipHorizontally;
            }
            else { spriteFx = SpriteEffects.None; }

            if (frameTimer <= 0)
            {
                frameTimer = frameInterval;
                frame++;
                srcRec.X = (frame % 3) * 16;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures.playerSpriteSheet, Vector2.Zero, srcRec, Color.White, 0,
                new Vector2(), 1, spriteFx, 1);
        }
    }
}

