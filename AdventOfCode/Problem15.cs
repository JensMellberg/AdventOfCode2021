using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem15 : Problem
    {
        int[,] costs;
        bool[,] hasVisited;
        int[,] shortestPath;

        int[,] costs2;
        bool[,] hasVisited2;
        int[,] shortestPath2;
        List<QueuedNode> queue = new List<QueuedNode>();
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var inputList = input.ToList();
            var width = inputList.First().Length;
            var height = inputList.Count;
            costs = new int[width, height];
            hasVisited = new bool[width, height];
            shortestPath = new int[width, height];
            shortestPath.ForEach((x, a, b, c) => x[b, c] = int.MaxValue);
            for (var i = 0; i < inputList[0].Length; i++)
            {
                for (var x = 0; x < inputList.Count; x++)
                {
                    costs[i, x] = inputList[x][i] - '0';
                }
            }

            var width2 = width * 5;
            var height2 = height * 5;

            costs2 = new int[width2, height2];
            hasVisited2 = new bool[width2, height2];
            shortestPath2 = new int[width2, height2];

            this.WriteResult(resultWriter);

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    if (x == y && x == 0)
                    {
                        this.PutAtTile(0, 0, costs);
                    }
                    else
                    {
                        var Tile = this.copyWithIncrease(costs, x + y);
                        this.PutAtTile(x, y, Tile);
                    }
                }
            }

            costs = costs2;
            hasVisited = hasVisited2;
            shortestPath2.ForEach((x, a, b, c) => x[b, c] = int.MaxValue);
            shortestPath = shortestPath2;

            this.WriteResult(resultWriter);
        }

        private void WriteResult(ResultWriter writer)
        {
            this.Run();

            var answer = shortestPath[shortestPath.GetLength(0) - 1, shortestPath.GetLength(1) - 1] - costs[0, 0];
            writer.WriteResult(answer);
        }

        private int[,] copyWithIncrease(int[,] original, int increase)
        {
            var newarray = new int[original.GetLength(0), original.GetLength(1)];
            for (var x = 0; x < original.GetLength(0); x++)
            {
                for (var y = 0; y < original.GetLength(1); y++)
                {
                    var newValue = original[x, y] + increase;
                    if (newValue > 9)
                    {
                        newValue -= 9;
                    }

                    newarray[x, y] = newValue;
                }
            }
            return newarray;
        }

        private void PutAtTile(int xTile, int yTile, int[,] tile)
        {
            var startY = yTile * tile.GetLength(1);
            var startX = xTile * tile.GetLength(0);

            for (var x = 0; x < tile.GetLength(0); x++)
            {
                for (var y = 0; y < tile.GetLength(1); y++)
                {
                    costs2[startX + x, startY + y] = tile[x, y];
                }
            }
        }

        private void Run()
        {
           queue.Add(new QueuedNode { x = 0, y = 0, cost = costs[0, 0] });

            while (queue.Any())
            {
                var next = queue.First();
                queue.RemoveAt(0);
                this.VisitNode(next);
            }
        }

        private void VisitNode(QueuedNode node)
        {
            shortestPath[node.x, node.y] = node.cost;
            hasVisited[node.x, node.y] = true;
            AddIfNotVisited(node.x - 1, node.y);
            AddIfNotVisited(node.x + 1, node.y);
            AddIfNotVisited(node.x, node.y - 1);
            AddIfNotVisited(node.x, node.y + 1);
            queue = queue.OrderBy(x => x.cost).ToList();


            void AddIfNotVisited(int x2, int y2)
            {
                if (x2 < 0 || x2 >= costs.GetLength(0) || y2 < 0 || y2 >= costs.GetLength(1))
                {
                    return;
                }

                if (!hasVisited[x2, y2])
                {
                    hasVisited[x2, y2] = true;
                    queue.Add(new QueuedNode { x = x2, y = y2, cost = node.cost + costs[x2, y2] });
                }
            }
        }

        private class QueuedNode
        {
            public int x { get; set; }

            public int y { get; set; }

            public int cost { get; set; }
        }
    }
}
