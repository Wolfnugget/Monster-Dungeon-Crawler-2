using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D
{
    public enum TypeOfPotion
    {
        maxHealth, health, maxMana, mana, strength, intelligence, accuracy, luck, speed, xp, level
    }
    class Potion
    {
        private Random rand = new Random();
        private TypeOfPotion thisPotion;
        private Color color;
        private TextureManager textures;
        private Object.Player player;

        public Potion(TextureManager textures, Object.Player player)
        {
            this.textures = textures;
            this.player = player;

            int potionType = rand.Next(0, 120);
            if (potionType <= 15)
            {
                thisPotion = TypeOfPotion.health;
                color = Color.Red;
            }
            else if (potionType <= 30)
            {
                thisPotion = TypeOfPotion.mana;
                color = Color.Blue;
            }
            else if (potionType <= 40)
            {
                thisPotion = TypeOfPotion.maxHealth;
                color = Color.DarkRed;
            }
            else if (potionType <= 50)
            {
                thisPotion = TypeOfPotion.maxMana;
                color = Color.DarkBlue;
            }
            else if (potionType <= 60)
            {
                thisPotion = TypeOfPotion.strength;
                color = Color.Orange;
            }
            else if (potionType <= 70)
            {
                thisPotion = TypeOfPotion.intelligence;
                color = Color.Purple;
            }
            else if (potionType <= 80)
            {
                thisPotion = TypeOfPotion.accuracy;
                color = Color.Yellow;
            }
            else if (potionType <= 90)
            {
                thisPotion = TypeOfPotion.luck;
                color = Color.GreenYellow;
            }
            else if (potionType <= 100)
            {
                thisPotion = TypeOfPotion.speed;
                color = Color.Cyan;
            }
            else if (potionType <= 115)
            {
                thisPotion = TypeOfPotion.xp;
                color = Color.Green;
            }
            else if (potionType <= 120)
            {
                thisPotion = TypeOfPotion.level;
                color = Color.White;
            }
        }

        public void Use()
        {
            switch (thisPotion)
            {
                case TypeOfPotion.health:
                    player.stats.ChangeStat(Stat.health, 50 + 15 * player.stats.CheckStat(Stat.level));
                    break;
                case TypeOfPotion.mana:
                    player.stats.ChangeStat(Stat.mana, 30 + 15 * player.stats.CheckStat(Stat.level));
                    break;
                case TypeOfPotion.maxHealth:
                    player.stats.ChangeStat(Stat.maxHealth, 20);
                    player.stats.ChangeStat(Stat.health, 20);
                    break;
                case TypeOfPotion.maxMana:
                    player.stats.ChangeStat(Stat.maxMana, 15);
                    player.stats.ChangeStat(Stat.mana, 15);
                    break;
                case TypeOfPotion.strength:
                    player.stats.ChangeStat(Stat.strength, 1);
                    break;
                case TypeOfPotion.intelligence:
                    player.stats.ChangeStat(Stat.intelligence, 1);
                    break;
                case TypeOfPotion.accuracy:
                    player.stats.ChangeStat(Stat.accuracy, 1);
                    break;
                case TypeOfPotion.luck:
                    player.stats.ChangeStat(Stat.luck, 1);
                    break;
                case TypeOfPotion.speed:
                    player.stats.ChangeStat(Stat.speed, 1);
                    break;
                case TypeOfPotion.xp:
                    player.stats.ChangeStat(Stat.xp, 20);
                    break;
                case TypeOfPotion.level:
                    player.stats.ChangeStat(Stat.level, 1);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures.whiteSquare, Vector2.Zero, color);
        }
    }
}
