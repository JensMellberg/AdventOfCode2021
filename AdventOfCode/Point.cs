using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public class Point
    {
        public int X { get; set; }

        public int Y { get; set; }
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point FromString(string s, string separator)
        {
            var tokens = s.Split(separator);
            return new Point(int.Parse(tokens[0]), int.Parse(tokens[1]));
        }
    }
}
