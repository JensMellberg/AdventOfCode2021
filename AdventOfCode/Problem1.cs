using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode
{
    public class Problem1 : Problem
    {
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var count = 0;
            var countSums = 0;
            var lastSum = int.MaxValue;
            var buffer = new Buffer();

            int lastValue = int.MaxValue;
            foreach (var line in input)
            {
                var value = int.Parse(line);
                if (value > lastValue)
                {
                    count++;
                }
                lastValue = value;
                buffer.PushValue(value);
                if (buffer.IsFull)
                {
                    var sum = buffer.GetSum();
                    if (sum > lastSum)
                    {
                        countSums++;
                    }

                    lastSum = sum;
                }
            }
          
            resultWriter.WriteResult(count);
            resultWriter.WriteResult(countSums);
        }

        private class Buffer
        {
            readonly int[] buffer = new int[3];

            int timesPushed = 0;

            public void PushValue(int value)
            {
                buffer[2] = buffer[1];
                buffer[1] = buffer[0];
                buffer[0] = value;
                timesPushed++;
            }

            public int GetSum()
            {
                return buffer[0] + buffer[1] + buffer[2];
            }

            public bool IsFull => timesPushed >= 3;
        }
    }
}
