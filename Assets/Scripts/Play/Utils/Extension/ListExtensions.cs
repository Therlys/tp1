using System;
using System.Collections.Generic;

namespace Game
{
    public static class ListExtensions
    {
        public static T Random<T>(this List<T> list, Random random)
        {
            return list[random.Next(0, list.Count)];
        }

        public static T RemoveRandom<T>(this List<T> list, Random random)
        {
            var randomIndex = random.Next(0, list.Count);
            var randomValue = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomValue;
        }
    }
}