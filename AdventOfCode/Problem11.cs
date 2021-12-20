using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem11 : Problem
    {
        List<Octupus> allOctupuses = new List<Octupus>();
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var inputList = input.ToList();
            var fullTable = new Octupus[inputList[0].Length, inputList.Count];
            for (var y = 0; y < inputList.Count; y++)
            {
                for (var x = 0; x < inputList[y].Length; x++)
                {
                    var current = new Octupus(inputList[y][x] - '0');
                    fullTable[x, y] = current;
                    allOctupuses.Add(current);
                    if (x > 0)
                    {
                        fullTable[x - 1, y].AddNeighbour(current);
                    }
                    if (y > 0)
                    {
                        fullTable[x, y - 1].AddNeighbour(current);
                        if (x + 1 < inputList[y].Length)
                        {
                            fullTable[x + 1, y - 1].AddNeighbour(current);
                        }
                    }
                    if (y > 0 && x > 0)
                    {
                        fullTable[x - 1, y - 1].AddNeighbour(current);
                    }
                    
                }
            }


            var problem1 = CountFlashes(100);
            allOctupuses.ForEach(x => x.Reset());
            resultWriter.WriteResult(problem1);
            resultWriter.WriteResult(FirstAllFlashing());
        }

        private int CountFlashes(int rounds)
        {
            for (var i = 0; i < rounds; i++)
            {
                this.DoRound();
            }

            return allOctupuses.Sum(x => x.totalFlashes);
        }

        private int FirstAllFlashing()
        {
            var counter = 0;
            while (true)
            {
                counter++;
                this.DoRound();
                if (allOctupuses.All(x => x.hasFlashedThisRound))
                {
                    return counter;
                }
            }
        }

        private void DoRound()
        {
            allOctupuses.ForEach(x => x.IncreaseValue());
            allOctupuses.ForEach(x => x.FlashIfFull());
        }

        private class Octupus
        {
            public int totalFlashes = 0;

            int flashValue;

            int startValue;

            public bool hasFlashedThisRound = false;

            public Octupus(int startValue)
            {
                this.flashValue = startValue;
                this.startValue = startValue;
            }

            public void IncreaseValue()
            {
                if (hasFlashedThisRound)
                {
                    flashValue = 0;
                }
                flashValue++;
                hasFlashedThisRound = false;
            }

            public void FlashIfFull()
            { 
                if (!hasFlashedThisRound && flashValue > 9)
                {
                    hasFlashedThisRound = true;
                    totalFlashes++;
                    Neighbours.ForEach(x => x.FriendlyFire());
                }
            }

            public void FriendlyFire()
            {
                flashValue++;
                this.FlashIfFull();
            }

            public void AddNeighbour(Octupus o)
            {
                this.Neighbours.Add(o);
                o.Neighbours.Add(this);
            }

            public void Reset()
            {
                hasFlashedThisRound = false;
                this.flashValue = startValue;
                totalFlashes = 0;
            }

            public List<Octupus> Neighbours = new List<Octupus>();
        }
    }
}
