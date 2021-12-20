using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode
{
    public class Problem5 : Problem
    {

        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var fullDiagram = new int[1000, 1000];
            var fullDiagram2 = new int[1000, 1000];

            var counter = 0;
            var counter2 = 0;
            foreach (var inp in input)
            {
                var line = new Line(inp);

                foreach (var p in line.GetCrossedPoints(false))
                {
                    fullDiagram[p.x, p.y]++;
                    if (fullDiagram[p.x, p.y] == 2)
                    {
                        counter++;
                    }
                }

                foreach (var p in line.GetCrossedPoints(true))
                {
                    fullDiagram2[p.x, p.y]++;
                    if (fullDiagram2[p.x, p.y] == 2)
                    {
                        counter2++;
                    }
                }
            }

            resultWriter.WriteResult(counter);
            resultWriter.WriteResult(counter2);
        }

        private class Line
        {
            (int x, int y) start;
            (int x, int y) end;

            public Line(string line)
            {
                var tokens = line.Split(" ");
                var tokens2 = tokens[0].Split(",");
                start = (int.Parse(tokens2[0]), int.Parse(tokens2[1]));
                var tokens3 = tokens[2].Split(",");
                end = (int.Parse(tokens3[0]), int.Parse(tokens3[1]));
            }

            public IEnumerable<(int x, int y)> GetCrossedPoints(bool includeDiagonals)
            {
                if (IsDiagonal && !includeDiagonals)
                {
                    yield break;
                } 
                var x = start.x;
                var y = start.y;
                yield return (x, y);

                while (x != end.x || y != end.y)
                {
                    x = MoveTowards(x, end.x);
                    y = MoveTowards(y, end.y);
                    yield return (x, y);
                }
            }

            public bool IsDiagonal => !(start.x == end.x || start.y == end.y);

            private int MoveTowards(int value, int target)
            {
                if (value == target)
                {
                    return value;
                }

                return value < target ? value + 1 : value - 1;
            }
        }

  
    }
}
