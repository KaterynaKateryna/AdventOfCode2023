
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
        return new(_input.Sum(GetPossibleArrangements).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private int GetPossibleArrangements(string line)
    {
        string[] parts = line.Split(' ');
        string spring = parts[0];
        List<int> groups = parts[1].Split(',').Select(x => int.Parse(x)).ToList();

        int count = 0;
        List<int> unknowns = new List<int>();
        for (int i = 0; i < spring.Length; ++i)
        {
            if (spring[i] == '?')
            { 
                unknowns.Add(i);
            }
        }

        List<List<bool>> arrangements = GetAllArrangements(unknowns.Count);
        for (int i = 0; i < arrangements.Count; ++i)
        {
            char[] mutation = spring.ToCharArray();
            for (int j = 0; j < unknowns.Count; ++j)
            {
                mutation[unknowns[j]] = arrangements[i][j] ? '#' : '.';
            }
            bool isValid = IsValid(new string(mutation), groups);
            count += isValid ? 1 : 0;
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

    private List<List<bool>> GetAllArrangements(int length)
    {
        if (length == 0)
        {
            return new List<List<bool>>();
        }

        if (length == 1)
        {
            List<List<bool>> res = [[false], [true]];
            return res;
        }

        List<List<bool>> arr = GetAllArrangements(length - 1);
        List<List<bool>> f = arr.Select(x => x.Append(false).ToList()).ToList();
        List<List<bool>> t = arr.Select(x => x.Append(true).ToList()).ToList();

        f.AddRange(t);

        return f;
    }
}
