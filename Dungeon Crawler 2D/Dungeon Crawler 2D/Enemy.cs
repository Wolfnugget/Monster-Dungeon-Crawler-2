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
            #region Zombie
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
                    int speed = rand.Next(6, 15) + player.stats.CheckStat(Stat.level) / 2;
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
                    int speed = 7 + rand.Next(1, 6) + player.stats.CheckStat(Stat.level) / 2;
                    int xp = rand.Next(60, 100) + 2 * player.stats.CheckStat(Stat.level);
                    stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, xp, 0);
                }
                else
                {
                    int health = 150 + rand.Next(0, 50) + 2 * player.stats.CheckStat(Stat.level);  
                    int mana = 100 + rand.Next(20, 70) + player.stats.CheckStat(Stat.level);

                    int strenght = 20 + rand.Next(1, 7) + player.stats.CheckStat(Stat.level);
                    int inteligence = 0;
                    int dextarity = rand.Next(5, 10) + player.stats.CheckStat(Stat.level);
                    int luck = rand.Next(5, 20);
                    int speed = rand.Next(10, 20) + player.stats.CheckStat(Stat.level) / 2;
                    int xp = rand.Next(120, 200) + 2 * player.stats.CheckStat(Stat.level);
                    stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, xp, 0);
                }
            }
            #endregion

            #region Warlock

            if (theEnemy == EnemyType.warlock)
            {
                if (player.stats.CheckStat(Stat.level) <= 5)
                {
                    int health = rand.Next(20, 40) + 3 * player.stats.CheckStat(Stat.level);
                    int mana = rand.Next(30, 60) + 3 * player.stats.CheckStat(Stat.level);

                    int strenght = rand.Next(3, 9) + player.stats.CheckStat(Stat.level);
                    int inteligence = rand.Next(5,16) + player.stats.CheckStat(Stat.level);
                    int dextarity = rand.Next(5, 10) + player.stats.CheckStat(Stat.level);
                    int luck = 0;
                    int speed = rand.Next(6, 15) + player.stats.CheckStat(Stat.level) / 2;
                    int xp = rand.Next(30, 60) + 2 * player.stats.CheckStat(Stat.level);
                    stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, xp, 0);
                }
                else if (player.stats.CheckStat(Stat.level) <= 10)
                {
                    int health = rand.Next(40, 70) + 2 * player.stats.CheckStat(Stat.level);
                    int mana = rand.Next(60, 70) + 3 * player.stats.CheckStat(Stat.level);

                    int strenght = 8 + rand.Next(1, 3) + player.stats.CheckStat(Stat.level);
                    int inteligence = 10 + rand.Next(5, 10) + player.stats.CheckStat(Stat.level);
                    int dextarity = 5 + rand.Next(1, 15) + player.stats.CheckStat(Stat.level);
                    int luck = rand.Next(1, 6);
                    int speed = 7 + rand.Next(1, 6) + player.stats.CheckStat(Stat.level) / 2;
                    int xp = rand.Next(70, 120) + 2 * player.stats.CheckStat(Stat.level);
                    stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, xp, 0);
                }
                else
                {
                    int health = 100 + rand.Next(0, 30) + 2 * player.stats.CheckStat(Stat.level);
                    int mana = 100 + 4 * player.stats.CheckStat(Stat.level);

                    int strenght = 12 + rand.Next(1, 4) + player.stats.CheckStat(Stat.level);
                    int inteligence = 20 + rand.Next(1, 16) + player.stats.CheckStat(Stat.level);
                    int dextarity = rand.Next(5, 10) + player.stats.CheckStat(Stat.level);
                    int luck = rand.Next(3, 10);
                    int speed = rand.Next(10, 20) + player.stats.CheckStat(Stat.level) / 2;
                    int xp = rand.Next(150, 240) + 2 * player.stats.CheckStat(Stat.level);
                    stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, xp, 0);
                }
            }

            #endregion

            #region Boss
            if (theEnemy == EnemyType.boss)
            {
                int health = 250;
                int mana = 200;

                int strenght = 33;
                int inteligence = 36;
                int dextarity = 30;
                int luck = 18;
                int speed = 20;
                int xp = 300;
                stats = new Stats(textures, health, health, mana, mana, strenght, inteligence, dextarity, luck, speed, 0, xp, 0);
            }
            #endregion
        }
    }
}
