﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.Object
{
    class Player : Actor
    {
        public Stats stats;
        private TextureManager textures;
        private KeyboardState currentState, previousState;
        public Abilities abilities;
        public List<UsedAbility> playerAbilities;

        public Player(Texture2D texture, TextureManager textures, Vector2 position, float speed, Point frameSize, Point frames, float frameTime = 0.3f)
            : base(texture, position, speed, frameSize, frames, frameTime)
        {
            this.textures = textures;
            stats = new Stats(textures, 100, 100, 100, 100, 10, 10, 10, 10, 10, 50, 0, 1);
            abilities = new Abilities(UsedBy.player, textures);
            playerAbilities = new List<UsedAbility>(4);
            GetAbilities();
        }

        public override void Update(GameTime gameTime)
        {
            if (!moving)
            {
                CheckInput();
            }
            base.Update(gameTime);
        }

        void CheckInput()
        {
            Point direction = new Point(0, 0);
            previousState = currentState;
            currentState = Keyboard.GetState();

            if (currentState.IsKeyDown(Keys.W))
            {
                direction.Y = -1;
                startingFrame = new Point(0, 3);
            }
            else if (currentState.IsKeyDown(Keys.S))
            {
                direction.Y = 1;
                startingFrame = new Point(0, 0);
            }
            if (currentState.IsKeyDown(Keys.D)) //Detta är en "if" så man ska kunna gå diagonalt. Map klassen har stöd för det.
            {
                direction.X = 1;
                startingFrame = new Point(0, 2);
            }
            else if (currentState.IsKeyDown(Keys.A))
            {
                direction.X = -1;
                startingFrame = new Point(0, 1);
            }
            
            //Här ska frames bytas senare när vi har en sprite

            if (direction != new Point(0, 0))
            {
                ActorEventArgs args = new ActorEventArgs(PlayerEventType.CheckDirection);
                args.Direction = direction;
                args.Position = position;
                OnAction(args);
            }
        }

        public TurnOrder ChoseAbility(Enemy enemy)
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            if (currentState.IsKeyDown(Keys.Q) && previousState.IsKeyUp(Keys.Q))
            {
                abilities.Ability(enemy, this, playerAbilities[0]);
                return TurnOrder.enemy;
            }
            else if (currentState.IsKeyDown(Keys.W) && previousState.IsKeyUp(Keys.W))
            {
                if (abilities.CheckCost(UsedAbility.Magic) <= stats.CheckStat(Stat.mana))
                {
                    abilities.Ability(enemy, this, playerAbilities[1]);
                    stats.ChangeStat(Stat.mana, -abilities.CheckCost(UsedAbility.Magic));
                    return TurnOrder.enemy;
                }
                return TurnOrder.player;
            }
            else if (currentState.IsKeyDown(Keys.E) && previousState.IsKeyUp(Keys.E))
            {
                abilities.Ability(enemy, this, playerAbilities[2]);
                return TurnOrder.enemy;
            }
            else if (currentState.IsKeyDown(Keys.R) && previousState.IsKeyUp(Keys.R))
            {
                if (abilities.CheckCost(UsedAbility.PoisonHit) <= stats.CheckStat(Stat.mana))
                {
                    abilities.Ability(enemy, this, playerAbilities[3]);
                    stats.ChangeStat(Stat.mana, -abilities.CheckCost(UsedAbility.PoisonHit));
                    return TurnOrder.enemy;
                }
                return TurnOrder.player;
            }
            else return TurnOrder.player;
        }

        public void GetAbilities()
        {
            playerAbilities.Add(UsedAbility.Hit);
            playerAbilities.Add(UsedAbility.Magic);
            playerAbilities.Add(UsedAbility.Dodge);
            playerAbilities.Add(UsedAbility.PoisonHit);
        }

        public void UsePotion(TypeOfPotion potionType)
        {
            switch (potionType)
            {
                case TypeOfPotion.health:
                    stats.ChangeStat(Stat.health, 50 + 15 * stats.CheckStat(Stat.level));
                    break;
                case TypeOfPotion.mana:
                    stats.ChangeStat(Stat.mana, 30 + 15 * stats.CheckStat(Stat.level));
                    break;
                case TypeOfPotion.maxHealth:
                    stats.ChangeStat(Stat.maxHealth, 20);
                    stats.ChangeStat(Stat.health, 20);
                    break;
                case TypeOfPotion.maxMana:
                    stats.ChangeStat(Stat.maxMana, 15);
                    stats.ChangeStat(Stat.mana, 15);
                    break;
                case TypeOfPotion.strength:
                    stats.ChangeStat(Stat.strength, 1);
                    break;
                case TypeOfPotion.intelligence:
                    stats.ChangeStat(Stat.intelligence, 1);
                    break;
                case TypeOfPotion.accuracy:
                    stats.ChangeStat(Stat.accuracy, 1);
                    break;
                case TypeOfPotion.luck:
                    stats.ChangeStat(Stat.luck, 1);
                    break;
                case TypeOfPotion.speed:
                    stats.ChangeStat(Stat.speed, 1);
                    break;
                case TypeOfPotion.xp:
                    stats.ChangeStat(Stat.xp, 20);
                    break;
                case TypeOfPotion.level:
                    stats.ChangeStat(Stat.level, 1);
                    break;
            }
        }
    }
}
