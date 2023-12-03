namespace AdventOfCode;

public class Day03 : BaseDay
{
    private string[] _input;

    public Day03()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        return new(Solve().parts.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(Solve().gearRatio.ToString());
    }
    private (long parts, long gearRatio) Solve()
    {
        long parts = 0;
        long gearRatios = 0;

        Dictionary<Point, List<int>> gearsWithParts = new Dictionary<Point, List<int>>();

        for (int i = 0; i < _input.Length; ++i)
        {
            for (int j = 0; j < _input[0].Length; ++j)
            {
                if (char.IsDigit(_input[i][j]))
                {
                    int length = GetNumberLength(i, j);
                    (bool isPartNumber, List<Point> gears) = IsPartNumber(i, j, length);
                    if (isPartNumber)
                    {
                        int part = int.Parse(_input[i].Substring(j, length));
                        parts += part;

                        foreach (Point gear in gears)
                        {
                            if (gearsWithParts.ContainsKey(gear))
                            {
                                gearsWithParts[gear].Add(part);
                            }
                            else
                            {
                                gearsWithParts[gear] = new List<int> { part };
                            }
                        }
                    }
                    j += length;
                }
            }
        }

        foreach (var kv in gearsWithParts)
        {
            if (kv.Value.Count == 2)
            {
                int gearRatio = kv.Value[0] * kv.Value[1];
                gearRatios += gearRatio;
            }
        }

        return (parts, gearRatios);
    }


    private int GetNumberLength(int i, int jStart)
    { 
        int jEnd = jStart;
        int length = 1;

        while (jEnd + 1 < _input[i].Length)
        {
            if (char.IsDigit(_input[i][jEnd + 1]))
            {
                jEnd++;
                length++;
            }
            else
            { 
                break;
            }
        }

        return length;
    }

    private (bool isPartNumber, List<Point> gears) IsPartNumber(int i, int j, int length)
    {
        List<Point> gears = new List<Point>();
        bool isPartNumber = false;

        for (int k = j - 1; k <= j + length; ++k)
        {
            if (isSymbol(i - 1, k))
            {
                isPartNumber = true;
                if (_input[i - 1][k] == '*')
                { 
                    gears.Add(new Point(i - 1, k));
                }
            }
            if (isSymbol(i + 1, k))
            {
                isPartNumber = true;
                if (_input[i + 1][k] == '*')
                {
                    gears.Add(new Point(i + 1, k));
                }
            }
        }

        if (isSymbol(i, j - 1) )
        {
            isPartNumber = true;
            if (_input[i][j-1] == '*')
            {
                gears.Add(new Point(i, j-1));
            }
        }
        if (isSymbol(i, j + length))
        {
            isPartNumber = true;
            if (_input[i][j + length] == '*')
            {
                gears.Add(new Point(i, j + length));
            }
        }

        return (isPartNumber, gears);
    }

    private bool isSymbol(int i, int j)
    {
        if (i < 0 || i >= _input.Length)
        {
            return false;
        }

        if (j < 0 || j >= _input[0].Length)
        {
            return false;
        }

        return !char.IsDigit(_input[i][j]) && _input[i][j] != '.';
    }

    private record Point(int I, int J);
}
