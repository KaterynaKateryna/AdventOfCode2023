namespace AdventOfCode;

public class Day09 : BaseDay
{
    long[][] _input;

    public Day09()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(l => l.Split(" ").Select(long.Parse).ToArray()).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        long sum = _input.Aggregate(0L, (x, y) => x += GetNextValue(y));
        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long sum = _input.Aggregate(0L, (x, y) => x += GetPreviousValue(y));
        return new(sum.ToString());
    }

    private long GetNextValue(long[] line)
    {
        List<long[]> lines = GetDiffsList(line);
        return lines.Sum(l => l.Last());
    }

    private long GetPreviousValue(long[] line)
    {
        List<long[]> lines = GetDiffsList(line);

        long res = 0;
        for(int i = lines.Count - 1; i >=0; --i)
        {
            res = lines[i][0] - res;
        }
        return res;
    }

    private List<long[]> GetDiffsList(long[] line)
    {
        List<long[]> lines = [line];

        while (!lines.Last().All(x => x == 0))
        {
            long[] diffs = GetDiffs(lines.Last());
            lines.Add(diffs);
        }

        return lines;
    }

    private long[] GetDiffs(long[] line) 
    {
        long[] diffs = new long[line.Length - 1];
        for (int i = 1; i < line.Length; ++i)
        {
            diffs[i - 1] = line[i] - line[i - 1];
        }
        return diffs;
    }
}
