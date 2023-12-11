namespace AdventOfCode;

public class Day11 : BaseDay
{
    private string[] _input;

    public Day11()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        return new(Solve(2).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(Solve(1000000).ToString());
    }

    private long Solve(int factor)
    {
        List<Galaxy> galaxies = new List<Galaxy>();

        for (int i = 0; i < _input.Length; ++i)
        {
            for (int j = 0; j < _input[i].Length; ++j)
            {
                if (_input[i][j] == '#')
                {
                    galaxies.Add(new Galaxy(i, j));
                }
            }
        }

        List<Galaxy> expanded = Expand(galaxies, factor);

        long sum = 0;
        for (int i = 0; i < expanded.Count - 1; ++i)
        {
            for (int j = i + 1; j < expanded.Count; ++j)
            {
                long path = GetPath(expanded[i], expanded[j]);
                sum += path;
            }
        }
        return sum;
    }

    private long GetPath(Galaxy galaxy1, Galaxy galaxy2)
    {
        long iDiff = Math.Abs(galaxy1.I - galaxy2.I);
        long jDiff = Math.Abs(galaxy1.J - galaxy2.J);
        long path = iDiff + jDiff;
        return path;
    }

    private List<Galaxy> Expand(List<Galaxy> galaxies, int factor)
    {
        List<int> iToExpand = new List<int>();
        List<int> jToExpand = new List<int>();
        for (int i = 0; i < _input.Length; ++i)
        {
            if (!galaxies.Any(g => g.I == i))
            {
                iToExpand.Add(i);
            }
        }
        for (int j = 0; j < _input[0].Length; ++j)
        {
            if (!galaxies.Any(g => g.J == j))
            {
                jToExpand.Add(j);
            }
        }

        List<Galaxy> expanded = new List<Galaxy>();
        for (int i = 0; i < galaxies.Count; ++i)
        {
            long newI = galaxies[i].I + iToExpand.Count(c => c < galaxies[i].I) * (factor - 1);
            long newJ = galaxies[i].J + jToExpand.Count(c => c < galaxies[i].J) * (factor - 1);
            expanded.Add(new Galaxy(newI, newJ));
        }
        return expanded;
    }

    private record Galaxy(long I, long J);
}
