using System.Text;
using AOCHelper;

namespace AOC2024;

public class Day17(int year) : AOC_DayBase(year, 17)
{
    private class Computer
    {
        private int _a;
        private int _b;
        private int _c;

        public int A
        {
            get => _a;
            set => _a = value;
        }

        public int B
        {
            get => _b;
            set => _b = value;
        }

        public int C
        {
            get => _c;
            set => _c = value;
        }

        public int IP { get; set; }

        public List<int> Program { get; init; }

        public List<int> Output { get; } = [];

        public void Step()
        {
            var opcode = Program[IP];
            var operand = Program[IP + 1];

            switch (opcode)
            {
                case 0: // adv
                {
                    Xdv(out _a);
                    break;
                }
                case 1: // bxl
                {
                    _b ^= operand;
                    IP += 2;

                    break;
                }
                case 2: // bst
                {
                    _b = ResolveComboOperand(operand) % 8;
                    IP += 2;

                    break;
                }
                case 3: // jnz
                {
                    if (_a == 0)
                    {
                        IP += 2;
                    }
                    else
                    {
                        IP = operand;
                    }

                    break;
                }
                case 4: // bxc
                {
                    _b ^= _c;
                    IP += 2;

                    break;
                }
                case 5: // out
                {
                    Output.Add(ResolveComboOperand(operand) % 8);
                    IP += 2;

                    break;
                }
                case 6: // bdv
                {
                    Xdv(out _b);
                    break;
                }
                case 7: // cdv
                {
                    Xdv(out _c);
                    break;
                }
            }
        }

        private int ResolveComboOperand(int operand)
        {
            return operand switch
            {
                < 4 => operand,
                4 => _a,
                5 => _b,
                _ => _c,
            };
        }

        private void Xdv(out int reg)
        {
            var combo = ResolveComboOperand(Program[IP + 1]);

            if (combo == 0)
            {
                reg = 1;
            }
            else
            {
                var num = A;
                var den = 2 << (combo - 1);
                
                reg = num / den;
            }
            
            IP += 2;
        }
    }

    protected override void SolveA_Internal(string input)
    {
        var computer = Parse(input);

        while (computer.IP < computer.Program.Count)
        {
            computer.Step();
        }

        AOC_Logger.Display("Result: " + string.Join(',', computer.Output));
    }

    protected override void SolveB_Internal(string input)
    {
        int a = 0;

        var computer = Parse(input);

        while (true)
        {
            computer.A = a++;
            computer.B = 0;
            computer.C = 0;
            computer.IP = 0;
            computer.Output.Clear();

            while (computer.IP < computer.Program.Count)
            {
                computer.Step();

                if (computer.Output.Count > 0)
                {
                    if (computer.Output.Count > computer.Program.Count)
                    {
                        break;
                    }

                    var len = computer.Output.Count;

                    if (computer.Output[len - 1] != computer.Program[len - 1])
                    {
                        break;
                    }
                }
            }

            if (computer.Output.Count == computer.Program.Count)
            {
                AOC_Logger.Display($"A = {a - 1}");
                break;
            }

            a++;
        }
    }

    private Computer Parse(string input)
    {
        var lines = AOC_Utils.SplitLines(input).ToList();

        var computer = new Computer
        {
            A = int.Parse(lines[0]["Register A:".Length..]),
            B = int.Parse(lines[1]["Register B:".Length..]),
            C = int.Parse(lines[2]["Register C:".Length..]),
            Program = lines[4]["Program: ".Length..]
                .Split(',')
                .Select(int.Parse)
                .ToList(),
        };

        return computer;
    }

    protected override string TestInput => """
                                           Register A: 2024
                                           Register B: 0
                                           Register C: 0

                                           Program: 0,3,5,4,3,0
                                           """;
}