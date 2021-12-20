using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem17 : Problem
    {
        const int MinX = 88;
        const int MaxX = 125;
        const int MinY = -157;
        const int MaxY = -103;
        List<Point> SuccessfullVelocities = new List<Point>();
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var problem1 = Problem1();
            resultWriter.WriteResult(problem1);
            resultWriter.WriteResult(SuccessfullVelocities.Count);
        }

        private int Problem1()
        {
            var biggestVelocity = 0;
            for (var y = MinY; y < 200; y++)
            {
                for (var x = 1; x <= MaxX; x++)
                {
                    var point = new Point(0, 0);
                    var velocity = new Velocity(x, y);
                    var currentHighestY = 0;
                    while (point.X <= MaxX && point.Y >= MinY)
                    {
                        point = velocity.ApplyToPosition(point);
                        currentHighestY = Math.Max(currentHighestY, point.Y);
                        velocity.Update();
                        if (point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY)
                        {
                            biggestVelocity = Math.Max(biggestVelocity, currentHighestY);
                            SuccessfullVelocities.Add(new Point(x, y));
                            break;
                        }
                    }
                }
            }

            return biggestVelocity;
        }

        private class Velocity
        {
            public int X { get; set; }

            public int Y { get; set; }

            public Velocity(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public Point ApplyToPosition(Point p) => new Point(p.X + this.X, p.Y + this.Y);

            public void Update()
            {
                if (this.X != 0)
                {
                    this.X += this.X > 0 ? -1 : 1;
                }
                this.Y -= 1;
            }
        }
    }
}
