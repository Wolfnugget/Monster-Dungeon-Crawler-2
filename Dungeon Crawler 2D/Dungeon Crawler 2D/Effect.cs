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
        public void Update(EffectPoint currentTime)
        {
            if (timer < 0)
            {
                if (currentTime == EffectPoint.startPhase)
                {

                }
                else if (currentTime == EffectPoint.attack)
                {
                    if (effect == Effects.confusion)
                    {

                    }
                }
                else if (currentTime == EffectPoint.endPhase)
                {
                    if (effect == Effects.poison)
                    {
                        stats.ChangeStat(-power, Stat.health);
                    }
                    if (effect == Effects.bleed)
                    {
                        stats.ChangeStat(-power, Stat.health); //kommer vara olik från poison
                    }
                }
            }
        }
        
        // if there is a animation for the effect it will happen here
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
