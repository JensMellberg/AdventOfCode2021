using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public static class ProblemUtil
    {
        public static IEnumerable<string> GetPuzzleInput(string filePath)
        {
            if (!File.Exists(filePath))
            {
                yield break;
            }
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    var line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }
            }
        }

        public static void ForEach<T>(this T[,] array, Action<T[,], T, int, int> action)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var x = 0; x < array.GetLength(1); x++)
                {
                    action(array, array[i, x], i, x);
                }
            }
        }

        public static void ForEach<T>(this T[,] array, Action<T> action)
        {
            array.ForEach((_, x, y, f) => action(x));
        }

        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            foreach (var t in array)
            {
                action(t);
            }
        }

        public static void Print<T>(this T[,] array, string separator)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var x = 0; x < array.GetLength(1); x++)
                {
                    Console.Write(array[x, i].ToString() + separator);
                }
                Console.WriteLine();
            }
            Console.WriteLine("------------------");
        }
    }
}
