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

            spring = Preprocess(spring, groups);
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
            string spring = string.Join("?", Enumerable.Repeat(parts[0], 5));
            List<int> groups = parts[1].Split(',').Select(x => int.Parse(x)).ToList();
            groups = Enumerable.Repeat(groups, 5).SelectMany(x => x).ToList();

            spring = Preprocess(spring, groups);
            sum += GetPossibleArrangements(spring, groups);
        }

        return new(sum.ToString());
    }

    private string Preprocess(string spring, List<int> groups)
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
                char[] chars = spring.ToArray();
                for (int i = leftPos; i <= rightPos; ++i)
                {
                    chars[i] = '#';
                }
                spring = new(chars);
            }

            groupLength -= (group + 1);
            index += (group + 1);
        }
        return spring;
    }

    private int GetPossibleArrangements(string spring, List<int> groups, int index = 0)
    {
        if (index == spring.Length)
        {
            if (IsValid(spring, groups, spring.Length))
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
            string optionA = new string(option);
            if (IsValid(optionA, groups, index + 1))
            {
                int a = GetPossibleArrangements(optionA, groups, index + 1);
                count += a;
            }

            option[index] = '.';
            string optionB = new string(option);
            if (IsValid(optionB, groups, index + 1))
            {
                int b = GetPossibleArrangements(optionB, groups, index + 1);
                count += b;
            }
        }
        else
        {
            count += GetPossibleArrangements(spring, groups, index + 1);
        }
       
        return count;
    }

    private bool IsValid(string spring, List<int> groups, int length)
    {
        List<int> groupsToCheck = new List<int>(groups);

        int activeGroup = 0;
        for (int i = 0; i < length; ++i)
        {
            if (spring[i] == '#')
            {
                activeGroup++;
            }
            else if (activeGroup > 0)
            {
                if (groupsToCheck.FirstOrDefault() == activeGroup)
                {
                    groupsToCheck.Remove(activeGroup);
                    activeGroup = 0;
                }
                else
                {
                    return false;
                }
            }
        }
        if (activeGroup > 0)
        {
            if (length < spring.Length && activeGroup <= groupsToCheck.FirstOrDefault())
            {
                return true;
            }
            if (length == spring.Length && activeGroup == groupsToCheck.FirstOrDefault())
            {
                groupsToCheck.Remove(activeGroup);
                activeGroup = 0;
            }
            else
            {
                return false;
            }
        }

        return length < spring.Length || !groupsToCheck.Any();
    }
}
