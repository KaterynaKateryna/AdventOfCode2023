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
            char[] spring = parts[0].ToArray();
            List<int> groups = parts[1].Split(',').Select(x => int.Parse(x)).ToList();

            Preprocess(spring, groups);
            sum += GetPossibleArrangements(spring, groups);
        }

        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long sum = 0;
        foreach (var line in _input)
        {
            string[] parts = line.Split(' ');
            char[] spring = string.Join("?", Enumerable.Repeat(parts[0], 5)).ToArray();
            List<int> groups = parts[1].Split(',').Select(x => int.Parse(x)).ToList();
            groups = Enumerable.Repeat(groups, 5).SelectMany(x => x).ToList();

            Preprocess(spring, groups);
            sum += GetPossibleArrangements(spring, groups);
        }

        return new(sum.ToString());
    }

    private void Preprocess(char[] spring, List<int> groups)
    {
        int groupLength = groups.Sum() + groups.Count - 1;
        int springLength = spring.Length;

        int index = 0;
        foreach (var group in groups) 
        {
            for (int i = index; i < index + group; ++i)
            {
                if (spring[i] == '.')
                {
                    index = i + 1;
                }
            }
            int rightPos = index + group - 1;
            int leftPos = springLength - groupLength;

            if (leftPos <= rightPos)
            {
                for (int i = leftPos; i <= rightPos; ++i)
                {
                    spring[i] = '#';
                }
            }

            groupLength -= (group + 1);
            index += (group + 1);
        }
    }

    private int GetPossibleArrangements(char[] spring, List<int> groups, int index = 0, int curGroupIndex = 0)
    {
        if (index == spring.Length)
        {
            (bool isValid, int _) = IsValid(spring, groups, spring.Length, curGroupIndex);
            if (isValid)
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
            (bool isValid, int newIndex) = IsValid(option, groups, index, curGroupIndex);
            if (isValid)
            {
                int a = GetPossibleArrangements(option, groups, index + 1, newIndex);
                count += a;
            }

            option[index] = '.';
            (isValid, newIndex) = IsValid(option, groups, index, curGroupIndex);
            if (isValid)
            {
                int b = GetPossibleArrangements(option, groups, index + 1, newIndex);
                count += b;
            }
        }
        else
        {
            (bool isValid, int newIndex) = IsValid(spring, groups, index, curGroupIndex);
            if (isValid)
            {
                count += GetPossibleArrangements(spring, groups, index + 1, newIndex);
            }
        }
       
        return count;
    }

    private (bool isValid, int curGroupIndex) IsValid(char[] spring, List<int> groups, int index, int curGroupIndex)
    {
        if (index == spring.Length)
        {
            return (curGroupIndex == groups.Count, curGroupIndex);
        }

        int cur = curGroupIndex == groups.Count ? 0 : groups[curGroupIndex];

        if (spring[index] == '.')
        {
            int groupLength = 0;
            int i = index - 1;
            while (i >= 0 && spring[i] == '#')
            {
                groupLength++;
                i--;
            }

            if (groupLength > 0)
            {
                if (groupLength == cur)
                {
                    curGroupIndex++;
                }
                if (groupLength < cur)
                {
                    return (false, curGroupIndex);
                }
            }
        }
        else if (spring[index] == '#')
        {
            int groupLength = 1;
            int i = index - 1;
            while (i >= 0 && spring[i] == '#')
            { 
                groupLength++;
                i--;
            }

            if (groupLength > cur)
            {
                return (false, curGroupIndex);
            }
            else if (groupLength == cur && index == spring.Length - 1)
            {
                curGroupIndex++;
            }
        }

        return (true, curGroupIndex);
    }
}
