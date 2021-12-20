using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem8 : Problem
    {
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var Entries = input.Select(x => new Entry(x));
            var problem1 = Entries.Sum(x => x.NumberOfRelevantForProblem1());
            var problem2 = Entries.Sum(x => x.Decode());
            resultWriter.WriteResult(problem1);
            resultWriter.WriteResult(problem2);
        }

        private class Entry
        {
            string[] inputs;
            string[] outputs;
            string[] knownNumbers;

            public Entry(string input)
            {
                var tokens = input.Split('|');
                inputs = tokens[0].Split(' ').Select(OrderString).ToArray();
                outputs = tokens[1].Split(' ').Skip(1).Select(OrderString).ToArray();
                knownNumbers = new string[10];
                SolveEasyNumbers();
                SolveHard();
            }

            private string OrderString(string s)
            {
                return new string(s.OrderBy(x => x).ToArray());
            }

            public void SolveEasyNumbers()
            {
                foreach (var number in AllNumbers)
                {
                    if (number.Length == 2)
                    {
                        knownNumbers[1] = number;
                    }
                    if (number.Length == 3)
                    {
                        knownNumbers[7] = number;
                    }
                    if (number.Length == 4)
                    {
                        knownNumbers[4] = number;
                    }
                    if (number.Length == 7)
                    {
                        knownNumbers[8] = number;
                    }
                }
            }

            public void SolveHard()
            {
                foreach (var number in AllNumbers)
                {
                    if (number.Length == 6)
                    {
                        // Solve 6
                        if (RemoveAllFromNumber(number, knownNumbers[1]) != string.Empty)
                        {
                            knownNumbers[6] = number;
                            continue;
                        }

                        // Solve 9
                        if (RemoveAllFromNumber(number, knownNumbers[4]) == string.Empty)
                        {
                            knownNumbers[9] = number;
                            continue;
                        }

                        // Solve 0
                        knownNumbers[0] = number;
                        continue;
                    }

                    if (number.Length == 5)
                    {
                        // Solve 3
                        if (RemoveAllFromNumber(number, knownNumbers[7]) == string.Empty)
                        {
                            knownNumbers[3] = number;
                            continue;
                        }

                        //Solve 2
                        if (RemoveAllFromNumber(number, knownNumbers[4]).Length == 2)
                        {
                            knownNumbers[2] = number;
                            continue;
                        }

                        //Solve 5
                        knownNumbers[5] = number;
                    }
                }
            }

            public int NumberOfRelevantForProblem1()
            {
                return outputs.Count(IsOneFourSevenOrEight);
            }

            public int Decode()
            {
                return int.Parse(string.Join("", outputs.Select(PatternToNumber)));
            }

            private string PatternToNumber(string pattern) => Array.IndexOf(knownNumbers, pattern).ToString();

            private string RemoveAllFromNumber(string number, string removeFrom)
            {
                return new string(removeFrom.Where(x => !number.Contains(x)).ToArray());
            }

            private IEnumerable<string> AllNumbers => inputs.Union(outputs);

            private bool IsOneFourSevenOrEight(string v)
            {
                return v.Length == 2 || v.Length == 3 || v.Length == 4 || v.Length == 7;
            }
        }
    }
}
