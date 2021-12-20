using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem20 : Problem
    {
        public const char Dark = '.';
        public const char Light = '#';

        private char[,] picture = new char[300, 300];
        private int startIndex = 75;
        private int endIndex = 175;

        private char OutSideColor = Dark;

        private Dictionary<int, char> Lookup = new Dictionary<int, char>(); 
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            initPicture(picture);
            var frstLine = input.First();
            for (var i = 0; i < input.First().Length; i++)
            {
                Lookup.Add(i, frstLine[i]);
            }

            var list = input.Skip(2).ToList();
            for (var i = 0; i < list.Count; i++)
            {
                var line = list[i];
                for (var x = 0; x < line.Length; x++)
                {
                    picture[x + 75, i + 75] = line[x];
                }
            }
            Print();
            resultWriter.WriteFiller();
            this.Enhance();
            Print();
            resultWriter.WriteFiller();
       

            this.Enhance();

            resultWriter.WriteResult(CalcLights());

            for (var i = 0; i < 48; i++)
            {
                this.Enhance();
            }

            resultWriter.WriteResult(CalcLights());
        }

        private void initPicture(char[,] picture)
        {
            for (var x = 0; x < picture.GetLength(0); x++)
            {
                for (var y = 0; y < picture.GetLength(1); y++)
                {
                    picture[x, y] = Dark;
                }
            }
        }

        private void Print()
        {
            for (var i = startIndex; i < endIndex; i++)
            {
                for (var x = startIndex; x < endIndex; x++)
                {
                    Console.Write(picture[x, i].ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine("------------------");
        }

        private int CalcLights()
        {
            var count = 0;
            picture.ForEach(x => count += (x == Light) ? 1 : 0);
            return count;
        }

        private void Enhance()
        {
     
            char[,] newPic = new char[300, 300];
            initPicture(newPic);
            for (var x = startIndex - 1; x < endIndex + 1; x++)
            {
                for (var y = startIndex - 1; y < endIndex + 1; y++)
                {
                    newPic[x, y] = picture[x, y];
                    var sum = "";
                    sum += getDec(x - 1, y - 1);
                    sum += getDec(x, y - 1);
                    sum += getDec(x + 1, y - 1);
                    sum += getDec(x - 1, y);
                    sum += getDec(x, y);
                    sum += getDec(x + 1, y);
                    sum += getDec(x - 1, y + 1);
                    sum += getDec(x, y + 1);
                    sum += getDec(x + 1, y + 1);
                    var dec = Convert.ToInt32(sum, 2);
                    newPic[x, y] = Lookup[dec];
                }
            }

            endIndex++;
            startIndex--;
            OutSideColor = OutSideColor == Dark ? Light : Dark;
            picture = newPic;
        }

        private char getColor(int x, int y)
        {
            if (x < startIndex || y < startIndex || x >= endIndex || y >= endIndex)
            {
                return OutSideColor;
            }

            return picture[x, y];
        }

        private int getDec(int x, int y)
        {
            var color = getColor(x, y);
            return color == Dark ? 0 : 1; 

        }


    }
}
