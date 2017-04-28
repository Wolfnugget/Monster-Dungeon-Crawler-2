using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon_Crawler_2D.World
{
    public static class GeneratorUtility
    {
        /// <summary>
        /// Genererar ett tal mellan min och max, utesluter talen i listan exclude.
        /// </summary>
        /// <param name="exclude"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomNumberExcluding(Random random, HashSet<int> exclude, int min, int max)
        {
            HashSet<int> range = new HashSet<int>();

            for (int number = min; number <= max; number++)
            {
                if (!exclude.Contains(number))
                {
                    range.Add(number);
                }
            }

            int index = random.Next(0, range.Count());

             return range.ElementAt(index);
        }
    }
}
