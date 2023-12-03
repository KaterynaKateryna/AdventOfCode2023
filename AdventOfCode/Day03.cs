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
        long parts = 0;
        for (int i = 0; i < _input.Length; ++i)
        {
            for (int j = 0; j < _input[0].Length; ++j)
            {
                if (char.IsDigit(_input[i][j]))
                {
                    int length = GetNumberLength(i, j);
                    if (IsPartNumber(i, j, length))
                    {
                        int part = int.Parse(_input[i].Substring(j, length));
                        parts += part;
                    }
                    j += length;
                }
            }
        }

        return new(parts.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
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

    private bool IsPartNumber(int i, int j, int length)
    {
        for (int k = j - 1; k <= j + length; ++k)
        {
            if (isSymbol(i - 1, k) || isSymbol(i + 1, k))
            {
                return true;
            }
        }

        if (isSymbol(i, j - 1) || isSymbol(i, j + length))
        {
            return true;
        }

        return false;
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
}
