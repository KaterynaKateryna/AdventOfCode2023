using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        string[] lines = _input.Split(Environment.NewLine);
        long sum = 0;

        foreach (string item in lines)
        {
            MatchCollection matches = Regex.Matches(item, "[0-9]");
            int calibrationValue = int.Parse(matches.First().Value + matches.Last().Value);
            sum += calibrationValue;
        }

        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2() => new($"Solution to {ClassPrefix} {CalculateIndex()}, part 2");
}
