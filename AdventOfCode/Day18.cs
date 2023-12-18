namespace AdventOfCode;

public class Day18 : BaseDay
{
    string[] _input;

    public Day18()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        Move[] moves = _input.Select(l => 
        {
            string[] parts = l.Split(" ");
            return new Move(parts[0], int.Parse(parts[1]), parts[2]);
        }).ToArray();
        List<Point> points = new List<Point>();
        Point current = new Point(0, 0);
        points.Add(current);
        for (int i = 0; i < moves.Length; ++i)
        {
            for (int j = 0; j < moves[i].Steps; ++j)
            {
                switch (moves[i].Direction)
                {
                    case "R":
                        current = new Point(current.I, current.J + 1);
                        break;
                    case "L":
                        current = new Point(current.I, current.J - 1);
                        break;
                    case "D":
                        current = new Point(current.I + 1, current.J);
                        break;
                    case "U":
                        current = new Point(current.I - 1, current.J);
                        break;
                    default:
                        throw new NotSupportedException();
                }
                points.Add(current);
            }
        }

        int iMin = points.Min(p => p.I) - 1; 
        int iMax = points.Max(p => p.I) + 1;
        int jMin = points.Min(p => p.J) - 1;
        int jMax = points.Max(p => p.J) + 1;

        int height = iMax - iMin + 1;
        int width = jMax - jMin + 1;

        int outside = 0;

        Dictionary<int, List<Point>> border = points.GroupBy(x => x.I).ToDictionary(x => x.Key, x => x.ToList());
        Dictionary<int, List<Point>> visited = new Dictionary<int, List<Point>>();
        Stack<Point> toVisit = new Stack<Point>();
        toVisit.Push(new Point(iMin, jMin));

        while (toVisit.Any())
        {
            Point v = toVisit.Pop();
            if (border.ContainsKey(v.I) && border[v.I].Contains(v))
            {
                continue;
            }

            if (!visited.ContainsKey(v.I))
            {
                visited[v.I] = new List<Point>();
            }

            if (!visited[v.I].Contains(v))
            {
                visited[v.I].Add(v);
                outside++;

                List<Point> adj = GetAdjacent(iMin, iMax, jMin, jMax, v.I, v.J);
                foreach (Point p in adj)
                {
                    toVisit.Push(p);
                }
            }
        }

        int size = (height) * (width);
        int lava = size - outside;

        return new(lava.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("TBD");
    }

    private List<Point> GetAdjacent(int iMin, int iMax, int jMin, int jMax, int i, int j)
    { 
        List<Point> points = new List<Point>();
        if (i > iMin)
        { 
            points.Add(new Point(i - 1, j));
        }
        if (i < iMax)
        {
            points.Add(new Point(i + 1, j));
        }
        if (j > jMin)
        {
            points.Add(new Point(i, j - 1));
        }
        if (j < jMax)
        {
            points.Add(new Point(i, j + 1));
        }
        return points;
    }

    private record Point(int I, int J);

    private record Move(string Direction, int Steps, string Colour);
}
