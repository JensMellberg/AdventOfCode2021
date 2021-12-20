using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem3 : Problem
    {
        private (string gamma, string epsilon) CalculateGamma(IEnumerable<string> values)
        {
            var ones = new int[12];
            var total = 0;
            foreach (var v in values)
            {
                for (var i = 0; i < 12; i++)
                {
                    if (v[i] == '1')
                    {
                        ones[i]++;
                    }
                }
                total++;
            }

            var gamma = string.Empty;
            var epsilon = string.Empty;
            for (var i = 0; i < ones.Length; i++)
            {
                gamma += ones[i] >= total / 2.0 ? "1" : "0";
                epsilon += ones[i] < total / 2.0 ? "1" : "0";
            }

            return (gamma, epsilon);
        }
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var values = input.ToList();
            var (gamma, epsilon) = this.CalculateGamma(values);
            var result = Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
            resultWriter.WriteResult(result);

            var epsilonValues = values.Select(x => x).ToList();

            this.FilterList(values, x => this.CalculateGamma(x).gamma);
            this.FilterList(epsilonValues, x => this.CalculateGamma(x).epsilon);

            var result2 = Convert.ToInt32(values.Single(), 2) * Convert.ToInt32(epsilonValues.Single(), 2);
            resultWriter.WriteResult(result2);
        }

        private void FilterList(List<string> values, Func<IEnumerable<string>, string> pred)
        {
            for (var x = 0; x < 12; x++)
            {
                var filter = pred(values);
                for (var i = 0; i < values.Count; i++)
                {
                    var crnt = values[i];
                    if (crnt[x] != filter[x])
                    {
                        values.RemoveAt(i);
                        i--;
                        if (values.Count == 1)
                        {
                            break;
                        }
                    }
                }
                if (values.Count == 1)
                {
                    break;
                }
            }
        }
    }

  
}
