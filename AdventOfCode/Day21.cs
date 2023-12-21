namespace AdventOfCode;

public class Day21 : BaseDay
{
    private char[][] _input;

    private Point _start;

    public Day21() 
    {
        _input = File.ReadAllLines(InputFilePath).Select(l => l.ToArray()).ToArray();
        _start = GetStart();
    }

    public override ValueTask<string> Solve_1()
    {
        HashSet<Point> points = new HashSet<Point>();
        points.Add(_start);

        for (int i = 0; i < 64; ++i)
        {
            HashSet<Point> nextPoints = new HashSet<Point>();
            foreach (var point in points)
            {
                var adjacent = GetPossibleMoves(point);
                foreach (Point p in adjacent)
                {
                    nextPoints.Add(p);
                }
            }

            points = nextPoints;
        }

        return new(points.Count().ToString());
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
                    return new(i, j);
                }
            }
        }

        throw new Exception("start not found");
    }

    private List<Point> GetPossibleMoves(Point point)
    {
        List<Point> points = new List<Point>();

        if (point.I > 0 && _input[point.I - 1][point.J] != '#')
        {
            points.Add(new(point.I - 1, point.J));
        }
        if (point.I < _input.Length - 1 && _input[point.I + 1][point.J] != '#')
        {
            points.Add(new(point.I + 1, point.J));
        }
        if (point.J > 0 && _input[point.I][point.J - 1] != '#')
        {
            points.Add(new(point.I, point.J - 1));
        }
        if (point.J < _input[0].Length - 1 && _input[point.I][point.J + 1] != '#')
        {
            points.Add(new(point.I, point.J + 1));
        }

        return points;
    }

    private record Point(int I, int J);
}
