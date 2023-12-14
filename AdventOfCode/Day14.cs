namespace AdventOfCode;

public class Day14 : BaseDay
{
    private char[][] _input;

    public Day14() 
    {
        _input = File.ReadAllLines(InputFilePath).Select(l => l.ToArray()).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        TiltNorth();
        int load = GetLoad();
        return new(load.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private void TiltNorth()
    {
        for (int i = 1; i < _input.Length; ++i)
        {
            for (int j = 0; j < _input[i].Length; ++j)
            {
                if (_input[i][j] == 'O')
                {
                    RollNorth(i, j);
                }
            }
        }
    }

    private void RollNorth(int i, int j)
    {
        int replace = i-1;
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

    private int GetLoad()
    {
        int load = 0;
        for (int i = 0; i < _input.Length; ++i)
        {
            load += _input[i].Count(x => x == 'O') * (_input.Length - i);
        }
        return load;
    }
}
