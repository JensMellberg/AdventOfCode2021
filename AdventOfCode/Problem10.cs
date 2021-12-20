using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventOfCode
{
    public class Problem10 : Problem
    {
        char[] openingBrackets = { '(', '{', '[', '<' };
        char[] closingBrackets = { ')', '}', ']', '>' };
        Dictionary<char, int> IllegalPoints = new Dictionary<char, int>
        {
            {')', 3 },
            {']', 57},
            {'}', 1197},
            {'>', 25137 }
        };

        Dictionary<char, int> CompletetionPoints = new Dictionary<char, int>
        {
            {')', 1 },
            {']', 2},
            {'}', 3},
            {'>', 4 }
        };
        Stack<char> queue;
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            queue = new Stack<char>();
            var illegalScore = input.Sum(x => GetLineScore(x.Select(c => c), true));
            var allScores = input.Select(x => GetLineScore(x.Select(c => c), false)).Where(x => x > 0).OrderBy(x => x).ToList();
            resultWriter.WriteResult(illegalScore);
            resultWriter.WriteResult(allScores[allScores.Count / 2]);
        }

        private long GetLineScore(IEnumerable<char> brackets, bool illegalScore)
        {
           queue = new Stack<char>();
           int counter = 0;
           foreach (var bracket in brackets)
            {
                if (this.IsOpener(bracket))
                {
                    queue.Push(bracket);
                }
                else if (this.IsCloser(bracket))
                {
                    var previouslyOpened = queue.Pop();
                    if (IsCorrespondingCloser(previouslyOpened, bracket))
                    {
                        //All good
                    }
                    else
                    {
                        return (illegalScore ? 1 : -1) * IllegalPoints[bracket];
                    }
                }
                else
                {
                    throw new Exception($"Unknown bracket {bracket}");
                }
                counter++;
            }
            if (illegalScore)
            {
                return 0;
            }

            long score = 0;
            while (queue.Count > 0)
            {
                var closer = GetCorrespondingCloser(queue.Pop());
                score = this.AddScore(score, closer);
            }

            return score;

        }

        private long AddScore(long startScore, char bracket) => startScore * 5 + CompletetionPoints[bracket];

        private bool IsOpener(char c) => openingBrackets.Contains(c);

        private bool IsCloser(char c) => closingBrackets.Contains(c);

        private bool IsCorrespondingCloser(char opener, char closer) => Array.IndexOf(openingBrackets, opener) == Array.IndexOf(closingBrackets, closer);

        private char GetCorrespondingCloser(char opener) => closingBrackets[Array.IndexOf(openingBrackets, opener)];
    }
}
