using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AdventOfCode
{
    public class Problem202025 : Problem
    {

        const int startValue = 1;
        const int subjectNumber = 7;
        const int devicePK = 11349501;
        const int doorPK = 5107328;

        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var doorLoopSize = this.GetLoopSize(doorPK);
            var key = this.LoopValue(doorLoopSize, devicePK);
            resultWriter.WriteResult((int)key);
        }

        private int GetLoopSize(int finalValue)
        {
            var counter = 0;
            long currentValue = startValue;
            while (currentValue != finalValue)
            {
                this.TransformValue(ref currentValue, subjectNumber);
                counter++;
            }
            return counter;
        }

        private long LoopValue(long loopSize, long subjNbr) 
        {
            long currentValue = startValue;
            for (var i = 0; i < loopSize; i++)
            {
                TransformValue(ref currentValue, subjNbr);
            }
            return currentValue;
        }

        private void TransformValue(ref long value, long subjNbr)
        {
            value *= subjNbr;
            value = value % 20201227;
        }
    }
}
