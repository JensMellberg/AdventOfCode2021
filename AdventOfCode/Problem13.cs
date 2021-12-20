using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem13 : Problem
    {
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var points = input.TakeWhile(x => x != "").Select(x => Point.FromString(x, ","));
            var instructions = input.SkipWhile(x => x != "").Skip(1).Select(x =>
            {
                var tokens = x.Split(" ")[2].Split("=");
                return new FoldingInstruction(tokens[0], int.Parse(tokens[1]));
            });

            var maxX = points.Max(x => x.X);
            var maxY = points.Max(x => x.Y);
            var paper = new bool[maxX + 1, maxY + 1];
            points.ForEach(x => paper[x.X, x.Y] = true);

            paper = DoFold(paper, instructions.First());
            resultWriter.WriteResult(CountDots(paper));
            instructions.Skip(1).ForEach(x =>
            {
                paper = DoFold(paper, x);
            });
            PrintPaper(paper);
        }

        private bool[,] DoFold(bool[,] paper, FoldingInstruction instruction)
        {
            if (instruction.direction == "x")
            {
                paper = Transpose(paper);
            }

            int startY = instruction.pos;
            int xSize = paper.GetLength(0);
            int ySize = paper.GetLength(1);
           
            var newPaper = new bool[xSize, Math.Max(startY + 1, ySize / 2)];

            for (var x = 0; x < xSize; x++)
            {
                for (var currentY = startY - 1; currentY >= 0; currentY--)
                {
                    var mirroredPos = startY + (startY - currentY);
                    newPaper[x, currentY] = paper[x, currentY] || GetPaperValue(paper, x, mirroredPos);
                }
            }

            return instruction.direction == "x" ? Transpose(newPaper) : newPaper;
        }

        private bool GetPaperValue(bool[,] paper, int x, int y)
        {
            if (y >= paper.GetLength(1))
            {
                return false;
            }

            return paper[x, y];
        }

        private void PrintPaper(bool[,] paper)
        {
            Console.WriteLine();
            for (int x = 0; x < paper.GetLength(1); x++)
            {
                for (int y = 0; y < paper.GetLength(0); y++)
                {
                    Console.Write(paper[y, x] ? "#" : ".");
                }
                Console.WriteLine();
            }
        }

        public bool[,] Transpose(bool[,] matrix)
        {
            var w = matrix.GetLength(0);
            var h = matrix.GetLength(1);

            var result = new bool[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        private int CountDots(bool[,] paper)
        {
            var counter = 0;
            paper.ForEach(x => counter += x ? 1 : 0);
            return counter;
        }

        private class FoldingInstruction
        {
            public string direction;
            public int pos;
            public FoldingInstruction(string direction, int pos)
            {
                this.direction = direction;
                this.pos = pos;
            }
        }
    }
}
