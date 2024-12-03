using AOCHelper;

namespace AOC2024;

public class Day03(int year) : AOC_DayBase(year, 3)
{
    protected override void SolveA_Internal(string input)
    {
        var result = 0L;

        var parser = new ILParser(input);
        var nodes = parser.Parse();

        foreach (var node in nodes)
        {
            if (node is MulNode mulNode)
            {
                result += mulNode.A * mulNode.B;
            }
        }

        AOC_Logger.Display("Result: " + result);
    }

    protected override void SolveB_Internal(string input)
    {
        long result = 0;

        var parser = new ILParser(input);
        var nodes = parser.Parse();

        var enabled = true;

        foreach (var node in nodes)
        {
            if (node is MulNode mulNode)
            {
                if (enabled) result += mulNode.A * mulNode.B;
            }
            else if (node is MulStateNode stateNode)
            {
                enabled = stateNode.Enable;
            }
        }

        AOC_Logger.Display("Result: " + result);
    }

    private class ILParser(string input)
    {
        private int _i;

        public List<ILNode> Parse()
        {
            var nodes = new List<ILNode>();

            _i = 0;

            while (_i < input.Length)
            {
                var c = input[_i];

                if (c == 'm')
                {
                    if (Peek(_i + 1) == 'u' &&
                        Peek(_i + 2) == 'l' &&
                        Peek(_i + 3) == '(')
                    {
                        _i += 4;

                        if (Mul(out var a, out var b))
                        {
                            nodes.Add(new MulNode
                            {
                                A = a,
                                B = b,
                            });
                        }

                        continue;
                    }
                }
                else if (c == 'd')
                {
                    if (Peek(_i + 1) == 'o')
                    {
                        if (Peek(_i + 2) == '(' &&
                            Peek(_i + 3) == ')')
                        {
                            _i += 4;

                            nodes.Add(new MulStateNode
                            {
                                Enable = true,
                            });

                            continue;
                        }

                        if (Peek(_i + 2) == 'n' &&
                            Peek(_i + 3) == '\'' &&
                            Peek(_i + 4) == 't' &&
                            Peek(_i + 5) == '(' &&
                            Peek(_i + 6) == ')')
                        {
                            _i += 7;

                            nodes.Add(new MulStateNode
                            {
                                Enable = false,
                            });

                            continue;
                        }
                    }
                }

                // default advance pointer
                _i++;
            }

            return nodes;
        }

        private bool Mul(
            out long a,
            out long b)
        {
            a = 0L;
            b = 0L;

            var settingB = false;

            var isASet = false;
            var isBSet = false;

            var success = false;

            while (true)
            {
                var cc = Peek(_i++);

                if (cc >= '0' && cc <= '9')
                {
                    var num = cc - '0';

                    if (settingB)
                    {
                        b = 10L * b + num;
                        isBSet = true;
                    }
                    else
                    {
                        a = 10L * a + num;
                        isASet = true;
                    }

                    continue;
                }
                else if (cc == ',')
                {
                    if (isASet && !settingB)
                    {
                        settingB = true;
                        continue;
                    }
                }
                else if (cc == ')')
                {
                    if (isBSet)
                    {
                        success = true;
                    }
                }

                break;
            }

            return success;
        }

        private char Peek(int pos)
        {
            return pos >= input.Length ? '\0' : input[pos];
        }
    }

    private abstract class ILNode
    {
    }

    private class MulNode : ILNode
    {
        public long A { get; init; }

        public long B { get; init; }
    }

    private class MulStateNode : ILNode
    {
        public bool Enable { get; init; }
    }

    protected override string TestInput => """
                                           xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))
                                           """;
}