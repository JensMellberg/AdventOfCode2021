using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem7 : Problem
    {
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var positions = input.First().Split(",").Select(int.Parse).ToList();
            resultWriter.WriteResult(this.BruteForce(positions));
            resultWriter.WriteResult(this.BruteForceScnd(positions));

        }

        private int BruteForce(IList<int> input)
        {
            var highest = input.Max();
            var lowestCost = int.MaxValue;
            for (var i = 0; i < highest; i++)
            {
                var cost = input.Sum(x => Math.Abs(i - x));
                lowestCost = Math.Min(lowestCost, cost);
            }
            return lowestCost;
        }

        private int BruteForceScnd(IList<int> input)
        {
            var highest = input.Max();
            var lowestCost = int.MaxValue;
            for (var i = 0; i < highest; i++)
            {
                var cost = input.Sum(x => SumFrom0ToX(Math.Abs(i - x)));
                lowestCost = Math.Min(lowestCost, cost);
            }
            return lowestCost;
        }

        private int SumFrom0ToX(int x)
        {
            return (x * (x + 1)) / 2;
        }
    }
}
