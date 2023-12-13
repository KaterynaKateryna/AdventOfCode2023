
namespace AdventOfCode;

public class Day13 : BaseDay
{
    string[][] _input;

    public Day13() 
    {
        string all = File.ReadAllText(InputFilePath);
        _input = all.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split(Environment.NewLine))
            .ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        int sum = 0;
        for (int i = 0; i < _input.Length; ++i)
        {
            sum += GetMirror(_input[i], Direction.Vertical);
            sum += GetMirror(_input[i], Direction.Horizontal) * 100;
        }

        return new(sum.ToString());
    }

    private int GetMirror(string[] pattern, Direction direction)
    {
        int length = direction == Direction.Vertical ? pattern[0].Length : pattern.Length;
        for (int i = 0; i < length - 1; ++i)
        {
            if (IsMirror(pattern, i, direction, length))
            {
                return i + 1;
            }
        }

        return 0;
    }

    private bool IsMirror(string[] pattern, int splitAfter, Direction direction, int length)
    {
        int a = splitAfter;
        int b = splitAfter + 1;

        while (a >= 0 && b < length)
        {
            if (!(direction == Direction.Vertical ? ColumnsEqual(pattern, a, b) : RowEqual(pattern, a, b)))
            { 
                return false;
            }
            a--;
            b++;
        }
        return true;
    }

    private bool ColumnsEqual(string[] pattern, int a, int b)
    {
        for (int i = 0; i < pattern.Length; ++i)
        {
            if (pattern[i][a] != pattern[i][b])
            { 
                return false;
            }
        }
        return true;
    }

    private bool RowEqual(string[] pattern, int a, int b)
    {
        return pattern[a] == pattern[b];
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private enum Direction
    { 
        Horizontal,
        Vertical
    }
}
