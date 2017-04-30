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
    class Combat
    {
        Object.Player player;
        TextureManager textures;
        Enemy enemy;

        bool playerTurn, enemyTurn;

        public Combat(Object.Player player, TextureManager textures)
        {
            this.player = player;
            this.textures = textures;
            playerTurn = true;
            enemyTurn = true;

            enemy = new Enemy(textures, EnemyType.zombie, player);
        }

        public void Update()
        {
            if (playerTurn == true)
            {
                player.ChoseAbility(enemy);
            }
            else if (enemyTurn == true && playerTurn == false)
            {
                enemy.Update();
            }
            else
            {
                if (player.abilities.usedAbility == UsedAbility.Dodge || 
                    enemy.ability.usedAbility == UsedAbility.Dodge || 
                    player.abilities.usedAbility == UsedAbility.Miss && enemy.ability.usedAbility == UsedAbility.Miss
                    || player.abilities.usedAbility == UsedAbility.Defence && enemy.ability.usedAbility == UsedAbility.Defence)
                {

                } 
                else if (player.abilities.usedAbility == UsedAbility.Defence && enemy.ability.usedAbility != UsedAbility.Miss) 
                {
                    int damage = enemy.ability.power - player.abilities.power;
                    player.stats.ChangeStat(Stat.health, -damage);
                    player.stats.AddEffect(2, enemy.ability.effect, 1);
                }
                else if (enemy.ability.usedAbility == UsedAbility.Defence && player.abilities.usedAbility != UsedAbility.Miss)
                {
                    int damage = player.abilities.power - enemy.ability.power;
                    enemy.stats.ChangeStat(Stat.health, -damage);
                    enemy.stats.AddEffect(2, player.abilities.effect, 1);
                }
                else if (player.abilities.usedAbility == UsedAbility.Miss)
                {
                    player.stats.ChangeStat(Stat.health, -enemy.ability.power);
                    player.stats.AddEffect(2, enemy.ability.effect, 1);
                }
                else if (enemy.ability.usedAbility == UsedAbility.Miss)
                {
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    enemy.stats.AddEffect(2, player.abilities.effect, 1);
                }
                else if (player.stats.CheckStat(Stat.speed) >= enemy.stats.CheckStat(Stat.speed))
                {
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    if (enemy.stats.CheckStat(Stat.health) <= 0)
                    {
                        //ends encounter
                    }
                    enemy.stats.AddEffect(2, player.abilities.effect, 1);
                    player.stats.ChangeStat(Stat.health, -enemy.ability.power);
                    player.stats.AddEffect(2, enemy.ability.effect, 1);
                }
                else
                {
                    player.stats.ChangeStat(Stat.health, -enemy.ability.power);
                    if (player.stats.CheckStat(Stat.health) <= 0)
                    {
                        //ends encounter
                    }
                    player.stats.AddEffect(2, enemy.ability.effect, 1);
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    enemy.stats.AddEffect(2, player.abilities.effect, 1);
                }

                if (player.stats.CheckStat(Stat.health) <= 0)
                {
                    //gameover
                }
                if (enemy.stats.CheckStat(Stat.health) <= 0)
                {
                    //ends encounter
                }

                NextTurn();
            }
        }

        public void Draw()
        {

        }

        public void NextTurn()
        {
            playerTurn = true;
            enemyTurn = true;
        }
    }
}
