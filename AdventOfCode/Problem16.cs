using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Problem16 : Problem
    {
        Parser parser;
        long problem1 = 0;
        public void Solve(IEnumerable<string> input, ResultWriter resultWriter)
        {
            parser = new Parser(input.First());
            
            var outerPacket = ParsePacket();
            resultWriter.WriteResult(problem1);
            var problem2 = outerPacket.Value;
            resultWriter.WriteResult(problem2);
        }

        private Packet ParsePacket()
        {
            var version = parser.Pop(3);
            problem1 += BinToInt(version);
            var type = parser.Pop(3);
            switch (type)
            {
                case "100":
                    return this.ParseLiteral();
                default:
                    return this.ParseOperator(type);
            }
        }

        private Literal ParseLiteral()
        {
            var totalLength = 6;
            var lastGroupParsed = false;
            var builder = new StringBuilder();
            while (!lastGroupParsed)
            {
                lastGroupParsed = parser.Pop(1) == "0" ? true : false;
                builder.Append(parser.Pop(4));
                totalLength += 5;
            }

            return new Literal { LengthInternal = totalLength, ValueInternal = builder.ToString() };
        }

        private Operator ParseOperator(string typeId)
        {
            var lengthTypeId = parser.Pop(1);
            if (lengthTypeId == "0")
            {
                var length = BinToInt(parser.Pop(15));
                var packets = new List<Packet>();
                var totalLength = 0;
                while (totalLength < length)
                {
                    var packet = this.ParsePacket();
                    packets.Add(packet);
                    totalLength += packet.Length;
                }

                return new Operator { SubPackets = packets, HeaderLength = 22, TypeId = typeId};

            }
            else if (lengthTypeId == "1")
            {
                var nbrOfPackets = BinToInt(parser.Pop(11));
                var packets = Enumerable.Range(0, (int)nbrOfPackets).Select(x => ParsePacket());
                return new Operator { SubPackets = packets.ToList(), HeaderLength = 18, TypeId = typeId };
            }
            else
            {
                throw new Exception("Uknown length type " + lengthTypeId);
            }
        }

        private static long BinToInt(string bin)
        {
            return Convert.ToInt64(bin, 2);
        }

        private class Parser
        {
            int pointer;
            string line;
            public Parser(string line)
            {
                this.line = string.Join("", line.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                pointer = 0;
            }

            public string Pop(int length)
            {
                var s = line.Substring(pointer, length);
                pointer += length;
                return s;
            }
        }

        private class Literal : Packet
        {
            public override int Length => LengthInternal;

            public int LengthInternal { get; set; }

            public override long Value => BinToInt(ValueInternal);

            public string ValueInternal { get; set; }
        }

        private class Operator : Packet
        {
            public IList<Packet> SubPackets { get; set; }

            public string TypeId { get; set; }

            public int HeaderLength { get; set; }

            public override int Length => SubPackets.Sum(x => x.Length) + HeaderLength;

            public override long Value
            {
                get
                {
                    switch (TypeId)
                    {
                        case "000":
                            return SubPackets.Sum(x => x.Value);
                        case "001":
                            return SubPackets.Select(x => x.Value).Aggregate((a, b) => a * b);
                        case "010":
                            return SubPackets.Min(x => x.Value);
                        case "011":
                            return SubPackets.Max(x => x.Value);
                        case "101":
                            return SubPackets[0].Value > SubPackets[1].Value ? 1 : 0;
                        case "110":
                            return SubPackets[0].Value < SubPackets[1].Value ? 1 : 0;
                        case "111":
                            return SubPackets[0].Value == SubPackets[1].Value ? 1 : 0;
                        default:
                            throw new Exception("Unknown TypeId " + TypeId);
                    }
                }
            }
        }

        private abstract class Packet
        {
            public abstract int Length { get; }

            public abstract long Value { get; }
        }
    }


}
