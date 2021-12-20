using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem14 : Problem
    {
        string lastLetter;

        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var pairsByCount = GetInitialPairCount(input.First());

            var instructions = input.Skip(2).Select(x => new Instruction(x));
            for (var i = 0; i < 10; i++)
            {
                var newDict = DoRound(pairsByCount, instructions);
                pairsByCount = newDict;
            }

            var letterByCount = CountElements(pairsByCount);
            var orderedCounts = letterByCount.Values.OrderBy(x => x);
            var problem1 = orderedCounts.Last() - orderedCounts.First();

            for (var i = 0; i < 30; i++)
            {
                var newDict = DoRound(pairsByCount, instructions);
                pairsByCount = newDict;
            }

            letterByCount = CountElements(pairsByCount);
            orderedCounts = letterByCount.Values.OrderBy(x => x);
            var problem2 = orderedCounts.Last() - orderedCounts.First();

            resultWriter.WriteResult(problem2);
        }

        private Dictionary<string, long> DoRound(Dictionary<string, long> pairsCount, IEnumerable<Instruction> instructions)
        {
            var newDict = pairsCount.ToDictionary(kv => kv.Key, kv => kv.Value);
            instructions.ForEach(x => DoInstruction(pairsCount, newDict, x));
            return newDict;
        }

        private Dictionary<string, long> CountElements(Dictionary<string, long> pairsCount)
        {
            var letterByCount = new Dictionary<string, long>();
            foreach (var kv in pairsCount)
            {
                IncreaseValue(kv.Key[0], kv.Value);
            }

            letterByCount[lastLetter]++;

            void IncreaseValue(char character, long value)
            {
                var letter = character.ToString();
                if (!letterByCount.ContainsKey(letter))
                {
                    letterByCount.Add(letter, 0);
                }

                letterByCount[letter] += value;
            }

            return letterByCount;
        }

        private void DoInstruction(Dictionary<string, long> pairsCount, Dictionary<string, long> newPairsCount, Instruction instrunction)
        {
            var currentCount = pairsCount[instrunction.Pair];
            newPairsCount[instrunction.Pair] -= currentCount;
            this.IncreaseValue(newPairsCount, instrunction.Pair[0] + instrunction.Replacement, currentCount);
            this.IncreaseValue(newPairsCount, instrunction.Replacement + instrunction.Pair[1], currentCount);
        }

        private void IncreaseValue(Dictionary<string, long> newPairsCount, string pair, long value)
        {
            newPairsCount[pair] += value;
        }

        private Dictionary<string, long> GetInitialPairCount(string line)
        {
            lastLetter = line[line.Length - 1].ToString();
            var allLetters = line.Distinct().Concat(new[] { 'P' });
            var pairCount = new Dictionary<string, long>();
            foreach (var l in allLetters)
            {
                allLetters.Select(x => x.ToString() + l.ToString()).ForEach(x => pairCount.Add(x, 0));
            }

            for (var i = 0; i < line.Length - 1; i++)
            {
                pairCount[line.Substring(i, 2)]++;
            }

            return pairCount;
        }

        private class Instruction
        {
            public string Pair { get; set; }

            public string Replacement { get; set; }
            public Instruction(string line)
            {
                var tokens = line.Split(" ");
                Pair = tokens[0];
                Replacement = tokens[2];
            }
        }
    }
}
