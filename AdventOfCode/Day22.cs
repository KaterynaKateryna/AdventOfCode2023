namespace AdventOfCode;

public class Day22 : BaseDay
{
    private Brick[] _bricks;

    public Day22()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        _bricks = new Brick[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split("~");
            string[] startCoordinates = parts[0].Split(",");
            string[] endCoordinates = parts[1].Split(",");

            Coordinate start = new Coordinate(
                int.Parse(startCoordinates[0]), 
                int.Parse(startCoordinates[1]), 
                int.Parse(startCoordinates[2])
            );

            Coordinate end = new Coordinate(
                int.Parse(endCoordinates[0]),
                int.Parse(endCoordinates[1]),
                int.Parse(endCoordinates[2])
            );

            _bricks[i] = new Brick(start, end);

            if (_bricks[i].Start.Z > _bricks[i].End.Z ||
                _bricks[i].Start.Y > _bricks[i].End.Y ||
                _bricks[i].Start.X > _bricks[i].End.X)
            {
                throw new Exception("unexpected input");
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        Brick[] orderedBricks = _bricks.OrderBy(b => b.Start.Z).ToArray();

        for (int i = 0; i < orderedBricks.Length; ++i)
        {
            while (CanFall(i, orderedBricks))
            {
                orderedBricks[i].Start.Z--;
                orderedBricks[i].End.Z--;
            }
        }

        var cantBedesintegrated = orderedBricks
            .Where(b => b.SupportedBy.Count() == 1)
            .SelectMany(b => b.SupportedBy)
            .Distinct()
            .Count();
        int canBedesintegrated = orderedBricks.Length - cantBedesintegrated;

        return new(canBedesintegrated.ToString());
    }

    private bool CanFall(int current, Brick[] orderedBricks)
    {
        Brick brick = orderedBricks[current];

        if (brick.Start.Z == 1)
        {
            return false;
        }

        List<Brick> potentialHits = orderedBricks.Where(b => b.End.Z == brick.Start.Z - 1).ToList();
        HashSet<Brick> bottomHits = new HashSet<Brick>();

        if (brick.Start.X != brick.End.X)
        {
            for (int i = brick.Start.X; i <= brick.End.X; ++i)
            {
                var hit = potentialHits.FirstOrDefault(b => ContainsCoordinate(b, new Coordinate(i, brick.Start.Y, brick.Start.Z - 1)));
                if (hit != null)
                {
                    bottomHits.Add(hit);
                }
            }
        }
        else if (brick.Start.Y != brick.End.Y)
        {
            for (int i = brick.Start.Y; i <= brick.End.Y; ++i)
            {
                var hit = potentialHits.FirstOrDefault(b => ContainsCoordinate(b, new Coordinate(brick.Start.X, i, brick.Start.Z - 1)));
                if (hit != null)
                {
                    bottomHits.Add(hit);
                }
            }
        }
        else
        {
            var hit = potentialHits.FirstOrDefault(b => ContainsCoordinate(b, new Coordinate(brick.Start.X, brick.Start.Y, brick.Start.Z - 1)));
            if (hit != null)
            {
                bottomHits.Add(hit);
            }
        }

        if (bottomHits.Any())
        { 
            brick.SupportedBy.AddRange(bottomHits);
            return false;
        }

        return true;
    }

    private bool ContainsCoordinate(Brick b, Coordinate coordinate)
    {
        bool contains = (coordinate.X >= b.Start.X && coordinate.X <= b.End.X) &&
            (coordinate.Y >= b.Start.Y && coordinate.Y <= b.End.Y) &&
            (coordinate.Z >= b.Start.Z && coordinate.Z <= b.End.Z);

        return contains;
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private record Coordinate(int X, int Y, int Z)
    {
        public int X { get; set; } = X;

        public int Y { get; set; } = Y;

        public int Z { get; set; } = Z;
    }

    private record Brick(Coordinate Start, Coordinate End)
    {
        public List<Brick> SupportedBy { get; set; } = new List<Brick>();
    }
}
