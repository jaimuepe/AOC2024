using AOCHelper;

namespace AOC2024;

internal class Update
{
    public readonly int[] nums;

    public readonly int[] indices;

    public Update(string raw)
    {
        var tokens = raw.Split(",");

        nums = new int[tokens.Length];

        indices = new int[100];
        Array.Fill(indices, -1);

        for (var i = 0; i < tokens.Length; i++)
        {
            nums[i] = int.Parse(tokens[i]);
            indices[nums[i]] = i;
        }
    }
}

internal class Rules
{
    private readonly List<int>?[] _rules = new List<int>?[100];

    public void AddRule(int left, int right)
    {
        _rules[left] ??= [];
        _rules[left]!.Add(right);
    }
    
    public bool HasRule(int left, int right)
    {
        return _rules[left] != null && _rules[left]!.Contains(right);
    }

    public List<int>? GetRules(int num) => _rules[num];
}

public class Day05(int year) : AOC_DayBase(year, 5)
{
    protected override void SolveA_Internal(string input)
    {
        var (rules, updates) = Parse(input);

        var result = 0;

        foreach (var update in updates)
        {
            if (IsCorrectlyOrderedUpdate(update, rules))
            {
                result += update.nums[update.nums.Length / 2];
            }
        }

        AOC_Logger.Display("Result: " + result);
    }

    protected override void SolveB_Internal(string input)
    {
        var (rules, updates) = Parse(input);

        var result = 0;

        foreach (var update in updates)
        {
            if (IsCorrectlyOrderedUpdate(update, rules)) continue;
            
            var numsList = new List<int>(update.nums);
                
            numsList.Sort((n1, n2) =>
            {
                if (rules.HasRule(n1, n2)) return -1;
                if (rules.HasRule(n2, n1)) return 1;
                return 0;
            });

            result += numsList[numsList.Count / 2];
        }

        AOC_Logger.Display("Result: " + result);
    }

    private static (Rules rules, List<Update>) Parse(string input)
    {
        var rules = new Rules();
        
        var beforeSeparator = true;

        var updates = new List<Update>();

        foreach (var line in AOC_Utils.SplitLines(input))
        {
            if (line == "")
            {
                beforeSeparator = false;
            }
            else if (beforeSeparator)
            {
                var left = int.Parse(line[..2]);
                var right = int.Parse(line[3..]);

                rules.AddRule(left, right);
            }
            else
            {
                updates.Add(new Update(line));
            }
        }

        return (rules, updates);
    }

    private static bool IsCorrectlyOrderedUpdate(
        Update update,
        Rules rules)
    {
        foreach (var num in update.nums)
        {
            var idx = update.indices[num];

            var numRules = rules.GetRules(num);
            if (numRules == null) continue;

            foreach (var rule in numRules)
            {
                var otherIdx = update.indices[rule];
                if (otherIdx == -1) continue;

                if (otherIdx < idx) return false;
            }
        }

        return true;
    }

    protected override string TestInput => """
                                           47|53
                                           97|13
                                           97|61
                                           97|47
                                           75|29
                                           61|13
                                           75|53
                                           29|13
                                           97|29
                                           53|29
                                           61|53
                                           97|53
                                           61|29
                                           47|13
                                           75|47
                                           97|75
                                           47|61
                                           75|61
                                           47|29
                                           75|13
                                           53|13

                                           75,47,61,53,29
                                           97,61,53,29,13
                                           75,29,13
                                           75,97,47,61,53
                                           61,13,29
                                           97,13,75,29,47
                                           """;
}