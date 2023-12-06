namespace AdventOfCode;

public class Day06 : BaseDay
{
    private string[] _lines;

    public Day06()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        string[] times = _lines[0].Replace("Time:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);
        string[] distances = _lines[1].Replace("Distance:", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);

        Race[] races = new Race[times.Length];
        for (int i = 0; i < times.Length; ++i)
        {
            races[i] = new Race(int.Parse(times[i]), int.Parse(distances[i]));
        }

        long result = races.Aggregate(1L, (result, race) => result * GetWaysToWin(race));
        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long time = long.Parse(_lines[0].Replace("Time:", "").Replace(" ", ""));
        long distance = long.Parse(_lines[1].Replace("Distance:", "").Replace(" ", ""));

        return new(GetWaysToWin(new Race(time, distance)).ToString());
    }

    private long GetWaysToWin(Race race)
    {
        long waysToWin = 0;
        for (long i = 0; i < race.Time; ++i)
        {
            long distance = (race.Time - i) * i;
            waysToWin += distance > race.Distance ? 1 : 0;
        }
        return waysToWin;
    }

    private record Race(long Time, long Distance);
}
