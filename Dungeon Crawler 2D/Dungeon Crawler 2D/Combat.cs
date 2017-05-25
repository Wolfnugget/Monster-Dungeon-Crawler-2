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
        protected int frame;
        protected Rectangle srcRec = new Rectangle(0, 0, 16, 16);
        TurnOrder currentTurn;
        bool confusedPlayer, confusedEnemy;
        Effects effectPlayer, effectEnemy;
        Random rand = new Random();
        int animationTimer, animationTimer2;

        public Combat(Object.Player player, TextureManager textures, HUDManager hud)
        {
            this.player = player;
            this.textures = textures;
            this.hud = hud;
            animationTimer = 0;
            animationTimer2 = 0;
        }

        public void StartCombat(EnemyType type)
        {
            hud.turnEvents = "Plan your move...";
            enemy = new Enemy(textures, type, player);
            currentTurn = TurnOrder.animation;
            effectPlayer = Effects.none;
            effectEnemy = Effects.none;
            confusedPlayer = false;
            confusedEnemy = false;
            player.abilities.usedAbility = UsedAbility.Miss;
            enemy.ability.usedAbility = UsedAbility.Miss;
        }

        public void Update(GameTime gameTime)
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
                        confusedPlayer = true;
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
                        confusedEnemy = true;
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
                        effectPlayer = enemy.ability.effect;
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
                        effectEnemy = player.abilities.effect;
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
                    effectPlayer = enemy.ability.effect;
                    hud.CombatText(3, enemy);
                }
                else if (enemy.ability.usedAbility == UsedAbility.Miss)
                {
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    AddEffect(player.abilities.effect, UsedBy.player);
                    effectEnemy = player.abilities.effect;
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
                    effectEnemy = player.abilities.effect;
                    player.stats.ChangeStat(Stat.health, -enemy.ability.power);
                    AddEffect(enemy.ability.effect, UsedBy.enemy);
                    effectPlayer = enemy.ability.effect;
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
                    effectPlayer = enemy.ability.effect;
                    enemy.stats.ChangeStat(Stat.health, -player.abilities.power);
                    AddEffect(player.abilities.effect, UsedBy.player);
                    effectEnemy = player.abilities.effect;
                    hud.CombatText(8, enemy);
                }
                #endregion

                player.stats.UpdateEffects();
                enemy.stats.UpdateEffects();

                currentTurn = TurnOrder.animation;
            }

            if (currentTurn == TurnOrder.animation)
            {
                if (animationTimer == 10)
                {
                    enemy.BattleAnimation();
                    PlayerAnimation();
                    if (confusedEnemy == true) { enemy.ability.usedAbility = UsedAbility.confusion; }
                    if (confusedPlayer == true) { player.abilities.usedAbility = UsedAbility.confusion; }
                    animationTimer2++;
                    if (animationTimer2 == 4)
                    {
                        currentTurn = TurnOrder.conclusion;
                        animationTimer2 = 0;
                    }
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

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            hud.DrawBattle(spriteBatch, this);
            enemy.Draw(spriteBatch);
            spriteBatch.Draw(textures.playerBattleAnimations, new Vector2(125, 250), srcRec, Color.White, 0,
                new Vector2(), 16, SpriteEffects.None, 1);
            if (currentTurn == TurnOrder.animation)
            {
                player.abilities.Draw(spriteBatch, UsedBy.player);
                player.stats.DrawEffect(spriteBatch, effectPlayer, UsedBy.player);
                enemy.ability.Draw(spriteBatch, UsedBy.enemy);
                enemy.stats.DrawEffect(spriteBatch, effectEnemy, UsedBy.enemy);
            }
        }

        public void NextTurn()
        {
            currentTurn = TurnOrder.player;
            effectEnemy = Effects.none;
            effectPlayer = Effects.none;
            confusedEnemy = false;
            confusedPlayer = false;
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

        public void PlayerAnimation()
        {
            frame++;
            srcRec.X = (frame % 4) * 16;
            player.abilities.BattleAnimation();
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
                    enemy.stats.AddEffect(rand.Next(Math.Min((player.stats.CheckStat(Stat.intelligence) / 5) + (player.stats.CheckStat(Stat.luck) / 10),
                        (player.stats.CheckStat(Stat.intelligence) / 2) - 1), (player.stats.CheckStat(Stat.intelligence) / 2)), 
                        player.abilities.effect, player.stats.CheckStat(Stat.intelligence));
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
                    player.stats.AddEffect(rand.Next(Math.Min((enemy.stats.CheckStat(Stat.intelligence) / 5) + (enemy.stats.CheckStat(Stat.luck) / 10),
                        (enemy.stats.CheckStat(Stat.intelligence)/ 2) - 1), (enemy.stats.CheckStat(Stat.intelligence) / 2)),
                        enemy.ability.effect, enemy.stats.CheckStat(Stat.intelligence));

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
