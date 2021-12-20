using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem12 : Problem
    {
        Dictionary<string, Cave> CavesByName = new Dictionary<string, Cave>();
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var start = GetCaveFromName("start");
            start.isSmallCave = false;
            var end = new EndCave();
            CavesByName.Add("end", end);
            input.ForEach(x =>
            {
                var caves = x.Split('-');
                var startCave = GetCaveFromName(caves[0]);
                var endCave = GetCaveFromName(caves[1]);
                startCave.AddNeighbour(endCave);
            });

            var problem1 = start.GetPathsFrom("", true);
         
            //var problem2 = CavesByName.Keys.Where(x => CavesByName[x].isSmallCave).Select(x => start.GetPathsFrom("", false)).Sum();
            resultWriter.WriteResult(problem1);
            var problem2 = start.GetPathsFrom("", false);
            resultWriter.WriteResult(problem2);
        }

        private Cave GetCaveFromName(string caveName)
        {
            CavesByName.TryGetValue(caveName, out var cave);
            if (cave == null)
            {
                cave = new Cave(caveName[0] >= 'a' && caveName[0] <= 'z', caveName);
                CavesByName.Add(caveName, cave);
            }

            return cave;
        }

        private class Cave
        {
            public bool isSmallCave;

            public string name;
            public Cave(bool isSmallCave, string name)
            {
                this.isSmallCave = isSmallCave;
                this.name = name;
            }

            public virtual int GetPathsFrom(string visitedCaves, bool hasDoubleVisited)
            {
                var visited = visitedCaves.Split(",");
                if (this.isSmallCave)
                {
                    visitedCaves += "," + this.name;
                }
              
                var sum = 0;
                if (!hasDoubleVisited)
                {
                    sum = Neighbours.Where(x => visited.Contains(x.name)).Sum(x => x.GetPathsFrom(visitedCaves, true));
                }
                return sum + Neighbours.Where(x => !visited.Contains(x.name)).Sum(x => x.GetPathsFrom(visitedCaves, hasDoubleVisited));
            }

            public List<Cave> Neighbours = new List<Cave>();

            public void AddNeighbour(Cave c)
            {
                this.Neighbours.Add(c);
                if (this.name != "start")
                {
                    c.Neighbours.Add(this);
                }
            }
        }

        private class EndCave : Cave
        {
            public EndCave() : base(false, "end")
            {

            }

            public override int GetPathsFrom(string visitedCaves, bool hasDoubleVisited)
            {
                return 1;
            }
        }
    }
}
