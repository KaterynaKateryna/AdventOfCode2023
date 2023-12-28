namespace AdventOfCode;

public class Day14 : BaseDay
{
    private char[][] _input;

    private HashSet<Point> _roundRocks = new();
    private HashSet<Point> _squareRocks = new();

    private HashSet<Point> _rocks = new();
    private Dictionary<int, List<Point>> _roundRocksByRow = new();

    public Day14()
    {
        _input = File.ReadAllLines(InputFilePath).Select(l => l.ToArray()).ToArray();

        for (int i = 0; i < _input.Length; ++i)
        {
            for (int j = 0; j < _input[i].Length; ++j)
            {
                if (_input[i][j] == 'O')
                {
                    _roundRocks.Add(new Point(i, j));
                    _rocks.Add(new Point(i, j));
                }
                if (_input[i][j] == '#')
                {
                    _squareRocks.Add(new Point(i, j));
                    _rocks.Add(new Point(i, j));
                }
            }
        }

        _roundRocksByRow = _roundRocks.GroupBy(k => k.I).ToDictionary(k => k.Key, k => k.ToList());
        for (int i = 0; i < _input.Length; ++i)
        {
            if (!_roundRocksByRow.ContainsKey(i))
            {
                _roundRocksByRow[i] = new List<Point>();
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        TiltNorth();
        int load = GetLoad();
        return new(load.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        List<string> maps = new List<string>();

        string key = string.Join(Environment.NewLine, _input.Select(l => new string(l)));
        while (!maps.Contains(key))
        {
            maps.Add(key);

            Cycle();
            key = string.Join(Environment.NewLine, _input.Select(l => new string(l)));
        }

        int cycleStart = maps.IndexOf(key);
        int cycleLength = maps.Count - cycleStart;

        int rest = (1_000_000_000 - maps.Count) % cycleLength;

        for (int i = 0; i < rest; ++i)
        {
            Cycle();
        }

        int load = GetLoad();
        return new(load.ToString());
    }

    private void Cycle()
    {
        TiltNorth();
        TiltWest();
        TiltSouth();
        TiltEast();
    }

    private void TiltNorth()
    {
        for (int i = 1; i < _input.Length; ++i)
        {
            for (int j = 0; j < _input.Length; ++j)
            {
                if (_input[i][j] == 'O')
                {
                    int replace = i - 1;
                    while (replace >= 0 && _input[replace][j] == '.')
                    {
                        replace--;
                    }
                    replace++;
                    if (replace != i)
                    {
                        _input[i][j] = '.';
                        _input[replace][j] = 'O';
                    }
                }
            }
        }
    }

    private void TiltWest()
    {
        for (int i = 0; i < _input.Length; ++i)
        {
            for (int j = 1; j < _input[i].Length; ++j)
            {
                if (_input[i][j] == 'O')
                {
                    int replace = j - 1;
                    while (replace >= 0 && _input[i][replace] == '.')
                    {
                        replace--;
                    }
                    replace++;
                    if (replace != j)
                    {
                        _input[i][j] = '.';
                        _input[i][replace] = 'O';
                    }
                }
            }
        }
    }

    private void TiltSouth()
    {
        for (int i = _input.Length - 2; i >= 0; --i)
        {
            for (int j = 0; j < _input[0].Length; ++j)
            {
                if (_input[i][j] == 'O')
                {
                    int replace = i + 1;
                    while (replace < _input.Length && _input[replace][j] == '.')
                    {
                        replace++;
                    }
                    replace--;
                    if (replace != i)
                    {
                        _input[i][j] = '.';
                        _input[replace][j] = 'O';
                    }
                }
            }
        }
    }


    private void TiltEast()
    {
        for (int i = 0; i < _input.Length; ++i)
        {
            for (int j = _input[i].Length - 2; j >= 0; --j)
            {
                if (_input[i][j] == 'O')
                {
                    int replace = j + 1;
                    while (replace < _input.Length && _input[i][replace] == '.')
                    {
                        replace++;
                    }
                    replace--;
                    if (replace != j)
                    {
                        _input[i][j] = '.';
                        _input[i][replace] = 'O';
                    }
                }
            }
        }
    }

    private int GetLoad()
    {
        int load = 0;
        for (int i = 0; i < _input.Length; ++i)
        {
            load += _input[i].Count(x => x == 'O') * (_input.Length - i);
        }
        return load;
    }

    private record Point(int I, int J)
    {
        public int I { get; set; } = I;

        public int J { get; set; } = J;
    }
}
