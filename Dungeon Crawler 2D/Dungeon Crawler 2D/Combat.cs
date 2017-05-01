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
        public Object.Player player;
        public TextureManager textures;
        public Enemy enemy;
        private HUDManager hud;

        bool playerTurn, enemyTurn;

        public Combat(Object.Player player, TextureManager textures, HUDManager hud)
        {
            this.player = player;
            this.textures = textures;
            this.hud = hud;
            playerTurn = true;
            enemyTurn = true;

            enemy = new Enemy(textures, EnemyType.zombie, player);
        }

        public void Update()
        {
            if (playerTurn == true)
            {
                playerTurn = player.ChoseAbility(enemy);
            }
            else if (enemyTurn == true && playerTurn == false)
            {
                enemy.Update();
                enemyTurn = false;
            }
            else
            {
                if (player.abilities.usedAbility == UsedAbility.Dodge || enemy.ability.usedAbility == UsedAbility.Dodge)
                {
                    hud.CombatText(0, enemy);
                }
                else if (player.abilities.usedAbility == UsedAbility.Miss && enemy.ability.usedAbility == UsedAbility.Miss)
                {
                    hud.CombatText(0, enemy);
                }
                else if (player.abilities.usedAbility == UsedAbility.Defence && enemy.ability.usedAbility == UsedAbility.Defence)
                {
                    hud.CombatText(0, enemy);
                }
                else if (player.abilities.usedAbility == UsedAbility.Defence && enemy.ability.usedAbility == UsedAbility.Miss)
                {
                    hud.CombatText(0, enemy);
                }
                else if (enemy.ability.usedAbility == UsedAbility.Defence && player.abilities.usedAbility == UsedAbility.Miss)
                {
                    hud.CombatText(0, enemy);
                }
                else if (player.abilities.usedAbility == UsedAbility.Defence && enemy.ability.usedAbility != UsedAbility.Miss)
                {
                    int damage = enemy.ability.power - player.abilities.power;
                    if (damage > 0)
                    {
                        player.stats.ChangeStat(Stat.health, -damage);
                        player.stats.AddEffect(2, enemy.ability.effect, 1);
                        hud.CombatText(1, enemy);
                    }
                    else{ hud.CombatText(10000, enemy);  }
                }
                else if (enemy.ability.usedAbility == UsedAbility.Defence && player.abilities.usedAbility != UsedAbility.Miss)
                {
                    int damage = player.abilities.power - enemy.ability.power;
                    if (damage > 0)
                    {
                        enemy.stats.ChangeStat(Stat.health, -damage);
                        enemy.stats.AddEffect(2, player.abilities.effect, 1);
                        hud.CombatText(2, enemy);
                    }
                    else { hud.CombatText(10001, enemy); }

                }
                else if (player.abilities.usedAbility == UsedAbility.Miss)
                {
                    player.stats.ChangeStat(Stat.health, -enemy.ability.power);
                    player.stats.AddEffect(2, enemy.ability.effect, 1);
                    hud.CombatText(3, enemy);
                }
                else if (enemy.ability.usedAbility == UsedAbility.Miss)
                {
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    enemy.stats.AddEffect(2, player.abilities.effect, 1);
                    hud.CombatText(4, enemy);
                }
                else if (player.stats.CheckStat(Stat.speed) >= enemy.stats.CheckStat(Stat.speed))
                {
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    if (enemy.stats.CheckStat(Stat.health) <= 0)
                    {
                        hud.CombatText(5, enemy);
                        //ends encounter
                    }
                    enemy.stats.AddEffect(2, player.abilities.effect, 1);
                    player.stats.ChangeStat(Stat.health, -enemy.ability.power);
                    player.stats.AddEffect(2, enemy.ability.effect, 1);
                    hud.CombatText(6, enemy);
                }
                else
                {
                    player.stats.ChangeStat(Stat.health, -enemy.ability.power);
                    if (player.stats.CheckStat(Stat.health) <= 0)
                    {
                        hud.CombatText(7, enemy);
                        //game over
                    }
                    player.stats.AddEffect(2, enemy.ability.effect, 1);
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    enemy.stats.AddEffect(2, player.abilities.effect, 1);
                    hud.CombatText(8, enemy);
                }

                player.stats.UpdateEffects();
                enemy.stats.UpdateEffects();

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

        public void Draw(SpriteBatch spriteBatch)
        {
            hud.DrawBattle(spriteBatch, this);
            enemy.Draw(spriteBatch);
            player.CombatDraw(spriteBatch);
        }

        public void NextTurn()
        {
            playerTurn = true;
            enemyTurn = true;
        }

        public void BattleWon()
        {
            if (enemy.stats.CheckStat(Stat.health) <= 0)
            {
                // whatever you get from the encounter
            }
        }
    }
}
