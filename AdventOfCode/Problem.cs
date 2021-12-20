using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public interface Problem
    {
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter);
    }
}
