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

        public static List<int> GetRandomListofIntFromList(Random random, List<int> list, int min, int max)
        {
            List<int> randomList = new List<int>();

            int number = random.Next(min, max);
            int index;

            while (randomList.Count < number)
            {
                index = random.Next(0, list.Count - 1);
                randomList.Add(list[index]);
            }

            return randomList;
        }

        public static List<int> ConvertByteListToIntList(List<byte> toConvert)
        {
            List<int> converted = new List<int>();
            for (int i = 0; i < toConvert.Count; i++)
            {
                converted.Add(toConvert[i]);
            }
            return converted;
        }

        public static List<byte> ConvertIntListToByteList(List<int> toConvert)
        {
            List<byte> converted = new List<byte>();
            for (int i = 0; i < toConvert.Count; i++)
            {
                converted.Add((byte)toConvert[i]);
            }
            return converted;
        }
    }
}
