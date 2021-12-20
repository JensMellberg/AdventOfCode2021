using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem6 : Problem
    {
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var currentFishes = input.First().Split(",").Select(int.Parse).ToList();

            var firstProblem = NonBruteForce(currentFishes, 80);
            var scndProblem = NonBruteForce(currentFishes, 256);
            resultWriter.WriteResult(firstProblem);
            resultWriter.WriteResult(scndProblem);
        }

        private long NonBruteForce(IList<int> startingFishies, int days)
        {
            var currentFishies = startingFishies.Select(x => x);
            var values = Enumerable.Repeat((long)int.MinValue, days + 8).ToArray();
            var sum = currentFishies.Sum(x => resultWhenBornOnX(days + (8 - x), values));
            return sum;
        }

        private long resultWhenBornOnX(int day, long[] values)
        {
            if (day <= 7)
            {
                return 1;
            }
            if (values[day] != int.MinValue)
            {
                return values[day];
            }

            long returner = 1;
            for (var i = day - 9; i >= 0; i -= 7)
            {
                returner += resultWhenBornOnX(i, values);
            }

            values[day] = returner;
            return returner;
        }
    }
}
