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
    class Effect
    {
        TextureManager textures;
        Texture2D texture;
        Stats stats;
        public Effects effect;
        public int timer, power;

        public Effect(TextureManager textures, Effects effect, int timer, Stats stats, int power)
        {
            this.textures = textures;
            this.effect = effect;
            this.timer = timer;
            this.stats = stats;
            this.power = power;
        }

        // the effect of the effect happens
        public void Update()
        {
            if (timer > 0)
            {
                if (effect == Effects.poison)
                {
                    stats.ChangeStat(Stat.health, -power);
                    timer--;
                }
                if (effect == Effects.bleed)
                {
                    timer--;
                    if (timer == 0)
                    {
                        stats.ChangeStat(Stat.health, -power);
                    }
                }
                if (effect == Effects.confusion)
                {
                    timer--;
                }
            }
        }
        
        // if there is a animation for the effect it will happen here
        public void Draw(SpriteBatch spriteBatch, UsedBy on)
        {
            Vector2 position;
            if (on == UsedBy.enemy)
            {
                position = new Vector2(800, 150);
            }
            else { position = new Vector2(325, 200); }

            switch (effect)
            {
                case Effects.poison:
                    texture = textures.poisonIcon;
                    break;
                case Effects.bleed:
                    texture = textures.bleedIcon;
                    break;
                case Effects.confusion:
                    texture = textures.confusionIcon;
                    break;
                case Effects.none:
                    texture = null;
                    break;
            }

            try
            {
                spriteBatch.Draw(texture, position, new Rectangle(0,0,16,16), Color.White, 0,
                new Vector2(), 8, SpriteEffects.None, 1);
            }
            catch   {  }
        }
    }
}
