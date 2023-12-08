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
        return new("TBD");
    }

    private record Turns(string Left, string Right);
}
