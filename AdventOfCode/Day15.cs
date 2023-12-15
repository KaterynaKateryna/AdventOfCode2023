namespace AdventOfCode;

public class Day15 : BaseDay
{
    private string _input;

    public Day15()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        string[] steps = _input.Split(',', StringSplitOptions.RemoveEmptyEntries);
        int sum = steps.Sum(GetHashCode);

        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private int GetHashCode(string step)
    {
        int result = 0;
        foreach(char ch in step) 
        {
            result += ch;
            result *= 17;
            result %= 256;
        }

        return result;
    }
}
