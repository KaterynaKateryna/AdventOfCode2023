using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => Solve(GetCalibrationValue1);
    public override ValueTask<string> Solve_2() => Solve(GetCalibrationValue2);

    private ValueTask<string> Solve(Func<string, int> getCalibrationValue)
    {
        string[] lines = _input.Split(Environment.NewLine);
        long sum = 0;

        foreach (string item in lines)
        {
            sum += getCalibrationValue(item);
        }

        return new(sum.ToString());
    }

    private int GetCalibrationValue1(string item)
    {
        MatchCollection matches = Regex.Matches(item, "[0-9]");

        return int.Parse(
            matches.First().Value +
            matches.Last().Value
        );
    }

    private int GetCalibrationValue2(string item)
    {
        MatchCollection matches = Regex.Matches(item, "(?=([0-9]|one|two|three|four|five|six|seven|eight|nine))");

        return int.Parse(
            ConvertToDigit(matches.First().Groups[1].Value) +
            ConvertToDigit(matches.Last().Groups[1].Value)
        );
    }

    private string ConvertToDigit(string value)
    { 
        switch (value) 
        {
            case "one": return "1";
            case "two": return "2";
            case "three": return "3";
            case "four": return "4";
            case "five": return "5";
            case "six": return "6";
            case "seven": return "7";
            case "eight": return "8";
            case "nine": return "9";
            default: return value;
        }
    }
}
