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
        zombie, warlock, boss
    }

    class Enemy
    {
        TextureManager textures;
        public Stats stats;
        Random rand = new Random();
        public Abilities ability;
        Object.Player player;
        public EnemyType theEnemy;
        protected int frame;
        protected Rectangle srcRec = new Rectangle(0, 0, 16, 16);
        Texture2D texture;

        public Enemy(TextureManager textures, EnemyType theEnemy, Object.Player player)
        {
            this.textures = textures;
            this.theEnemy = theEnemy;
            this.player = player;
            ability = new Abilities(UsedBy.enemy, textures);
            GiveStats();
        }

        public bool Update()
        {
            #region Zombie
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
            #endregion

            #region Warlock

            if (theEnemy == EnemyType.warlock)
            {
                if (stats.CheckStat(Stat.mana) >= ability.CheckCost(UsedAbility.Magic))
                {
                    switch (rand.Next(0, 3))
                    {
                        case 1:
                            ability.Ability(this, player, UsedAbility.Magic);
                            stats.ChangeStat(Stat.mana, -ability.CheckCost(UsedAbility.Magic));
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
                            ability.Ability(this, player, UsedAbility.Miss);
                            break;
                        default:
                            ability.Ability(this, player, UsedAbility.Defence);
                            break;
                    }
                }
            }

            #endregion

            #region Boss
            if (theEnemy == EnemyType.boss)
            {
                if (stats.CheckStat(Stat.mana) >= ability.CheckCost(UsedAbility.PoisonHit))
                {
                    switch (rand.Next(0, 6))
                    {
                        case 0:
                            ability.Ability(this, player, UsedAbility.Hit);
                            break;
                        case 1:
                            ability.Ability(this, player, UsedAbility.PoisonHit);
                            stats.ChangeStat(Stat.mana, -ability.CheckCost(UsedAbility.PoisonHit));
                            break;
                        case 2:
                            ability.Ability(this, player, UsedAbility.Magic);
                            stats.ChangeStat(Stat.mana, -ability.CheckCost(UsedAbility.Magic));
                            break;
                        default:
                            ability.Ability(this, player, UsedAbility.Defence);
                            break;
                    }
                }
                else if (stats.CheckStat(Stat.mana) >= ability.CheckCost(UsedAbility.Magic))
                {
                    switch (rand.Next(0, 4))
                    {
                        case 0:
                            ability.Ability(this, player, UsedAbility.Hit);
                            break;
                        case 1:
                            ability.Ability(this, player, UsedAbility.Magic);
                            stats.ChangeStat(Stat.mana, -ability.CheckCost(UsedAbility.Magic));
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
            #endregion

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (theEnemy)
            {
                case EnemyType.zombie:
                    texture = textures.zombie;
                    break;
                case EnemyType.warlock:
                    texture = textures.warlock;
                    break;
                case EnemyType.boss:
                    texture = textures.demon;
                    break;
            }
            spriteBatch.Draw(texture, new Vector2(800, 250), srcRec, Color.White, 0,
                new Vector2(), 16, SpriteEffects.None, 1);
        }

        public void BattleAnimation()
        {
            frame++;
            srcRec.X = (frame % 4) * 16;

            ability.BattleAnimation();
        }

        public void GiveStats()
        {
            int health = 0, mana = 0, strenght = 0, inteligence = 0, dextarity = 0, luck = 0, speed = 0, xp = 0;

            #region Zombie
            if (theEnemy == EnemyType.zombie)
            {
                health = 50 + 5 * rand.Next((int)(0.5f * player.stats.CheckStat(Stat.level)), player.stats.CheckStat(Stat.level));
                mana = 20 + 2 * player.stats.CheckStat(Stat.level);

                strenght = 8 + rand.Next((int)(0.5f * player.stats.CheckStat(Stat.level)), 2 * player.stats.CheckStat(Stat.level));
                inteligence = 0;
                dextarity = 2 + player.stats.CheckStat(Stat.level) / 2;
                luck = 0;
                speed = 8 + player.stats.CheckStat(Stat.level) / 2;
                xp = rand.Next(20, 40) + 5 * rand.Next((int)(0.5f * player.stats.CheckStat(Stat.level)), (int)(1.5f * player.stats.CheckStat(Stat.level)));

            }
            #endregion
            #region Warlock
            if (theEnemy == EnemyType.warlock)
            {
                health = 30 + 5 * rand.Next((int)(0.25f * player.stats.CheckStat(Stat.level)), (int)(0.5f * player.stats.CheckStat(Stat.level)));
                mana = 40 + 5 * player.stats.CheckStat(Stat.level);

                strenght = 10;
                inteligence = 10 + rand.Next(1 * player.stats.CheckStat(Stat.level), 2 * player.stats.CheckStat(Stat.level));
                dextarity = rand.Next(5, 12);
                luck = rand.Next(0, player.stats.CheckStat(Stat.level));
                speed = rand.Next(6 + player.stats.CheckStat(Stat.level), 15 + player.stats.CheckStat(Stat.level));
                xp = rand.Next(30, 60) + 5 * rand.Next(1 * player.stats.CheckStat(Stat.level), 2 * player.stats.CheckStat(Stat.level));

            }
            #endregion
            #region Boss
            if (theEnemy == EnemyType.boss)
            {
                health = 250 + 10 * player.stats.CheckStat(Stat.level);
                mana = 200;

                strenght = 10 + rand.Next(2 * player.stats.CheckStat(Stat.level), 3 * player.stats.CheckStat(Stat.level));
                inteligence = 10 + rand.Next(2 * player.stats.CheckStat(Stat.level), 3 * player.stats.CheckStat(Stat.level));
                dextarity = 10 + rand.Next(2 * player.stats.CheckStat(Stat.level), 3 * player.stats.CheckStat(Stat.level));
                luck = 10 + rand.Next(2 * player.stats.CheckStat(Stat.level), 3 * player.stats.CheckStat(Stat.level));
                speed = 10 + rand.Next(2 * player.stats.CheckStat(Stat.level), 3 * player.stats.CheckStat(Stat.level));
                xp = Math.Max(10 * player.stats.CheckStat(Stat.level) * player.stats.CheckStat(Stat.level), 250);
            }
            #endregion
            stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, xp, player.stats.CheckStat(Stat.level));
        }
    }
}
