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
    public enum TurnOrder
    {
        player, enemy, function, animation, conclusion
    }

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
        TurnOrder currentTurn;
        Random rand = new Random();
        int animationTimer;

        public Combat(Object.Player player, TextureManager textures, HUDManager hud)
        {
            this.player = player;
            this.textures = textures;
            this.hud = hud;
            frameTimer = 100;
            frameInterval = 100;
            animationTimer = 0;
        }

        public void StartCombat(EnemyType type)
        {
            hud.turnEvents = "Plan your move...";
            enemy = new Enemy(textures, type, player);
            currentTurn = TurnOrder.player;
        }

        public void Update()
        {
            if (currentTurn == TurnOrder.player)
            {
                currentTurn = player.ChoseAbility(enemy);
            }
            
            if (currentTurn == TurnOrder.enemy)
            {
                enemy.Update();
                currentTurn = TurnOrder.function;
            }
            if (currentTurn == TurnOrder.function)
            {

                #region Confusion
                if (player.stats.CheckEffects(Effects.confusion) == true)
                {
                    if (rand.Next(0, 100) <= 70 - player.stats.CheckStat(Stat.luck))
                    {
                        player.stats.ChangeStat(Stat.health, -player.abilities.power);
                        if (player.stats.CheckStat(Stat.health) <= 0)
                        {
                            hud.CombatText(11, enemy);
                            BattleResult();
                        }
                        player.abilities.usedAbility = UsedAbility.Miss;
                        hud.CombatText(9, enemy);
                    }
                }
                if (enemy.stats.CheckEffects(Effects.confusion) == true)
                {
                    if (rand.Next(0, 100) <= 70 - enemy.stats.CheckStat(Stat.luck))
                    {
                        enemy.stats.ChangeStat(Stat.health, -enemy.ability.power);
                        if (enemy.stats.CheckStat(Stat.health) <= 0)
                        {
                            hud.CombatText(12, enemy);
                            BattleResult();
                        }
                        enemy.ability.usedAbility = UsedAbility.Miss;
                        hud.CombatText(10, enemy);
                    }
                }
                #endregion

                #region Nothing Happens
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
                #endregion

                #region Someone Defends
                else if (player.abilities.usedAbility == UsedAbility.Defence && enemy.ability.usedAbility != UsedAbility.Miss)
                {
                    int damage = enemy.ability.power - player.abilities.power;
                    if (damage > 0)
                    {
                        player.stats.ChangeStat(Stat.health, -damage);
                        AddEffect(enemy.ability.effect, UsedBy.enemy);
                        hud.CombatText(1, enemy);
                    }
                    else{ hud.CombatText(13, enemy);  }
                }
                else if (enemy.ability.usedAbility == UsedAbility.Defence && player.abilities.usedAbility != UsedAbility.Miss)
                {
                    int damage = player.abilities.power - enemy.ability.power;
                    if (damage > 0)
                    {
                        enemy.stats.ChangeStat(Stat.health, -damage);
                        AddEffect(player.abilities.effect, UsedBy.player);
                        hud.CombatText(2, enemy);
                    }
                    else { hud.CombatText(14, enemy); }
                }
                #endregion

                #region Only One Attacks
                else if (player.abilities.usedAbility == UsedAbility.Miss)
                {
                    player.stats.ChangeStat(Stat.health, -enemy.ability.power);
                    AddEffect(enemy.ability.effect, UsedBy.enemy);
                    hud.CombatText(3, enemy);
                }
                else if (enemy.ability.usedAbility == UsedAbility.Miss)
                {
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    AddEffect(player.abilities.effect, UsedBy.player);
                    hud.CombatText(4, enemy);
                }
                #endregion

                #region Both Attack
                else if (player.stats.CheckStat(Stat.speed) >= enemy.stats.CheckStat(Stat.speed))
                {
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    if (enemy.stats.CheckStat(Stat.health) <= 0)
                    {
                        hud.CombatText(5, enemy);
                        BattleResult();
                    }
                    AddEffect(player.abilities.effect, UsedBy.player);
                    player.stats.ChangeStat(Stat.health, -enemy.ability.power);
                    AddEffect(enemy.ability.effect, UsedBy.enemy);
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
                    AddEffect(enemy.ability.effect, UsedBy.enemy);
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    AddEffect(player.abilities.effect, UsedBy.player);
                    hud.CombatText(8, enemy);
                }
                #endregion

                player.stats.UpdateEffects();
                enemy.stats.UpdateEffects();

                currentTurn = TurnOrder.animation;
            }

            if (currentTurn == TurnOrder.animation)
            {
                if (animationTimer == 100)
                {
                    currentTurn = TurnOrder.conclusion;
                    animationTimer = 0;
                }
                animationTimer++;
            }

            if (currentTurn == TurnOrder.conclusion)
            {
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
            currentTurn = TurnOrder.player;
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

        public void AddEffect(Effects effect, UsedBy user)
        {
            if (user == UsedBy.player)
            {
                if (effect == Effects.bleed)
                {
                    enemy.stats.AddEffect(player.stats.CheckStat(Stat.strength) / 
                        player.stats.CheckStat(Stat.accuracy) / 2, player.abilities.effect, 
                        player.stats.CheckStat(Stat.level) + player.stats.CheckStat(Stat.strength) / 2);

                }
                else if (effect == Effects.poison)
                {
                    enemy.stats.AddEffect(player.stats.CheckStat(Stat.luck) / 3, 
                        player.abilities.effect, player.stats.CheckStat(Stat.level));
                }
                else if (effect == Effects.confusion)
                {
                    enemy.stats.AddEffect(player.stats.CheckStat(Stat.intelligence) / 4, 
                        player.abilities.effect, player.stats.CheckStat(Stat.intelligence));
                }
            }
            else
            {
                if (effect == Effects.bleed)
                {
                    player.stats.AddEffect(enemy.stats.CheckStat(Stat.strength) / 
                        enemy.stats.CheckStat(Stat.accuracy) / 2, enemy.ability.effect,
                        enemy.stats.CheckStat(Stat.level) + enemy.stats.CheckStat(Stat.strength) / 2);

                }
                else if (effect == Effects.poison)
                {
                    player.stats.AddEffect(enemy.stats.CheckStat(Stat.luck) / 3, 
                        enemy.ability.effect, enemy.stats.CheckStat(Stat.level));

                }
                else if (effect == Effects.confusion)
                {
                    player.stats.AddEffect(enemy.stats.CheckStat(Stat.intelligence) / 4, 
                        enemy.ability.effect, enemy.stats.CheckStat(Stat.intelligence));
                }
            }
        }
    }
}
