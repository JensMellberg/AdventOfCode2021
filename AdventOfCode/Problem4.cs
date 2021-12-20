using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem4 : Problem
    {
        IList<Board> boards = new List<Board>();
        public void Solve(IEnumerable<string> input, ResultWriter writer)
        {
            var drawnNumbers = input.First().Split(',');
            this.ParseBoards(input.Skip(2));

            this.FirstPart(drawnNumbers, writer);
            this.SecondPart(drawnNumbers, writer);
        }

        private void SecondPart(IEnumerable<string> drawnNumbers, ResultWriter writer)
        {
            foreach (var nbr in drawnNumbers)
            {
                if (boards.Count == 1)
                {
                    if (boards.Single().MatchNumber(nbr))
                    {
                        writer.WriteResult(boards.Single().GetScore() * int.Parse(nbr));
                        return;
                    }
                }
                else
                {
                    boards = boards.Where(x => !x.MatchNumber(nbr)).ToList();
                }
            }
        }

        private void FirstPart(IEnumerable<string> drawnNumbers, ResultWriter writer)
        {
            foreach (var nbr in drawnNumbers)
            {
                var winner = boards.FirstOrDefault(x => x.MatchNumber(nbr));
                if (winner != null)
                {
                    writer.WriteResult(winner.GetScore() * int.Parse(nbr));
                    return;
                }
            }
        }

        private void ParseBoards(IEnumerable<string> boardInput)
        {
            Board currentBoard = new Board();
            foreach (var line in boardInput)
            {
                if (line == string.Empty)
                {
                    boards.Add(currentBoard);
                    currentBoard = new Board();
                }
                else
                {
                    currentBoard.AddRow(line);
                }
            }
            boards.Add(currentBoard);
        }
        private class Board
        {
            List<Row> rows = new List<Row>();
            public void AddRow(string row)
            {
                rows.Add(new Row(row));
            }

            public bool MatchNumber(string number)
            {
                if (rows.Any(x => x.MatchNumber(number)))
                {
                    return true;
                }

                for (int i = 0; i < rows[0].Length; i++)
                {
                    var index = 0;
                    while (index < rows.Count && rows[index][i] == "M")
                    {
                        index++;
                    }
                    if (index == rows.Count)
                    {
                        return true;
                    }
                }

                return false;
            }

            public int GetScore()
            {
               return rows.Sum(x => x.GetScore());
            }
        }

        private class Row
        {
            string[] values;

            public Row(string row)
            {
                values = row.Replace("  ", " ").Trim().Split(' ');
            }

            public string this[int key]
            {
                get => values[key];
            }

            public int Length => values.Length;

            public bool MatchNumber(string number)
            {
                values = values.Select(x => x == number ? "M" : x).ToArray();
                return values.Count(x => x == "M") == values.Length;
            }

            public int GetScore()
            {
                return values.Where(x => x != "M").Sum(int.Parse);
            }
        }
    }
}
