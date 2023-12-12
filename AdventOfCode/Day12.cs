namespace AdventOfCode;

public class Day12 : BaseDay
{
    string[] _input;

    public Day12()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        long sum = 0;
        foreach (var line in _input)
        {
            string[] parts = line.Split(' ');
            string spring = parts[0];
            List<int> groups = parts[1].Split(',').Select(x => int.Parse(x)).ToList();
            sum += GetPossibleArrangements(spring, groups);
        }

        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long sum = 0;
        //foreach (var line in _input)
        //{
        //    string[] parts = line.Split(' ');
        //    string spring = string.Join("?", Enumerable.Repeat(parts[0], 5));
        //    List<int> groups = parts[1].Split(',').Select(x => int.Parse(x)).ToList();
        //    groups = Enumerable.Repeat(groups, 5).SelectMany(x => x).ToList();
        //    sum += GetPossibleArrangements(spring, groups);
        //}

        return new(sum.ToString());
    }

    private int GetPossibleArrangements(string spring, List<int> groups, int index = 0)
    {
        if (index == spring.Length)
        {
            if (IsValid(spring, groups))
            {
                return 1;
            }
            return 0;
        }

        int count = 0;

        if (spring[index] == '?')
        {
            char[] option = spring.ToArray();
            option[index] = '#';
            int a = GetPossibleArrangements(new string(option), groups, index + 1);
            count += a;
            option[index] = '.';
            int b = GetPossibleArrangements(new string(option), groups, index + 1);
            count += b;
        }
        else
        {
            count += GetPossibleArrangements(spring, groups, index + 1);
        }
       
        return count;
    }

    private bool IsValid(string spring, List<int> groups)
    {
        List<int> actualGroups = new List<int>();
        int activeGroup = 0;
        for (int i = 0; i < spring.Length; ++i)
        {
            if (spring[i] == '#')
            {
                activeGroup++;
            }
            else if (activeGroup > 0)
            {
                actualGroups.Add(activeGroup);
                activeGroup = 0;
            }
        }
        if (activeGroup > 0)
        {
            actualGroups.Add(activeGroup);
            activeGroup = 0;
        }

        return actualGroups.SequenceEqual(groups);
    }
}
