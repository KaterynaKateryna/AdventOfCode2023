using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Day08 : BaseDay
{
    string _instructions;
    Dictionary<string, Turns> _map = new Dictionary<string, Turns>();

    public Day08()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        _instructions = lines[0];

        for (int i = 2; i < lines.Length; i++)
        {
            Match match = Regex.Match(lines[i], @"([A-Z]{3}) = \(([A-Z]{3}), ([A-Z]{3})\)");
            string value = match.Groups[1].Value;
            string left = match.Groups[2].Value;
            string right = match.Groups[3].Value;

            _map[value] = new Turns(left, right);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int steps = 0;
        int instruction = 0;
        string current = "AAA";

        while (current != "ZZZ")
        {
            Turns turns = _map[current];
            if (_instructions[instruction] == 'L')
            {
                current = turns.Left;
            }
            else
            {
                current = turns.Right;
            }
            steps++;
            instruction = (instruction + 1) % _instructions.Length;
        }

        return new(steps.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        string[] current = _map.Keys.Where(x => x[2] == 'A').ToArray();

        List<ulong> found = new List<ulong>();

        for (int i = 0; i < current.Length; ++i)
        {
            ulong steps = 0;
            int instruction = 0;
            string cur = current[i];
            while (cur[2] != 'Z' || instruction != 0)
            {
                char ins = _instructions[instruction];

                Turns turns = _map[cur];
                if (_instructions[instruction] == 'L')
                {
                    cur = turns.Left;
                }
                else
                {
                    cur = turns.Right;
                }
                steps++;
                instruction = (instruction + 1) % _instructions.Length;
            }
            found.Add(steps);
        }

        ulong leastCommonMultiplier = found.SelectMany(GetMultipliers).Distinct().Aggregate((a, b) => a * b);

        return new(leastCommonMultiplier.ToString());
    }

    private List<ulong> GetMultipliers(ulong number)
    {
        List<ulong> res = new List<ulong>();
        ulong cur = 2;
        ulong num = number;
        while (cur <= (num / 2))
        {
            while (num % cur == 0)
            { 
                res.Add(cur);
                num /= cur;
            }
            cur++;
        }
        res.Add(num);
        return res;
    }

    private record Turns(string Left, string Right);
}
