namespace AdventOfCode;

internal class Day24 : BaseDay
{
    private Path[] _paths;

    public Day24()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        _paths = new Path[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Replace(" ", "").Split("@");
            string[] pointValues = parts[0].Split(",");
            string[] velocityValues = parts[1].Split(",");

            Coordinate point = new Coordinate(
                double.Parse(pointValues[0]),
                double.Parse(pointValues[1]),
                double.Parse(pointValues[2])
            );

            Coordinate velocity = new Coordinate(
                double.Parse(velocityValues[0]),
                double.Parse(velocityValues[1]),
                double.Parse(velocityValues[2])
            );
            _paths[i] = new(point, velocity);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        double boundaryMin = 200000000000000;
        double boundaryMax = 400000000000000;

        Line[] lines = _paths.Select(p => GetLine(p, boundaryMin, boundaryMax)).ToArray();
        int collisions = 0;
        for (int i = 0; i < lines.Length - 1; ++i)
        {
            for (int j = i + 1; j < lines.Length; ++j)
            {
                if (Collide(lines[i], lines[j], boundaryMin, boundaryMax))
                {
                    collisions++;
                }
            }
        }

        return new(collisions.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private bool Collide(Line line1, Line line2, double boundaryMin, double boundaryMax)
    {
        // find collision point and check that it's within range
        // https://www.jeffreythompson.org/collision-detection/line-line.php

        double uA = ((line2.PointB.X - line2.PointA.X) * (line1.PointA.Y - line2.PointA.Y) - (line2.PointB.Y - line2.PointA.Y) * (line1.PointA.X - line2.PointA.X)) / 
            ((line2.PointB.Y - line2.PointA.Y) * (line1.PointB.X - line1.PointA.X) - (line2.PointB.X - line2.PointA.X) * (line1.PointB.Y - line1.PointA.Y));

        double uB = ((line1.PointB.X - line1.PointA.X) * (line1.PointA.Y - line2.PointA.Y) - (line1.PointB.Y - line1.PointA.Y) * (line1.PointA.X - line2.PointA.X)) / 
            ((line2.PointB.Y - line2.PointA.Y) * (line1.PointB.X - line1.PointA.X) - (line2.PointB.X - line2.PointA.X) * (line1.PointB.Y - line1.PointA.Y));
        
        if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
        {
            double intersectionX = line1.PointA.X + (uA * (line1.PointB.X - line1.PointA.X));
            double intersectionY = line1.PointA.Y + (uA * (line1.PointB.Y - line1.PointA.Y));

            if (intersectionX >= boundaryMin && intersectionX <= boundaryMax &&
                intersectionY >= boundaryMin && intersectionY <= boundaryMax)
            {
                return true;
            }
        }
        return false;
    }

    private Line GetLine(Path path, double boundaryMin, double boundaryMax)
    {
        long timeX = 0;
        if (path.Velocity.X > 0) // moving right
        {
            timeX = (long)((boundaryMax - path.Point.X) / path.Velocity.X) + 1;
        }
        else if (path.Velocity.X < 0) // moving left
        {  
            timeX = (long)((path.Point.X - boundaryMin) / -path.Velocity.X) + 1;
        }

        long timeY = 0;
        if (path.Velocity.Y > 0) // moving up
        {
            timeY = (long)((boundaryMax - path.Point.Y) / path.Velocity.Y) + 1;
        }
        else if (path.Velocity.Y < 0) // moving down
        {
            timeY = (long)((path.Point.Y - boundaryMin) / -path.Velocity.Y) + 1;
        }

        long time = timeX > timeY ? timeX : timeY;

        Coordinate pointB = new(
            path.Point.X + path.Velocity.X * time,
            path.Point.Y + path.Velocity.Y * time,
            path.Point.Z + path.Velocity.Z * time
        );

        Line line = new(path.Point, pointB);
        return line;
    }

    private record Path(Coordinate Point, Coordinate Velocity);

    private record Line(Coordinate PointA, Coordinate PointB);

    private record Coordinate(double X, double Y, double Z);
}
