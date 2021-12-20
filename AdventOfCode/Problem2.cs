using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode
{
    class Problem2 : Problem
    {

        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var horizontalPos = 0;
            var depthScnd = 0;
            var verticalPos = 0;
            var aim = 0;
           foreach (var line in input)
            {
                if (line.StartsWith("forward"))
                {
                    horizontalPos += int.Parse(line.Substring("forward ".Length));
                    depthScnd += (int.Parse(line.Substring("forward ".Length)) * aim);
                }
                else if (line.StartsWith("down"))
                {
                    verticalPos += int.Parse(line.Substring("down ".Length));
                    aim += int.Parse(line.Substring("down ".Length));
                }
                else if (line.StartsWith("up"))
                {
                    verticalPos -= int.Parse(line.Substring("up ".Length));
                    aim -= int.Parse(line.Substring("up ".Length));
                }
            }

            resultWriter.WriteResult(horizontalPos * verticalPos);
            resultWriter.WriteResult(horizontalPos * depthScnd);
        }
    }
}
