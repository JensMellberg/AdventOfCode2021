using System;
using System.Diagnostics;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var command = Console.ReadLine();
                if (command == string.Empty)
                {
                    SolveLastProblem();
                }
                else if (command.StartsWith("go "))
                {
                    var problemNumber = command.Substring(3);
                    if (!SolveProblem(problemNumber))
                    {
                        Console.WriteLine("Cannot find solution for problem " + problemNumber);
                    }
                }
                else
                {
                    Console.WriteLine("Unknown command");
                }
            }
        }

        static bool SolveProblem(string problemNumber)
        {
            var classString = "AdventOfCode.Problem" + problemNumber;
            var type = Type.GetType(classString);
            if (type == null)
            {
                return false;
            }
            Console.WriteLine("Running solution for problem " + problemNumber);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var problem = (Problem)Activator.CreateInstance(type);
            var input = ProblemUtil.GetPuzzleInput($"Problem{problemNumber}.txt");
            problem.Solve(input, new ResultWriter());
            stopWatch.Stop();
            Console.WriteLine($"total time elapsed: {stopWatch.Elapsed}");
            return true;
        }

        static void SolveLastProblem()
        {
            var currentGuess = 25;
            while (!SolveProblem(currentGuess.ToString()))
            {
                currentGuess--;
            }
        }
    }
}
