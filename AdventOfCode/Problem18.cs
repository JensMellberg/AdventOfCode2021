using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem18 : Problem
    {

        public static bool HasFoundLeft = false;
        public static bool HasFoundRight = false;
        public static int LeftValue;
        public static int RightValue;
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            var AllPairs = input.Select(x => ParsePair(x));

            var result = AllPairs.First();
            resultWriter.WriteFiller();
            foreach (var pair in AllPairs.Skip(1))
            {
                result = result.Add(pair);
                ReducePair(result);
            }

            resultWriter.WriteResult(result.Magnitude);

            var highestMagn = 0;
            var pairsList = AllPairs.ToList();
            for (var i = 0; i < pairsList.Count; i++)
            {
                for (var x = 0; x < pairsList.Count;x++)
                {
                    if (i != x)
                    {
                        var reuslt = pairsList[i].Add(pairsList[x]);
                        ReducePair(reuslt);
                        highestMagn = Math.Max(highestMagn, reuslt.Magnitude);
                    }
                }
            }

            resultWriter.WriteResult(highestMagn);
        }

        private void ReducePair(Pair pair)
        {
            int step = 0;
            while (step < 2)
            {
                if (step == 1)
                {
                    if (!pair.Split())
                    {
                        step++;
                    }
                    else
                    {
                        step = 0;
                    }
                }
                if (step == 0)
                {
                    HasFoundLeft = false;
                    HasFoundRight = false;
                    if (!pair.Explode())
                    {
                        step++;
                    }
                }
            }
        }

        private Pair ParsePair(string line)
        {
            var pointer = 0;
            return ParseElement(line, ref pointer) as Pair;
        }

        private Element ParseElement(string line, ref int pointer)
        {
            if (line[pointer] == '[')
            {
                pointer++;
                var Left = ParseElement(line, ref pointer);
                var Right = ParseElement(line, ref pointer);
                pointer++;
                return new Pair { Left = Left, Right = Right };
            }

            if (line[pointer] >= '0' && line[pointer] <= '9')
            {
                var value = line[pointer] - '0';
                pointer += 2;
                return new Number { Value = value };
            }

            throw new Exception($"Unexpected character '{line[pointer]}' at position {pointer}.");
        }

        private class Pair : Element
        {
            public Element Left { get; set; }

            public Element Right { get; set; }

            public Pair Add(Pair other) => new Pair { Left = this.Copy(), Right = other.Copy() };

            public override bool IsPair => true;

            public override string Print => "[" + Left.Print + "," + Right.Print + "]";

            public override void AddToRightMostValue(int value)
            {
                this.Right.AddToRightMostValue(value);
            }

            public override void AddToLeftMostValue(int value)
            {
                this.Left.AddToLeftMostValue(value);
            }

            public bool Split()
            {
                if (!this.Left.IsPair)
                {
                    var leftNbr = Left as Number;
                    if (leftNbr.Value > 9)
                    {
                        Left = leftNbr.GetSplittedPair();
                        return true;
                    }
                }
                else
                {
                    var leftPair = Left as Pair;
                    if (leftPair.Split())
                    {
                        return true;
                    }
                }
                if (!this.Right.IsPair)
                {
                    var rightNbr = Right as Number;
                    if (rightNbr.Value > 9)
                    {
                        Right = rightNbr.GetSplittedPair();
                        return true;
                    }
                }
                else
                {
                    var rightPair = Right as Pair;
                    if (rightPair.Split())
                    {
                        return true;
                    }
                }

                return false;
            }

            public bool Explode() => this.ExplodeInternal(1);


            public bool ExplodeInternal(int depth)
            {
                if (depth == 4)
                {
                    if (Left.IsPair)
                    {
                        LeftValue = ((Left as Pair).Left as Number).Value;
                        RightValue = ((Left as Pair).Right as Number).Value;
                        HasFoundLeft = false;
                        HasFoundRight = true;
                        Right.AddToLeftMostValue(RightValue);
                        Left = new Number { Value = 0 };
                        return true;
                    }
                     else if (Right.IsPair)
                    {
                        LeftValue = ((Right as Pair).Left as Number).Value;
                        RightValue = ((Right as Pair).Right as Number).Value;
                        HasFoundRight = false;
                        HasFoundLeft = true;
                        Right = new Number { Value = 0 };
                        Left.AddToRightMostValue(LeftValue);
                        return true;
                    }
                }
                else if (Left.IsPair && ((Pair)Left).ExplodeInternal(depth + 1))
                {
                    if (!HasFoundRight)
                    {
                        HasFoundRight = true;
                        Right.AddToLeftMostValue(RightValue);
                    }
                    return true;
                }
                else if (Right.IsPair && ((Pair)Right).ExplodeInternal(depth + 1))
                {
                    if (!HasFoundLeft)
                    {
                        HasFoundLeft = true;
                        Left.AddToRightMostValue(LeftValue);
                    }
                    return true;
                }

                return false;
            }

            public override int Magnitude => Left.Magnitude * 3 + Right.Magnitude * 2;

            public override Element Copy()
            {
                return new Pair { Left = Left.Copy(), Right = Right.Copy() };
            }
        }

        private class Number : Element
        {
            public int Value { get; set; }

            public override bool IsPair => false;

            public override string Print => Value.ToString();

            public override void AddToLeftMostValue(int value)
            {
                this.Value += value;
            }

            public override void AddToRightMostValue(int value)
            {
                this.Value += value;
            }

            public Pair GetSplittedPair()
            {
                var left = this.Value / 2;
                var right = this.Value / 2;
                if (this.Value % 2 == 1)
                {
                    right++;
                }

                return new Pair { Left = new Number { Value = left }, Right = new Number { Value = right } };
            }

            public override int Magnitude => this.Value;

            public override Element Copy()
            {
                return new Number { Value = Value };
            }
        }

        private abstract class Element
        {
            public abstract bool IsPair { get; }

            public abstract string Print { get; }

            public abstract void AddToLeftMostValue(int value);

            public abstract void AddToRightMostValue(int value);

            public abstract int Magnitude { get; }

            public abstract Element Copy();
        }
    }


}
