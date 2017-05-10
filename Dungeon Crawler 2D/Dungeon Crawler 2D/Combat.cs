﻿using System;
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
        protected int startingFrame, frame, frames, frameSize;
        SpriteEffects spriteFx;
        double frameTimer, frameInterval;
        protected Rectangle srcRec = new Rectangle(0, 0, 16, 16);

        bool playerTurn, enemyTurn;

        public Combat(Object.Player player, TextureManager textures, HUDManager hud)
        {
            this.player = player;
            this.textures = textures;
            this.hud = hud;
            frameTimer = 100;
            frameInterval = 100;
            
        }

        public void StartCombat(EnemyType type)
        {
            hud.turnEvents = "Plan your move...";
            enemy = new Enemy(textures, type, player);
            playerTurn = true;
            enemyTurn = true;
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
                        BattleResult();
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
                        BattleResult();
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
                    BattleResult();
                }
                if (enemy.stats.CheckStat(Stat.health) <= 0)
                {
                    BattleResult();
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

        public void BattleResult()
        {
            BattleEvensArgs args = new BattleEvensArgs(); ;

            if (enemy.stats.CheckStat(Stat.health) <= 0)
            {
                player.stats.ChangeStat(Stat.xp, enemy.stats.CheckStat(Stat.xp));
                args.result = EndCombat.Won;
                hud.HandleCombatSummary(true, enemy.stats.CheckStat(Stat.xp));
                args.enemyType = enemy.theEnemy;
            }
            else if (player.stats.CheckStat(Stat.health) <= 0)
            {
                args.result = EndCombat.Lost;
                hud.HandleCombatSummary(false, 0);
            }
            OnCombatEnd(args);
        }

        public event CombatEventHandler Event;

        public void OnCombatEnd(BattleEvensArgs e)
        {
            Event.Invoke(this, e);
        }

        public void BattleAnimation(SpriteBatch spriteBatch, GameTime gameTime, UsedBy by, UsedAbility ability)
        {
            frameTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            
            if (by == UsedBy.enemy)
            {
                spriteFx = SpriteEffects.FlipHorizontally;
            }
            else { spriteFx = SpriteEffects.None; }

            if (frameTimer<= 0)
            {
                frameTimer = frameInterval;
                frame++;
                srcRec.X = (frame % 3) * 16;
            }


            spriteBatch.Draw(textures.playerSpriteSheet, Vector2.Zero, srcRec, Color.White, 0, 
                new Vector2(), 1, spriteFx, 1);
        }
    }
}
