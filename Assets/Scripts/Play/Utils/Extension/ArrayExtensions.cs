using System;

namespace Game
{
    public static class ArrayExtensions
    {
        public static T[] Fill<T>(this T[] array, T value)
        {
            for (var i = 0; i < array.Length; i++) array[i] = value;
            return array;
        }

        public static T[,] Fill<T>(this T[,] array, T value)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            for (var j = 0; j < array.GetLength(1); j++)
                array[i, j] = value;
            return array;
        }

        public static T Random<T>(this T[] array, Random random)
        {
            return array[random.Next(0, array.Length)];
        }
    }
}