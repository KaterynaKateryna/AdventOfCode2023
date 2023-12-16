namespace AdventOfCode;

public class Day16 : BaseDay
{
    private string[] _input;

    public Day16() 
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        return new(GetEnergizedCount(new(new Point(0, 0), Direction.Right)).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int maxCount = 0;
        for (int i = 0; i < _input.Length; ++i)
        {
            int cnt = GetEnergizedCount(new Move(new Point(i, 0), Direction.Right));
            if (cnt > maxCount)
            { 
                maxCount = cnt;
            }

            cnt = GetEnergizedCount(new Move(new Point(i, _input[0].Length - 1), Direction.Left));
            if (cnt > maxCount)
            {
                maxCount = cnt;
            }
        }

        for (int j = 0; j < _input[0].Length; ++j)
        {
            int cnt = GetEnergizedCount(new Move(new Point(0, j), Direction.Down));
            if (cnt > maxCount)
            {
                maxCount = cnt;
            }

            cnt = GetEnergizedCount(new Move(new Point(_input.Length - 1, j), Direction.Up));
            if (cnt > maxCount)
            {
                maxCount = cnt;
            }
        }

        return new(maxCount.ToString());
    }

    private int GetEnergizedCount(Move start)
    {
        HashSet<Move> energized = new HashSet<Move>();
        Stack<Move> toVisit = new Stack<Move>();

        toVisit.Push(start);

        while (toVisit.Any())
        {
            Move visited = toVisit.Pop();
            if (energized.Add(visited))
            {
                (Move nextOne, Move nextTwo) = NextMove(visited);
                if (nextOne?.Point != null)
                {
                    toVisit.Push(nextOne);
                }
                if (nextTwo?.Point != null)
                {
                    toVisit.Push(nextTwo);
                }
            }
        }
        return energized.Select(x => x.Point).Distinct().Count();
    }

    private (Move one, Move two) NextMove(Move current)
    {
        Direction direction = current.Direction;
        char value = _input[current.Point.I][current.Point.J];

        switch ((value, direction))
        {
            case ('.', Direction.Right):
            case ('\\', Direction.Down):
            case ('/', Direction.Up):
            case ('-', Direction.Right):
                return (new (current.Point.J < _input[0].Length - 1 ? new Point(current.Point.I, current.Point.J + 1) : null, Direction.Right), null);
            case ('.', Direction.Left):
            case ('\\', Direction.Up):
            case ('/', Direction.Down):
            case ('-', Direction.Left):
                return (new (current.Point.J > 0 ? new Point(current.Point.I, current.Point.J - 1) : null, Direction.Left), null);
            case ('.', Direction.Up):
            case ('\\', Direction.Left):
            case ('/', Direction.Right):
            case ('|', Direction.Up):
                return (new (current.Point.I > 0 ? new Point(current.Point.I - 1, current.Point.J) : null, Direction.Up), null);
            case ('.', Direction.Down):
            case ('\\', Direction.Right):
            case ('/', Direction.Left):
            case ('|', Direction.Down):
                return (new (current.Point.I < _input.Length - 1 ? new Point(current.Point.I + 1, current.Point.J) : null, Direction.Down), null);
            case ('|', Direction.Left):
            case ('|', Direction.Right):
                return (new(current.Point.I > 0 ? new Point(current.Point.I - 1, current.Point.J) : null, Direction.Up),
                    new(current.Point.I < _input.Length - 1 ? new Point(current.Point.I + 1, current.Point.J) : null, Direction.Down));
            case ('-', Direction.Up):
            case ('-', Direction.Down):
                return (new(current.Point.J > 0 ? new Point(current.Point.I, current.Point.J - 1) : null, Direction.Left),
                    new(current.Point.J < _input[0].Length - 1 ? new Point(current.Point.I, current.Point.J + 1) : null, Direction.Right));
            default:
                throw new NotSupportedException();
        }
    }

    private record Point(int I, int J);

    private enum Direction
    { 
        Right,
        Left,
        Down,
        Up
    }

    private record Move(Point Point, Direction Direction)
    {
    }
}
