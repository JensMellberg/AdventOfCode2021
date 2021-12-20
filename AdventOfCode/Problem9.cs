using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem9 : Problem
    {
        List<int> lowPoints = new List<int>();
        List<int> basinSizes = new List<int>();
        List<string> visitedPoints = new List<string>();
        ConsoleColor[,] colors;
        ConsoleColor lastColor;

        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            lastColor = (ConsoleColor)1;
            var inputList = input.ToList();
            var fullTable = new int[inputList[0].Length, inputList.Count];
            colors = new ConsoleColor[inputList[0].Length, inputList.Count];

            for (var y = 0; y < inputList.Count; y++)
            {
                for (var x = 0; x < inputList[y].Length; x++)
                {
                    fullTable[x, y] = inputList[y][x] - '0';
                    colors[x, y] = ConsoleColor.White;
                }
            }

            fullTable.ForEach(IsLowPoint);
            var problem1 = lowPoints.Sum(GetRiskLevel);
            basinSizes.Sort();
            basinSizes.Reverse();
            Console.WriteLine(basinSizes.Sum());
            var problem2 = basinSizes.Take(3).Aggregate((x, y) => x * y);
            for (var y = 0; y < inputList.Count; y++)
            {
                for (var x = 0; x < inputList[y].Length; x++)
                {
                    Console.ForegroundColor = colors[x, y];
                    Console.Write(fullTable[x, y].ToString());
                }
                Console.WriteLine();
            }

            resultWriter.WriteResult(problem1);
            resultWriter.WriteResult(problem2);
        }

        private int GetRiskLevel(int point) => 1 + point;

        private void IsLowPoint(int[,] fullTable, int point, int x, int y)
        {
            if (x > 0 && fullTable[x - 1, y] <= point)
            {
                return;
            }
            if (x < fullTable.GetLength(0) - 1 && fullTable[x + 1, y] <= point)
            {
                return;
            }
            if (y > 0 && fullTable[x, y - 1] <= point)
            {
                return;
            }
            if (y < fullTable.GetLength(1) - 1 && fullTable[x, y + 1] <= point)
            {
                return;
            }
            lowPoints.Add(point);
            var basinPoints = new List<int>();
            var color = lastColor + 1;

            lastColor = color;
            if (color == (ConsoleColor)14)
            {
                lastColor = (ConsoleColor)1;
            }
            AddToBasin(x, y, basinPoints, fullTable, color);
            basinSizes.Add(basinPoints.Count);
        }

        private void AddToBasin(int x, int y, List<int> basinPoints, int[,] fullTable, ConsoleColor basinColor)
        {
            var point = fullTable[x, y];
            var pointStr = x.ToString() + ":" + y.ToString();
            if (visitedPoints.Contains(pointStr))
            {
                return;
            }
            visitedPoints.Add(pointStr);
            basinPoints.Add(point);
            colors[x, y] = basinColor;
           
            AddToBasinLocal(x - 1, y);
            AddToBasinLocal(x + 1, y);
            AddToBasinLocal(x, y - 1);
            AddToBasinLocal(x, y + 1);

            void AddToBasinLocal(int newX, int newY)
            {
                if (newX < 0 || newX >= fullTable.GetLength(0) || newY < 0 || newY >= fullTable.GetLength(1))
                {
                    return;
                }
                var newPoint = fullTable[newX, newY];

                if (newPoint > point && newPoint != 9)
                {
                    AddToBasin(newX, newY, basinPoints, fullTable,basinColor);
                }
            }
        }

    }
}
