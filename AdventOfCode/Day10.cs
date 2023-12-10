namespace AdventOfCode;

public class Day10 : BaseDay
{
    private string[] _input;

    public Day10() 
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        
        Point start = GetStart();
        (char value, Direction from)[] startOptions = 
        [
            ('|', Direction.North), 
            ('-', Direction.East), 
            ('L', Direction.North), 
            ('J', Direction.West), 
            ('7', Direction.West), 
            ('F', Direction.South)
        ];

        foreach ((char startOption, Direction from) in startOptions)
        {
            Point next = start with { Value = startOption };
            Direction nextFrom = from;
            long steps = 0;

            while (next != null && next.Value != 'S')
            {
                (next, nextFrom) = GetNextMove(next, nextFrom) ?? default;
                steps++;
            }

            if (next?.Value == 'S')
            {
                return new((steps / 2).ToString());
            }
        }
        return new("Loop not found");
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private Point GetStart()
    {
        for (int i = 0; i < _input.Length; ++i)
        {
            for (int j = 0; j < _input[i].Length; ++j)
            {
                if (_input[i][j] == 'S')
                { 
                    return new Point(i, j, _input[i][j]);
                }
            }
        }

        throw new Exception("Start not found");
    }

    private (Point point, Direction direction)? GetNextMove(Point point, Direction from)
    {
        switch ((point.Value, from))
        {
            case ('|', Direction.North):
            case ('7', Direction.West):
            case ('F', Direction.East):
                return point.I + 1 >= _input.Length ? null : (new Point(point.I + 1, point.J, _input[point.I + 1][point.J]), Direction.North);
            case ('|', Direction.South):
            case ('L', Direction.East):
            case ('J', Direction.West):
                return point.I - 1 < 0 ? null : (new Point(point.I - 1, point.J, _input[point.I - 1][point.J]), Direction.South);
            case ('-', Direction.West):
            case ('L', Direction.North):
            case ('F', Direction.South):
                return point.J + 1 >= _input.Length ? null : (new Point(point.I, point.J + 1, _input[point.I][point.J + 1]), Direction.West);
            case ('-', Direction.East):
            case ('J', Direction.North):
            case ('7', Direction.South):
                return point.J - 1 < 0 ? null : (new Point(point.I, point.J - 1, _input[point.I][point.J - 1]), Direction.East);
            case ('.', Direction.North):
            case ('.', Direction.East):
            case ('.', Direction.South):
            case ('.', Direction.West):
            default:
                return null;
        }
    }

    private record Point(int I, int J, char Value);

    private enum Direction
    { 
        North,
        East, 
        South,
        West
    }
}
