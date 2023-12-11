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
        List<Point> points = GetLoopPoints();
        return new((points.Count / 2).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        List<Point> loopPoints = GetLoopPoints();
        var insidePoints = GetInsidePoints(loopPoints);
        return new(insidePoints.Count.ToString());
    }

    private List<Point> GetInsidePoints(List<Point> loopPoints)
    {
        List<Point> points = new List<Point>();
        for (int i = 0; i < _input.Length; ++i)
        {
            var lineLoopPoints = loopPoints.Where(p => p.I == i);
            bool inside = false;
            for (int j = 0; j < _input[0].Length; ++j)
            {
                Point p = lineLoopPoints.SingleOrDefault(p => p.I == i && p.J == j);
                if (p != null)
                {
                    if (p.Value == '|')
                    {
                        inside = !inside;
                    }
                    else if (p.Value == 'L' || p.Value == 'F')
                    {
                        char nextP = 'X';
                        do
                        {
                            j++;
                            nextP = _input[i][j];
                        } 
                        while (nextP != '7' && nextP != 'J');

                        if ((p.Value == 'L' && nextP == '7') || (p.Value == 'F' && nextP == 'J'))
                        {
                            inside = !inside;
                        }
                    }
                }
                else if (inside)
                {
                    points.Add(new Point(i, j, 'I'));
                }
            }
        }
        return points;
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
                return point.I + 1 >= _input[0].Length ? null : (new Point(point.I + 1, point.J, _input[point.I + 1][point.J]), Direction.North);
            case ('|', Direction.South):
            case ('L', Direction.East):
            case ('J', Direction.West):
                return point.I - 1 < 0 ? null : (new Point(point.I - 1, point.J, _input[point.I - 1][point.J]), Direction.South);
            case ('-', Direction.West):
            case ('L', Direction.North):
            case ('F', Direction.South):
                return point.J + 1 >= _input[0].Length ? null : (new Point(point.I, point.J + 1, _input[point.I][point.J + 1]), Direction.West);
            case ('-', Direction.East):
            case ('J', Direction.North):
            case ('7', Direction.South):
                return point.J - 1 < 0 ? null : (new Point(point.I, point.J - 1, _input[point.I][point.J - 1]), Direction.East);
            default:
                return null;
        }
    }

    private List<Point> GetLoopPoints()
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
            List<Point> points = new List<Point>();
            Point next = start with { Value = startOption };
            Direction nextFrom = from;
            points.Add(next);

            while (next != null && next.Value != 'S')
            {
                (next, nextFrom) = GetNextMove(next, nextFrom) ?? default;
                points.Add(next);
            }

            if (next?.Value == 'S' && from == nextFrom)
            {
                points.Remove(next);
                return points;
            }
        }
        throw new Exception("Loop not found");
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
