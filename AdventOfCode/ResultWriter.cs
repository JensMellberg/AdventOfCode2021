using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public class ResultWriter
    {
        int resultsCount = 0;

        public void WriteResult(string result)
        {
            if (resultsCount == 0)
            {
                Console.WriteLine("First result: " + result);
            }
            else if (resultsCount == 1)
            {
                Console.WriteLine("Second result: " + result);
            }
            else
            {
                Console.WriteLine("Additional result: " + result);
            }
            resultsCount++;
        }

        public void WriteResult(int result)
        {
            this.WriteResult(result.ToString());
        }

        public void WriteResult(long result)
        {
            this.WriteResult(result.ToString());
        }

        public void WriteFiller()
        {
            Console.WriteLine("-----------------------");
        }
    }
}
