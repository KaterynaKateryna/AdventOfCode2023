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

        List<Galaxy> expanded = Expand(galaxies);

        int sum = 0;
        for (int i = 0; i < expanded.Count - 1; ++i)
        {
            for (int j = i + 1; j < expanded.Count; ++j)
            {
                int path = GetPath(expanded[i], expanded[j]);
                sum += path;
            }
        }

        return new(sum.ToString());
    }

    private int GetPath(Galaxy galaxy1, Galaxy galaxy2)
    {
        int iDiff = Math.Abs(galaxy1.I - galaxy2.I);
        int jDiff = Math.Abs(galaxy1.J - galaxy2.J);
        int path = iDiff + jDiff;
        return path;
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private List<Galaxy> Expand(List<Galaxy> galaxies)
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
            int newI = galaxies[i].I + iToExpand.Count(c => c < galaxies[i].I);
            int newJ = galaxies[i].J + jToExpand.Count(c => c < galaxies[i].J);
            expanded.Add(new Galaxy(newI, newJ));
        }
        return expanded;
    }

    private record Galaxy(int I, int J);
}
