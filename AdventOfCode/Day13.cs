
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
        int sum = Solve(0);
        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int sum = Solve(1);
        return new(sum.ToString());
    }

    private int Solve(int expectedSmudges)
    {
        int sum = 0;
        for (int i = 0; i < _input.Length; ++i)
        {
            sum += GetMirror(_input[i], Direction.Vertical, expectedSmudges);
            sum += GetMirror(_input[i], Direction.Horizontal, expectedSmudges) * 100;
        }
        return sum;
    }

    private int GetMirror(string[] pattern, Direction direction, int expectedSmudges)
    {
        int length = direction == Direction.Vertical ? pattern[0].Length : pattern.Length;
        for (int i = 0; i < length - 1; ++i)
        {
            if (IsMirror(pattern, i, direction, length, expectedSmudges))
            {
                return i + 1;
            }
        }

        return 0;
    }

    private bool IsMirror(string[] pattern, int splitAfter, Direction direction, int length, int expectedSmudges)
    {
        int a = splitAfter;
        int b = splitAfter + 1;

        int smudges = 0;

        while (a >= 0 && b < length)
        {
            smudges += direction == Direction.Vertical ? ColumnsSmudges(pattern, a, b) : RowSmudges(pattern, a, b);
            if (smudges > expectedSmudges)
            { 
                return false;
            }
            a--;
            b++;
        }
        return smudges == expectedSmudges;
    }

    private int ColumnsSmudges(string[] pattern, int a, int b)
    {
        int smudges = 0;
        for (int i = 0; i < pattern.Length; ++i)
        {
            if (pattern[i][a] != pattern[i][b])
            {
                smudges++;
            }
        }
        return smudges;
    }

    private int RowSmudges(string[] pattern, int a, int b)
    {
        int smudges = 0;
        for (int i = 0; i < pattern[0].Length; ++i)
        {
            if (pattern[a][i] != pattern[b][i])
            {
                smudges++;
            }
        }

        return smudges;
    }

    private enum Direction
    { 
        Horizontal,
        Vertical
    }
}
